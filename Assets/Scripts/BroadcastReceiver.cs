using UnityEngine;
using System.Collections;
using System;

public class BroadcastReceiver : BallController
{
     private float stepThreshold = 0.1f;
     private float inputAngle = 0;
     private float angleMod = 13f;

     private const float launchThreshold = 1.5f;
     private float inputAccel;

     private float inputEffect = 0;

     private const float maxLaunchMagnitude = 150;
     public Vector3 launchVector = new Vector3(0, 0.25f, 1);
     public float launchModifier = 60;

     public float Angle { get { return inputAngle * angleMod; } private set { inputAngle = value; } }

    AndroidJavaClass jc;
    string javaMessage = "Ciao";

    void Start()
    {
        Debug.Log("Starting Receiver");
        // Acces the android java receiver we made
        jc = new AndroidJavaClass("com.visport.unitypluginlibrary.UnityReceiver");
        // We call our java class function to create our MyReceiver java object
        jc.CallStatic("createInstance");
        Debug.Log("Started receiver");
    }

    void Update()
    {
        //Debug.Log("Update");
        // We get the text property of our receiver
        javaMessage = jc.GetStatic<string>("message");

          string[] parts = javaMessage.Split(" ".ToCharArray());
          
          var z = float.Parse(parts[2]);
          var y = float.Parse(parts[1]);
          var x = float.Parse(parts[0]);

          if (Math.Abs(z) > stepThreshold) {
               inputAngle -= z;
               inputAccel = z;
          }

          inputEffect = -x / 30;

          if (inputEffect < -0.025) {
               inputEffect = -0.025f;
          } else if (inputEffect > 0.025) {
               inputEffect = 0.025f;
          }

          //Debug.Log(javaMessage + " " + parts.Length + "\nAngle: " + Angle);

          //Debug.Log("Message: " + javaMessage);
     }

     private float RadianToDegree(float rad) {
          return (float)rad * 180 / Mathf.PI;
     }

     public override float GetInputAngle() {
          return Angle;
     }

     public override bool HasPlayerLaunched() {
          return inputAccel > launchThreshold;
     }

     public override Vector3 GetLaunchForce() {
          return (launchVector + new Vector3(inputEffect,0,0)).normalized * Math.Min(launchModifier * inputAccel, maxLaunchMagnitude);
     }

     public override Vector3 GetLaunchSpin() {
          return Vector3.zero;
     }

     public override float GetLateralOffset() {
          return 0;
     }

     public override void Reset() {
          inputAngle = 0;
     }
}