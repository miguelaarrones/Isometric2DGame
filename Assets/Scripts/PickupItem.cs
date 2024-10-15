using TMPro;
using UnityEngine;

public enum PickupType
{
    HealthPotion,
    SpeedPotion
}

public class PickupItem : MonoBehaviour
{
    [SerializeField] PickupType pickupType;
    [SerializeField] float amount;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] TextMeshProUGUI pickupText;

    public PickupType GetPickupType() => pickupType;
    public float GetAmount() => amount;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector3.zero, 10f, playerLayer);
        if (hit)
        {
            pickupText.gameObject.SetActive(true);
        } 
        else
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    public void RemovePickup()
    {
        Destroy(gameObject);
    }
}
