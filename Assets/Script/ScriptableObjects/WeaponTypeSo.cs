using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponType")]
public class WeaponTypeSo : ScriptableObject {
    public float fireRate;
    public float fireRange;
    public int damage;
    public float bulletSpeed;
    public float bulletLifeLength;
}
