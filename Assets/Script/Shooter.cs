using UnityEngine;

[RequireComponent(typeof(UnitRTS))]
public class Shooter : MonoBehaviour {

    [SerializeField] private UnityTypeSO unitType;

    // [SerializeField] private float rangeRadius;

    private float lookForTargetTimer;
    // [SerializeField] private float lookForTargetTimerMax;

    private float shootTimer;
    // [SerializeField] private float shootTimerMax;

    // [SerializeField] private float circleWidth;
    // [SerializeField] private int segments = 50;
    private LineRenderer rangeCircle;

    private UnitRTS unitRTS;

    private UnitRTS target;


    private void Awake() {
        rangeCircle = transform.Find("rangeCircle").GetComponent<LineRenderer>();
        DrawRadius();
        rangeCircle.gameObject.SetActive(false);

        lookForTargetTimer = unitType.fireRate;
        shootTimer = unitType.fireRate;
    }

    public void DrawRadius() {
        int segments = 50;

        rangeCircle.positionCount = segments + 1;

        rangeCircle.startWidth = .2f;
        rangeCircle.endWidth = .2f;

        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        rangeCircle.material = whiteDiffuseMat;

        float x, y, z;
        float angle = 20f;

        for (int i = 0; i < (segments + 2); i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * unitType.fireRange + transform.position.x;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * unitType.fireRange + transform.position.y;

            rangeCircle.SetPosition(i, new Vector3(x, y, 0f));

            angle += (360f / segments);
        }
    }

    private void Start() {
        unitRTS = GetComponent<UnitRTS>();

        unitRTS.OnSetSelectedVisibleChanged += UnitRTS_OnSetSelectedVisibleChanged;
    }

    private void UnitRTS_OnSetSelectedVisibleChanged(object sender, UnitRTS.OnSetSelectedVisibleChangedEventArgs e) {
        rangeCircle.gameObject.SetActive(e.isVisible);
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
        DrawRadius();
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += unitType.fireRate;
            LookForTarget();
        }
    }

    private void HandleShooting() {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f) {
            shootTimer += unitType.fireRate;
            if (target != null) {
                Vector3 shootPosition = transform.position + (target.transform.position - transform.position).normalized * 0.6f;
                Bullet.Create(shootPosition, target, unitType.damage);
            }
        }
    }

    private void LookForTarget() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, unitType.fireRange);

        target = null;

        foreach (Collider2D collider2D in collider2DArray) {
            UnitRTS unit = collider2D.GetComponent<UnitRTS>();
            if (unit != null && !unit.IsTeammate(unitRTS)) {
                if (target == null) {
                    target = unit;
                } else {
                    if (Vector3.Distance(transform.position, unit.transform.position) <
                        Vector3.Distance(transform.position, target.transform.position)) {
                        // Closer!
                        target = unit;
                    }
                }
            }
        }
    }
}
