using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /***** SINGLETON SETUP *****/
    private static Inventory _instance;
    public static Inventory Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }
    }
    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
    /******************************/

    private List<Pickup> inventory;
    public Transform UiContainer;

    private void Start()
    {
        inventory = new List<Pickup>();
        foreach (Transform child in UiContainer)
            GameObject.Destroy(child.gameObject);
    }

    public void Add(Pickup item)
    {
        inventory.Add(item);
        Instantiate(item.InventoryPrefab, UiContainer);
    }

    public Pickup Remove(string name)
    {
        Pickup item = null;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (name == inventory[i].Name)
            {
                item = inventory[i];
                inventory.RemoveAt(i);
                Destroy(UiContainer.GetChild(i).gameObject);
                break;
            }
        }
        return item;
    }

    public bool Contains(string name)
    {
        foreach (Pickup item in inventory)
        {
            if (name == item.Name)
                return true;
        }
        return false;
    }
}
