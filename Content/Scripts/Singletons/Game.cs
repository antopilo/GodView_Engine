using Godot;
using System;

public class Game : Node
{
    // Change to off if releasing.
    public static bool DebugMode = true;

    public static Node2D GameNode;
    public static Control FirstSpellSlot;
    public static Control SecondSpellSlot;
    public static YSort Entities;
    public static Level CurrentLevel;
    public static PackedScene PlayerScene;

    public static Hand Hand;

    // Editor Scene
    private PackedScene EditorScene;

    public static bool InGame = false;

    public override void _Ready()       
    {
        PlayerScene = ResourceLoader.Load("res://Content/Scenes/Player.tscn") as PackedScene;

        // If the singleton is loaded in the DebugMode the references won't
        // be valid so we skip the script. Making the singleton useless.
        if (!GetTree().GetRoot().HasNode("Game")) return;

        GameNode = GetTree().GetRoot().GetNode("Game") as Node2D;
        FirstSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SelectedSpell") as Control;
        SecondSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SecondSpell") as Control;

        LoadLevel("res://editorLevel.tscn");

        CurrentLevel = GameNode.GetNode("CurrentLevel") as Level;
        Entities = CurrentLevel.GetNode("Layers/Entities") as YSort;

        InGame = true;

        CurrentLevel.SpawnPlayer();
        Hand = Entities.GetNode("Player/Hand") as Hand;

        if(CurrentLevel is null) 
            GD.Print("Current Level not found");
            
        // The rest should only be references used in the debug mode.
        if(!DebugMode) return;
        
        // Gets the Editor scene in case the player has access to the debug mode.
        EditorScene = ResourceLoader.Load("res://Engine/Scenes/Main.tscn") as PackedScene;
    }

    private void GetNodes()
    {
       if (!GetTree().GetRoot().HasNode("Game")) return;

        GameNode = GetTree().GetRoot().GetNode("Game") as Node2D;
        FirstSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SelectedSpell") as Control;
        SecondSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SecondSpell") as Control;

        LoadLevel("res://editorLevel.tscn");

        CurrentLevel = GameNode.GetNode("CurrentLevel") as Level;
        Entities = CurrentLevel.GetNode("Layers/Entities") as YSort;

        InGame = true;
        
        CurrentLevel.SpawnPlayer();
        Hand = Entities.GetNode("Player/Hand") as Hand;

        if(CurrentLevel is null) 
            GD.Print("Current Level not found");
            
    }
    override public void _Process(float delta)
    {
        if( !InGame )
            GetNodes();
        
        // Checks for the debugMode toggle every frame
        // only if the DebugMode boolean is enabled.
        if( Input.IsActionJustPressed("DebugMode") && DebugMode && InGame)
            EnterDebugMode();
    }
    
    // This handles the transition between the debug mode and the game scne.
    // All it does is saves the currentLevel in a packedScene so that the editor can
    // Edit and load it afterwards. The same will be done with loading the levels.
    private void EnterDebugMode()
    {
        GD.Print("Entering Debug Mode");
        InGame = false;
        //PackedScene savedLevel = new PackedScene();
        //savedLevel.Pack(CurrentLevel); // Packs the level into a scene.
        //ResourceSaver.Save("res://editorLevel.tscn", savedLevel); // Save the PackedScene
        GetTree().ChangeSceneTo(EditorScene); // Switch to the editor view.
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

            //GD.Print("ChangedOwner of :" + node.Name + " -> to: " + CurrentLevel.Name);
            //if(node is Entity)
            //    continue;

            if(node.GetChildren().Count > 0) SetOwners(node); // Recursivity
        }       
    }

    public void LoadLevel(string pPath)
    {
        GD.Print("Debug Mode - Loading Level... at path: " + pPath);
        Level level = (ResourceLoader.Load(pPath) as PackedScene).Instance() as Level;
        level.Name = "CurrentLevel";
        if(GameNode != null) GameNode.AddChild(level);
            
        //(GetTree().GetRoot().GetNode("Game") as Node).AddChild(CurrentLevel);
    }
}   
