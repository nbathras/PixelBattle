using UnityEngine;

[RequireComponent(typeof(BuildingDataHolder))]
public class Building : MonoBehaviour {
    private BuildingDataHolder buildingDataHolder;

    private float timer;
    private float timerMax = 5;

    private void Awake() {
        buildingDataHolder = GetComponent<BuildingDataHolder>();

        timer = timerMax;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            timer += timerMax;
            SpawnUnit();
        }
    }

    private void SpawnUnit() {
        UnitDataHolder.Create(transform.position, buildingDataHolder.unitTypeList[0], buildingDataHolder.buildingTeam);
    }
}
