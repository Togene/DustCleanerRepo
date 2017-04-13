using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBM_Renderer : MonoBehaviour
{
    public Material bufA, bufB, bufC, bufD, Main;
    RenderTexture A, B, C, D, M;
    public Vector2 rTexSize;

    [Range(0.000f, 5.000f)]
    public float w = 1.95f;
    // Use this for initialization
    public float updateInterval;
    private float lastUpdateTime = 0;

    public int interations;

    void Start ()
    {
        A = new RenderTexture((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        B = new RenderTexture((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        C = new RenderTexture((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        D = new RenderTexture((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        M = new RenderTexture((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);

        A.wrapMode = TextureWrapMode.Clamp;
        B.wrapMode = TextureWrapMode.Clamp;
        D.wrapMode = TextureWrapMode.Clamp;
        D.wrapMode = TextureWrapMode.Clamp;
        M.wrapMode = TextureWrapMode.Clamp;

        A.filterMode = FilterMode.Point;
        B.filterMode = FilterMode.Point;
        C.filterMode = FilterMode.Point;
        M.filterMode = FilterMode.Point;
        D.filterMode = FilterMode.Bilinear;

        bufA.mainTexture = D;
        bufB.mainTexture = A;

        bufC.mainTexture = B;
        bufD.mainTexture = C;

        Main.mainTexture = A;

        UpdateTextures();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > lastUpdateTime + updateInterval)
        {
            for(int i = 0; i < interations; i++)
            UpdateTextures();

            lastUpdateTime = Time.time;
        }
    }
     
    public void UpdateTextures()
    {
        // w = 1.95
        Vector4 mPoint = Camera.main.ViewportToScreenPoint(Input.mousePosition);
        Vector2 pixelUv = RayCast.texCoords;
        //pixelUv *= rTexSize;
        //pixelUv *= rTexSize;

        if (Input.GetMouseButton(0))
        {
            bufA.SetVector("iMouse", pixelUv);
            bufB.SetVector("iMouse", pixelUv);
            bufC.SetVector("iMouse", pixelUv);
            bufD.SetVector("iMouse", pixelUv);
        }


        bufA.SetInt("iFrame", Time.frameCount);
        bufB.SetInt("iFrame", Time.frameCount);
        bufC.SetInt("iFrame", Time.frameCount);
        bufD.SetInt("iFrame", Time.frameCount);
    

        bufA.SetVector("_TexSize", new Vector2((int)rTexSize.x, (int)rTexSize.y));
        bufB.SetVector("_TexSize", new Vector2((int)rTexSize.x, (int)rTexSize.y));
        bufC.SetVector("_TexSize", new Vector2((int)rTexSize.x, (int)rTexSize.y));
        bufD.SetVector("_TexSize", new Vector2((int)rTexSize.x, (int)rTexSize.y));
        Main.SetVector("_TexSize", new Vector2((int)rTexSize.x, (int)rTexSize.y));

        bufA.SetFloat("_W", w);
        bufB.SetFloat("_W", w);
        bufC.SetFloat("_W", w);
        bufD.SetFloat("_W", w);


        RenderTexture Aresults = RenderTexture.GetTemporary((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        Aresults.filterMode = FilterMode.Point;
        Aresults.wrapMode = TextureWrapMode.Clamp;
        Aresults.enableRandomWrite = true;

        Graphics.Blit(A, Aresults, bufA); // def draws the Texture
        Graphics.Blit(Aresults, A); // def draws the Texture

        RenderTexture Bresults = RenderTexture.GetTemporary((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        Bresults.filterMode = FilterMode.Point;
        Bresults.wrapMode = TextureWrapMode.Clamp;
        Bresults.enableRandomWrite = true;

        Graphics.Blit(B, Bresults, bufB); // def draws the Texture
        Graphics.Blit(Bresults, B); // def draws the Texturew

        RenderTexture Cresults = RenderTexture.GetTemporary((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        Cresults.filterMode = FilterMode.Point;
        Cresults.wrapMode = TextureWrapMode.Clamp;
        Cresults.enableRandomWrite = true;

        Graphics.Blit(C, Cresults, bufC); // def draws the Texture
        Graphics.Blit(Cresults, C); // def draws the Texture

        RenderTexture Dresults = RenderTexture.GetTemporary((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        Dresults.filterMode = FilterMode.Bilinear;
        Dresults.wrapMode = TextureWrapMode.Clamp;
        Dresults.enableRandomWrite = true;

        Graphics.Blit(D, Dresults, bufD); // def draws the Texture
        Graphics.Blit(Dresults, D); // def draws the Texturew

        RenderTexture Mresults = RenderTexture.GetTemporary((int)rTexSize.x, (int)rTexSize.y, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
        Mresults.filterMode = FilterMode.Trilinear;
        Mresults.wrapMode = TextureWrapMode.Clamp;

        Graphics.Blit(M, Mresults, Main); // def draws the Texture
        Graphics.Blit(Mresults, M); // def draws the Texturew

        Aresults.Release();
        Bresults.Release();
        Cresults.Release();
        Dresults.Release();
        Mresults.Release();
    }

}
