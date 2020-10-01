using CodeMonkey.Utils;
using UnityEngine;
using static BuildingManager;

public class BuildingGhost : MonoBehaviour {

    private GameObject spriteFoundation;

    private void Awake() {
        HideGhost();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, OnActiveBuildingTypeChangedEventsArgs e) {
        if (e.activeBuildingType == null) {
            HideGhost();
        } else {
            ShowGhost(e.activeBuildingType.spriteFoundation);
        }
    }

    private void Update() {
        Vector2 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector2 buildingPosition = new Vector2(
            Mathf.RoundToInt(mousePosition.x),
            Mathf.RoundToInt(mousePosition.y)
        );
        transform.position = buildingPosition;
    }

    private void ShowGhost(GameObject inSpriteFoundation) {
        if (spriteFoundation != null) {
            Destroy(spriteFoundation);
        }
        spriteFoundation = Instantiate(inSpriteFoundation, transform.position, Quaternion.identity);
        spriteFoundation.transform.SetParent(transform);
        SetAlpha(.5f);
        spriteFoundation.SetActive(true);
    }

    private void HideGhost() {
        if (spriteFoundation != null) {
            spriteFoundation.SetActive(false);
        }
    }

    private void SetAlpha(float alpha) {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        Color newColor;
        foreach (SpriteRenderer child in children) {
            newColor = child.color;
            newColor.a = alpha;
            child.color = newColor;
        }
    }
}
