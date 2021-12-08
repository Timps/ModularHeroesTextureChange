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

        if (GUILayout.Button("Load Modular Hero Defaults", GUILayout.Width(189)))
        {
            MHSampleData();
        }
        if (GUILayout.Button("Set Colours", GUILayout.Width(189)))
        {
            
            string texturePath = AssetDatabase.GetAssetPath(theTexture);
            int pos = texturePath.LastIndexOf("/") +1 ;
            string originalFileName = texturePath.Substring(pos, texturePath.Length - pos);
            string originalFolder = texturePath.Substring(0, pos - 1);
            LoopThroughTiles();
            //Debug.Log(originalFileName);
            //Debug.Log(originalFolder);

        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box");
        theTexture = (Texture)EditorGUILayout.ObjectField(theTexture, typeof(Texture), true);
        GUILayout.EndHorizontal();

        for (int i = 0; i < colorTiles.Count; i++)
        {
            GUILayout.Label(colorTiles[i].tileName, EditorStyles.boldLabel);
            colorTiles[i].color = EditorGUILayout.ColorField(colorTiles[i].tileName,
            colorTiles[i].color);
        }

    }

    public Color colorPicker;
    List<Color> colors;
    public string saveFolder;
    public Texture theTexture;
    public Texture EditTexture;
    string originalFileName;
    string originalFolder;


    Texture2D tex = null;

    public List<ModularColorTile> colorTiles = new List<ModularColorTile>();

    public class ModularColorTile
    {
        public string tileName;
        public Texture texture;
        public Vector2 startPosition;
        public Vector2 size;
        public Color color;


        public ModularColorTile(string theTile, Texture theTexture, Vector2 theStart, Vector2 theSize, Color theColor)
        {
            tileName = theTile;
            texture = theTexture;
            startPosition = theStart;
            size = theSize;
            color = theColor;
        }
    }

    

    public void LoopThroughTiles()
    {
        foreach (ModularColorTile tile in colorTiles)
        {
            Debug.Log("Looping through tiles. There are");
            CreateTexture(tile.color, (int)tile.size.x, (int)tile.size.y, (int)tile.startPosition.x, (int)tile.startPosition.y);
        }
    }

public void SetColour()
    {
       // CreateTexture(colorPicker, zoneWidth, zoneHeight);
    }

    void CreateTexture(Color colorChoice, int zoneWidth, int zoneHeight, int startX, int startY)
    {
        Debug.Log("Writing pixels to " + startX +","+ startY);
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
        var iconPath = $"Assets/IconOutput/{originalFileName}__MHCOLORCHANGE.png";
        //write the actual file
        File.WriteAllBytes(iconPath, bytes);
    }

    void MHSampleData()
    {
        colorTiles.Clear();
        colorTiles.Add(new ModularColorTile("Primary", EditTexture, new Vector2(268, 355), new Vector2(46, 46), new Color(0.6f, 0.3f, 0.2f)));
        colorTiles.Add(new ModularColorTile("Secondary", EditTexture, new Vector2(328, 355), new Vector2(46, 46), new Color(0.9f, 0.1f, 0.7f)));
        colorTiles.Add(new ModularColorTile("Leather Primary", EditTexture, new Vector2(373, 355), new Vector2(46, 46), new Color(0.9f, 0.3f, 0.4f)));
        colorTiles.Add(new ModularColorTile("Metal Primary", EditTexture, new Vector2(283, 310), new Vector2(46, 46), new Color(0.9f, 0.2f, 0.7f)));
        colorTiles.Add(new ModularColorTile("Leather Secondary", EditTexture, new Vector2(418, 355), new Vector2(46, 46), new Color(0.9f, 0.9f, 0.1f)));
        colorTiles.Add(new ModularColorTile("Metal Dark", EditTexture, new Vector2(374, 310), new Vector2(46, 46), new Color(0.9f, 0.3f, 0.9f)));
        colorTiles.Add(new ModularColorTile("Metal Secondary", EditTexture, new Vector2(328, 309), new Vector2(46, 46), new Color(0.9f, 0.1f, 0.6f)));
        colorTiles.Add(new ModularColorTile("Hair", EditTexture, new Vector2(0, 82), new Vector2(46, 46), new Color(0.9f, 0.2f, 0.1f)));
        colorTiles.Add(new ModularColorTile("Skin", EditTexture, new Vector2(0, 149), new Vector2(46, 46), new Color(0.8f, 0.1f, 0.2f)));
        colorTiles.Add(new ModularColorTile("Stubble", EditTexture, new Vector2(0, 23), new Vector2(46, 46), new Color(0.4f, 0.1f, 0.3f)));
        colorTiles.Add(new ModularColorTile("Scar", EditTexture, new Vector2(0, 151), new Vector2(46, 46), new Color(0.7f, 0.2f, 0.4f)));
        colorTiles.Add(new ModularColorTile("Body art", EditTexture, new Vector2(464, 256), new Vector2(46, 46), new Color(0.2f, 0.7f, 0.5f)));
        colorTiles.Add(new ModularColorTile("Eyes", EditTexture, new Vector2(0, 0), new Vector2(46, 46), new Color(0.1f, 0.3f, 0.5f)));

    }
}