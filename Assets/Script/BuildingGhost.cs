using UnityEngine;
using static BuildingManager;

public class BuildingGhost : MonoBehaviour {

    private GameObject spriteGameObject;
    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        spriteGameObject = transform.Find("sprite").gameObject;

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

            if (BuildingManager.CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition())) {
                ValidSpawnGhost();
            } else {
                InvalidSpawnGhost();
            }
        }
    }

    private void ShowGhost(BuildingTypeSO inActiveBuildingType) {
        /*
        if (spriteFoundation != null) {
            Destroy(spriteFoundation);
        }
        spriteFoundation = Instantiate(inSpriteFoundation, transform.position, Quaternion.identity);
        spriteFoundation.transform.SetParent(transform);
        SetAlpha(.5f);
        spriteFoundation.SetActive(true);
        */

        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = inActiveBuildingType.sprite;
        activeBuildingType = inActiveBuildingType;
    }

    private void HideGhost() {
        spriteGameObject.SetActive(false);
    }

    private void InvalidSpawnGhost() {
        // TODO: cash the sprite renderer
        spriteGameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, .49f);
    }

    private void ValidSpawnGhost() {
        // TODO: cash the sprite renderer
        spriteGameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .49f);
    }

    /*
    private void SetAlpha(float alpha) {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        Color newColor;
        foreach (SpriteRenderer child in children) {
            newColor = child.color;
            newColor.a = alpha;
            child.color = newColor;
        }
    }
    */
}
