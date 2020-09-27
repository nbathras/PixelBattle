using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private int targetFrameRate = 60;

    void Awake() {
        Application.targetFrameRate = targetFrameRate;
    }
}
