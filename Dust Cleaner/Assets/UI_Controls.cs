using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controls : MonoBehaviour {

    public GameObject Duster, BoreHole, Turbulance, Mapping;

    public GameObject mapButton;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
	}

    public void MapVisualize()
    {
        Debug.Log("Mixer SHow");
        OnOff(Mapping);
    }

    public void MixerVisualize()
    {
        Debug.Log("Mixer SHow");
        OnOff(Turbulance);
        OnOff(mapButton);
        OnOff(Mapping);
    }

    public void BoreHoleVisualize()
    {
        Debug.Log("BoreHole SHow");
        OnOff(BoreHole);
        OnOff(mapButton);
        OnOff(Mapping);
    }

    public void DusterVisualize()
    {
        Debug.Log("Duster SHow");
        OnOff(Duster);
        OnOff(mapButton);
        OnOff(Mapping);
    }

    public void OnOff(GameObject obj)
    {
        if (!obj.activeSelf)
            obj.gameObject.SetActive(true);
        else
            obj.gameObject.SetActive(false);
    }
}
