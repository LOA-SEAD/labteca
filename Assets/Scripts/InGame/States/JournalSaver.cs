using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves a Reaction and it's properties in a text file.
/// <summary>
/// Controls I/O of journal items (phase step descriptions)
/// </summary>
public class JournalSaver : MonoBehaviour {

	private static int numberOfJournals;
	private static JSONEditor json;

	/*public static void AddJournalUIItem(JournalUIItem journalItem,int expo){
		Dictionary<int, JournalUIItem> JournalItems = JournalSaver.LoadJournalUIItems (expo);
		if(JournalItems.ContainsKey(journalItem.index))
		   JournalItems.Remove(journalItem.index);
		JournalItems.Add (journalItem.index
		                  , journalItem);
		//SaveJournalUIItems (JournalItems,expo);
	}*/

	//Saves the reactions from a dictionary to the text file
	/*public static void SaveJournalUIItems(Dictionary<int, JournalUIItem> JournalItems,int expo)
	{
		text = new TextEdit("Assets/Resources/journalItems"+expo.ToString()+".txt");
		text.ClearFile ();
		text.SetInt ("numberOfJournalItems", JournalItems.Count);
		
		int counter = 0;
		foreach (JournalUIItem JournalItem in JournalItems.Values)
		{
			text.SetInt("index" + counter.ToString(), counter);
			text.SetString("name" + counter.ToString(), JournalItem.name);
			text.SetBool("isDone" + counter.ToString(),JournalItem.isDone);
			text.SetInt("numberOfPrerequisites" + counter.ToString() , JournalItem.prerequisites.Count);
			for(int i = 0; i < JournalItem.prerequisites.Count;i++){
				Debug.Log("indexPrerequisiteOf" + counter.ToString()+"_"+i.ToString()+"= "+JournalItem.prerequisites[i]);
				text.SetInt("indexPrerequisiteOf" + counter.ToString()+"_"+i.ToString(), JournalItem.prerequisites[i]);
			}
			counter++;
		}
	}*/
	//Loads the reactions from a file, and returns a dictionary

	public static Dictionary<int, JournalUIItem> LoadJournalUIItems(int expo)
	{
		json = new JSONEditor("journalItems"+expo.ToString());
		
		int numberOfJournalItems = json.NumberOfFields (0);
		
		Dictionary<int, JournalUIItem> journalUIItems = new Dictionary<int, JournalUIItem>();
		
		if (numberOfJournalItems > 0) 
		{
			for (int i = 0; i < numberOfJournalItems; i++) 
			{
				JournalUIItem journalItem = new JournalUIItem();
				
				journalItem.index = json.GetInt(i, "index");
				
				journalItem.name = json.GetString(i, "name");
				journalItem.isDone = json.GetBool(i, "isDone");
				journalItem.prerequisites.Clear();
				for(int n = 0;n < json.GetInt(i, "numberOfPrerequisites"); n++){
					int indexOfPre = int.Parse(json.GetSubValue(i, "indexPrerequisiteOf", n));
					journalItem.prerequisites.Add(indexOfPre);
				}
				journalItem.prerequisitesDone=false;

				if(!journalUIItems.ContainsKey(journalItem.index))
					journalUIItems.Add(journalItem.index, journalItem);	
			}
		}
		return journalUIItems;
	}
	public static string GetExperimentName(int experimentNumber) {
		json = new JSONEditor("journalItems"+experimentNumber.ToString());
		return json.GetMainValue ("name");
	}
}
