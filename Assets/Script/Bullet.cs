using CodeMonkey.Utils;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public static Bullet Create(Vector3 position, UnitRTS targetUnit, WeaponTypeSo weaponType) {
        Transform pfBullet = Resources.Load<Transform>("pfBullet");
        Transform bulletTransform = Instantiate(pfBullet, position, Quaternion.identity);

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        bullet.SetTarget(targetUnit);
        bullet.SetWeaponType(weaponType);

        return bullet;
    }

    private WeaponTypeSo weaponType;

    private Vector3 moveDir;
    private float bulletLifeLength;

    private void Update() {
        transform.position += moveDir * weaponType.bulletSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(moveDir));

        bulletLifeLength -= Time.deltaTime;
        if (bulletLifeLength < 0f) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(UnitRTS targetUnit) {
        moveDir = (targetUnit.transform.position - transform.position).normalized;
    }

    private void SetWeaponType(WeaponTypeSo weaponType) {
        this.weaponType = weaponType;
        bulletLifeLength = weaponType.bulletLifeLength;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        UnitRTS enemy = collision.GetComponent<UnitRTS>();

        if (enemy != null) {
            // Hit an enemy!
            enemy.GetComponent<HealthSystem>().Damage(weaponType.damage);
            Destroy(gameObject);
        }
    }
}
