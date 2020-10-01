using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }

    public EventHandler<OnSelectedBuildingChangedEventArgs> OnBuildingSelectedChange;
    public EventHandler<OnActiveBuildingTypeChangedEventsArgs> OnActiveBuildingTypeChanged;

    public class OnSelectedBuildingChangedEventArgs : EventArgs {
        public Building building;
    }
    public class OnActiveBuildingTypeChangedEventsArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }

    // Set in other
    private Building selectedBuilding;
    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        // Check for active building and placement 
        if (activeBuildingType != null) {

            if (Input.GetMouseButtonDown(0)) {
                if (CanSpawnBuilding(UtilsClass.GetMouseWorldPosition())) {
                    Vector2 mousePosition = UtilsClass.GetMouseWorldPosition();
                    Vector2 buildingPosition = new Vector2(
                        Mathf.RoundToInt(mousePosition.x),
                        Mathf.RoundToInt(mousePosition.y)
                    );

                    Instantiate(activeBuildingType.prefab, buildingPosition, Quaternion.identity);
                    SetActiveBuildingType(null);
                }
            }
        }

        // Check for Clicking on a Building
        if (Input.GetMouseButton(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                Building newSelectedBuilding = hit.transform.GetComponent<Building>();
                if (selectedBuilding == null && newSelectedBuilding == null) {
                    return; // If nothing was selected and we select nothing do nothing
                } else {
                    if (newSelectedBuilding != null) {
                        selectedBuilding = newSelectedBuilding;
                    } else {
                        selectedBuilding = null;
                    }
                    OnBuildingSelectedChange?.Invoke(this, new OnSelectedBuildingChangedEventArgs() { building = selectedBuilding });
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetActiveBuildingType(null);
        }
    }

    private bool CanSpawnBuilding(Vector3 vector3) {
        return true;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventsArgs { activeBuildingType = buildingType }
        );
    }
}
