using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

//! Manages all 'types' of Inventory Controller.
/*! It has information of all of the four types of InventoryController ( Solid, Liquid, Glassware and Others ). */
public class InventoryManager : MonoBehaviour {

	public GameObject itemPrefab;
	public ArrayList ObjectList     = new ArrayList();
    // need to be set on the inspector with the correct game object containing each InventoryController
	public ArrayList SolidReagents  = new ArrayList();       /*!< InventoryController for Solid Reagents. */
	public ArrayList LiquidReagents = new ArrayList();      /*!< InventoryController for Liquid Reagents. */
	public ArrayList Glassware      = new ArrayList();           /*!< InventoryController for Glassware. */
	public ArrayList Others         = new ArrayList();              /*!< InventoryController for Others. */

	//private ArrayList SelectedList   = new ArrayList();

	private ItemType itemType;
	private int count = 0;
	public int listIndex;
	public RectTransform content;
	public Button action;

    public ItemInventoryBase selectedItem = null;     /*!< Current selected item (game object). */
    private ItemStackableBehavior selectedUIItem = null;    /*!< Current selected item from inventory UI. */
	///-----------REFACTOR-------------------
	public void DebugList(Component obj){
		selectList (0);
		ItemInventoryBase test = new ItemInventoryBase ();
		test.Info (obj);
		test.name = "Teste";
		addItem (test);
	}

	void Start(){
		listIndex = 0;
		selectList(listIndex);
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
			Debug.Log(auxItem.index);
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
				break;
			case ItemType.Liquids:
				LiquidReagents.Add(auxItem);
				break;
			case ItemType.Others:
				Others.Add(auxItem);
				break;
			case ItemType.Solids:
				SolidReagents.Add(auxItem);
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
	}

	public void removeItem(GameObject item){
		string index = item.GetComponent<ItemInventoryBase> ().index;
		Debug.Log (index);
		ObjectList.Remove (item);

		int n = 0;

		foreach (ItemInventoryBase it in getCurrentList()) {
			Debug.Log (it.index);
			if (it.index == index) {
				Debug.Log (it.index);
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
		item.addReagent(reagent);
		item.HoldItem (reagent);
		addItem(item);
	}

	public void setSelectedItem(AnyObjectInstantiation i)
	{
		this.selectedItem = i;
	}

	public void setSelectedUIItem(ItemStackableBehavior i)
	{
		this.selectedUIItem = i;
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
		this.selectedItem = i;
		ItemInventoryBase aux = new ItemInventoryBase ();
		aux.copyData (i);
		action.onClick.AddListener (() => this.actionButtonClick (aux));
	}

	public void actionButtonClick(ItemInventoryBase item ){
		GameStateBase currentState = GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState();
		if (currentState.GetComponent<WorkBench> () != null)
			CallWorkbenchToTable (item);
		else {
			gameObject.GetComponentInParent<InventoryContent>().removeItemUI(gameObject.GetComponent<ItemStackableBehavior>());
		}
		
		
	}

	public void CallWorkbenchToTable(ItemInventoryBase item) {
		//GameObject tempItem = Instantiate (itemBeingHeld.gameObject) as GameObject;
		//gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (tempItem);
		GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (item.itemBeingHeld,gameObject.GetComponent<ReagentsBaseClass>());
	}
	
	
	///------------REFACTOR------------------
	//TODO: Remover depois!!!
	//public GameObject bequerPrefab;
	///////APAGA AQUI/////////////

    //! Does the operation on the inventory (insert, remove or destroy).
    /*! Based on the string passed as parameter, takes the selectedItem or selectedUIItem and does the operation. *//*
	void operateOnInventory(string op){
        // Insertion
		if(string.Compare(op,"insert") == 0 && selectedItem != null){
            switch(selectedItem.getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.AddItem(selectedItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.AddItem(selectedItem);
                    break;

                case ItemType.Glassware:
                    Glassware.AddItem(selectedItem);
                    break;

                case ItemType.Others:
                    Others.AddItem(selectedItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
		}
        // Removal
		else if(string.Compare(op,"remove") == 0 && selectedUIItem != null)
        {
            switch (selectedUIItem.getObject().getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Glassware:
                    Glassware.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Others:
                    Others.RemoveItem(selectedUIItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
		}
        // Destruction
        else if (string.Compare(op, "destroy") == 0 && selectedUIItem != null)
        {
            switch (selectedUIItem.getObject().getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Glassware:
                    Glassware.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Others:
                    Others.DestroyItem(selectedUIItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
        }

        selectedItem = null;
	}

    // TODO: Remover estes botoes de teste e utilizar as funcoes nos devidos locais.
    // Ex.: Quando o objeto eh clicado dentro do laboratorio.
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
			operateOnInventory("insert");
		}
		if(Input.GetKeyDown(KeyCode.S))
        {
			operateOnInventory("remove");
		}
        if(Input.GetKeyDown(KeyCode.D))
        {
            operateOnInventory("destroy");
        }
	}

    //! Set the selected Item.
    public void setSelectedItem(AnyObjectInstantiation i)
    {
        this.selectedItem = i;
    }

    //! Set the selected Item at the inventory UI.
    public void setSelectedUIItem(ItemStackableBehavior i)
    {
        this.selectedUIItem = i;
    }

	//! Add the item to inventory
	public void AddItemToInventory(AnyObjectInstantiation item) {
		Debug.Log (item.gameObject.name);
		setSelectedItem (item);
		operateOnInventory ("insert");
	}
	
	//! Remove from inventory
	public void RemoveItemFromInventory(AnyObjectInstantiation item) {
		setSelectedItem (item);
		operateOnInventory ("remove");
	}

	//-------------LeMigue for Glassware------------------------
	public void AddGlasswareToInventory(Glassware gl) {
		Glassware.GetComponentInChildren<InventoryContent> ().addNewGlasswareUI (gl);
		//Need to save "gl" within the InventoryItemGlassware informations
		//!(It is not actually saving any information right now, only creating the GameObject inside the inventory with the proper name)
	}

	//Written in a temporary way!!!!!!
	public void RemoveGlasswareFromInventory() {
		//Destroy the button (this will be used when the item is being put in the workbench slot)
		GameController gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		GameObject glasswareInScene = Instantiate(bequerPrefab) as GameObject;// = Instantiate the glassware saved in the informations

		//This here will be done using the PutGlassInTable() method from the WorkBench.
//		glasswareInScene.transform.position = gameController.gameStates [gameController.currentStateIndex].GetComponent<FumeHoodState> ().positionGlass1.transform.position;
		//gameController.GetCurrentState ().GetComponentInParent<WorkBench> ().PutGlassOnTable (true);
	}

	public void DestroyGlasswareFromInvenory() {

	}
	//-------------END OF LeMigue for Glassware-------------------------

	//-------------LeMigue for Reagents------------------------
	public void AddReagentToInventory(ReagentsBaseClass reagent, ReagentsBaseClass r ) {
		if(reagent.isSolid)
			SolidReagents.GetComponentInChildren<InventoryContent> ().addNewReagentsUI (reagent,r);
		if(!reagent.isSolid)
			LiquidReagents.GetComponentInChildren<InventoryContent> ().addNewReagentsUI (reagent,r);
	}
	
	public void RemoveReagentFromInventory() {
		//Destroy the button (this will be used when the item is being put in the workbench slot)
	}*/
	//-------------END OF LeMigue for Reagents-------------------------
}
