using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalUIItem : MonoBehaviour {
	public int index;
	public string name;
	public Text journalText;
	public RectTransform self;
	public GameObject checkmark;
	public bool isDone,prerequisitesDone;
	public JournalUIItem[] prerequisites;
	
	public void Start(){
		checkmark.SetActive (isDone?true:false);
		journalText.text = name;
		//resize ();
	}

	public void checkItem(){

		if (!isDone && prerequisitesDone){
			isDone=true;
			GameObject[] journalItems = GameObject.FindGameObjectsWithTag("JournalUIItem");
			for(int i = 0;i<journalItems.Length;i++){
				journalItems[i].GetComponent<JournalUIItem>().checkPrerequisites();
			}
			//GameObject.Find("GameController").GetComponent<GameController>().CallJSaver(this);
			checkmark.SetActive (true);
		}
	}

	public void resize(){
		journalText.text = name;

		Debug.Log (journalText.preferredHeight);
		self.sizeDelta = new Vector2 (self.sizeDelta.x, journalText.preferredHeight);

	}
	
	public void checkPrerequisites(){
		bool allDone=true;
		for (int i=0; i < prerequisites.Length&&allDone; i++) {
			allDone = prerequisites[i].isDone;
		}

		if (allDone) 
			prerequisitesDone=true;

	}
}
