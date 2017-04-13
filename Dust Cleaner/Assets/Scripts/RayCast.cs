using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

    public static Ray ray;
    public static Vector2 texCoords;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin , ray.direction * 100);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hitting");
            texCoords = hit.textureCoord;
        }
    }
}
