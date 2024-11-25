using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;

    public void SetItem(ItemData itemData, int count)
    {
        itemIcon.sprite = itemData.ItemIcon;
        itemName.text = itemData.ItemName;
        itemCount.text = count.ToString();
    }
}
