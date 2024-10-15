using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI itemName;
    public Image image;
    public Color color;
    public TextMeshProUGUI amount;
    public RectTransform tooltip;
    public TextMeshProUGUI tooltipText;

    public PlayerController player;

    public void OnPointerClick(PointerEventData eventData)
    {
        player.UseItem((PickupType)Enum.Parse(typeof(PickupType), itemName.text));
    }

    // Called when the mouse or pointer enters the object
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
    }

    // Called when the mouse or pointer exits the object
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
