using UnityEngine;

public class UnitDataHolder : MonoBehaviour {
    public static GameObject Create(Vector3 spawnPosition, UnitTypeSO inUnitType, TeamSO inTeam) {
        GameObject newUnitDataHolderGameObject = Instantiate(inUnitType.prefab, spawnPosition, Quaternion.identity).gameObject;

        newUnitDataHolderGameObject.SetActive(false);

        UnitDataHolder newUnitDataHolder = newUnitDataHolderGameObject.GetComponent<UnitDataHolder>();
        newUnitDataHolder.unitTeam = inTeam;
        newUnitDataHolder.unitType = inUnitType;

        newUnitDataHolderGameObject.SetActive(true);

        return newUnitDataHolderGameObject;
    }

    public UnitTypeSO unitType;
    public TeamSO unitTeam;
}
