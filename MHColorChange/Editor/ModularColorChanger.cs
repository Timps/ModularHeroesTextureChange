using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;



public class ModularColorChange : EditorWindow
{

    public Color colorPicker;
    List<Color> colors;
    //We declare a string to use as the folder to save into. We’ll append “Assets/” to the start so it’s visible inside the project
    public string saveFolder = "_ModularTextures";
    //This is a public variable to hold a reference to the texture we’ll be working from.
    public Texture theTexture;
    //This is a public variable to reference the texture a specific tile is using. Right now, this variable has no effect. It’s here for future compatibility
    public Texture EditTexture;
    //This string will be populated with the original file name of the texture in ‘theTexture’. This is used to save the new texture, containing the name of the original.
    string originalFileName;
    //This string is used to hold the suffix we append onto the file name. The suffix helps make sure we don’t overwrite the original file.
    string newFileName = "_MHTexture";
    //This is a variable created for the new texture. We read the current texture into this, and then do all our colour changes on it in memory.
    Texture2D tex;


    //This is a list made of the class below. A list of a class is like a multi dimensional array. Each “list item” contains ALL of the pieces of ModularColorTile. So each item in the list has a name, a texture reference, start positions etc
    public List<ModularColorTile> colorTiles = new List<ModularColorTile>();

    public class ModularColorTile
    {
        //The tiles name
        public string tileName;
        //The texture that’s being used
        public Texture texture;
        //The start position to check
        public Vector2 startPosition;
        //The size in x and y coordinates that we are using in the texture
        public Vector2 size;
        //The color we are using for the tile
        public Color color;


        //This is a constructor. It allows us to create a new ‘ModularColorTile’ at any time as needed with a single call. The variable types and names work just like calling any other method in C#.
        public ModularColorTile(string theTile, Texture theTexture, Vector2 theStart, Vector2 theSize, Color theColor)
        {
            tileName = theTile;
            texture = theTexture;
            startPosition = theStart;
            size = theSize;
            color = theColor;
        }
    }



    //The MenuItem creates the actual menu item you click on to open it. Slashes indicate levels of the tree. So, removing the “/gamedevbits” from it would show it directly in the Tools menu.
    [MenuItem("Tools/GameDevBits/Modular Heroes colour change")]
    //This is where we create the inspector window
    public static void ShowWindow()
    {
        //First we declare the intended size of the new window with a vector2
        Vector2 dockWindow = new Vector2(500, 260);
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow(typeof(ModularColorChange));
        //And then we set the windows minimum size to the values we declared earlier
        window.minSize = dockWindow;
    }


