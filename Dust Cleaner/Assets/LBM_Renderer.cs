using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBM_Renderer : MonoBehaviour
{
    public Material bufA, bufB, bufC, bufD, Main;
    RenderTexture A, B, C, D, M;
    public int rTexSize = 4;
    // Use this for initialization
    void Start ()
    {
        A = B = C = D = M = new RenderTexture(rTexSize, rTexSize, 16);

        bufA.SetInt("iFrame", Time.frameCount);
        bufA.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufB.SetInt("iFrame", Time.frameCount);
        bufB.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufC.SetInt("iFrame", Time.frameCount);
        bufC.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufD.SetInt("iFrame", Time.frameCount);
        bufD.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufA.mainTexture = B;
        bufB.mainTexture = C;
        bufC.mainTexture = D;
        bufD.mainTexture = A;
        Main.mainTexture = A;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Render A

        bufA.SetInt("iFrame", Time.frameCount);
        bufA.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufB.SetInt("iFrame", Time.frameCount);
        bufB.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufC.SetInt("iFrame", Time.frameCount);
        bufC.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufD.SetInt("iFrame", Time.frameCount);
        bufD.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        Graphics.Blit(null, A, bufA); // def draws the Texture
        Graphics.Blit(null, B, bufB);
        Graphics.Blit(null, C, bufC);
        Graphics.Blit(null, D, bufD);


        bufA.mainTexture = B;
        bufB.mainTexture = C;
        bufC.mainTexture = D;
        bufD.mainTexture = A;
        Main.mainTexture = A;


        bufA.SetFloat("_TexSize", rTexSize);
        bufB.SetFloat("_TexSize", rTexSize);
        bufC.SetFloat("_TexSize", rTexSize);
        bufD.SetFloat("_TexSize", rTexSize);

        Main.SetFloat("_TexSize", rTexSize);
    }
     
    public void UpdateTextures()
    {

        //Blit sets dest to be active render texture, sets source as _MainTex property on the material, and draws a full - screen quad.
        bufA.SetInt("iFrame", Time.frameCount);
        bufA.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufB.SetInt("iFrame", Time.frameCount);
        bufB.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufC.SetInt("iFrame", Time.frameCount);
        bufC.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        bufD.SetInt("iFrame", Time.frameCount);
        bufD.SetVector("iMouse", Camera.main.ViewportToScreenPoint(Input.mousePosition));

        Graphics.Blit(null, A, bufA); // def draws the Texture
        Graphics.Blit(null, B, bufB);
        Graphics.Blit(null, C, bufC);
        Graphics.Blit(null, D, bufD);


        bufA.mainTexture = B;
        bufB.mainTexture = C;
        bufC.mainTexture = D;
        bufD.mainTexture = A;
        Main.mainTexture = A;

        // Graphics.Blit(null, M, Main);


    }

}
