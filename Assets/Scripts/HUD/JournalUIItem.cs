﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalUIItem : MonoBehaviour {
	public bool isDone,prerequisitesDone;
	public JournalUIItem[] prerequisites;

	public void Start(){
		checkPrerequisites ();
	}

	public void checkItem(){
		if (!isDone && prerequisitesDone){
			isDone=true;
			GameObject[] journalItems = GameObject.FindGameObjectsWithTag("JournalUIItem");
			for(int i = 0;i<journalItems.Length;i++){
				journalItems[i].GetComponent<JournalUIItem>().checkPrerequisites();
			}
		}
	}
	
	public void checkPrerequisites(){
		bool allDone=true;
		for (int i=0; i < prerequisites.Length&&allDone; i++) {
			allDone = prerequisites[i].isDone;
		}

		if (allDone) {
			prerequisitesDone=true;
			this.GetComponent<Image> ().color = new Color (this.GetComponent<Image> ().color.r, this.GetComponent<Image> ().color.g, this.GetComponent<Image> ().color.b, 100 / 255f);
		}
		else
			this.GetComponent<Image> ().color = new Color (this.GetComponent<Image> ().color.r, this.GetComponent<Image> ().color.g, this.GetComponent<Image> ().color.b, 0f);
	}
}
