using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controls : MonoBehaviour {

    public GameObject Duster, BoreHole, Turbulance, Mapping;
    public MineAnimationManager lerpControls;
    public GameObject mapButton;
    public Slider slideToLerp;
	// Use this for initialization
	void Start ()
    {
        lerpControls = FindObjectOfType<MineAnimationManager>();

    }
	
	// Update is called once per frame
	void Update ()
    {

        lerpControls.lerpVal = slideToLerp.value;

		if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
	}

    public void MapVisualize()
    {
        Debug.Log("Mixer SHow");
        OnOff(slideToLerp.gameObject);
        OnOff(Mapping);
        lerpControls.lerpVal = 0;
        slideToLerp.value = 0;
    }

    public void MixerVisualize()
    {
        Debug.Log("Mixer SHow");
        OnOff(mapButton);
        OnOff(slideToLerp.gameObject);
        // OnOff(Mapping);

        if (!Turbulance.activeSelf)
        {
            lerpControls.lerpVal = 1;
            slideToLerp.value = 1;
            Turbulance.gameObject.SetActive(true);
        }

        else
        {
            lerpControls.lerpVal = 0;
            slideToLerp.value = 0;
            Turbulance.gameObject.SetActive(false);
        }

    }

    public void BoreHoleVisualize()
    {
        OnOff(slideToLerp.gameObject);
        Debug.Log("BoreHole SHow");
        OnOff(BoreHole);
        OnOff(mapButton);
        // OnOff(Mapping);
        lerpControls.lerpVal = 0;
        slideToLerp.value = 0;
    }

    public void DusterVisualize()
    {
        OnOff(slideToLerp.gameObject);
        Debug.Log("Duster SHow");
        OnOff(Duster);
        OnOff(mapButton);
        // OnOff(Mapping);
        lerpControls.lerpVal = 0;
        slideToLerp.value = 0;
    }

    public void OnOff(GameObject obj)
    {
        if (!obj.activeSelf)
            obj.gameObject.SetActive(true);
        else
            obj.gameObject.SetActive(false);
    }
}
