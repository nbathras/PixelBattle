using CodeMonkey.Utils;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public static Bullet Create(Vector3 position, UnitRTS targetUnit, int damage) {
        Transform pfBullet = Resources.Load<Transform>("pfBullet");
        Transform bulletTransform = Instantiate(pfBullet, position, Quaternion.identity);

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        bullet.SetTarget(targetUnit);
        bullet.SetDamage(damage);

        return bullet;
    }
    
    // ToDo: Add two implementation
    //      1. Bullet follows target
    //      2. Bullet shoots where target was

    [SerializeField] private float timeToDie = 1.2f;
    [SerializeField] private float speed;

    // private Vector3 lastMoveDir;
    // private UnitRTS targetUnit;
    // private Vector3 targetPosition;
    private Vector3 moveDir;
    private int damage;

    private void Update() {
        /*
        Vector3 moveDir;
        if (targetUnit != null) {
            moveDir = (targetUnit.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        } else {
            moveDir = lastMoveDir;
        }
        */
        // Vector3 moveDir = (targetPosition - transform.position).normalized;

        transform.position += moveDir * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(moveDir));

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(UnitRTS targetUnit) {
        // this.targetUnit = targetUnit;
        // targetPosition = targetUnit.transform.position;
        moveDir = (targetUnit.transform.position - transform.position).normalized;
    }

    private void SetDamage(int damage) {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        UnitRTS enemy = collision.GetComponent<UnitRTS>();

        if (enemy != null) {
            // Hit an enemy!
            enemy.GetComponent<HealthSystem>().Damage(damage); // ToDo this throws error
            Destroy(gameObject);
        }
    }
}
