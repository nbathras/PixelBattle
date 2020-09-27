using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingDataHolder))]
public class Building : MonoBehaviour {
    private BuildingDataHolder buildingDataHolder;

    private Queue<UnitTypeSO> constructionQueue;

    private float timer;
    private float timerMax = 5;

    private void Awake() {
        buildingDataHolder = GetComponent<BuildingDataHolder>();
        constructionQueue = new Queue<UnitTypeSO>();

        timer = timerMax;
    }

    private void Update() {
        if (constructionQueue.Count > 0) {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                timer += timerMax;
                SpawnUnit(constructionQueue.Dequeue());
                BuildingConstructionSelectUI.instance.UpdateUnitConstructionQueue();
            }
        }
    }

    private void SpawnUnit(UnitTypeSO unitType) {
        UnitDataHolder.Create(transform.position, unitType, buildingDataHolder.buildingTeam);
    }

    public float GetTimerNormalized() {
        return timer / timerMax;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            BuildingConstructionSelectUI.instance.ShowUI(this);
        }
    }

    public void ConstructUnit(UnitTypeSO unitType) {
        constructionQueue.Enqueue(unitType);
        BuildingConstructionSelectUI.instance.UpdateUnitConstructionQueue();
    }

    public Queue<UnitTypeSO> GetConstructionQueue() {
        return constructionQueue;
    }

    public List<UnitTypeSO> GetConstructableUnits() {
        return buildingDataHolder.constructableUnits;
    }
}
