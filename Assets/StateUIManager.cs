using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateUIManager : MonoBehaviour {

	public GameObject optionDialogGlass;        /*!< Dialog. */
	public GameObject optionDialogReagent;      /*!< Dialog. */
	public GameObject optionDialogEquipment;    /*!< Should only be linked when there's an equipment in the state */
	public WorkBench currentWorkbench;
	
	void Start () {
		if (gameObject.GetComponentInParent<WorkBench> () != null)
			currentWorkbench = gameObject.GetComponentInParent<WorkBench> ();
	}

	public void OpenOptionDialogEquipment(){
		optionDialogEquipment.SetActive(true);
		transform.GetComponentInParent<WorkBench>().BlockClicks();
	}

	public void CloseAll(){
		optionDialogGlass.SetActive(false);
		optionDialogReagent.SetActive(false);
		optionDialogEquipment.SetActive(false);
		transform.GetComponentInParent<WorkBench>().UnblockClicks();
	}

	public void OpenOptionDialog(ItemToInventory item){
		transform.GetComponentInParent<WorkBench>().BlockClicks();
		if (item is Glassware) {
			if(gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament!=null&&
			   gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament.childCount != 0 &&
			   item.gameObject.Equals(gameObject.GetComponentInParent<WorkBench> ().positionGlassEquipament.GetChild(0).gameObject)){

				List<int> ids = new List<int>();
				ids.Add (0);
				ids.Add(2);
				optionDialogGlass.GetComponent<OptionDialogBehaviour>().changeIDs(ids);
			}else{
				List<int> ids = new List<int>();
				ids.Add (0);
				ids.Add(1);
				optionDialogGlass.GetComponent<OptionDialogBehaviour>().changeIDs(ids);
			}
			optionDialogGlass.SetActive(true);
			optionDialogGlass.GetComponent<OptionDialogBehaviour>().setCurrentItem(item);

			return;
		}

		if (item is ReagentPot) {
			optionDialogReagent.SetActive(true);
			optionDialogReagent.GetComponent<OptionDialogBehaviour>().setCurrentItem(item);

			return;
		}
	}
}
