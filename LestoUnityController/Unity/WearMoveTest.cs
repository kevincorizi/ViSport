using UnityEngine;
using System.Collections;

public class WearMoveTest : MonoBehaviour {
    Quaternion _toRotation = Quaternion.identity;
    private const double RADIANS_DEGREES = 180.0 / System.Math.PI;

	// Use this for initialization
	void Start () {
	
	}

    private float RadianToDegree(float angle)
    {
        return (float)(angle * RADIANS_DEGREES);
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Lerp(transform.rotation, _toRotation, Time.deltaTime * 10);
	}

    public void WearOrientationChanged(string message)
    {
        var parts = message.Split(" ".ToCharArray());
        var wearAzimuth = 0 - RadianToDegree(float.Parse(parts[2]));
        var wearPitch = 0 - (180 + RadianToDegree(float.Parse(parts[1])));
        var wearRoll = RadianToDegree(float.Parse(parts[0]));
        _toRotation = Quaternion.Euler(wearPitch, wearRoll, wearAzimuth);
    }
}
