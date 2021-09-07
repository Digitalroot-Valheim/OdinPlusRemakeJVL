using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class OdinStore : MonoBehaviour
{
    private static OdinStore m_instance;

    [SerializeField, ShowInInspector] private GameObject m_StorePanel;
    [SerializeField, ShowInInspector] private RectTransform ListRoot;
    [SerializeField, ShowInInspector] private Text SelectedItemDescription;
    [SerializeField, ShowInInspector] private Image ItemDropIcon;
    [SerializeField, ShowInInspector] private Text SelectedCost;
    [SerializeField, ShowInInspector] private Text StoreTitle;
    
    //ElementData
    [SerializeField, ShowInInspector] private Image ElementIcon;
    [SerializeField, ShowInInspector] private Text ElementName;
    [SerializeField, ShowInInspector] private Text ElementCost;
    [SerializeField, ShowInInspector] private GameObject ElementGO;
    
    //StoreInventoryListing
    [ShowInInspector]
    private Dictionary<ItemDrop, int> storeInventory = new Dictionary<ItemDrop, int>();

    public static OdinStore instance => m_instance;
    private int m_hiddenFrames;
    private void Awake() 
    {
        m_instance = this;
    }

    private void OnDestroy()
    {
        if (m_instance == this)
        {
            m_instance = null;
        }
    }

    /// <summary>
    /// This method is invoked to add an item to the visual display of the store, it expects the ItemDrop.ItemData and the stack as arguments
    /// </summary>
    /// <param name="_drop"></param>
    /// <param name="stack"></param>
    public void AddItemToDisplayList(ItemDrop _drop, int stack)
    {
        ElementFormat NewElement = new ElementFormat();
        NewElement.Element = ElementGO; //ElementGO has a gameObject assigned to it via UnityEditor, it is the template GO for the element that populates the store list
        NewElement.Name = ElementName; //ElementName is a child of ElementGO and has been assigned in UnityEditor
        NewElement._drop = _drop; //Assign our elements ItemDrop.ItemData with the arguments value
        NewElement._drop.m_itemData.m_stack = stack; //Assign ItemDrop.ItemData stack value based on argument of method.
        NewElement.ItemPrefab = _drop.m_itemData.m_dropPrefab; //This might be redundant code because you can call to NewElement._drop.m_dropPrefab but I thought it would be handy to have a quick call variable
        
        //Still not sure the above is a good solution for this. In Essence I could branch off _drop. and gain all information required.
        Instantiate(NewElement.Element, ListRoot.transform, false);
        NewElement.Element.transform.SetSiblingIndex(ListRoot.transform.GetSiblingIndex() - 1);
    }

    public void ReadItems()
    {
        foreach (var itemData in storeInventory)
        {
            AddItemToDisplayList(itemData.Key,itemData.Key.m_itemData.m_stack);
        }
    }
    
    /// <summary>
    /// This methods invocation should return the index offset of the ItemDrop passed as an argument, this is for use with other functions that expect an index to be passed as an integer argument
    /// </summary>
    /// <param name="itemDrop"></param>
    /// <returns></returns>
    public int FindIndex(ItemDrop itemDrop)
    {
        var templist = storeInventory.Keys.ToList();
        var index = templist.IndexOf(itemDrop);
        
        return index;
        
    }

    /// <summary>
    /// Invoke this method to instantiate an item from the storeInventory dictionary. This method expects an integer argument this integer should identify the index in the dictionary that the item lives at you wish to vend
    /// </summary>
    /// <param name="i"></param>
    public void SellItem(int i)
    {
        //Should there be solution to remove sold items from dictionary? If so need to solve for that here.

        var temp = storeInventory.ToList();
        temp[i].Key.m_itemData.m_shared.m_name.ToString();

        //Instantation logic
        var itemDrop = Player.m_localPlayer.GetInventory().AddItem(temp[i].Key.m_itemData.m_dropPrefab.name, temp[i].Key.m_itemData.m_stack, temp[i].Key.m_itemData.m_quality, temp[i].Key.m_itemData.m_variant, 0L, "");
        if (itemDrop == null) return;
        itemDrop.m_stack = temp[i].Key.m_itemData.m_stack;
        itemDrop.m_durability = itemDrop.GetMaxDurability();
    }
    

    /// <summary>
    ///  Adds item to stores dictionary pass ItemDrop.ItemData and an integer for price
    /// </summary>
    /// <param name="itemDrop"></param>
    /// <param name="price"></param>
    public void AddItemToDict(ItemDrop itemDrop, int price)
    {
        storeInventory.Add(itemDrop, price);
    }
    
    /// <summary>
    /// Format of the Element GameObject that populates the for sale list.
    /// </summary>
    public class ElementFormat
    {
        internal GameObject Element;
        internal Image Icon;
        internal Text Name;
        internal int Price;
        internal int Stack;
        internal ItemDrop _drop;
        internal GameObject ItemPrefab;

    }


    public void Hide()
    {
        m_StorePanel.SetActive(false);
    }

    public void Show()
    {
        m_StorePanel.SetActive(true);
        ReadItems();
    }
    public static bool IsVisible()
    {
        if ((bool)m_instance)
        {
            return m_instance.m_hiddenFrames <= 1;
        }

        return false;
    }
}
