using UnityEngine;
using System.Collections;

public abstract class BallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     public abstract float GetInputAngle();
     public abstract bool HasPlayerLaunched();
     public abstract Vector3 GetLaunchForce();
     public abstract Vector3 GetLaunchSpin();

}
