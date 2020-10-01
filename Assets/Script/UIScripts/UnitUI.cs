using UnityEngine;
using UnityEngine.UI;

public class UnitUI : BottomUI {

    // Serialized
    [SerializeField] private Vector2 initialOffSet = new Vector2(75f, 75f);
    [SerializeField] private Vector2 offset = new Vector2(125f, 0f);

    [SerializeField] private BuildingTypeListSO buildingTypeList;

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

        Transform btnEmpty = Instantiate(btnTemplateOption, transform);
        btnEmpty.SetParent(optionContainer);
        btnEmpty.gameObject.SetActive(true);

        btnEmpty.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

        btnEmpty.Find("text").GetComponent<Text>().text = "Empty Arrow";

        btnEmpty.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list) {
            Transform btnTransform = Instantiate(btnTemplateOption, transform);
            btnTransform.SetParent(optionContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

            btnTransform.Find("text").GetComponent<Text>().text = buildingType.name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            index++;
        }
    }
}
