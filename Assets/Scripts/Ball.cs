using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

     public Vector3 lockedPosition;
     private Vector3 startingPosition;
     private Vector3 startingPivotPosition;
     private Vector3 launchDirection = new Vector3(0, 1, 1);
     public GameObject pivot;
     public float radius;
     public GameObject controllerObject;
     private BallController controller;

     private Rigidbody rb;
     private bool locked = false;
     public bool InPlay { get; private set; }
     private float currentAngle = 0;
     private float nextAngle = 0;
     private float itpDelta = 0.2f;
     private float itpTime = 0;

     public void SetAngle(float angle) {
          nextAngle = angle;
     }

     private float InterpolateAngle(float alpha) {
          if (alpha > 1) alpha = 1;
          if (alpha < 0) alpha = 0;

          return Mathf.Lerp(currentAngle, nextAngle, alpha);
          //return currentAngle + (nextAngle - currentAngle) * alpha;
     }

     public void Launch(Vector3 force, Vector3 spin) {
          InPlay = true;

          rb.isKinematic = false;
          rb.useGravity = true;
          rb.AddForce(force, ForceMode.Impulse);
          rb.AddTorque(spin, ForceMode.Impulse);
     }

     public void Reset() {
          InPlay = false;
          transform.position = startingPosition;
          transform.rotation = Quaternion.Euler(0, 0, 0);
          rb.isKinematic = true;
          rb.useGravity = false;
     }

     public void Lock() {
          locked = true;
          rb.position = lockedPosition;
          rb.constraints = RigidbodyConstraints.FreezeAll;
     }

     public void Unlock() {
          locked = false;
          rb.position = startingPosition;
          rb.constraints = RigidbodyConstraints.None;
     }

     // Use this for initialization
     void Start() {
          startingPosition = transform.position;
          lockedPosition = transform.position;
          startingPivotPosition = pivot.transform.position;
          rb = GetComponent<Rigidbody>();
          controller = controllerObject.GetComponent<BallController>();
          Reset();
          controller.Reset();
     }

     // Update is called once per frame
     void Update() {
          if (InPlay) {
               // stuff?
          } else if (controller.HasPlayerLaunched()) {
               Launch(controller.GetLaunchForce(), controller.GetLaunchSpin());
          } else if (!locked) {
               //Update Angle
               float inputAngle = controller.GetInputAngle();
               if (inputAngle != nextAngle) {
                    currentAngle = nextAngle;
                    nextAngle = inputAngle;
                    itpTime = 0;
               } else {
                    itpTime += Time.deltaTime;
               }
               currentAngle = InterpolateAngle(itpTime / itpDelta);
               //Debug.Log("Current Angle" + currentAngle + "\nalpha: " + itpTime / itpDelta + "\nNextAngle " + nextAngle);

               //Update transform
               transform.position = pivot.transform.position + Quaternion.Euler(-currentAngle, 0, 0) * new Vector3(0, -radius, 0);

               //Update pivo
               pivot.transform.position = startingPivotPosition;
               pivot.transform.Translate(controller.GetLateralOffset(), 0, 0);
          }
     }
}