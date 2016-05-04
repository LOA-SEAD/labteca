using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

//! Manages all 'types' of Inventory Controller.
/*! It has information of all of the four types of InventoryController ( Solid, Liquid, Glassware and Others ). */
public class InventoryManager : MonoBehaviour {

	private int count = 0;
	public Text selectedName;
	public int[] indexButtons = new int[]{1,2};
	public int listIndex;
	public GameController gameController;
	public GameObject itemPrefab;
	public GameObject selectedObject;
	public List<Button> listButton;
	public ArrayList ObjectList     = new ArrayList();
    // need to be set on the inspector with the correct game object containing each InventoryController
	public ArrayList SolidReagents  = new ArrayList();       /*!< InventoryController for Solid Reagents. */
	public ArrayList LiquidReagents = new ArrayList();      /*!< InventoryController for Liquid Reagents. */
	public ArrayList Glassware      = new ArrayList();           /*!< InventoryController for Glassware. */
	public ArrayList Others         = new ArrayList();              /*!< InventoryController for Others. */

	public List<Sprite> backgroundInventory;
	public List<Sprite> backgroundPanel;
	public List<Sprite> backgroundIcons;
	public List<Sprite> backgroundButtons;
	public List<Sprite> backgroundAction;
	private Sprite selectedIcon;

	public Image inventoryImage,tabImage,scrollbar;


	//private ArrayList SelectedList   = new ArrayList();

	private ItemType itemType;
	public RectTransform content;

    public ItemInventoryBase selectedItem = null;     /*!< Current selected item (game object). */
    private ItemStackableBehavior selectedUIItem = null;    /*!< Current selected item from inventory UI. */
	///-----------REFACTOR-------------------
	void Start(){
		listIndex = 0;
		changeList(listIndex);
		selectedIcon = backgroundIcons [0];
		refreshGrid ();
	}

