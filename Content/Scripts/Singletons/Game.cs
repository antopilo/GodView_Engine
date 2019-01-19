using Godot;
using System;

public class Game : Node
{
    // Change to off if releasing.
    public static bool DebugMode = true;
    public static bool InGameMode = false;

    private PackedScene EditorScene; // Editor Scene

    public static Node2D GameNode;
    public static Control FirstSpellSlot;
    public static Control SecondSpellSlot;

    public static Level CurrentLevel;
    public static YSort Entities;
    public static PackedScene PlayerScene;
    public static Hand Hand;


    public override void _Ready()       
    {
        PlayerScene = (PackedScene)ResourceLoader.Load("res://Content/Scenes/Player.tscn");

        // If the singleton is loaded in the DebugMode the references won't
        // be valid so we skip the script. Making the singleton useless.
        if (!GetTree().GetRoot().HasNode("Game")) 
            return;

        // Get useful References for Global use.
        GetNodes();

        // The rest should only be references used in the debug mode.
        if(!DebugMode) 
            return;
        
        // Gets the Editor scene in case the player has access to the debug mode.
        EditorScene = ResourceLoader.Load("res://Engine/Scenes/Main.tscn") as PackedScene;
    }


    private void GetNodes()
    {
        // If in editor, skip.
        if (!GetTree().GetRoot().HasNode("Game")) 
            return;

        GameNode = GetTree().GetRoot().GetNode("Game") as Node2D;
        FirstSpellSlot = GameNode.GetNode("UI/Screen/SelectedSpell") as Control;
        SecondSpellSlot = GameNode.GetNode("UI/Screen/SecondSpell") as Control;

        // TODO: Add path support.
        LoadLevel("res://editorLevel.tscn"); // Load level. 

        CurrentLevel = GameNode.GetNode("CurrentLevel") as Level;
        Entities = CurrentLevel.GetNode("Layers/Entities") as YSort;

        InGameMode = true;
        
        // Spawn Player.
        CurrentLevel.SpawnPlayer();
        Hand = Entities.GetNode("Player/Hand") as Hand;

        if(CurrentLevel is null) 
            GD.PrintErr("[GAME] Current Level not found");
            
    }

    /// Executed every frame.
    override public void _Process(float delta)
    {
        if( !InGameMode ) 
            GetNodes();
        
        // Checks for the debugMode toggle every frame
        // only if the DebugMode boolean is enabled.
        if( Input.IsActionJustPressed("DebugMode") && DebugMode && InGameMode)
            LeaveGameMode();
    }
    

    // This handles the transition between the debug mode and the game scne.
    // All it does is saves the currentLevel in a packedScene so that the editor can
    // Edit and load it afterwards. The same will be done with loading the levels.
    private void LeaveGameMode()
    {
        GD.Print("[Game] Leaving Game mode");
        InGameMode = false;
        GetTree().ChangeSceneTo(EditorScene); // Switch to the editor view.
    }


    // This makes sure that everynode contained in the Currentlevel gets saved
    // in the packed scene level. See EnterDebugMode() for more.
    private void SetOwners(Node pNode)
    {
        foreach(Node node in pNode.GetChildren() )
        {
            if(node is Player) // Don't need the player saved in the level.
                continue;

            node.SetOwner( CurrentLevel );

            // If the node has childrens, then SetOwners recursivly.
            if(node.GetChildren().Count > 0) 
                SetOwners(node); // Recursivity
        }       
    }
    

    public void LoadLevel(string pPath)
    {
        GD.Print("[Game] Loading level: " + pPath);
        Level level = (ResourceLoader.Load(pPath) as PackedScene).Instance() as Level;
        level.Name = "CurrentLevel";
        if(GameNode != null) 
            GameNode.AddChild(level);
    }


}   
