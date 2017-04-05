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
	private StateUIManager ui_manager;

	public Button editLabelButton; //TODO:refactor later. Using for enabling/disabling on StateUIManager

	void Start(){
		changeIDs (actualIDs);
		if (gameObject.name == "OptionDialogGlass"){
			Glassware[] glassList = GameObject.FindObjectOfType<GetGlasswareState> ().glasswareList;
			foreach (Glassware g in glassList){
				prefabs.Add(g.gameObject);
			}
		}
		ui_manager = GetComponentInParent<StateUIManager> ();
	}

	public void changeIDs(List<int> newIDs){
		int index = 0;
		foreach (int id in newIDs) {
			if(index < actualIDs.Count) { 
				actualIDs [index] = id;
				optionsImages [index].sprite = optionsSprites [id];
				index++;
			}
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
						if(((item as Glassware).gl.Contains(prefabs[i].GetComponent<Glassware>().name)))
							temp = prefabs[i];
					}
					GameObject.Find("InventoryManager").GetComponent<InventoryManager>().AddGlasswareToInventory(temp.GetComponent<Glassware>());
					Destroy(item.gameObject);
				}else{
					if(GetComponentInParent<WorkBench>().equipmentController!=null && item.transform.parent == GetComponentInParent<WorkBench>().positionGlassEquipament)
						GetComponentInParent<WorkBench>().equipmentController.RemoveObjectInEquipament(item.gameObject);
					GameObject.Find("InventoryManager").GetComponent<InventoryManager>().AddProductToInventory(item.gameObject);
				}
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

		ui_manager.CloseAll ();
	}

	public void onClickLabelButton() {
		ui_manager.CloseAll ();
		ui_manager.glasswareLabelEditor.SetActive (true);
		GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (true);
		GetComponentInParent<WorkBench> ().writingLabel = true;
		ui_manager.glasswareLabelEditor.GetComponentInChildren<InputField> ().text = item.GetComponent<Glassware> ().label;
	}

	//! Saves the label name
	public void SaveLabelName() {
		item.GetComponent<Glassware>().label = ui_manager.glasswareLabelEditor.GetComponentInChildren<InputField> ().text;
		GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (false);
		GetComponentInParent<WorkBench> ().writingLabel = false;
		ui_manager.CloseAll ();
	}

	public void onClickPrepareTurbidimeter() {
		(GetComponentInParent<WorkBench> ().equipmentController as TurbidimeterController).PrepareGlassware (item as Glassware);
		//gameObject.SetActive (false);
		ui_manager.CloseAll ();
	}

	/// <summary>
	/// Method to control the interaction with the bucket interaction canvas
	/// </summary>
	/// <param name="button">Button that was pressed.</param>
	public void onClickBucket(int button) {
		if (button == 0) { 
			if ((GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetEquipmentController() as TurbidimeterController).GetGlassInEquipment() != null) { //Colocar na bancada
				(GetComponentInParent<WorkBench>().equipmentController as TurbidimeterController).PutBackBucket();
			} else { //Colocar no equipamento
				GetComponentInParent<WorkBench>().PutGlassInEquip(item.gameObject);
			}
		} else if (button == 1) { //Retornar soluçao a vidraria
			(GetComponentInParent<WorkBench> ().equipmentController as TurbidimeterController).GiveBackReagent ();
		}
		gameObject.SetActive (false);
	}
}
