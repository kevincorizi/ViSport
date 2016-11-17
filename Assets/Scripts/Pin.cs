﻿using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour {

	public float standingThreshold = 3f;
	public float distToRaise = 2f;

    private Rigidbody rigidBody;
    private Vector3 startingPosition;
	
	// Use this for initialization
	void Start () {
		  rigidBody = GetComponent<Rigidbody> ();
          startingPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
          
	}
	
	public bool IsStanding () {
		Vector3 rotationInEuler = transform.rotation.eulerAngles;

		float tiltInX = Mathf.Abs(270 - rotationInEuler.x);
		float tiltInZ = Mathf.Abs(rotationInEuler.z);

		if (tiltInX < standingThreshold && tiltInZ < standingThreshold) {
          return true;
		} else {
          return false;
		}
	}

	public void Raise () {
          transform.localPosition = startingPosition;
          rigidBody.velocity = Vector3.zero;
          rigidBody.angularVelocity = Vector3.zero;
          rigidBody.useGravity = false;
		  transform.Translate (new Vector3 (0, distToRaise, 0), Space.World);
		  transform.rotation = Quaternion.Euler (270f, 0, 0);
	}

	public void Lower () {
          transform.Translate(new Vector3(0, -distToRaise, 0), Space.World);
          transform.rotation = Quaternion.Euler(270f, 0, 0);
          rigidBody.useGravity = true;
	}

}