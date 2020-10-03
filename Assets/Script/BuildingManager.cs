using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }

    [SerializeField]
    private TeamSO playerControlledTeam;

    public EventHandler<OnSelectedBuildingChangedEventArgs> OnBuildingSelectedChange;
    public EventHandler<OnActiveBuildingTypeChangedEventsArgs> OnActiveBuildingTypeChanged;

    public class OnSelectedBuildingChangedEventArgs : EventArgs {
        public Building building;
    }
    public class OnActiveBuildingTypeChangedEventsArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }

    // Set in other
    // private Building selectedBuilding;
    private BuildingTypeSO activeBuildingType;

    private bool leftMouseReleased = true;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        // Left Click on Screen
        if (Input.GetMouseButtonDown(0) && leftMouseReleased && !EventSystem.current.IsPointerOverGameObject()) {
            leftMouseReleased = false;

            // Building Construction Selected
            if (activeBuildingType != null) {
                // Can Spawn Building
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY), out string errorMessage)) {
                    // Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY), Quaternion.identity);
                    Building.Create(
                        UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY),
                        activeBuildingType,
                        playerControlledTeam
                    );
                    SetActiveBuildingType(null);
                    return;
                }
            }

            // Attempt to Click On Building
            if (activeBuildingType == null) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null) {
                    OnBuildingSelectedChange?.Invoke(this, new OnSelectedBuildingChangedEventArgs() { building = hit.transform.GetComponent<Building>() });
                } else {
                    OnBuildingSelectedChange?.Invoke(this, new OnSelectedBuildingChangedEventArgs() { building = null });
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            leftMouseReleased = true;
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
