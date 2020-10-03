using System;
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
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY), out string errorMessage)) {
                    Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY), Quaternion.identity);
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

    public static bool CanSpawnBuilding(BuildingTypeSO inActiveBuilding, Vector3 inPosition, out string errorMessage) {
        errorMessage = "";

        BoxCollider2D boxCollider2D = inActiveBuilding.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(inPosition + (Vector3) boxCollider2D.offset, boxCollider2D.size, 0);

        // Not clear
        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) {
            errorMessage = "Area is not clear!";
            return false;
        }

        return true;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventsArgs { activeBuildingType = buildingType }
        );
    }
}
