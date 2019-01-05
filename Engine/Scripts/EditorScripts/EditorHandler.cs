using Godot;
using System;

/// <summary>
/// Le EditorHandler gere tout ce qui a rapport a l'activation et la desactivation du mode Editor.
/// Il s'occupe principalement a conserver l'etat du jeu lorsque le mode est active. Il fait un 
/// freeze du jeu, et active les menus ainsi que les options du mode editor. Il garde reference 
/// aux autre handler pour pouvoir les appeler si besoin. Si aucun menu n'a ete fait encore c'est 
/// parce que c'est une decision d'équipe de choisir une bonne vision pour le editor.
///   
/// TODO: Choisir a quoi l'editor devrait ressemble.
/// </summary>
public class EditorHandler : Node2D
{
    public bool EditorMode = false;

    // Reference to other nodes.
    private Node2D GameHandler;
    private Node2D NodeHandler;
    private Node2D LevelHandler;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Fait les references aux autres nodes.
        GameHandler = GetNode("../GameHandler") as Node2D;
        NodeHandler = GetNode("NodeHandler") as Node2D;
        LevelHandler = GetNode("LevelHandler") as Node2D;
    }

    // Verifie les Inputs a chaque frame.
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("EditorMode")) // EditorMode est un controle dans project settings
        {
            if (EditorMode)
                LeaveEditorMode();
            else
                EnterEditorMode();
        }
    }

    private void EnterEditorMode()
    {
        // TODO: Activer le mode Editor.
        GD.Print("Editor mode activated");
    }

    private void LeaveEditorMode()
    {
        // TODO: Desactiver le mode Editor.
        GD.Print("Editor mode deacivated");
    }
}