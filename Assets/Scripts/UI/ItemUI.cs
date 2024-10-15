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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Object clicked: " + itemName);
    }

    // Called when the mouse or pointer enters the object
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered object: " + gameObject.name);
        // Add logic for hover state (e.g., highlight object)
    }

    // Called when the mouse or pointer exits the object
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited object: " + gameObject.name);
        // Add logic for exiting hover state (e.g.,  object)
    }
}
