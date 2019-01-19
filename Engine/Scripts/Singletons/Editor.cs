using Godot;
using System;

public class Editor : Node2D
{
    public static bool EditorMode = false;

    // Static References 
    public static Viewport Viewport;
    public static Level CurrentLevel;
    public static YSort Entities;
    public static Node2D Nodes;
    public static EditorCamera Camera;
    public static PopupMenu MainMenu;

    public static PackedScene GameScene;


    public void GetNodes()
    {
        if (!GetTree().GetRoot().HasNode("Editor")){
            EditorMode = false;
            return;
        }

        // Making sure that this code doesn't get executed more than once.
        EditorMode = true;

        // Getting the viewport first because thats where we are going to palce the loaded level.
        // Also where the camera is located.
        Viewport = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport") as Viewport;
        Camera = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/Camera") as EditorCamera;

        LoadLevel("res://editorLevel.tscn"); // Loading the level scene.

        // Getting other refs.
        Entities = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/CurrentLevel/Layers/Entities") as YSort;
        CurrentLevel = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/CurrentLevel") as Level;

    }


    public override void _Ready()
    {
        GameScene = (PackedScene)ResourceLoader.Load("res://Content/Scenes/EmptyLevel.tscn");
        GetNodes();

        GD.Print("- EDITOR INITIALIZED. -");
    }


    public override void _Process(float delta){
        if(!EditorMode)
            GetNodes();

        if(EditorMode && Input.IsActionJustPressed("EditorMode"))
            LeaveEditorMode();
    }


    public void SaveLevel(string pPath)
    {
        PackedScene savedLevel = new PackedScene();
        CurrentLevel = GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport/CurrentLevel") as Level;
        SetOwners(CurrentLevel);
        GD.Print("[Editor] - Setting Owners...");
        savedLevel.Pack(CurrentLevel); // Packs the level into a scene.
        ResourceSaver.Save(pPath, savedLevel); // Save the PackedScene
        GD.Print("[Editor] Level Saved.");
    }


    // This makes sure that everynode contained in the Currentlevel gets saved
    // in the packed scene level. See EnterDebugMode() for more.
    private void SetOwners(Node pNode)
    {
        foreach(Node node in pNode.GetChildren() )
        {
            if(node is Player)
                continue;

            node.SetOwner( CurrentLevel );

            GD.Print(node);

            if(node is Entity)
                continue;

            if(node.GetChildren().Count > 0) 
                SetOwners(node); // Recursivity
        }       
    }


    private void LeaveEditorMode()
    {
        GD.Print("[Editor] - Leaving Editor mode.");
        SaveLevel("res://editorLevel.tscn");
        EditorMode = false; 
        GetTree().ChangeSceneTo(GameScene);
    }


    public void LoadLevel(string pPath)
    {
        GD.Print("[Editor] - Loading level at: " + pPath);
        Level level = (ResourceLoader.Load(pPath) as PackedScene).Instance() as Level;
        level.Name = "CurrentLevel";
        GetTree().GetRoot().GetNode("Editor/ViewportContainer/Viewport").AddChild(level);
    }

    public static Node2D GetEntity(Vector2 pPosition)
    {
        foreach(Node2D node in Editor.Entities.GetChildren())
        {
            if( node.GetGlobalPosition().x - 8 < pPosition.x && 
                node.GetGlobalPosition().x + 8 > pPosition.x &&
                node.GetGlobalPosition().y + 8 > pPosition.y &&
                node.GetGlobalPosition().y - 8 < pPosition.y)
            {
                GD.Print("[Editor] - Found Entity " + node.Name + " at: " + pPosition);
                return node;
            }
        }
        return null;
    }
    
}
