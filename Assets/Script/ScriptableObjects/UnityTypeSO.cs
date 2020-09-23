using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UnitType")]
public class UnityTypeSO : ScriptableObject {
    public float fireRate;
    public float fireRange;
    public int damage;
    public float bulletSpeed;
}
