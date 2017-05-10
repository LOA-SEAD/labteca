using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JournalUIItem : MonoBehaviour {
	public int index;
	public string name;
	public Text journalText;
	public Image checkmarkPlace;
	public RectTransform self;
	public GameObject checkmark;	
	public bool isDone=false,prerequisitesDone=false;
	public List<int> prerequisites = new List<int>();
	
	public void Start(){
		checkmark.SetActive (isDone?true:false);
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

	public void SetPrerequisites(int index){
		prerequisites.Add (index);
	}

	public void checkPrerequisites(){
		Debug.Log ("Checking prerequirements");
		if (prerequisites [0] == null)
			prerequisites.RemoveAt (0);
		bool allDone=true;
		for (int i=0; i < prerequisites.Count&&allDone; i++) {
			allDone = GameObject.Find("JournalUIItem"+prerequisites[i]).GetComponent<JournalUIItem>().isDone;
		}

		Debug.Log ("Gonna check if allDone");
		if (allDone) { 
			Debug.Log ("It's all done");
			prerequisitesDone = true;
			checkmarkPlace.color = new Color (255, 255, 255, 255);
			journalText.enabled = true;
			checkmarkPlace.enabled = true;
		} else {
			Debug.Log ("It's not all done ):");
			journalText.enabled = false;
			checkmarkPlace.enabled = false;
		}

	}
}
