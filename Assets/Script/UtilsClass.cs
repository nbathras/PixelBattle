using UnityEngine;

public class UtilsClass {
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector2 GetMouseGridPosition(int gridSizex, int gridSizeY) {
        if (gridSizex != gridSizeY) {
            throw new System.Exception("Error: GetMosueGridPosition implementation can only handle squares");
        }

        Vector2 mousePosition = GetMouseWorldPosition();
        
        Vector2 positon = new Vector2(
            Mathf.RoundToInt(mousePosition.x),
            Mathf.RoundToInt(mousePosition.y)
        );
        if (gridSizex % 2 == 0) {
            positon += new Vector2(0.5f, 0.5f);
        }

        return positon;
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector) {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }
}
