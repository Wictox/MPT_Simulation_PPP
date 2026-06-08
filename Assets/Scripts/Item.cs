using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID; 
    public string Name;
    public int quantity = 1;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float volume = 0.8f;

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }

    public virtual void PickUp()
    {
        // 1. Play the sound (This spawns an invisible audio object that survives the Destroy command)
        if (pickupSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, volume);
        }

        // 2. Safely get the icon from the SpriteRenderer (since it's on the ground, not UI)
        Sprite itemIcon = null;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            itemIcon = sr.sprite;
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on " + Name + "!");
        }

        // 3. Trigger the UI Popup
        if (ItemPickupUIController.Instance != null && itemIcon != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}