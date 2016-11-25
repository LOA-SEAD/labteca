using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateUIManager : MonoBehaviour {

	public GameObject optionDialogGlass;        /*!< Dialog. */
	public GameObject optionDialogReagent;      /*!< Dialog. */
	public GameObject glasswareLabelEditor;		// Interaction box for editing the glassware label.
	public GameObject optionDialogEquipment;    /*!< Should only be linked when there's an equipment in the state */
	public WorkBench currentWorkbench;
	
	void Start () {
		if (gameObject.GetComponentInParent<WorkBench> () != null)
			currentWorkbench = gameObject.GetComponentInParent<WorkBench> ();
	}

	public void OpenOptionDialogEquipment(){
		if (CursorManager.GetCurrentState () != MouseState.ms_interacting) {
			optionDialogEquipment.SetActive (true);
			CursorManager.SetMouseState (MouseState.ms_interacting);
		}
	}

	public void CloseAll(){
		optionDialogGlass.SetActive(false);
		optionDialogReagent.SetActive(false);
		optionDialogEquipment.SetActive(false);
		glasswareLabelEditor.SetActive (false);

		if (CursorManager.GetCurrentState () == MouseState.ms_interacting) {
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
		}
	}

	public void OpenOptionDialog(ItemToInventory item){
		if (CursorManager.GetCurrentState () != MouseState.ms_interacting) {
			CursorManager.SetMouseState (MouseState.ms_interacting);
			if (item is Glassware) {
				if (gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament != null &&
					gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament.childCount != 0 &&
					item.gameObject.Equals (gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament.GetChild (0).gameObject)) {

					List<int> ids = new List<int> ();
					ids.Add (0);
					ids.Add (2);
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().changeIDs (ids);
				} else {
					List<int> ids = new List<int> ();
					ids.Add (0);
					ids.Add (1);
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().changeIDs (ids);
				}
				optionDialogGlass.SetActive (true);
				optionDialogGlass.GetComponent<OptionDialogBehaviour> ().setCurrentItem (item);
				if(item.GetComponent<Glassware>().content == null) { //Enabling or disabling the edit label button. TODO: Refactor later
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().editLabelButton.enabled = false;
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().editLabelButton.image.color = Color.grey;
				} else {
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().editLabelButton.enabled = true;
					optionDialogGlass.GetComponent<OptionDialogBehaviour> ().editLabelButton.image.color = Color.white;
				}
				glasswareLabelEditor.GetComponent<OptionDialogBehaviour> ().setCurrentItem (item);
				return;
			}

			if (item is ReagentPot) {
				optionDialogReagent.SetActive (true);
				optionDialogReagent.GetComponent<OptionDialogBehaviour> ().setCurrentItem (item);

				return;
			}
		}
	}
}
