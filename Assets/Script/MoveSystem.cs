using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UnitDataHolder))]
public class MoveSystem : MonoBehaviour {

    // Set in Awake
    private Rigidbody2D rigidbody2D;
    private Vector3 movePosition;

    // Set in Start
    private float speed;

    // Set elsewhere
    private Vector3 velocityVector;


    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Set inital position
        movePosition = transform.position;
    }

    private void Start() {
        speed = GetComponent<UnitDataHolder>().unitType.speed;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * speed;
    }

    // ToDo: once close enough disable and then enable again when SetMovePosition is called
    private void Update() {
        Vector3 moveDir = (movePosition - transform.position).normalized;
        if (Vector3.Distance(movePosition, transform.position) < .1f) {
            // Stop moving when near
            moveDir = Vector3.zero;
        }
        SetVelocity(moveDir);
    }

    private void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    public void SetMovePosition(Vector3 movePosition) {
        this.movePosition = movePosition;
    }

    public void Disable() {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }
}
