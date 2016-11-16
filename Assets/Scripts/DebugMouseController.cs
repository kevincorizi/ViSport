using UnityEngine;
using System.Collections;
using System;

public class DebugMouseController : BallController {

     private const float ANGLE_MULTIPLIER = 100;

     public float forceMultiplier = 10;
     public Vector3 launchVector = new Vector3(0, 10, 10);
     private Vector3 LaunchVector { get { return launchVector * forceMultiplier; } set { launchVector = value; } }

     public Vector3 effectSpin = new Vector3();
     private Vector3 spin = Vector3.zero;
     private bool launched = false;

     private float angle;
     private float Angle { get { return angle; } set { angle = value * ANGLE_MULTIPLIER; } }

     public override float GetInputAngle() {
          return Angle;
     }

     public override Vector3 GetLaunchForce() {
          return LaunchVector;
     }

     public override Vector3 GetLaunchSpin() {
          return spin;
     }

     public override bool HasPlayerLaunched() {
          return launched;
     }

     // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
          if (Input.GetMouseButtonDown(0)) {
               launched = true;
               spin = Vector3.zero;
          } else if (Input.GetMouseButtonDown(1)) {
               launched = true;
               spin = effectSpin;
          } else {
               launched = false;
          }
          Angle = Input.GetAxis("Mouse ScrollWheel");
	}
}
