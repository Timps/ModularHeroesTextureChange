using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ModularColorChanger : EditorWindow
{
    [MenuItem("Tools/GameDevBits/Modular Heroes colour change")]
    public static void ShowWindow()
    {
        Vector2 dockWindow = new Vector2(400.0f, 292.0f);
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow(typeof(ModularColorChanger));
        window.minSize = dockWindow;
    }


    void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Modular Heroes Colours", EditorStyles.boldLabel);
        GUILayout.Label("Change colours of the Synty Modular Heroes texture for use with other shaders");
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Set Colours", GUILayout.Width(189)))
        {

        }
        foreach (ModularColorTile tile in colorTiles)
        {
            GUILayout.Label("Colours", EditorStyles.boldLabel);
        }

    }

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

    public struct ModularColorTile
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
            CreateTexture(tile.color, (int)tile.size.x, (int)tile.size.y);
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