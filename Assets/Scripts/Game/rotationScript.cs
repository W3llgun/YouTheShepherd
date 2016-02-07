using UnityEngine;
using System.Collections;

public class rotationScript : MonoBehaviour {

	private float rotSpeed;
	void Start ()
	{
		rotSpeed = -0.6f;
        transform.Rotate(new Vector3(-30, 180, 0));
	}
	void Update () {
		transform.Rotate (new Vector3 (0, rotSpeed, 0));
	
	}
}
