using Godot;
using System;

/// <summary>
/// Le EditorHandler gere tout ce qui a rapport a l'activation et la desactivation du mode Editor.
/// Il s'occupe principalement a conserver l'etat du jeu lorsque le mode est active. Il fait un 
/// freeze du jeu, et active les menus ainsi que les options du mode editor. Il garde reference 
/// aux autre handler pour pouvoir les appeler si besoin. Si aucun menu n'a ete fait encore c'est 
/// parce que c'est une decision d'equipe de choisir une bonne vision pour le editor.
///   
/// TODO: Choisir a quoi l'editor devrait ressemble.
/// </summary>
public class EditorHandler : Node2D
{
    // Global
    public bool EditorMode = false;

    // Entities
    private bool PlacingEnt = false;
    private bool MovingEnt = false;
    private bool Snapping = true;

    private PackedScene Chest;
    private PackedScene FireArea, OilArea;
    private PackedScene Tree, Rock, Bush1, Bush2;

    private Entity SelectedEnt;
    private Vector2 TargetPosition;

    private Node2D PreviewEnt;

    // Reference to other nodes.
    private Node2D GameHandler;
    private Node2D NodeHandler;
    private Node2D LevelHandler;

    private bool InEditorMenu;
    private PopupMenu EditorMenu;
    private Popup EntExplorer;
    private Popup ScaleBar;
    private TextureProgress ScaleBarBar;
    

    public override void _Ready()
    {
        GD.Print("[Editor] EditorHandler Initialized.");

        // Fait les references aux autres nodes.
        GameHandler = GetNode("../GameHandler") as Node2D;
        NodeHandler = GetNode("NodeHandler") as Node2D;
        LevelHandler = GetNode("LevelHandler") as Node2D;

        EditorMenu = GetNode("EditorMenu") as PopupMenu;
        EntExplorer = GetNode("EntExplorer") as Popup;

        ScaleBar = GetNode("ScaleBar") as Popup;
        ScaleBarBar = ScaleBar.GetNode("Bar") as TextureProgress;

        MakeMenu();
        GD.Print("[Editor] MainMenu Initialized.");

        FireArea = ResourceLoader.Load("res://Content/Scenes/Entities/Spells/FireAOE/FireArea.tscn") as PackedScene;
        Chest = ResourceLoader.Load("res://Content/Scenes/Entities/Chest.tscn") as PackedScene;
        OilArea = ResourceLoader.Load("res://Content/Scenes/Entities/Oil/OilArea.tscn") as PackedScene;
        Tree = ResourceLoader.Load("res://Content/Scenes/Entities/Trees/Tree.tscn") as PackedScene;
        Rock = ResourceLoader.Load("res://Content/Scenes/Entities/Rocks/Rock.tscn") as PackedScene;
        Bush1 = ResourceLoader.Load("res://Content/Scenes/Entities/Bush/Bush1.tscn") as PackedScene;
        Bush2 = ResourceLoader.Load("res://Content/Scenes/Entities/Bush/Bush2.tscn") as PackedScene;

        GD.Print("[Editor] EditorHandler references loaded.");
    }

    // Verifie les Inputs a chaque frame.
    public override void _Input(InputEvent @event)
    {
        if(MovingEnt && !PlacingEnt && @event.IsActionPressed("Delete"))
        {
            SelectedEnt.QueueFree();
            MovingEnt = PlacingEnt = false;
        }

        // EditorMode est un controle dans project settings
        if (@event.IsActionPressed("RightClick")) 
        {
            if(MovingEnt)
                PlaceEntity();

            ClearSelected();

            EditorMenu.PopupCenteredMinsize();
            EditorMenu.RectGlobalPosition = GetGlobalMousePosition();
        }
        if (@event.IsActionPressed("Click"))
        {
            // If is not in any menu.
            if (!InEditorMenu)
            {
                // If not placing or moving, then select.
                if (!PlacingEnt && !MovingEnt)
                {
                    SelectedEnt = Editor.GetEntity(Editor.Camera.GetGlobalMousePosition()) as Entity;

                    if (SelectedEnt != null)
                        MovingEnt = true;
                }
                else if(PlacingEnt != MovingEnt)
                {
                    // Else, it means you are placing an entity.
                    PlaceEntity();
                }
            }
        }
        
        // If Placing or moving an entity
        // SHIFT + SCROLL = Scale the selected entity.
        if(PlacingEnt || MovingEnt)
        {
            if (@event.IsActionPressed("Shift") && @event.IsActionPressed("ZoomIn"))
            {

                SelectedEnt.Scale += new Vector2(0.15f, 0.15f);
                ScaleBarBar.Value = SelectedEnt.Scale.x;
            }
            else if (@event.IsActionPressed("Shift") && @event.IsActionPressed("ZoomOut"))
            {
                SelectedEnt.Scale -= new Vector2(0.15f, 0.15f);
                ScaleBarBar.Value = SelectedEnt.Scale.x;
            }
            else if (@event.IsActionPressed("ui_cancel"))
            {
                ClearSelected();
            }
        }
    }


