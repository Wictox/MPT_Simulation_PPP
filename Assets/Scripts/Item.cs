using UnityEngine;
using UnityEngine.UI; // REQUIRED: You need this to use 'Image'

public class Item : MonoBehaviour
{
    public int ID; 
    public string Name;
    public int quantity = 1;

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }

    public virtual void PickUp()
    {
        // She uses Image here, but remember to use SpriteRenderer if this is a ground item!
        Sprite itemIcon = GetComponent<Image>().sprite; 

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}