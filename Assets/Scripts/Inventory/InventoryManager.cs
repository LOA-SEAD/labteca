using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

//! Manages all 'types' of Inventory Controller.
/*! It has information of all of the four types of InventoryController ( Solid, Liquid, Glassware and Others ). */
// Buttons are define as: 0-Products, 1-Reagents, 2-Glassware
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour {

	private int count = 0;
	public List<Text> tabValues;
	public int[] indexButtons = {1,2,3};
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
	public ArrayList Products         = new ArrayList();              /*!< InventoryController for Others. */

	public List<Sprite> backgroundInventory;
	public List<Sprite> backgroundPanel;
	public List<Sprite> backgroundIcons;
	public List<Sprite> backgroundButtons;
	public List<Sprite> backgroundAction;
	public List<Sprite> icons;
	public List<Sprite> productsIcons;
	private Sprite selectedIcon;

	public Image inventoryImage,tabImage,scrollbar;

	private ItemType[] itemType = new ItemType[2];
	public RectTransform content;
	public Transform limbo;

    public ItemInventoryBase selectedItem = null;     /*!< Current selected item (game object). */
    private ItemStackableBehavior selectedUIItem = null;    /*!< Current selected item from inventory UI. */
	private List<string> listOfIndexes = new List<string>();

	private RectTransform labelRect;
	private string hoverName;

	void Start(){
		refreshTab (null,false);
		itemType [0] = ItemType.Liquids;
		itemType [1] = ItemType.Solids;
		listIndex = 0;
		changeList(listIndex);
		selectedIcon = backgroundIcons [1];
		refreshGrid ();

		labelRect = GameObject.Find ("GameController").GetComponent<HUDController>().hover;
	}
	/// <summary>
	/// Generates the index for an ItemInventoryBase.
	/// </summary>
	/// <returns>The index generated.</returns>
	/// <param name="item">Item to generate index.</param>
	public string GenerateIndex(ref ItemInventoryBase item){
		if (item.index == null) {
			StringBuilder sb;
			do{
				sb = new StringBuilder();
				for (int i = 0; i < 6; i++) {
					char aux = (char)Random.Range (65, 126);
					sb.Append(aux);
				}
			}while(listOfIndexes.Contains(sb.ToString()));
			item.index=sb.ToString();
			listOfIndexes.Add(sb.ToString());
		}

		return item.index;
	}
	/// <summary>
	/// Adds the item to the inventory.
	/// </summary>
	/// <returns>The item or null .</returns>
	/// <param name="item">Item.</param>
	public GameObject addItem(ref ItemInventoryBase item){
		GenerateIndex (ref item);
		//If the item added is of current list type, adds it to the grid
		if (item.getItemType () == itemType[0]||item.getItemType () == itemType[1]) {
			GameObject aux = instantiateObject (item);

			switch (item.getItemType ()) {
			case ItemType.Glassware:
				Glassware.Add (item);
				break;
			case ItemType.Liquids:
			case ItemType.Solids:
				Reagents.Add (item);
				break;
			case ItemType.Others:
				Products.Add (item);
				break;
			}
			refreshGrid ();

			return aux;
		//else, adds it to the List directly and changes the list
		} else {
			switch (item.getItemType ()) {
			case ItemType.Glassware:
				Glassware.Add (item);
				changeList(2);
				break;
			case ItemType.Liquids:
			case ItemType.Solids:
				Reagents.Add(item);
				changeList(1);
				break;
			case ItemType.Others:
				Products.Add(item);
				changeList(0);
				break;
			}
			return null;
		}
	}
	/// <summary>
	/// Changes the list to the index sent.
	/// </summary>
	/// <param name="index">Index.</param>
	public void changeList(int index){
		selectedItem = null;
		//cleans the grid
		for (int i = ObjectList.Count -1; i>=0; i--) {
			ObjectList.Remove (ObjectList[i]);
			Destroy(content.GetChild(i).gameObject);
		}

		selectedObject = null;
		refreshActionButton ();
		count = 0;
		//changes the current list and instantiantes the objects from the list
		selectList (index);
		foreach (ItemInventoryBase item in getCurrentList()) {
			instantiateObject(item);
		}
		refreshGrid ();
	}
	/// <summary>
	/// Changes the background image of the selected item.
	/// </summary>
	/// <param name="obj">GameObject of the selected item.</param>
	public void changeImage(GameObject obj){
		ItemInventoryBase item = obj.GetComponent<ItemInventoryBase> ();
		Image icon = null;
		Image[] img = obj.GetComponentsInChildren<Image> ();
		//from images on obj, gets the Item Image
		for (int i = 0; i < img.Length; i++) {
			if(img[i].gameObject.name == "Item Image")
				icon = img[i];
		}

		Text txt = obj.GetComponentInChildren<Text> ();
		//chooses image depending on item
		//if reagent, sets underneath text to obj formula
		switch (item.getItemType ()) {
		case ItemType.Liquids:
			icon.sprite = icons[0];
			txt.text = CompoundFactory.GetInstance().GetCupboardCompound(item.reagent).Formula;
			break;
		case ItemType.Solids:
			icon.sprite = icons[1];
			txt.text = CompoundFactory.GetInstance().GetCupboardCompound(item.reagent).Formula;
			break;
		case ItemType.Glassware:
			txt.text = "";
			if(item.gl.name.Contains("Balão Vol.")){
				if(item.gl.name.Contains("25")){
					icon.sprite = icons[2];
					break;
				}
				if(item.gl.name.Contains("50")){
					icon.sprite = icons[3];
					break;
				}
				if(item.gl.name.Contains("100")){
					icon.sprite = icons[4];
					break;
				}
			}
			if(item.gl.name.Contains("Béquer")){
				icon.sprite = icons[5];
				break;
			}
			if(item.gl.name.Contains("Erlenmeyer")){
				if(item.gl.name.Contains(" 50ml")){
					icon.sprite = icons[6];
					break;
				}
				if(item.gl.name.Contains("125")){
					icon.sprite = icons[7];
					break;
				}
				if(item.gl.name.Contains("250")){
					icon.sprite = icons[8];
					break;
				}
			}
			break;
		case ItemType.Others:
			txt.text = "";
			string gl = GameObject.Find(item.index).GetComponent<Glassware>().gl;
			if(gl.Contains("Balão Vol.")){
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasSolid){
					item.solid.sprite = productsIcons[0];
					item.solid.enabled = true;
				}
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasLiquid){
					item.liquid.sprite = productsIcons[1];
					item.liquid.enabled = true;
				}

				if(gl.Contains("25")){
					icon.sprite = icons[2];
					break;
				}
				if(gl.Contains("50")){
					icon.sprite = icons[3];
					break;
				}
				if(gl.Contains("100")){
					icon.sprite = icons[4];
					break;
				}
			}
			if(gl.Contains("Béquer")){
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasSolid){
					item.solid.sprite = productsIcons[2];
					item.solid.enabled = true;
				}
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasLiquid){
					item.liquid.sprite = productsIcons[3];
					item.liquid.enabled = true;
				}
				icon.sprite = icons[5];
				break;
			}
			if(gl.Contains("Erlenmeyer")){
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasSolid){
					item.solid.sprite = productsIcons[4];
					item.solid.enabled = true;
				}
				if(GameObject.Find(item.index).GetComponent<Glassware>().hasLiquid){
					item.liquid.sprite = productsIcons[5];
					item.liquid.enabled = true;
				}
				if(gl.Contains(" 50ml")){
					icon.sprite = icons[6];
					break;
				}
				if(gl.Contains("125")){
					icon.sprite = icons[7];
					break;
				}
				if(gl.Contains("250")){
					icon.sprite = icons[8];
					break;
				}
			}
			break;
		}
	}
	/// <summary>
	/// Instantiates the item on the grid.
	/// </summary>
	/// <returns>The GameObject.</returns>
	/// <param name="item">The Item to instantiate.</param>
	public GameObject instantiateObject(ItemInventoryBase item){
		GameObject tempItem = Instantiate(itemPrefab) as GameObject;
		tempItem.transform.SetParent(content.transform,false); //adds item as child of the grid
		tempItem.name = "InventoryItem"+count.ToString();
		count++;
		//copies the data from item to the ItemInventoryBase component
		tempItem.GetComponent<ItemInventoryBase>().copyData(item);
		ObjectList.Add(tempItem);
		changeImage (tempItem);
		tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => this.setSelectedItem(tempItem.GetComponent<ItemInventoryBase>())); //adds click listener on runtime

		if (item.getItemType() != ItemType.Glassware) {
			if(item.getItemType()==ItemType.Others){
				//TODO: Implement the triggers for products
				EventTrigger trigger = tempItem.gameObject.GetComponent<EventTrigger> ();
				EventTrigger.Entry enter = new EventTrigger.Entry ();
				enter.eventID = EventTriggerType.PointerEnter;
				//trigger for entering
				enter.callback.AddListener ((eventData) => {
					if (limbo.Find(item.index).GetComponent<Glassware>() != null) {
						if (limbo.Find (item.index).GetComponent<Glassware> ().label.Length != 0) {
							labelRect.gameObject.SetActive (true);
							labelRect.rotation = Quaternion.Euler (0f, 0f, 0f);
							labelRect.GetComponentInChildren<Text> ().rectTransform.localRotation = Quaternion.Euler (0f, 0f, 0f);
							labelRect.GetComponentInChildren<Text> ().text = limbo.Find (item.index).GetComponent<Glassware> ().label;
							labelRect.transform.position = new Vector2(tempItem.GetComponent<ItemInventoryBase> ().posTab.position.x,
							                                           tempItem.GetComponent<ItemInventoryBase> ().posTab.position.y - 50.0f);
						}
					}
					/*refreshLabel (item,true); 
					glasswareLabel.transform.position = tempItem.GetComponent<ItemInventoryBase> ().posTab.position;*/
				});
				trigger.triggers.Add (enter);
				//trigger for leaving
				EventTrigger.Entry exit = new EventTrigger.Entry ();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener ((eventData) => { 
					//refreshLabel (null,false);
					labelRect.gameObject.SetActive (false);
				});
				trigger.triggers.Add (exit);
			}
			else{
				//if reagent, adds triggers for info tab
				EventTrigger trigger = tempItem.gameObject.GetComponent<EventTrigger> ();
				EventTrigger.Entry enter = new EventTrigger.Entry ();
				enter.eventID = EventTriggerType.PointerEnter;
				//trigger for entering
				enter.callback.AddListener ((eventData) => { 
					refreshTab (item,true); 
					actionTab.transform.position = tempItem.GetComponent<ItemInventoryBase> ().posTab.position;
				});
				trigger.triggers.Add (enter);
				//trigger for leaving
				EventTrigger.Entry exit = new EventTrigger.Entry ();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener ((eventData) => { 
					refreshTab (null,false); 
				});
				trigger.triggers.Add (exit);
			}
		}
		//returns the instantiated object
		return tempItem;
	}
	/// <summary>
	/// Selects the list, changing the sprite to the respective colour.
	/// </summary>
	/// <param name="index">Index.</param>
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
		//changes sprites to respective colors
		inventoryImage.sprite = backgroundInventory [index];
		tabImage.sprite = backgroundPanel [index];
		selectedIcon = backgroundIcons [index];
		refreshButtons ();
	}
	/// <summary>
	/// Refreshs side buttons.
	/// </summary>
	public void refreshButtons(){
		//To swap the buttons
		/*int aux = 0;
		for(int i = 0; i < 3; i++){
			//if the index isn't the current index, adds it to side buttons
			if(i!=listIndex){
				indexButtons[aux]=i;
				listButton[aux++].GetComponent<Image>().sprite = backgroundButtons[i];
			}
		}*/

		/*	Changin colors
		 * for (int i = 0; i < 3; i++) {
			if(i == listIndex) {
				//Activate using sprite
				listButton[i].image.color = Color.grey;
			}
			else {
				listButton[i].image.color = Color.white;
			}
		}*/

		//Swapping sprites
		for (int i = 0; i < 3; i++) {
			if(i == listIndex) {
				//Activate using sprite
				listButton[i].interactable = false; //listButton[i].spriteState.highlightedSprite;

			}
			else {
				//listButton[i].image.;
				listButton[i].interactable = true;
			}
		}

		refreshActionButton ();
	}
	/// <summary>
	/// Refreshs the tab with Item info.
	/// </summary>
	/// <param name="i">ItemInventoryBase to get info from.</param>
	/// <param name="val">If set to <c>true</c>, activates tab. Else, deactivates.</param>
	public void refreshTab(ItemInventoryBase i,bool val){
		if (!val) {
			actionTab.SetActive (false);
			return;
		}
		//gets info from "i"
		switch (i.getItemType ()) {
		case ItemType.Glassware:
			tabValues[0].text = i.gl.name;
			break;
		case ItemType.Liquids:
		case ItemType.Solids:
			tabValues[0].text = i.reagent;
			tabValues[1].text = CompoundFactory.GetInstance().GetCupboardCompound(i.reagent).MolarMass + " g/mol";
			tabValues[2].text = CompoundFactory.GetInstance().GetCupboardCompound(i.reagent).Density+ " g/ml";
			tabValues[3].text = CompoundFactory.GetInstance().GetCupboardCompound(i.reagent).Purity*100+ "%";
			tabValues[4].text = CompoundFactory.GetInstance().GetCupboardCompound(i.reagent).Solubility+ " g/1g";
			break;
		}

		actionTab.SetActive (true);

	}
	/// <summary>
	/// Aux method to click side button
	/// </summary>
	/// <param name="i">The index of index(indexception) of clicked button.</param>
	public void auxSideButton(int i){
		changeList (indexButtons [i]);
	}
	/// <summary>
	/// Refreshs the action button.
	/// </summary>
	public void refreshActionButton(){
		GameStateBase currentState = gameController.GetCurrentState ();
		//refreshs action button depending on the current state and list index
		if (currentState != null) {
			listButton[3].interactable = true;
			if (currentState.GetComponent<InGameState> () == null) {
				if ((currentState.GetComponent<GetGlasswareState> () != null && listIndex == 2) 
				    || (currentState.GetComponent<GetReagentState> () != null && listIndex == 1))
						listButton [3].GetComponent<Image> ().sprite = backgroundAction [2];
				else {
					if (currentState.GetComponent<WorkBench> () != null || 
					   (currentState.GetComponent<LIAState> () != null && listIndex == 0)){
						listButton [3].GetComponent<Image> ().sprite = backgroundAction [1];
					}
					else{
						listButton[3].interactable = false;
						listButton [3].GetComponent<Image> ().sprite = backgroundAction [0];
					}
				}
			} else {
				listButton[3].interactable = false;
				listButton [3].GetComponent<Image> ().sprite = backgroundAction [0];
			}
		} else {
			listButton[3].interactable = false;
			listButton [3].GetComponent<Image> ().sprite = backgroundAction [0];
		}

		if (selectedObject == null) {
			listButton[3].interactable = false;
			listButton [3].GetComponent<Image> ().sprite = backgroundAction [0];
		}
	}
	/// <summary>
	/// Refreshs the grid.
	/// </summary>
	public void refreshGrid(){
		//calculates grid size based on number of items in list
		content.sizeDelta = new Vector2(0f , (Mathf.Ceil (getCurrentList().Count / 3f)) * 90 - 10f);
		int n = 0;
		int top = (int)content.sizeDelta.y / 2 - 40;
		//places instantiated objects in place
		foreach (GameObject obj in ObjectList) {
			int posX = 40 + 100*(n%3);
			int posY = top-(n/3)*90;
			obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX,posY);
			n++;
		}
		//if number of items is greater than 12, sets scrollbar visible
		if (n > 12) {
			scrollbar.color = new Color (1, 1, 1, 1);
		} else {
			scrollbar.color = new Color (1, 1, 1, 0);
		}
	}
	/// <summary>
	/// Removes the item from grid and list.
	/// </summary>
	/// <param name="item">Item to be removed.</param>
	public void removeItem(GameObject item){
		//gets item index and removes from index list and object list
		string index = item.GetComponent<ItemInventoryBase> ().index;
		listOfIndexes.Remove (index);
		ObjectList.Remove (item);

		int n = 0;
		//search for item index in list
		foreach (ItemInventoryBase it in getCurrentList()) {
			if (it.index == index) {
				getCurrentList().RemoveAt(n);
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
	/// <summary>
	/// Adds the glassware to inventory.
	/// </summary>
	/// <param name="gl">Glassware to be added.</param>
	public void AddGlasswareToInventory(Glassware gl) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.addGlassware(gl);
		item.HoldItem (gl);
		addItem(ref item);
	}
	/// <summary>
	/// Adds the reagent to inventory.
	/// </summary>
	/// <param name="reagent">Reagent to be added.</param>
	/// <param name="r">The component info.</param>
	public void AddReagentToInventory(ReagentPot reagent, Compound r ) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.HoldItem (reagent);
		item.addReagent(r);
		addItem(ref item);
	}
	/// <summary>
	/// Adds the product to inventory.
	/// </summary>
	/// <param name="itm">Product to be added.</param>
	public void AddProductToInventory(GameObject itm) {
		ItemInventoryBase item = new ItemInventoryBase();
		item.physicalObject = true;
		item.itemType = ItemType.Others;
		GenerateIndex (ref item);
		itm.name = item.index;

		addItem(ref item);
		//sends item to limbo
		itm.transform.SetParent (limbo, false);
	}
	/// <summary>
	/// Gets the current list.
	/// </summary>
	/// <returns>Current list.</returns>
	public ArrayList getCurrentList(){
		switch (listIndex) {
		case 0:
			return Products;
		case 1:
			return Reagents;
		case 2:
			return Glassware;;
		}
		return null;
	}
	/// <summary>
	/// Sets the selected item.
	/// </summary>
	/// <param name="i">The item to be set.</param>
	public void setSelectedItem(ItemInventoryBase i)
	{
		//if there was a selected object, sets its background to default 
		if (selectedObject != null) {
			selectedObject.GetComponentInChildren<Image> ().sprite = backgroundIcons [3];
		}
		//sets "i" as selected item and changes its background
		this.selectedItem = i;
		selectedObject = GameObject.Find (selectedItem.gameObject.name);
		selectedObject.GetComponentInChildren<Image>().sprite = selectedIcon;

		refreshButtons ();
	}
	/// <summary>
	/// Determinates what to do when action button is clicked
	/// </summary>
	public void actionButtonClick(){
		bool remove = true;
		if (selectedItem != null) {
			ItemInventoryBase item = new ItemInventoryBase();
			item = selectedItem;
			GameStateBase currentState = gameController.GetCurrentState ();

			if (currentState.GetComponent<WorkBench> () != null)
				remove = SendObjectToWorkbench (item);

			if (currentState.GetComponent<LIAState> () != null)
				remove = currentState.GetComponent<LIAState>().ReceiveProduct(item,GameObject.Find(item.index));

			if(remove)
			removeItem (GameObject.Find (selectedItem.gameObject.name));
		}
	}
	/// <summary>
	/// Sends the object to workbench.
	/// </summary>
	/// <returns><c>true</c>, if object to workbench was sent, <c>false</c> otherwise.</returns>
	/// <param name="item">Item to be sent.</param>
	public bool SendObjectToWorkbench(ItemInventoryBase item) {
		return gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (item);
	}
}
