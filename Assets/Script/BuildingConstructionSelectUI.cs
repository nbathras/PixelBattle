using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingConstructionSelectUI : MonoBehaviour {

    // Test fields
    [SerializeField] private UnitTypeSO testUnitType;

    // Serialized
    [SerializeField] private Vector2 initialOffSet = new Vector2(75f, 75f);
    // [SerializeField] private Vector2 initialOffsetQueue = new Vector2(-75f, 75f);
    [SerializeField] private Vector2 offset = new Vector2(125f, 0f);
    // [SerializeField] private Vector2 offsetQueue = new Vector2(-125f, 0f);

    // Set in Awake
    private Transform optionContainer;
    private Transform btnTemplateOption;
    private Transform queueContainer;
    private Transform btnTemplateQueue;

    // Other
    private Dictionary<UnitTypeSO, Transform> btnTransformDictionary;
    private Queue<ConstructionQueueEntry> constructionQueue; 

    private void Awake() {
        // Get btn templates
        optionContainer = transform.Find("optionContainer");
        btnTemplateOption = optionContainer.Find("btnTemplateOption");
        btnTemplateOption.gameObject.SetActive(false);

        queueContainer = transform.Find("queueContainer");
        btnTemplateQueue = queueContainer.Find("btnTemplateQueue");
        btnTemplateQueue.gameObject.SetActive(false);

        // Initalize Data Structures
        btnTransformDictionary = new Dictionary<UnitTypeSO, Transform>();

        constructionQueue = new Queue<ConstructionQueueEntry>();

        UpdateOptions();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            AddConstructionQueueEntry();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PopConstructionQueueEntry();
        }
    }

    private void UpdateOptions() {
        UnitTypeListSO unitTypeList = Resources.Load<UnitTypeListSO>(typeof(UnitTypeListSO).Name);
        int index = 0;
        foreach (UnitTypeSO unitType in unitTypeList.list) {
            Transform btnTransform = Instantiate(btnTemplateOption, transform);
            btnTransform.SetParent(optionContainer);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;

            btnTransform.Find("text").GetComponent<Text>().text = unitType.name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                // Click here
            });

            btnTransformDictionary[unitType] = btnTransform;

            index++;
        }
    }

    private bool AddConstructionQueueEntry() {
        if (constructionQueue.Count >= 5) {
            return false;
        }
        Transform btnTransform = Instantiate(btnTemplateQueue, transform);
        btnTransform.SetParent(queueContainer);
        btnTransform.gameObject.SetActive(true);

        btnTransform.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * constructionQueue.Count;

        btnTransform.Find("text").GetComponent<Text>().text = testUnitType.name + constructionQueue.Count;

        btnTransform.GetComponent<Button>().onClick.AddListener(() => {
            // Click here
        });

        constructionQueue.Enqueue(new ConstructionQueueEntry(btnTransform, testUnitType));

        return true;
    }

    private UnitTypeSO PopConstructionQueueEntry() {
        if (constructionQueue.Count == 0) {
            return null;
        } 
        ConstructionQueueEntry constructionQueueEntry = constructionQueue.Dequeue();
        Destroy(constructionQueueEntry.btn.gameObject);

        int index = 0;
        foreach (ConstructionQueueEntry entry in constructionQueue) {
            entry.btn.GetComponent<RectTransform>().anchoredPosition = initialOffSet + offset * index;
            index++;
        }

        return constructionQueueEntry.unitType;
    }

    private class ConstructionQueueEntry {
        public Transform btn;
        public UnitTypeSO unitType;

        public ConstructionQueueEntry(Transform inBtn, UnitTypeSO inUnitType) {
            btn = inBtn;
            unitType = inUnitType;
        }
    }
}
