using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();  // Get the Item ScriptableObject from the GameObject
            if (item != null)
            {
                bool itemAdded = inventoryController.AddItem(item.gameObject);  // Pass the Item (not the GameObject)
                if (itemAdded)
                {
                    item.PickUp();  // Call the PickUp function of the Item ScriptableObject
                    Destroy(collision.gameObject);  // Destroy the item in the scene
                }
            }
        }
    }
}
