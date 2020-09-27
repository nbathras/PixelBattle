using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BuildingConstructionSelectUI : MonoBehaviour {

    // Test fields
    [SerializeField] private UnitTypeSO testUnitType;

    // Instance
    public static BuildingConstructionSelectUI instance;

    // Serialized
    [SerializeField] private Vector2 initialOffSet = new Vector2(75f, 75f);
    // [SerializeField] private Vector2 initialOffsetQueue = new Vector2(-75f, 75f);
    [SerializeField] private Vector2 offset = new Vector2(125f, 0f);
    // [SerializeField] private Vector2 offsetQueue = new Vector2(-125f, 0f);

    // Set in Awake
    private Transform ui;
    private Transform optionContainer;
    private Transform btnTemplateOption;
    private Transform queueContainer;
    private Transform btnTemplateQueue;

    // Other
    private List<Transform> unitConstructionOptionTransformList;
    private List<Transform> unitConstructionQueueTransformList;
    private Building selectedBuilding;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        ui = transform.Find("ui");

        // Get btn templates
        optionContainer = ui.Find("optionContainer");
        btnTemplateOption = optionContainer.Find("btnTemplateOption");
        btnTemplateOption.gameObject.SetActive(false);

        queueContainer = ui.Find("queueContainer");
        btnTemplateQueue = queueContainer.Find("btnTemplateQueue");
        btnTemplateQueue.gameObject.SetActive(false);

        // Initalize Data Structures
        unitConstructionOptionTransformList = new List<Transform>();
        unitConstructionQueueTransformList = new List<Transform>();

        HideUI();
    }

    public void UpdateUnitConstructionOptions() {
        foreach (Transform t in unitConstructionOptionTransformList) {
            Destroy(t.gameObject);
        }

        unitConstructionOptionTransformList = new List<Transform>();

        int index = 0;
        foreach (UnitTypeSO unitType in selectedBuilding.GetConstructableUnits()) {
            Transform btnTransform = Instantiate(btnTemplateOption, transform);
            btnTransform.SetParent(optionContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

            btnTransform.Find("text").GetComponent<Text>().text = unitType.name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                instance.selectedBuilding.ConstructUnit(unitType);
            });

            unitConstructionOptionTransformList.Add(btnTransform);

            index++;
        }
    }

    public void UpdateUnitConstructionQueue() {
        foreach (Transform t in unitConstructionQueueTransformList) {
            Destroy(t.gameObject);
        }

        unitConstructionQueueTransformList = new List<Transform>();

        List<UnitTypeSO> unitTypeList = selectedBuilding.GetConstructionQueue().ToList<UnitTypeSO>();
        for (int i = 0; i < unitTypeList.Count; i++) {
            Transform btnTransform = Instantiate(btnTemplateQueue, transform);
            btnTransform.SetParent(queueContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * i;

            btnTransform.Find("text").GetComponent<Text>().text = unitTypeList[i].name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                // BuildingConstructionSelectUI.instance.AddConstructionQueueEntry(unitTypeList[i]);
            });

            unitConstructionQueueTransformList.Add(btnTransform);
        }
    }

    public void HideUI() {
        ui.gameObject.SetActive(false);
        selectedBuilding = null;
    }

    // ToDo: update container using events
    public void ShowUI(Building building) {
        selectedBuilding = building;
        UpdateUnitConstructionOptions();
        ui.gameObject.SetActive(true);
    }
}
