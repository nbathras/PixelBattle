using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingDataHolder))]
public class Building : MonoBehaviour {

    public EventHandler OnUnitConstructed;

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
                OnUnitConstructed?.Invoke(this, new EventArgs());
                // Debug.Log(OnUnitConstructed?.GetInvocationList().Length);
            }
        }
    }

    private void SpawnUnit(UnitTypeSO unitType) {
        Unit.Create(transform.position, unitType, buildingDataHolder.buildingTeam);
    }

    public float GetTimerNormalized() {
        return timer / timerMax;
    }

    public void ConstructUnit(UnitTypeSO unitType) {
        constructionQueue.Enqueue(unitType);
        // BuildingConstructionSelectUI.instance.UpdateUnitConstructionQueue();
    }

    public List<UnitTypeSO> GetConstructionQueue() {
        return constructionQueue.ToList();
    }

    public List<UnitTypeSO> GetConstructableUnits() {
        return buildingDataHolder.buildingType.constructableUnits;
    }
}
