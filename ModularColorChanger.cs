using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ModularColorChanger : MonoBehaviour
{

    public Color colorPicker;
    List<Color> colors;
    public string saveFolder;
    public Texture theTexture;
    public int startX;
    public int startY;
    public int zoneWidth;
    public int zoneHeight;

    Texture2D tex = null;

    public List<ModularColorTile> colorTiles = new List<ModularColorTile>();

    struct ModularColorTile()
    {
        public string tileName;
        public Texture texture;
        public Vector2 startPosition;
        public Vector2 size;
        public Color color;
    }

    public void LoopThroughTiles()
    {
        foreach (ModularColorTile tile in colorTiles)
        {
        
        }
    }

    public void SetColour()
    {
        CreateTexture(colorPicker, zoneWidth, zoneHeight);
    }

    void CreateTexture(Color colorChoice, int zonewidth, int zoneheight)
    {
        Color[] colors = new Color[zoneHeight*zoneWidth];
        for (int i = 0; i < zoneHeight * zoneWidth; i++)
        {
            colors[i] = colorChoice;
        }
        tex = new Texture2D(2, 2);
        tex = (Texture2D)theTexture;

        tex.SetPixels(startX, startY, zoneWidth, zoneHeight, colors, 0);
        var bytes = tex.EncodeToPNG();
        //We create the full path of folder and file name
        var iconPath = $"{saveFolder}/666.png";
        //write the actual file
        File.WriteAllBytes(iconPath, bytes);
    }
}