    public override void _Process(float delta)
    {
        if(PlacingEnt) (GetNode("Action") as Label).Text = "Placing";
        else if(MovingEnt)  (GetNode("Action") as Label).Text = "Moving";
        else (GetNode("Action") as Label).Text = "Idling";

        if (PlacingEnt || MovingEnt)
        {
            TargetPosition = Editor.Camera.GetGlobalMousePosition() + new Vector2(4,4);
            if(Snapping)
            {
                int SnapX = (int)(TargetPosition.x - TargetPosition.x % 8);
                int SnapY = (int)(TargetPosition.y - TargetPosition.y % 8);
                SelectedEnt.GlobalPosition = new Vector2(SnapX, SnapY);
            }
                    
            SelectedEnt.Selected = true;
            ScaleBar.RectGlobalPosition = GetGlobalMousePosition();
        }
        ScaleBarBar.Visible = PlacingEnt || MovingEnt;
    }


    private void MakeMenu()
    {
        EditorMenu.AddItem("Add new Entity", 0);
    }


    private void PlaceEntity()
    {
        // Duplicating & Settings propreties for the new Entity
        var position = SelectedEnt.GlobalPosition;
        var Child = SelectedEnt.Duplicate() as Entity;

        Child.GlobalPosition = position;
        Child.Selected = false;

        // Special Exeption for some Entities.
        if(SelectedEnt is FireArea)
            (Child as FireArea).Burning = true;

        // Adding the entity to the current level.
        Editor.Entities.AddChild(Child, true);

        if(MovingEnt)
            ClearSelected();
        else
            ScaleBarBar.Value =  SelectedEnt.Scale.x;
       
    }

    // Not sure if useful. really.
    private void ClearSelected()
    {
        // Set everything to falseé
        InEditorMenu = MovingEnt = PlacingEnt = false;

        if(SelectedEnt != null) 
            SelectedEnt.QueueFree();

        SelectedEnt = null;
        
    }

    // When selecting something in the Right click menu.
	private void _on_EditorMenu_id_pressed(int ID)
	{
        switch (ID)
        {
            case 0: // Opening the asset explorer.
                EntExplorer.PopupCenteredMinsize();
                
                (EntExplorer.GetNode("ItemList") as ItemList).UnselectAll();
                break;
        }
	}

    // TO ADD NEW ENTITIES:
    // First be sure to add the item in the Godot editor.
    // Then add a new case for your new item
	private void EntExplorerSelected(int index)
	{
        //ClearSelected();
        Entity newEnt = null;

        switch (index)
        {
            case 0: newEnt = Chest.Instance() as Entity; break;
            case 1: newEnt = FireArea.Instance() as Entity; break;
            case 2: newEnt = OilArea.Instance() as Entity; break;
            case 3: newEnt = Tree.Instance() as Entity; break;
            case 4: newEnt = Rock.Instance() as Entity; break;
            case 5: newEnt = Bush1.Instance() as Entity; break;
            case 6: newEnt = Bush2.Instance() as Entity; break;
            //case 7: newEnt = ****.Instance() as Entity; break;
            // Add new entity here. Same as above.
        }

        // Add Special per entity conditions here
        // Exemple:
        // if(newEnt is FireArea) 
        //     (newEnt as FireArea).Burning = false;

        // Adding to the scene.
        Editor.Entities.AddChild(newEnt, true);
        SelectedEnt = newEnt as Entity;
        PlacingEnt = SelectedEnt.Selected = true;

        EntExplorer.Hide();
        InEditorMenu = false;

        // Making sure that the preview doesnt Interact
        SelectedEnt.SetProcess(false);
        SelectedEnt.SetPhysicsProcess(false);
    }

    public void _on_EditorMenu_popup_hide()
    {
        return;
    } 

    public void _on_EntExplorer_popup_hide() 
        => InEditorMenu = false;
}