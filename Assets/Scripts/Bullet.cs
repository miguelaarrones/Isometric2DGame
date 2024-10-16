using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float destructionTime;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer > destructionTime)
        {
            Destroy(gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public float GetBulletSpeed() => bulletSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.GetComponent<EnemyController>().Hit(bulletDamage);
        }
    }
}
