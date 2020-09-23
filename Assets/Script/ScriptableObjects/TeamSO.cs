using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Team")]

public class TeamSO : ScriptableObject {
    public bool isPlayerControlled;
    public Color color;
}
