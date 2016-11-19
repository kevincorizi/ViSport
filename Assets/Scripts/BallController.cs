using UnityEngine;
using System.Collections;

public abstract class BallController : MonoBehaviour {
     public abstract float GetInputAngle();
     public abstract bool HasPlayerLaunched();
     public abstract Vector3 GetLaunchForce();
     public abstract Vector3 GetLaunchSpin();
     public abstract float GetLateralOffset();
     public abstract void Reset();
}
