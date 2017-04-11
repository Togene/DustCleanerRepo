using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rotate : MonoBehaviour {

    public GameObject pivotPoint;

    [Range(0, 100)]
    public float speed = 5;

    [Range(0, 10)]
    public float offsetScale = 5;

    public Vector3 offset;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //transform.Rotate(0, Mathf.PI / 4 * Time.deltaTime * speed, 0) ;
        transform.RotateAround(pivotPoint.transform.position, Vector3.up, (Mathf.PI / 4 * Time.deltaTime * speed));
        transform.LookAt(pivotPoint.transform);
	}
}
