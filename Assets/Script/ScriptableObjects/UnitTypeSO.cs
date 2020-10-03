using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UnitType")]
public class UnitTypeSO : ScriptableObject {
    public GameObject prefab;

    public float speed;
    public int health;

    public Sprite sprite;

    public WeaponData weapon;
}
