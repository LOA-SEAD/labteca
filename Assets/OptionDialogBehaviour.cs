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
