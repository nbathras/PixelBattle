using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {
    public List<UnitTypeSO> constructableUnits;
    public GameObject spriteFoundation;
    public GameObject prefab;
}
