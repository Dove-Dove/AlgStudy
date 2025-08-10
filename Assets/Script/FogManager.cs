using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public static FogManager Instance;

    public int textureSize = 512;
    public float worldSize = 100f;

    public RenderTexture fogTexture;

    private Texture2D clearTexture;
    private Camera fogCam;

    void Awake()
    {
        Instance = this;

        // RenderTexture ����
        fogTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
        fogTexture.Create();

        // �þ� ī�޶� ���� (RT�� ����)
        GameObject fogCamObj = new GameObject("FogCamera");
        fogCam = fogCamObj.AddComponent<Camera>();
        fogCam.orthographic = true;
        fogCam.orthographicSize = worldSize / 2;
        fogCam.clearFlags = CameraClearFlags.Color;
        fogCam.backgroundColor = Color.black;
        fogCam.cullingMask = LayerMask.GetMask("FogVision");
        fogCam.targetTexture = fogTexture;

        // �ʱ�ȭ�� Texture2D
        clearTexture = new Texture2D(textureSize, textureSize);
        ClearFog();
    }

    public void ClearFog()
    {
        Color[] cols = new Color[textureSize * textureSize];
        for (int i = 0; i < cols.Length; i++)
            cols[i] = Color.black;

        clearTexture.SetPixels(cols);
        clearTexture.Apply();

        Graphics.Blit(clearTexture, fogTexture);
    }

    public RenderTexture GetFogTexture()
    {
        return fogTexture;
    }
}