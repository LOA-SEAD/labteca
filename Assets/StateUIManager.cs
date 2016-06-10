using UnityEngine;
using System.Collections;

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
	}

	public void CloseAll(){
		optionDialogGlass.SetActive(false);
		optionDialogReagent.SetActive(false);
		optionDialogEquipment.SetActive(false);
	}

	public void OpenOptionDialog(ItemToInventory item){
		if (item is Glassware) {
			optionDialogGlass.SetActive(true);

			return;
		}

		if (item is ReagentPot) {
			optionDialogReagent.SetActive(true);
			optionDialogReagent.GetComponent<OptionDialogBehaviour>().setCurrentItem(item);

			return;
		}
	}
}
