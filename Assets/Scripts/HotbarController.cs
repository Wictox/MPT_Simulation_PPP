using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{

    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;

    private ItemDictionary itemDictionary;

    private Key[] hotbarKeys;

    private void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        hotbarKeys = new Key[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i<9? (Key)((int)Key.Digit1 + i) : Key.Digit0; // Assign keys 1-0 to the hotbar slots
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0 ; i<slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return hotbarData;
    }
    
    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject); // Clear existing slots
        }

        //Create new slots 

        for (int i = 0 ; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        //populate slots with saved items
        foreach(InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Center the item in the slot
                    slot.currentItem = item; // Set the current item in the slot
                }
                else
                {
                    Debug.LogWarning("Item ID " + data.itemID + " not found in ItemDictionary.");
                }
            }
        }
    }
}
