using UnityEngine;
using System.Collections;
using System;

public class DebugMouseController : BallController {

     private const float ANGLE_MULTIPLIER = 10;

     public float forceMultiplier = 10;
     public Vector3 launchVector = new Vector3(0, 10, 10);
     private Vector3 LaunchVector { get { return launchVector * forceMultiplier; } set { launchVector = value; } }

     public Vector3 effectSpin = new Vector3();
     private Vector3 spin = Vector3.zero;
     private bool launched = false;

     private float angle;
     private float Angle { get { return angle; } set { angle = value * ANGLE_MULTIPLIER; } }

     private const float MAX_OFFSET = 2.5f;
     public float movementStepping = 0.1f;
     private float lateralOffset = 0;

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

     public override float GetLateralOffset() {
          return lateralOffset;
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

          if (Input.GetKeyDown(KeyCode.UpArrow)) {
               angle += ANGLE_MULTIPLIER;
          } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
               angle -= ANGLE_MULTIPLIER;
          }

          //Angle = Input.GetAxis("Mouse ScrollWheel");

          if (Input.GetKey(KeyCode.A)) {
               lateralOffset -= movementStepping;
          } else if (Input.GetKey(KeyCode.D)) {
               lateralOffset += movementStepping;
          }

          if (lateralOffset < -MAX_OFFSET)
               lateralOffset = -MAX_OFFSET;
          else if (lateralOffset > MAX_OFFSET)
               lateralOffset = MAX_OFFSET;
	}

     public override void Reset() {
          return;
     }
}
