using UnityEngine;

[RequireComponent(typeof(UnitRTS))]
public class Weapon : MonoBehaviour {

    [SerializeField] private WeaponTypeSo weaponTypeSo;

    // Componenets
    private LineRenderer weaponRangeLR;
    private UnitRTS unitRTS;

    // Timers
    private float lookForTargetTimer;
    private float shootTimer;

    // Other
    private UnitRTS currentTarget;

    private void Awake() {
        // Find Componenets
        weaponRangeLR = transform.Find("weaponRangeLR").GetComponent<LineRenderer>();
        unitRTS = GetComponent<UnitRTS>();

        // Initalize Values
        lookForTargetTimer = weaponTypeSo.fireRate;
        shootTimer = weaponTypeSo.fireRate;
    }

    private void Start() {
        weaponRangeLR.gameObject.SetActive(false);

        // Subscriptions
        unitRTS.OnSetSelectedVisibleChanged += UnitRTS_OnSetSelectedVisibleChanged;
    }

    private void UnitRTS_OnSetSelectedVisibleChanged(object sender, UnitRTS.OnSetSelectedVisibleChangedEventArgs e) {
        weaponRangeLR.gameObject.SetActive(e.isVisible);
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
        DrawWeaponRangeLR();
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += weaponTypeSo.fireRate;
            LookForTarget();
        }
    }

    private void HandleShooting() {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f) {
            shootTimer += weaponTypeSo.fireRate;
            if (currentTarget != null) {
                Vector3 shootPosition = transform.position + (currentTarget.transform.position - transform.position).normalized * 0.6f;
                Bullet.Create(shootPosition, currentTarget, weaponTypeSo);
            }
        }
    }

    private void LookForTarget() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, weaponTypeSo.fireRange);

        currentTarget = null;

        foreach (Collider2D collider2D in collider2DArray) {
            UnitRTS unit = collider2D.GetComponent<UnitRTS>();
            if (unit != null && !unit.IsTeammate(unitRTS)) {
                if (currentTarget == null) {
                    currentTarget = unit;
                } else {
                    if (Vector3.Distance(transform.position, unit.transform.position) <
                        Vector3.Distance(transform.position, currentTarget.transform.position)) {
                        // Closer!
                        currentTarget = unit;
                    }
                }
            }
        }
    }

    public void DrawWeaponRangeLR() {
        int segments = 360;
        float lineWidth = .2f;

        weaponRangeLR.useWorldSpace = false;
        weaponRangeLR.startWidth = lineWidth;
        weaponRangeLR.endWidth = lineWidth;
        weaponRangeLR.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * weaponTypeSo.fireRange, Mathf.Cos(rad) * weaponTypeSo.fireRange, 0);
        }

        weaponRangeLR.SetPositions(points);
    }
}
