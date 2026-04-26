using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    
    // Cache these references to improve performance
    private GameObject player;
    private CinemachineConfiner2D confiner;

    void Start()
    {
        // Define the save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json"); 
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        
        // Find our objects once at the start
        player = GameObject.FindGameObjectWithTag("Player");
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();

        // Load the game state when the game starts
        LoadGame(); 
    }


    public void SaveGame()
    {
        // Safety check to ensure the player wasn't destroyed before saving
        if (player == null) return;

        // Safely get the boundary name if it exists
        string boundaryName = "";
        if (confiner != null && confiner.BoundingShape2D != null)
        {
            boundaryName = confiner.BoundingShape2D.gameObject.name;
        }

        // Create a new instance of our SaveData class and populate it
        SaveData saveData = new SaveData()
        {
            playerPosition = player.transform.position,
            mapBoundary = boundaryName,
            inventorySaveData = inventoryController.GetInventoryItems(), // Get the current inventory state
            hotbarSaveData = hotbarController.GetHotbarItems() // Get the current hotbar state
        };

        // Convert the data object to JSON and write it to the file
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveLocation, json); 
        
        Debug.Log("Game successfully saved to: " + saveLocation); // Helpful for debugging
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            // Read the JSON string and convert it back into a SaveData object
            string json = File.ReadAllText(saveLocation);
            SaveData data = JsonUtility.FromJson<SaveData>(json); 

            // Apply the saved position
            if (player != null)
            {
                player.transform.position = data.playerPosition; 
            }

            // Find the saved boundary object and apply it to the confiner
            if (!string.IsNullOrEmpty(data.mapBoundary))
            {
                GameObject boundaryObj = GameObject.Find(data.mapBoundary);
                if (boundaryObj != null && confiner != null)
                {
                    confiner.BoundingShape2D = boundaryObj.GetComponent<PolygonCollider2D>(); 
                    inventoryController.SetInventoryItems(data.inventorySaveData); // Restore the inventory state
                }
            }
            hotbarController.SetHotbarItems(data.hotbarSaveData); // Restore the hotbar state
        }
        else
        {
            // If no save file exists, create one with the current game state
            SaveGame(); 
        }
    }
}