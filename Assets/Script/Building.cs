using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingDataHolder))]
public class Building : MonoBehaviour {

    public static GameObject Create(Vector3 spawnPosition, BuildingTypeSO buildingType, TeamSO buildingTeam) {
        GameObject buildingGameObject = Instantiate(
            buildingType.prefab,
            spawnPosition,
            Quaternion.identity
        ).gameObject;

        buildingGameObject.SetActive(false);

        Building building = buildingGameObject.GetComponent<Building>();
        building.buildingDataHolder.buildingTeam = buildingTeam;
        building.UpdateTeamColor();

        buildingGameObject.SetActive(true);

        return buildingGameObject;
    }

    public EventHandler OnUnitConstructed;

    private SpriteRenderer spriteRenderer;
    private BuildingDataHolder buildingDataHolder;

    private Queue<UnitTypeSO> constructionQueue;

    private float timer;
    private float timerMax = 5;

    private void Awake() {
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();

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

    private void UpdateTeamColor() {
        spriteRenderer.color = buildingDataHolder.buildingTeam.color;
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
