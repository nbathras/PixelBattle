using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : BottomUI {
    // Serialized
    [SerializeField] private Vector2 initialOffSet = new Vector2(75f, 75f);
    // [SerializeField] private Vector2 initialOffsetQueue = new Vector2(-75f, 75f);
    [SerializeField] private Vector2 offset = new Vector2(125f, 0f);
    // [SerializeField] private Vector2 offsetQueue = new Vector2(-125f, 0f);

    // Set in Awake
    // private Transform ui;
    private Transform optionContainer;
    private Transform btnTemplateOption;
    private Transform queueContainer;
    private Transform btnTemplateQueue;

    // Other
    private List<Transform> unitConstructionOptionTransformList;
    private List<Transform> unitConstructionQueueTransformList;
    private Building selectedBuilding;

    protected override void Awake() {
        base.Awake();

        // Get btn templates
        optionContainer = GetUI().Find("optionContainer");
        btnTemplateOption = optionContainer.Find("btnTemplateOption");
        btnTemplateOption.gameObject.SetActive(false);

        queueContainer = GetUI().Find("queueContainer");
        btnTemplateQueue = queueContainer.Find("btnTemplateQueue");
        btnTemplateQueue.gameObject.SetActive(false);

        // Initalize Data Structures
        unitConstructionOptionTransformList = new List<Transform>();
        unitConstructionQueueTransformList = new List<Transform>();

        HideUI();
    }

    private void Start() {
        BuildingManager.Instance.OnBuildingSelectedChange += BuildingManager_OnBuildingSelectedChange;
    }

    private void BuildingManager_OnBuildingSelectedChange(object sender, BuildingManager.OnSelectedBuildingChangedEventArgs e) {
        if (e.building == null) {
            if (selectedBuilding != null) { selectedBuilding.OnUnitConstructed -= Building_OnUnitConstructed; }
            selectedBuilding = null;
            HideUI();
        } else {
            if (selectedBuilding != null) { selectedBuilding.OnUnitConstructed -= Building_OnUnitConstructed; }
            e.building.OnUnitConstructed += Building_OnUnitConstructed;
            selectedBuilding = e.building;
            UpdateUnitConstructionOptions();
            UpdateUnitConstructionQueue();
            ShowUI();
        }
    }

    private void Building_OnUnitConstructed(object sender, EventArgs e) {
        UpdateUnitConstructionOptions();
        UpdateUnitConstructionQueue();
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
                selectedBuilding.ConstructUnit(unitType);
                UpdateUnitConstructionQueue();
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

        int index = 0;
        foreach (UnitTypeSO unitType in selectedBuilding.GetConstructionQueue()) {
            Transform btnTransform = Instantiate(btnTemplateQueue, transform);
            btnTransform.SetParent(queueContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

            btnTransform.Find("text").GetComponent<Text>().text = unitType.name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                // BuildingConstructionSelectUI.instance.AddConstructionQueueEntry(unitTypeList[i]);
            });

            unitConstructionQueueTransformList.Add(btnTransform);

            index++;
        }
    }
}
