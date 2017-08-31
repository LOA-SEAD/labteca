using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Receives from InputController what actions should be done, and generate the events accordingly.
/// </summary>
public class GameInputManager : MonoBehaviour {

	#region Singleton pattern
	protected static GameInputManager singleton;

	public static GameInputManager Singleton { 
		get { 
			if (singleton==null) singleton = FindObjectOfType<GameInputManager>();
			return singleton; 
		} 
	}
	#endregion

	#region Input event parameter
	public class EventData {
		/*public string axis = null;
		public string button = null;
		public KeyCode keyCode = KeyCode.None;
		public bool used = false;
		public float value = 0f;

		public EventData(KeyCode keyCode) { this.keyCode = keyCode; }
		public EventData(string axis, float value) { this.axis = axis; this.value = value; }
		public EventData(string button) { this.button = button; }*/
		public string inputName = null;
		public bool used = false;

		public EventData(string inputName) { this.inputName = inputName; }
	}
	#endregion

	public const int MAX_PRIORITY = 10000;

	#region Public static methods (API)
	/// <summary>Register an axis as one of interest.</summary>
	public static void ObserveAxis(string axis) {
		if (!string.IsNullOrEmpty(axis) && Singleton) Singleton.observedAxes.Add(axis);
	}

	/// <summary>Register a button as one of interest.</summary>
	public static void ObserveButton(string button) {
		if (!string.IsNullOrEmpty(button) && Singleton) Singleton.observedButtons.Add(button);
	}

	/// <summary>Register a keycode as one of interest.</summary>
	public static void ObserveKeyCode(KeyCode keyCode) {
		if (keyCode!=KeyCode.None && Singleton) Singleton.observedKeycodes.Add(keyCode);
	}

	/// <summary>Register a handler method for hotkey event with one above currently highest priority.</summary>
	/// <param name="Action">Handler method that is called when hotkey event triggers. That method has one EventData parameter.</param>
	public static void Register(System.Action<EventData> Action) {
		if (Action!=null && Singleton!=null) Singleton.GetBlock(Singleton.highestPriority + 1).Event += Action;
	}

	/// <summary>Register a handler method for hotkey event with the specified priority.</summary>
	/// <param name="Action">Handler method that is called when hotkey event triggers. That method has one EventData parameter.</param>
	/// <param name="priority">Callbacks are made in order of priority (from the highest to the lowest).</param>
	public static void Register(System.Action<EventData> Action, int priority) {
		if (Action!=null && Singleton!=null) Singleton.GetBlock(priority).Event += Action;
	}

	/// <summary>Unregister a callback method from all Input events.</summary>
	public static void Unregister(System.Action<EventData> Action) {
		if (Action!=null && Singleton!=null) foreach (EventBlock b in Singleton.eventBlocks) b.Event -= Action;
	}
	#endregion

	#region Unity magic methods
	protected void Awake() {
		singleton = this;

		//singleton.
	}

	protected void Update() {
		foreach (string a in observedAxes) {
			SendEvent(new EventData(a, Input.GetAxis(a)));
		}
		foreach (string b in observedButtons) {
			if (Input.GetButtonDown(b)) SendEvent(new EventData(b));
		}
		foreach (KeyCode k in observedKeycodes) {
			if (Input.GetKeyDown(k)) SendEvent(new EventData(k));
		}
	}
	#endregion

	#region Internals (under the hood)
	protected class EventBlock : System.IComparable<EventBlock> {

		public int priority;
		public event System.Action<EventData> Event;

		public EventBlock(int p) { priority = p; }

		public void AppendTo(ref System.Action<EventData> deleg) { if (Event!=null) deleg += Event; }

		// Order highest to lowest
		public int CompareTo(EventBlock other) { return -priority.CompareTo(other.priority); }

		public void Invoke(EventData eventData) { if (Event!=null) Event(eventData); }

		public bool IsEmpty { get { return Event==null; } }
	}

	protected List<EventBlock> eventBlocks = new List<EventBlock>();
	protected HashSet<string> observedAxes = new HashSet<string>();
	protected HashSet<string> observedButtons = new HashSet<string>();
	protected HashSet<KeyCode> observedKeycodes = new HashSet<KeyCode>();

	protected EventBlock GetBlock(int priority) {
		foreach (EventBlock b in eventBlocks) if (b.priority==priority) return b;
		EventBlock newBlock = new EventBlock(priority);
		eventBlocks.Add(newBlock);
		eventBlocks.Sort();
		return newBlock;
	}

	protected int highestPriority { 
		get {
			// eventBlocks is always sorted in reversed priority order (i.e., highest to lowest), so first non-empty block is the correct result
			foreach (EventBlock b in eventBlocks) if (b.priority<MAX_PRIORITY && !b.IsEmpty) return b.priority;
			return 0;
		}
	}

	protected void SendEvent(EventData data) {
		System.Action<EventData> callStack = null;
		foreach (EventBlock block in eventBlocks) block.AppendTo(ref callStack);
		if (callStack!=null) callStack(data);
	}
	#endregion
}