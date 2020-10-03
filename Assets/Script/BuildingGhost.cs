using UnityEngine;
using static BuildingManager;

public class BuildingGhost : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();

        HideGhost();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, OnActiveBuildingTypeChangedEventsArgs e) {
        if (e.activeBuildingType == null) {
            HideGhost();
        } else {
            ShowGhost(e.activeBuildingType);
        }
    }

    private void Update() {
        if (activeBuildingType != null) {
            transform.position = UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY);

            if (BuildingManager.CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseGridPosition(activeBuildingType.gridSizeX, activeBuildingType.gridSizeY), out string errorMessage)) {
                ValidSpawnGhost();
            } else {
                InvalidSpawnGhost();
            }
        }
    }

    private void ShowGhost(BuildingTypeSO inActiveBuildingType) {
        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = inActiveBuildingType.sprite;
        activeBuildingType = inActiveBuildingType;
    }

    private void HideGhost() {
        spriteRenderer.gameObject.SetActive(false);
    }

    private void InvalidSpawnGhost() {
        spriteRenderer.color = new Color(1f, 0f, 0f, .49f);
    }

    private void ValidSpawnGhost() {
        spriteRenderer.color = new Color(1f, 1f, 1f, .49f);
    }
}
