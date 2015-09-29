using UnityEngine;
using System.Collections;

public class JournalUIItem : MonoBehaviour {
	
	public bool isDone;
	public JournalUIItem[] prerequisites;

	public void checkItem(){
		if (!isDone && checkPrerequisites ()){
			isDone=true;
		}
	}
	
	private bool checkPrerequisites(){
		bool allDone=true;
		for (int i=0; i < prerequisites.Length&&allDone; i++) {
			allDone = prerequisites[i].isDone;
		}
		return allDone;
	}
}
