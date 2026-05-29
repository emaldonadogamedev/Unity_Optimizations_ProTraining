using UnityEngine;

public class PackingManager : MonoBehaviour
{
    [SerializeField]
    Texture2D[] inputTextures;
    Rect[] rects;

    void Start()
    {
        // For education purposes, packing 8 512x512 textures into a grid of 4 x 2
        // width is 4*512 = 2048
        // height is 2*512 = 1024
        var atlast = new Texture2D(2048, 1024);
        rects = atlast.PackTextures(inputTextures, 0, 2048);
    }
}
