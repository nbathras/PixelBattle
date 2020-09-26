using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitDataHolder))]
public class Weapon : MonoBehaviour {

    // Set in Awake
    private LineRenderer weaponRangeLineRenderer;
    private Unit unit;

    // Set in Start
    private WeaponData weaponData;
    private float lookForTargetTimer;
    private float shootTimer;

    // Other
    private Unit currentTarget;

    private void Awake() {
        weaponRangeLineRenderer = transform.Find("weaponRangeLineRenderer").GetComponent<LineRenderer>();
        unit = GetComponent<Unit>();
    }

    private void Start() {
        weaponData = GetComponent<UnitDataHolder>().unitType.weapon;

        weaponRangeLineRenderer.gameObject.SetActive(false);

        lookForTargetTimer = weaponData.fireRate;
        shootTimer = weaponData.fireRate;

        // Subscriptions
        unit.OnSetSelectedVisibleChanged += UnitRTS_OnSetSelectedVisibleChanged;
    }

    private void UnitRTS_OnSetSelectedVisibleChanged(object sender, Unit.OnSetSelectedVisibleChangedEventArgs e) {
        weaponRangeLineRenderer.gameObject.SetActive(e.isVisible);
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
        DrawWeaponRangeLR();
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += weaponData.fireRate;
            LookForTarget();
        }
    }

    private void HandleShooting() {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f) {
            shootTimer += weaponData.fireRate;
            if (currentTarget != null) {
                Vector3 shootPosition = transform.position + (currentTarget.transform.position - transform.position).normalized * 0.6f;
                Bullet.Create(shootPosition, currentTarget.transform.position, weaponData);
            }
        }
    }

    private void LookForTarget() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, weaponData.range);

        currentTarget = null;

        foreach (Collider2D collider2D in collider2DArray) {
            Unit target = collider2D.GetComponent<Unit>();
            if (target != null && !unit.IsTeammate(target)) {
                if (currentTarget == null) {
                    currentTarget = target;
                } else {
                    if (Vector3.Distance(transform.position, target.transform.position) <
                        Vector3.Distance(transform.position, currentTarget.transform.position)) {
                        // Closer!
                        currentTarget = target;
                    }
                }
            }
        }
    }

    public void DrawWeaponRangeLR() {
        int segments = 360;
        float lineWidth = .2f;

        weaponRangeLineRenderer.useWorldSpace = false;
        weaponRangeLineRenderer.startWidth = lineWidth;
        weaponRangeLineRenderer.endWidth = lineWidth;
        weaponRangeLineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * weaponData.range, Mathf.Cos(rad) * weaponData.range, 0);
        }

        weaponRangeLineRenderer.SetPositions(points);
    }
}
