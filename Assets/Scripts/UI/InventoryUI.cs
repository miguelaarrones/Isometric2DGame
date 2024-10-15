using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform itemTemplate;
    [SerializeField] private PlayerController player;

    private void Awake()
    { 
        itemTemplate.gameObject.SetActive(false);
    }

    public void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (PickupItem item in player.GetInventoryItems())
        {
            ItemUI itemUI = Instantiate(itemTemplate, container).GetComponent<ItemUI>();
            itemUI.gameObject.SetActive(true);
            itemUI.itemName.SetText(item.GetName());
            itemUI.image.sprite = item.GetSprite();
            itemUI.image.color = item.GetColor();
            itemUI.amount.SetText("Amount: 1");
            itemUI.tooltipText.SetText(item.GetTooltipText());
            itemUI.player = player;
        }
    }
}
