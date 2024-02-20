using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InventoryHandler : MonoBehaviour
{
    private Dictionary<string, InventoryItemData> _inventoryData = new Dictionary<string, InventoryItemData>();
    private const int MaxInventorySize = 12;
    private bool[] _isSlotEmpty = new bool[MaxInventorySize] {true,true,true,true,true,true,true,true,true,true,true,true};
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItem(ItemSO itemSo)
    {
        if (!CheckInventoryUsable())
        {
            Debug.Log("인벤토리가 꽉 차 있습니다.");
            return;
        }
        
        if (CheckItemExist(itemSo.Id))
        {
            var inventoryitemData = _inventoryData[itemSo.Id];
            inventoryitemData.count += 1;
            UpdateItemCount(inventoryitemData);
            return;
        }

        int usableSlotIndex = FindUsableSlotIndex();

        _isSlotEmpty[usableSlotIndex] = false;
        var newInventoryItemData = new InventoryItemData(1,usableSlotIndex);

        _inventoryData.Add(itemSo.Id, newInventoryItemData);
        
        var itemUI = Instantiate(itemSo.UIPrefab,transform.GetChild(newInventoryItemData.slotIndex), false);
        transform.GetChild(newInventoryItemData.slotIndex).GetComponentInChildren<TMP_Text>().text = newInventoryItemData.count.ToString();
    }

    public bool CheckInventoryUsable()
    {
        return _inventoryData.Count <= MaxInventorySize;
    }

    public bool CheckItemExist(string itemId)
    {
        return _inventoryData.ContainsKey(itemId);
    }

    public void UpdateItemCount(InventoryItemData inventoryItemData)
    {
        transform.GetChild(inventoryItemData.slotIndex).GetComponentInChildren<TMP_Text>().text =
            inventoryItemData.count.ToString();
    }

    public int FindUsableSlotIndex()
    {
        int usableSlotIndex = 0;
        foreach (var slot in _isSlotEmpty)
        {
            if (slot)
            {
                break;
            }

            usableSlotIndex++;
        }

        return usableSlotIndex;
    }
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