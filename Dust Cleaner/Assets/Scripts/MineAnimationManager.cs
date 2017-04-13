using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class line
{
    public GameObject[] points;
    public GameObject obj;
    Vector3 startPos;

    public line(int size, GameObject _obj)
    {
        obj = _obj;
        points = new GameObject[size];
        startPos = _obj.transform.position;
    }


    public void initalize()
    {
        startPos = obj.transform.position;
    }

    public void UpdateObject(float speed)
    {
        int i = 0;
        Vector3 pos = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
        Vector3 p = points[i].transform.position;

        obj.transform.position = Vector3.Lerp(startPos, p, speed * points.Length);        

    }
}

public class MineAnimationManager : MonoBehaviour
{
    public GameObject[] objects;
    public line[] lines;

     [Range(0, 1)]
        public float lerpVal = 0;

    public int[] skippable;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < lines.Length; i++)
            lines[i].initalize();

    }
	
	// Update is called once per frame
	void Update ()
    {
        for(int i = 0; i < lines.Length; i++)
            lines[i].UpdateObject(lerpVal);

    }
}
