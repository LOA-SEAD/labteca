using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalUIItem : MonoBehaviour {
	public int index;
	public string name;
	public Text journalText;
	public Image checkmarkPlace;
	public RectTransform self;
	public GameObject checkmark;
	public bool isDone,prerequisitesDone;
	public JournalUIItem[] prerequisites;
	
	public void Start(){
		checkmark.SetActive (isDone?true:false);
		//journalText.text = name;
	}

	public void checkItem(){

		if (!isDone && prerequisitesDone){
			isDone=true;
			GameObject[] journalItems = GameObject.FindGameObjectsWithTag("JournalUIItem");
			for(int i = 0;i<journalItems.Length;i++){
				journalItems[i].GetComponent<JournalUIItem>().checkPrerequisites();
			}
			checkmark.SetActive (true);
		}
	}

	public void setText(string txt){
		journalText.text = txt;
		name = txt;
	}

	public void checkPrerequisites(){
		bool allDone=true;
		for (int i=0; i < prerequisites.Length&&allDone; i++) {
			allDone = prerequisites[i].isDone;
		}

		if (allDone) { 
			prerequisitesDone = true;
			checkmarkPlace.color = new Color(255,255,255,255);
		}

	}
}
