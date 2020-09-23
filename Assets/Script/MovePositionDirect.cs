/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

 /* Modified by Noah Bathras
  * 
  */

using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition {

    private Vector3 movePosition;

    private void Awake() {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 movePosition) {
        this.movePosition = movePosition;
    }

    // ToDo: once close enough disable and then enable again when SetMovePosition is called
    private void Update() {
        Vector3 moveDir = (movePosition - transform.position).normalized;
        if (Vector3.Distance(movePosition, transform.position) < .1f) {
            // Stop moving when near
            moveDir = Vector3.zero;
        }
        GetComponent<IMoveVelocity>().SetVelocity(moveDir);
    }
}
