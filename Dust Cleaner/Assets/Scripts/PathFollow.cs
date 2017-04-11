using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[ExecuteInEditMode]
public class Path
{
    
    public Transform trns;
    public GameObject[] points;

    public enum PathType
    {
        Stright,
        Curved,
        Custom,
    };

    public PathType pType;

    public Path(Transform _trns, int numPoints, PathType type = PathType.Stright)
    {
       
        trns = _trns;
        pType = type;

        if (numPoints < 1 && type == PathType.Stright)
            Debug.LogError("Cannot Define Path, Need More Then One Point");
        else if (numPoints < 4 && type == PathType.Curved)
            Debug.LogError("Cannot Define Curve, Need More Then 3 Points");
        else
        points = new GameObject[numPoints];
    }


    public void MoveTransform(float speed, float thresHold)
    {
        for(int i = 0; i < points.Length; i++)
        {
            Vector3 p0 = points[i].transform.position;
            Vector3 p1 = points[i + 1 % points.Length].transform.position;

            if ((trns.position.magnitude - p1.magnitude) <= thresHold)
            {
                p0 = points[i + 1 % points.Length].transform.position;
                p1 = points[i + 2 % points.Length].transform.position; 
            }

            trns.position = Vector3.Lerp(p0, p1, Time.deltaTime * speed);
  
        }

    }

    public void DrawPath()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p0 = points[i].transform.position;
            Vector3 p1 = points[i + 1 % points.Length].transform.position;

            Gizmos.DrawSphere(p0, 0.5f);
            Gizmos.DrawSphere(p1, 0.5f);

            Gizmos.DrawLine(p0, p1);
        }

    }

}


public class PathFollow : MonoBehaviour
{
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDrawGizmos()
    {

    }


}