	public GameObject addItem(ItemInventoryBase item){
		ItemInventoryBase auxItem = new ItemInventoryBase ();
		auxItem.copyData (item);

		if (auxItem.index == null) {
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 6; i++) {
				char aux = (char)Random.Range (65, 126);
				sb.Append(aux);
			}
			auxItem.index=sb.ToString();
		}
		if (item.getItemType () == itemType) {
			GameObject aux = instantiateObject (auxItem);

			switch (item.getItemType ()) {
			case ItemType.Glassware:
				Glassware.Add (auxItem);
				break;
			case ItemType.Liquids:
				LiquidReagents.Add (auxItem);
				break;
			case ItemType.Others:
				Others.Add (auxItem);
				break;
			case ItemType.Solids:
				SolidReagents.Add (auxItem);
				break;
			}
			refreshGrid ();

			return aux;

		} else {
			switch (item.getItemType ()) {
			case ItemType.Glassware:
				Glassware.Add (auxItem);
				changeList(2);
				break;
			case ItemType.Liquids:
				LiquidReagents.Add(auxItem);
				changeList(1);
				break;
			case ItemType.Others:
				Others.Add(auxItem);
				break;
			case ItemType.Solids:
				SolidReagents.Add(auxItem);
				changeList(0);
				break;
			}
			return null;
		}
	}

	public void changeList(int index){

		selectedItem = null;

		for (int i = ObjectList.Count -1; i>=0; i--) {
			ObjectList.Remove (ObjectList[i]);
			Destroy(content.GetChild(i).gameObject);
		}

		count = 0;

		selectList (index);
		foreach (ItemInventoryBase item in getCurrentList()) {
			instantiateObject(item);
		}
		refreshGrid ();
	}

	public GameObject instantiateObject(ItemInventoryBase item){
		GameObject tempItem = Instantiate(itemPrefab) as GameObject;
		tempItem.transform.SetParent(content.transform,false);
		tempItem.name = "InventoryItem"+count.ToString();
		count++;
		tempItem.GetComponent<ItemInventoryBase>().copyData(item);
		ObjectList.Add(tempItem);

		tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => this.setSelectedItem(tempItem.GetComponent<ItemInventoryBase>()));

		return tempItem;
	}

	public void selectList(int index){
		listIndex = index;
		switch (index) {
		case 0:
			//SelectedList=SolidReagents;
			itemType=ItemType.Solids;
			break;
		case 1:
			//SelectedList=LiquidReagents;
			itemType=ItemType.Liquids;
			break;
		case 2:
			//SelectedList=Glassware;
			itemType=ItemType.Glassware;
			break;
		case 3:
			//SelectedList=Others;
			itemType=ItemType.Others;
			break;
		}
		inventoryImage.sprite = backgroundInventory [index];
		tabImage.sprite = backgroundPanel [index];
		selectedIcon = backgroundIcons [index];
		refreshButtons ();
	}

	public void refreshButtons(){
		int aux = 0;
		for(int i = 0; i < 3; i++){
			if(i!=listIndex){
				indexButtons[aux]=i;
				listButton[aux++].GetComponent<Image>().sprite = backgroundButtons[i];
			}
		}
		refreshActionButton ();
	}

	public void auxSideButton(int i){
		changeList (indexButtons [i]);
	}

	public void refreshActionButton(){
		GameStateBase currentState = gameController.GetCurrentState ();
		if (currentState != null) {
			listButton[2].interactable = true;
			if (currentState.GetComponent<InGameState> () == null) {
				if ((currentState.GetComponent<GetGlasswareState> () != null && listIndex == 2) || (currentState.GetComponent<GetReagentState> () != null && (listIndex == 1 || listIndex == 0)))
					listButton [2].GetComponent<Image> ().sprite = backgroundAction [2];
				else {
					if (currentState.GetComponent<WorkBench> () != null){
						listButton [2].GetComponent<Image> ().sprite = backgroundAction [1];
					}
					else{
						listButton[2].interactable = false;
						listButton [2].GetComponent<Image> ().sprite = backgroundAction [0];
					}
				}
			} else {
				listButton[2].interactable = false;
				listButton [2].GetComponent<Image> ().sprite = backgroundAction [0];
			}
		} else {
			listButton[2].interactable = false;
			listButton [2].GetComponent<Image> ().sprite = backgroundAction [0];
		}
	}

	public void refreshGrid(){
		content.sizeDelta = new Vector2(0f , (Mathf.Ceil (getCurrentList().Count / 3f)) * 90 - 10f);
		int n = 0;
		int top = (int)content.sizeDelta.y / 2 - 40;
		foreach (GameObject obj in ObjectList) {
			int posX = 40 + 100*(n%3);
			int posY = top-(n/3)*90;
			obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX,posY);
			n++;
		}
		if (n > 12) {
			scrollbar.color = new Color (1, 1, 1, 1);
		} else {
			scrollbar.color = new Color (1, 1, 1, 0);
		}
	}

	public void removeItem(GameObject item){
		string index = item.GetComponent<ItemInventoryBase> ().index;
		Debug.Log (index);
		ObjectList.Remove (item);

		int n = 0;

		foreach (ItemInventoryBase it in getCurrentList()) {
			Debug.Log (it.index);
			if (it.index == index) {
				getCurrentList().RemoveAt(n);
				foreach (ItemInventoryBase db in getCurrentList()){
					Debug.Log("DEBUG: "+db.index);
				}
				break;
			}
			n++;
		}

		Destroy (item);
		refreshGrid ();
	}

	public void AddGlasswareToInventory(Glassware gl) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.addGlassware(gl);
		item.HoldItem (gl);
		addItem(item);
	}

	public void AddReagentToInventory(ReagentsBaseClass reagent, ReagentsBaseClass r ) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.HoldItem (reagent);
		item.addReagent(r);
		addItem(item);
	}

	public ArrayList getCurrentList(){
		switch (listIndex) {
		case 0:
			return SolidReagents;
		case 1:
			return LiquidReagents;
		case 2:
			return Glassware;;
		case 3:
			return Others;
		}
		return null;
	}

	public void setSelectedItem(ItemInventoryBase i)
	{
		if (selectedObject != null) {
			selectedObject.GetComponentInChildren<Image> ().sprite = backgroundIcons [3];
		}
		this.selectedItem = i;
		selectedObject = GameObject.Find (selectedItem.gameObject.name);
		selectedObject.GetComponentInChildren<Image>().sprite = selectedIcon;
		switch (i.getItemType ()) {
			case ItemType.Glassware:
				selectedName.text = i.gl.name;
				break;
			case ItemType.Liquids:
			case ItemType.Solids:
				selectedName.text = i.reagent;
				break;
		}

	}

	public void actionButtonClick(){
		if (selectedItem != null) {
			ItemInventoryBase item = new ItemInventoryBase();
			item = selectedItem;
			GameStateBase currentState = gameController.GetCurrentState ();
			Debug.Log (currentState);
			if (currentState.GetComponent<WorkBench> () != null)
				CallWorkbenchToTable (item);
			else {
				removeItem (GameObject.Find (selectedItem.gameObject.name));
			}
		}
	}

	public void CallWorkbenchToTable(ItemInventoryBase item) {
		//GameObject tempItem = Instantiate (itemBeingHeld.gameObject) as GameObject;
		//gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (tempItem);
		gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (item.itemBeingHeld,item.reagent);
	}
}
