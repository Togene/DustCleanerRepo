using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Movie_Player : MonoBehaviour {
    int vsyncprevious;
    MovieTexture movie;
    // Use this for initialization
    void Start ()
    {
        //Renderer r = GetComponent<Image>().material.mainTexture;
         movie = GetComponent<Image>().material.mainTexture as MovieTexture;
        movie.loop = true;

        vsyncprevious = QualitySettings.vSyncCount;
        QualitySettings.vSyncCount = 0;

        movie.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!movie.isPlaying)
        {
            QualitySettings.vSyncCount = vsyncprevious;
        }
    }
}
