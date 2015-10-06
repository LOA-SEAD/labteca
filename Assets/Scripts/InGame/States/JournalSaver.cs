using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves a Reaction and it's properties in a text file.

public class JournalSaver{

	private static TextEdit text = new TextEdit("Assets/Resources/journalItems.txt");

	public static void AddJournalUIItem(JournalUIItem journalItem){
		Dictionary<int, JournalUIItem> JournalItems = JournalSaver.LoadJournalUIItems ();
		if(JournalItems.ContainsKey(journalItem.index))
		   JournalItems.Remove(journalItem.index);
		JournalItems.Add (journalItem.index, journalItem);
		SaveJournalUIItems (JournalItems);
	}

	//Saves the reactions from a dictionary to the text file
	public static void SaveJournalUIItems(Dictionary<int, JournalUIItem> JournalItems)
	{
		text.ClearFile ();
		text.SetInt ("numberOfJournalItems", JournalItems.Count);
		
		int counter = 0;
		foreach (JournalUIItem JournalItem in JournalItems.Values)
		{
			text.SetInt("index" + counter.ToString(), counter);
			text.SetString("name" + counter.ToString(), JournalItem.name);
			text.SetBool("isDone" + counter.ToString(),JournalItem.isDone);
			text.SetInt("numberOfPrerequisites" + counter.ToString() , JournalItem.prerequisites.Length);
			for(int i = 0; i < JournalItem.prerequisites.Length;i++){
				Debug.Log("indexPrerequisiteOf" + counter.ToString()+"_"+i.ToString()+"= "+JournalItem.prerequisites[i].index);
				text.SetInt("indexPrerequisiteOf" + counter.ToString()+"_"+i.ToString(), JournalItem.prerequisites[i].index);
			}
			counter++;
		}
	}
	//Loads the reactions from a file, and returns a dictionary
	public static Dictionary<int, JournalUIItem> LoadJournalUIItems()
	{
		TextAsset loadText = Resources.Load("journalItems") as TextAsset;
		
		TextEdit textLoad = new TextEdit(loadText);
		
		int numberOfJournalItems = text.GetInt ("numberOfJournalItems");
		
		Dictionary<int, JournalUIItem> journalUIItems = new Dictionary<int, JournalUIItem>();
		
		if (numberOfJournalItems > 0) 
		{
			for (int i = 0; i < numberOfJournalItems; i++) 
			{
				JournalUIItem journalItem = new JournalUIItem();
				
				journalItem.index = textLoad.GetInt("index" + i.ToString ());
				
				journalItem.name = textLoad.GetString("name" + i.ToString ());
				journalItem.isDone = textLoad.GetBool("isDone" + i.ToString ());
				journalItem.prerequisites = new JournalUIItem[textLoad.GetInt("numberOfPrerequisites" + i.ToString ())];
				for(int n = 0;n < journalItem.prerequisites.Length; n++){
					int indexOfPre=textLoad.GetInt("indexPrerequisiteOf"+i.ToString()+"_"+n.ToString());
					JournalUIItem preReq;
					journalUIItems.TryGetValue(indexOfPre,out preReq);
					journalItem.prerequisites[n]=preReq;
				}
				
				journalUIItems.Add(journalItem.index, journalItem);
			}
		}
		return journalUIItems;
	}
}
