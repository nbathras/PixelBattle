using System;
using UnityEngine;

[RequireComponent(typeof(MoveSystem))]
[RequireComponent(typeof(UnitDataHolder))]
public class Unit : MonoBehaviour {
    // Set in Awake
    private UnitDataHolder unitDataHolder;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer outlineSpriteRenderer;
    private MoveSystem moveSystem;

    public event EventHandler<OnSetSelectedVisibleChangedEventArgs> OnSetSelectedVisibleChanged;

    public class OnSetSelectedVisibleChangedEventArgs : EventArgs {
        public bool isVisible;
    }

    private void Awake() {
        unitDataHolder = GetComponent<UnitDataHolder>();
        moveSystem = GetComponent<MoveSystem>();
        outlineSpriteRenderer = transform.Find("outlineSpriteRenderer").GetComponent<SpriteRenderer>();
        spriteRenderer = transform.Find("spriteRenderer").GetComponent<SpriteRenderer>();
    }

    private void Start() {
        spriteRenderer.color = unitDataHolder.unitTeam.color;
        SetOutlineVisible(false);
    }

    public void Initalize(UnitTypeSO inUnitType, TeamSO inTeam) {
        unitDataHolder.unitType = inUnitType;
        unitDataHolder.unitTeam = inTeam;
    }

    public void SetOutlineVisible(bool visible) {
        OnSetSelectedVisibleChanged?.Invoke(
            this,
            new OnSetSelectedVisibleChangedEventArgs {
                isVisible = visible
            }
        );

        outlineSpriteRenderer.gameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition) {
        moveSystem.SetMovePosition(targetPosition);
    }

    public bool IsPlayerControllable() {
        return unitDataHolder.unitTeam.isPlayerControlled;
    }

    public bool IsTeammate(Unit unit) {
        if (unitDataHolder.unitTeam == unit.unitDataHolder.unitTeam) {
            return true;
        } else {
            return false;
        }
    }
}
