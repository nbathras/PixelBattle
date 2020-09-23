using System;
using UnityEngine;

[RequireComponent(typeof(IMovePosition))]
public class UnitRTS : MonoBehaviour {

    [SerializeField] private TeamSO team;

    private SpriteRenderer selectedSprite;
    private SpriteRenderer spriteRenderer;

    private IMovePosition movePosition;

    public event EventHandler<OnSetSelectedVisibleChangedEventArgs> OnSetSelectedVisibleChanged;

    public class OnSetSelectedVisibleChangedEventArgs : EventArgs {
        public bool isVisible;
    }

    private void Awake() {
        selectedSprite = transform.Find("selectedSprite").GetComponent<SpriteRenderer>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();

        movePosition = GetComponent<IMovePosition>();

        spriteRenderer.color = team.color;

        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible) {
        OnSetSelectedVisibleChanged?.Invoke(
            this,
            new OnSetSelectedVisibleChangedEventArgs {
                isVisible = visible
            }
        );

        selectedSprite.gameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition) {
        movePosition.SetMovePosition(targetPosition);
    }

    public bool IsPlayerControllable() {
        return team.isPlayerControlled;
    }

    public bool IsTeammate(UnitRTS unitRts) {
        if (team == unitRts.team) {
            return true;
        } else {
            return false;
        }
    }
}
