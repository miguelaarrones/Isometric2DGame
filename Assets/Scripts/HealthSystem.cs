using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private float maxHealth;
    
    [Header("UI Settings")]
    [SerializeField] private RectTransform background;
    [SerializeField] private Image healthBar;
    [SerializeField] private Vector3 positionOffset = new Vector3(-0.5f, 0.5f, 0.0f);

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        background.position = transform.position + positionOffset;
    }

    public float GetCurrentHealth() => currentHealth;

    public void DecreaseCurrentHealth(float amount)
    {
        currentHealth -= amount;

        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
