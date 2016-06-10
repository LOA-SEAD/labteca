using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptionDialogBehaviour : MonoBehaviour {
	public List<int> actualIDs;
	public List<Sprite> optionsSprites;
	public List<Image> optionsImages;
	public List<GameObject> prefabs;
	public ItemToInventory item;

	void Start(){
		changeIDs (actualIDs);
		if (gameObject.name == "OptionDialogGlass"){
			Glassware[] glassList = GameObject.FindObjectOfType<GetGlasswareState> ().glasswareList;
			foreach (Glassware g in glassList){
				prefabs.Add(g.gameObject);
			}
		}
	}

	public void changeIDs(List<int> newIDs){
		int index = 0;
		foreach (int id in newIDs) {
			actualIDs[index] = id;
			optionsImages[index].sprite = optionsSprites[id];
			index++;
		}
	}

	public void setCurrentItem(ItemToInventory i){
		this.item = i;
	}

	public void onClick(int index){
		if(item is Glassware){
			switch (actualIDs[index]) {
			case 0:
				if(!item.GetComponent<Glassware>().hasReagents()){
					GameObject temp = null;
					for(int i = 0; i < prefabs.Count; i ++){
						Debug.Log(prefabs[i].GetComponent<Glassware>().name);
						if(((item as Glassware).name.Contains(prefabs[i].GetComponent<Glassware>().name)))
							temp = prefabs[i];
					}
					GameObject.Find("InventoryManager").GetComponent<InventoryManager>().AddGlasswareToInventory(temp.GetComponent<Glassware>());
					Destroy(item.gameObject);
				}else{}//TODO: fazer para quando tiver reagente
				break;
			case 1:
				GetComponentInParent<WorkBench>().PutGlassInEquip(item.gameObject);
				break;
			case 2:
				GetComponentInParent<WorkBench>().PutItemFromEquip(item.gameObject);
				break;
			}
		}

		if(item is ReagentPot){
			switch (actualIDs[index]) {
			case 0:
				ItemInventoryBase reagent = new ItemInventoryBase();
				Compound r = (item as ReagentPot).reagent.Clone() as Compound;
				reagent.addReagent(r);
				int prefabIndex = reagent.getItemType()==ItemType.Liquids?0:1;
				GameObject.Find("InventoryManager").GetComponent<InventoryManager>().AddReagentToInventory(prefabs[prefabIndex].GetComponent<ReagentPot>(),r);

				Destroy(item.gameObject);
				break;
			}
		}

		GetComponentInParent<StateUIManager> ().CloseAll ();
	}	
}
