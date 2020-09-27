using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingConstructionSelectUI : MonoBehaviour {

    private Dictionary<UnitTypeSO, Transform> btnTransformDictionary;

    private void Awake() {
        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        UnitTypeListSO unitTypeList = Resources.Load<UnitTypeListSO>(typeof(UnitTypeListSO).Name);

        btnTransformDictionary = new Dictionary<UnitTypeSO, Transform>();

        int index = 0;
        float offsetAmountInital = 75f;
        float offsetAmount = 125f;
        foreach (UnitTypeSO unitType in unitTypeList.list) {
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector3(offsetAmountInital + offsetAmount * index, 75);

            btnTransform.Find("text").GetComponent<Text>().text = unitType.name;

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                // Click here
            });

            btnTransformDictionary[unitType] = btnTransform;

            index++;
        }
    }
}
