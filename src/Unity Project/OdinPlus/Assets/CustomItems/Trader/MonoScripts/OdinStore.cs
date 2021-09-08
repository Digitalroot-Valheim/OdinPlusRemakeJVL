using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField, ShowInInspector] private GameObject ElementGO;
    
    //StoreInventoryListing
    [ShowInInspector] internal static Dictionary<ItemDrop, int> _storeInventory = new Dictionary<ItemDrop, int>();
    public static OdinStore instance => m_instance;

    private void Awake() 
    {
        m_instance = this;
        m_StorePanel.SetActive(false);
        StoreTitle.text = "Odins Store";
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
    public void AddItemToDisplayList(ItemDrop _drop, int stack, int cost)
    {
        ElementFormat NewElement = new ElementFormat();
        NewElement._drop = _drop;
        NewElement.Icon = _drop.m_itemData.m_shared.m_icons.FirstOrDefault();
        NewElement.Name = _drop.m_itemData.m_shared.m_name;
        NewElement._drop.m_itemData.m_stack = stack;
        NewElement.Element = ElementGO;

        NewElement.Element.transform.Find("icon").GetComponent<Image>().sprite = NewElement.Icon;
        NewElement.Element.transform.Find("name").GetComponent<Text>().text = NewElement.Name;
        NewElement.Element.transform.Find("price").GetComponent<Text>().text = cost.ToString();
        
        Instantiate(NewElement.Element, ListRoot.transform, false).GetComponent<Button>().onClick.AddListener(delegate { UpdateGenDescription(NewElement); });;
        NewElement.Element.transform.SetSiblingIndex(ListRoot.transform.GetSiblingIndex() - 1);
    }

    public void ReadItems()
    {
        foreach (var itemData in _storeInventory)
        {
            //need to add some type of second level logic here to think about if items exist do not repopulate.....
            AddItemToDisplayList(itemData.Key,itemData.Key.m_itemData.m_stack, itemData.Value);
        }
    }

    /// <summary>
    /// Invoke this method to instantiate an item from the storeInventory dictionary. This method expects an integer argument this integer should identify the index in the dictionary that the item lives at you wish to vend
    /// </summary>
    /// <param name="i"></param>
    public void SellItem(int i)
    {
        //Instantation logic
        Vector3 vector = Random.insideUnitSphere * 0.5f;
        var transform1 = Player.m_localPlayer.transform;
        var itemDrop = (ItemDrop)Instantiate(_storeInventory.ElementAt(i).Key.gameObject, transform1.position + transform1.forward * 2f + Vector3.up + vector,
            Quaternion.identity).GetComponent(typeof(ItemDrop));
        if (itemDrop == null || itemDrop.m_itemData == null) return;
        
        itemDrop.m_itemData.m_stack = _storeInventory.ElementAt(i).Key.m_itemData.m_stack;
        itemDrop.m_itemData.m_durability = itemDrop.m_itemData.GetMaxDurability();

        //If you want to remove the item after it's sold?
        //RemoveItemFromDict(_storeInventory.ElementAt(i).Key);
    }
    

    /// <summary>
    ///  Adds item to stores dictionary pass ItemDrop.ItemData and an integer for price
    /// </summary>
    /// <param name="itemDrop"></param>
    /// <param name="price"></param>
    public void AddItemToDict(ItemDrop itemDrop, int price)
    {
        _storeInventory.Add(itemDrop, price);
    }

    /// <summary>
    /// Pass this method an ItemDrop as an argument to drop it from the storeInventory dictionary.
    /// </summary>
    /// <param name="itemDrop"></param>
    /// <returns></returns>
    public bool RemoveItemFromDict(ItemDrop itemDrop)
    {
        if (_storeInventory.Remove(itemDrop))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// This methods invocation should return the index offset of the ItemDrop passed as an argument, this is for use with other functions that expect an index to be passed as an integer argument
    /// </summary>
    /// <param name="itemDrop"></param>
    /// <returns></returns>
    public int FindIndex(ItemDrop itemDrop)
    {
        var templist = _storeInventory.Keys.ToList();
        var index = templist.IndexOf(itemDrop);

        return index;

    }
    public void UpdateGenDescription(ElementFormat element)
    {
        SelectedCost.text = element.Price.ToString();
        SelectedItemDescription.text = element._drop.m_itemData.m_shared.m_description;
        SelectedItemDescription.gameObject.AddComponent<Localize>();
        SelectedCost.gameObject.AddComponent<Localize>();
        ItemDropIcon.sprite = element.Icon;
        var thing = FindIndex(element._drop);
        SellItem(thing);
    }
    
    /// <summary>
    /// Format of the Element GameObject that populates the for sale list.
    /// </summary>
    public class ElementFormat
    {
        internal GameObject Element;
        internal Sprite Icon;
        internal string Name;
        internal int Price;
        internal ItemDrop _drop;

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
}
