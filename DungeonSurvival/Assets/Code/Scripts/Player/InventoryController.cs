using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryController : MonoBehaviour
{
    #region Fields
    
    public Dictionary<string, InventoryItemData> inventoryData;
    public int maxInventorySize = 12;
    public bool[] isSlotEmpty;

    #endregion
    
    #region Functions

    public void UpdateItem(ItemSO itemSo)
    {
        if (!CheckInventoryUsable())
        {
            Debug.Log("인벤토리가 꽉 차 있습니다.");
            return;
        }
        
        if (CheckItemExist(itemSo.Id))
        {
            var inventoryitemData = inventoryData[itemSo.Id];
            inventoryitemData.count += 1;
            UpdateItemCount(inventoryitemData);
            return;
        }

        int usableSlotIndex = FindUsableSlotIndex();

        isSlotEmpty[usableSlotIndex] = false;
        var newInventoryItemData = new InventoryItemData(1,usableSlotIndex);

        inventoryData.Add(itemSo.Id, newInventoryItemData);
        
        Instantiate(itemSo.UIPrefab,transform.GetChild(newInventoryItemData.slotIndex), false);
        transform.GetChild(newInventoryItemData.slotIndex).GetComponentInChildren<TMP_Text>().text = newInventoryItemData.count.ToString();
    }

    public bool CheckInventoryUsable()
    {
        return inventoryData.Count <= maxInventorySize;
    }

    public bool CheckItemExist(string itemId)
    {
        return inventoryData.ContainsKey(itemId);
    }

    public void UpdateItemCount(InventoryItemData inventoryItemData)
    {
        transform.GetChild(inventoryItemData.slotIndex).GetComponentInChildren<TMP_Text>().text =
            inventoryItemData.count.ToString();
    }

    public int FindUsableSlotIndex()
    {
        int usableSlotIndex = 0;
        foreach (var slot in isSlotEmpty)
        {
            if (slot)
            {
                break;
            }

            usableSlotIndex++;
        }

        return usableSlotIndex;
    }
    
    #endregion
}

public class InventoryItemData
{
    public int count;
    public int slotIndex;

    public InventoryItemData(int count, int slotIndex)
    {
        this.count = count;
        this.slotIndex = slotIndex;
    }
}
