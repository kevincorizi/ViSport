using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

     public Vector3 lockedPosition;
     private Vector3 startingPosition;
     private Vector3 launchDirection = new Vector3(0, 1, 1);
     public GameObject pivot;
     public float radius;
     public GameObject controllerObject;
     private BallController controller;

     private Rigidbody rb;
     private bool locked = true;
     public bool InPlay { get; private set; }
     private float currentAngle = 0;
     private float nextAngle = 0;
     private float itpDelta = 0.1f;
     private float itpTime = 0;

     public void SetAngle(float angle) {
          nextAngle = angle;
     }

     private float InterpolateAngle(float alpha) {
          if (alpha > 1) alpha = 1;
          if (alpha < 0) alpha = 0;

          return currentAngle + (nextAngle - currentAngle) * alpha;
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
	void Start () {
          startingPosition = transform.position;
          rb = GetComponent<Rigidbody>();
          controller = controllerObject.GetComponent<BallController>();
          Reset();
	}
	
	// Update is called once per frame
	void Update () {
	      if (InPlay) {
               // stuff?
          } else if (controller.HasPlayerLaunched()) {
               Launch(controller.GetLaunchForce(), controller.GetLaunchSpin());
          } else {
               //Update Angle
               float inputAngle = controller.GetInputAngle();
               if (inputAngle != nextAngle) {
                    nextAngle = inputAngle;
                    itpTime = 0;
               } else {
                    itpTime += Time.deltaTime;
               }
               currentAngle = InterpolateAngle(itpTime / itpDelta);

               //Update transform
               transform.position = pivot.transform.position;
               transform.Translate(0, -radius, 0);
               transform.RotateAround(pivot.transform.position, Vector3.right, currentAngle);
          }
	}
}