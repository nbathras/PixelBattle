using UnityEngine;
using UnityEngine.UI;

public class UnitUI : BottomUI {

    // Serialized
    [SerializeField] private Vector2 initialOffSet = new Vector2(75f, 75f);
    [SerializeField] private Vector2 offset = new Vector2(125f, 0f);

    [SerializeField] private BuildingTypeListSO buildingTypeList;
    [SerializeField] private Sprite cursorSprite;

    // private Transform ui;
    private Transform optionContainer;
    private Transform btnTemplateOption;

    protected override void Awake() {
        base.Awake();

        // Get btn templates
        optionContainer = GetUI().Find("optionContainer");
        btnTemplateOption = optionContainer.Find("btnTemplateOption");
        btnTemplateOption.gameObject.SetActive(false);

        HideUI();
    }

    protected override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.B)) {
            ShowUI();
        }
    }

    private void Start() {
        int index = 0;

        Transform btnCursor = Instantiate(btnTemplateOption, transform);
        btnCursor.SetParent(optionContainer);
        btnCursor.gameObject.SetActive(true);

        btnCursor.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

        btnCursor.Find("image").GetComponent<Image>().sprite = cursorSprite;

        btnCursor.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list) {
            Transform btnTransform = Instantiate(btnTemplateOption, transform);
            btnTransform.SetParent(optionContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            index++;
        }
    }
}
