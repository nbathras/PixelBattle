using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

public class GameRTSController : MonoBehaviour {

    [SerializeField] private Transform selectionAreaTransform;
    [SerializeField] private bool changeFormation = false;

    private Vector3 startPosition;

    private List<UnitRTS> selectedUnitRTSList;

    private void Awake() {
        selectionAreaTransform.gameObject.SetActive(false);

        selectedUnitRTSList = new List<UnitRTS>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(0)) {
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0)) {
            // Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            foreach(UnitRTS unitRTS in selectedUnitRTSList) {
                unitRTS.SetSelectedVisible(false);
            }

            selectedUnitRTSList.Clear();

            foreach (Collider2D collider2D in collider2DArray) {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();

                if (unitRTS != null && unitRTS.IsPlayerControllable()) {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            // Right Mouse Button Pressed
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            List<Vector3> targetPositionList;
            if (changeFormation) {
                targetPositionList = GetFormationListCircle(moveToPosition, selectedUnitRTSList.Count);
            } else {
                targetPositionList = GetFormationListRectangle(moveToPosition, selectedUnitRTSList.Count, 2, 1);
            }

            int targetPositionListIndex = 0;

            foreach (UnitRTS unitRTS in selectedUnitRTSList) {
                unitRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private List<Vector3> GetFormationListRectangle(Vector3 startPosition, int numberOfRequestPositons, int widthRatio, int heightRatio) {
        List<Vector3> positionList = new List<Vector3>();

        float d = 1f;

        int multiplier = Mathf.CeilToInt(
            Mathf.Sqrt(numberOfRequestPositons / (float)(widthRatio * heightRatio))
        );
        int widithSize = multiplier * widthRatio;
        int heightSize = multiplier * heightRatio;

        Vector3 upperRight = new Vector3(
            startPosition.x + (d / 2) + (d * Mathf.Max(d * (widithSize / 2 - 1), 0)),
            startPosition.y + (d / 2) + (d * Mathf.Max(d * (heightSize / 2 - 1), 0))
        );

        for (int i = 0; i < numberOfRequestPositons; i++) {
            int x = i % widithSize;
            int y = Mathf.FloorToInt(i / widithSize);
            positionList.Add(new Vector3(
                upperRight.x - d * x,
                upperRight.y - d * y
            ));
        }

        return positionList;
    }

    private List<Vector3> GetFormationListCircle(Vector3 startPosition, int numberOfRequestPositons) {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);

        int index = 1;
        float ringeDistanceMultiplier = 1f;
        while(positionList.Count < numberOfRequestPositons) {
            float ringRadius = index * ringeDistanceMultiplier;
            int unitsPerRing = Mathf.RoundToInt(index * 2 * Mathf.PI);
            positionList.AddRange(GetPositionListAround(startPosition, ringRadius, unitsPerRing));
            index++;
        }

        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount) {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++) {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector3(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector3(Vector3 vec, float angle) {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
