
using UnityEngine;

public class BuildingOverlay : MonoBehaviour {

    [SerializeField] private Building building;

    private Transform barTransform;

    private void Start() {
        barTransform = transform.Find("bar");
    }

    private void Update() {
        barTransform.localScale = new Vector3(1 - building.GetTimerNormalized(), 1, 1);
    }
}