    void OnGUI()
    {
        //GuiLayout.Label creates a piece of text. And then we bold it.
        GUILayout.Label("Modular Heroes Colour Changer", EditorStyles.boldLabel);
        GUILayout.Label("Change colours of the Synty Modular Heroes texture for use with other shaders");
        //Buttons are constructed like this. We use an IF, then declare the button and then the code for it is wrapped up in between. IF the button is pushed, then the code is run.
        if (GUILayout.Button("Set Colours", GUILayout.Width(290), GUILayout.Height(50)))
        {
            //Gets the full path of the selected texture (Including folders)
            string texturePath = AssetDatabase.GetAssetPath(theTexture);
            //This will emit the path and only get the files name without the extension (so no .jpg, .png etc)
            string texturePathWithoutExtension = Path.GetFileNameWithoutExtension(texturePath);
            int pos = texturePath.LastIndexOf("/") + 1;
            //Here we set the original file name, this will be used when creating the new texture.
            originalFileName = texturePathWithoutExtension;
            string originalFolder = texturePath.Substring(0, pos - 1);
            //This calls the RunTextureProcess method which does all of the work.
            RunTextureProcess();
        }

        GUILayout.BeginHorizontal(GUILayout.Width(380));
        GUILayout.BeginVertical();
        theTexture = (Texture)EditorGUILayout.ObjectField(theTexture, typeof(Texture), true, GUILayout.Width(150), GUILayout.Height(150));
        GUILayout.EndVertical();


        GUILayout.BeginVertical();
        GUILayout.Label("Save folder: \"Assets\\\"", EditorStyles.boldLabel);
        saveFolder = EditorGUILayout.TextField(saveFolder, GUILayout.Width(300));
        this.Repaint();
        GUILayout.Label("New File suffix - Will be added to end of filename", EditorStyles.boldLabel);
        newFileName = EditorGUILayout.TextField(newFileName, GUILayout.Width(300));
        this.Repaint();

        if (GUILayout.Button("Load Modular Hero Defaults", GUILayout.Width(189), GUILayout.Height(20)))
        {
            MHSampleData();
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();


        for (int i = 0; i < colorTiles.Count; i++)
        {
            GUILayout.Label(colorTiles[i].tileName, EditorStyles.boldLabel);
            colorTiles[i].color = EditorGUILayout.ColorField(colorTiles[i].tileName,
            colorTiles[i].color);
        }


    }

    void RunTextureProcess()
    {
        ReadTexture();
        SetAllPixels();
        TextureWriter();
    }

    void ReadTexture()
    {
        tex = new Texture2D(2, 2);
        tex = (Texture2D)theTexture;
    }

    void SetAllPixels()
    {
        //Run a loop over all the tiles in colorTiles
        foreach (ModularColorTile tile in colorTiles)
        {
            //Create a color array with a range as big as the tiles x size times the tiles y size
            Color[] colors = new Color[(int)tile.size.x * (int)tile.size.y];
            //Run a loop over the tiles pixel size
            for (int i = 0; i < tile.size.x * tile.size.y; i++)
            {
                //Populates the colors array created above
                colors[i] = tile.color;
            }
            tex.SetPixels((int)tile.startPosition.x, (int)tile.startPosition.y, (int)tile.size.x, (int)tile.size.y, colors, 0);
        }
    }



    void TextureWriter()
    {
        var bytes = tex.EncodeToPNG();
        //If the folder isn't there, we make it
        if (!Directory.Exists($"Assets/{saveFolder}"))
        {
            Directory.CreateDirectory($"Assets/{saveFolder}");
        }

        //We create the full path of folder and file name by combining the original file name and the suffix provided.
        var iconPath = $"Assets/{saveFolder}/{originalFileName}__{newFileName}.png";
        //write the actual file
        File.WriteAllBytes(iconPath, bytes);
    }

    //This is the modular heroes data laid out and ready to be used. 
    //We’re using the constructor we built earlier to create ModularColorTile (our custom class) and add them to a list.
    //For each entry we declare it’s name, where the color tile starts (bottom left corner of it) and how wide and tall it is, then we give it a colour.
    void MHSampleData()
    {
        //we empty the list before we drop our data in. Otherwise if you pressed the button twice, you’d have two of each entry.
        colorTiles.Clear();
        colorTiles.Add(new ModularColorTile("Primary", EditTexture, new Vector2(283, 355), new Vector2(46, 47), new Color(0.6f, 0.3f, 0.2f)));
        colorTiles.Add(new ModularColorTile("Secondary", EditTexture, new Vector2(328, 355), new Vector2(47, 47), new Color(0.9f, 0.1f, 0.7f)));
        colorTiles.Add(new ModularColorTile("Leather Primary", EditTexture, new Vector2(374, 355), new Vector2(47, 47), new Color(0.9f, 0.3f, 0.4f)));
        colorTiles.Add(new ModularColorTile("Metal Primary", EditTexture, new Vector2(283, 309), new Vector2(46, 46), new Color(0.9f, 0.2f, 0.7f)));
        colorTiles.Add(new ModularColorTile("Leather Secondary", EditTexture, new Vector2(420, 355), new Vector2(46, 46), new Color(0.9f, 0.9f, 0.1f)));
        colorTiles.Add(new ModularColorTile("Metal Dark", EditTexture, new Vector2(374, 309), new Vector2(46, 46), new Color(0.9f, 0.3f, 0.9f)));
        colorTiles.Add(new ModularColorTile("Metal Secondary", EditTexture, new Vector2(328, 309), new Vector2(46, 46), new Color(0.9f, 0.1f, 0.6f)));
        colorTiles.Add(new ModularColorTile("Hair", EditTexture, new Vector2(96, 151), new Vector2(50, 73), new Color(0.9f, 0.2f, 0.1f)));
        colorTiles.Add(new ModularColorTile("Skin", EditTexture, new Vector2(0, 82), new Vector2(96, 70), new Color(0.8f, 0.1f, 0.2f)));
        colorTiles.Add(new ModularColorTile("Stubble", EditTexture, new Vector2(0, 22), new Vector2(96, 61), new Color(0.4f, 0.1f, 0.3f)));
        colorTiles.Add(new ModularColorTile("Scar", EditTexture, new Vector2(0, 151), new Vector2(96, 73), new Color(0.7f, 0.2f, 0.4f)));
        colorTiles.Add(new ModularColorTile("Body art", EditTexture, new Vector2(464, 265), new Vector2(49, 139), new Color(0.2f, 0.7f, 0.5f)));
        colorTiles.Add(new ModularColorTile("Eyes", EditTexture, new Vector2(0, 0), new Vector2(96, 22), new Color(0.1f, 0.3f, 0.5f)));

    }
}

