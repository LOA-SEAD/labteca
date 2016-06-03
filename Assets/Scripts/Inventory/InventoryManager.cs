using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

//! Manages all 'types' of Inventory Controller.
/*! It has information of all of the four types of InventoryController ( Solid, Liquid, Glassware and Others ). */
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour {

	private int count = 0;
	public Text selectedName;
	public int[] indexButtons = new int[]{1,2};
	public int listIndex;
	public GameController gameController;
	public GameObject itemPrefab;
	public GameObject actionTab;
	public GameObject selectedObject;
	public List<Button> listButton;
	public ArrayList ObjectList     = new ArrayList();
    // need to be set on the inspector with the correct game object containing each InventoryController
	public ArrayList Reagents  = new ArrayList();       /*!< InventoryController for Liquid and Solid Reagents. */
	public ArrayList Glassware      = new ArrayList();           /*!< InventoryController for Glassware. */
	public ArrayList Others         = new ArrayList();              /*!< InventoryController for Others. */

	public List<Sprite> backgroundInventory;
	public List<Sprite> backgroundPanel;
	public List<Sprite> backgroundIcons;
	public List<Sprite> backgroundButtons;
	public List<Sprite> backgroundAction;
	public List<Sprite> icons;
	private Sprite selectedIcon;

	public Image inventoryImage,tabImage,scrollbar;


	//private ArrayList SelectedList   = new ArrayList();

	private ItemType[] itemType = new ItemType[2];
	public RectTransform content;

    public ItemInventoryBase selectedItem = null;     /*!< Current selected item (game object). */
    private ItemStackableBehavior selectedUIItem = null;    /*!< Current selected item from inventory UI. */
	///-----------REFACTOR-------------------
	void Start(){
		refreshTab (false);
		itemType [0] = ItemType.Liquids;
		itemType [1] = ItemType.Solids;
		listIndex = 1;
		changeList(listIndex);
		selectedIcon = backgroundIcons [1];
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
		if (item.getItemType () == itemType[0]||item.getItemType () == itemType[1]) {
			GameObject aux = instantiateObject (auxItem);

			switch (item.getItemType ()) {
			case ItemType.Glassware:
				Glassware.Add (auxItem);
				break;
			case ItemType.Liquids:
			case ItemType.Solids:
				Reagents.Add (auxItem);
				break;
			case ItemType.Others:
				Others.Add (auxItem);
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
			case ItemType.Solids:
				Reagents.Add(auxItem);
				changeList(1);
				break;
			case ItemType.Others:
				Others.Add(auxItem);
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

		selectedObject = null;
		refreshActionButton ();
		count = 0;

		selectList (index);
		foreach (ItemInventoryBase item in getCurrentList()) {
			instantiateObject(item);
		}
		refreshGrid ();
	}

	public void changeImage(GameObject obj){
		ItemInventoryBase item = obj.GetComponent<ItemInventoryBase> ();
		Image icon = null;
		Image[] img = obj.GetComponentsInChildren<Image> ();
		for (int i = 0; i < img.Length; i++) {
			if(img[i].gameObject.name == "Item Image")
				icon = img[i];
		}

		Text txt = obj.GetComponentInChildren<Text> ();;
		switch (item.getItemType ()) {
		case ItemType.Liquids:
			icon.sprite = icons[0];
			txt.text = CompoundFactory.GetInstance().GetCompound(item.reagent).Formula;
			break;
		case ItemType.Solids:
			icon.sprite = icons[1];
			txt.text = CompoundFactory.GetInstance().GetCompound(item.reagent).Formula;
			break;
		case ItemType.Glassware:
			txt.text = "";
			if(item.gl.name.Contains("Balão Volumétrico")){
				icon.sprite = icons[2];
				break;
			}
			if(item.gl.name.Contains("Bequer")){
				icon.sprite = icons[5];
				break;
			}
			if(item.gl.name.Contains("Erlenmeyer")){
				if(item.gl.name.Contains("25")){
					icon.sprite = icons[6];
					break;
				}
				if(item.gl.name.Contains("50")){
					icon.sprite = icons[7];
					break;
				}
				if(item.gl.name.Contains("100")){
					icon.sprite = icons[8];
					break;
				}
			}
			break;
		}
	}

	public GameObject instantiateObject(ItemInventoryBase item){
		GameObject tempItem = Instantiate(itemPrefab) as GameObject;
		tempItem.transform.SetParent(content.transform,false);
		tempItem.name = "InventoryItem"+count.ToString();
		count++;
		tempItem.GetComponent<ItemInventoryBase>().copyData(item);
		ObjectList.Add(tempItem);
		changeImage (tempItem);
		tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => this.setSelectedItem(tempItem.GetComponent<ItemInventoryBase>()));

		if (item.getItemType() != ItemType.Glassware) {
			EventTrigger trigger = tempItem.gameObject.GetComponent<EventTrigger> ();
			EventTrigger.Entry enter = new EventTrigger.Entry ();
			enter.eventID = EventTriggerType.PointerEnter;
			enter.callback.AddListener ((eventData) => { 
				refreshSelectedName (item); 
				refreshTab (true); 
				actionTab.transform.position = tempItem.GetComponent<ItemInventoryBase> ().posTab.position;
			});
			trigger.delegates.Add (enter);

			EventTrigger.Entry exit = new EventTrigger.Entry ();
			exit.eventID = EventTriggerType.PointerExit;
			exit.callback.AddListener ((eventData) => { 
				refreshTab (false); 
			});
			trigger.delegates.Add (exit);
		}
		return tempItem;
	}

	public void selectList(int index){
		listIndex = index;
		switch (index) {
		case 0:
			itemType[0]=ItemType.Others;
			itemType[1]=ItemType.Others;
			break;
		case 1:
			itemType[0]=ItemType.Liquids;
			itemType[1]=ItemType.Solids;
			break;
		case 2:
			itemType[0]=ItemType.Glassware;
			itemType[1]=ItemType.Glassware;
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

	public void refreshTab(bool val){
		actionTab.SetActive (val);
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

		if (selectedObject == null) {
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
		if (item == selectedObject)
			selectedObject = null;
		Destroy (item);
		refreshButtons ();
		refreshGrid ();
	}

	public void AddGlasswareToInventory(Glassware gl) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.addGlassware(gl);
		item.HoldItem (gl);
		addItem(item);
	}

	public void AddReagentToInventory(ReagentPot reagent, Compound r ) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.HoldItem (reagent);
		item.addReagent(r);
		addItem(item);
	}

	public ArrayList getCurrentList(){
		switch (listIndex) {
		case 0:
			return Others;
		case 1:
			return Reagents;
		case 2:
			return Glassware;;
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

		refreshButtons ();

		refreshSelectedName (i);
	}

	public void refreshSelectedName (ItemInventoryBase i){
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
