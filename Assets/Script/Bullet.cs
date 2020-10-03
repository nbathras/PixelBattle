using UnityEngine;

public class Bullet : MonoBehaviour {
    public static Bullet Create(Vector3 spawnPosition, Vector3 target, WeaponData weaponData) {
        Transform pfBullet = Resources.Load<Transform>("pfBullet");
        Transform bulletTransform = Instantiate(pfBullet, spawnPosition, Quaternion.identity);

        Vector3 moveDir = (target - bulletTransform.position).normalized;

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        bullet.Initalize(moveDir, weaponData);

        return bullet;
    }

    private WeaponData weaponData;
    private Vector3 moveDir;

    private float bulletLifeLength  = 2f;

    private void Update() {
        transform.position += moveDir * weaponData.bulletSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(moveDir));

        bulletLifeLength -= Time.deltaTime;
        if (bulletLifeLength < 0f) {
            Destroy(gameObject);
        }
    }

    private void Initalize(Vector3 inMoveDir, WeaponData inWeaponData) {
        moveDir = inMoveDir;
        weaponData = inWeaponData;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Unit enemy = collision.GetComponent<Unit>();

        if (enemy != null) {
            // Hit an enemy!
            enemy.GetComponent<HealthSystem>().Damage(weaponData.damage);
            Destroy(gameObject);
        }
    }
}
