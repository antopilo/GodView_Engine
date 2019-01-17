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
    private PackedScene FireArea;
    private PackedScene OilArea;
    private PackedScene Tree;
    private PackedScene Rock;

    private Entity SelectedEnt;
    private Vector2 TargetPosition;

    private Node2D PreviewEnt;

    // Reference to other nodes.
    private Node2D GameHandler;
    private Node2D NodeHandler;
    private Node2D LevelHandler;

    private PopupMenu EditorMenu;
    private Popup EntExplorer;
    private Popup ScaleBar;
    private TextureProgress ScaleBarBar;
    

    public override void _Ready()
    {
        
        // Fait les references aux autres nodes.
        GameHandler = GetNode("../GameHandler") as Node2D;
        NodeHandler = GetNode("NodeHandler") as Node2D;
        LevelHandler = GetNode("LevelHandler") as Node2D;

        EditorMenu = GetNode("EditorMenu") as PopupMenu;
        EntExplorer = GetNode("EntExplorer") as Popup;

        ScaleBar = GetNode("ScaleBar") as Popup;
        ScaleBarBar = ScaleBar.GetNode("Bar") as TextureProgress;
        MakeMenu();

        FireArea = ResourceLoader.Load("res://Content/Scenes/Entities/Spells/FireAOE/FireArea.tscn") as PackedScene;
        Chest = ResourceLoader.Load("res://Content/Scenes/Entities/Chest.tscn") as PackedScene;
        OilArea = ResourceLoader.Load("res://Content/Scenes/Entities/Oil/OilArea.tscn") as PackedScene;
        Tree = ResourceLoader.Load("res://Content/Scenes/Entities/Trees/Tree.tscn") as PackedScene;
        Rock = ResourceLoader.Load("res://Content/Scenes/Entities/Rocks/Rock.tscn") as PackedScene;
    }


    // Verifie les Inputs a chaque frame.
    public override void _Input(InputEvent @event)
    {
        if(MovingEnt && !PlacingEnt && @event.IsActionPressed("Delete"))
        {
            SelectedEnt.QueueFree();
            MovingEnt = PlacingEnt = false;
        }
        if(!PlacingEnt && @event.IsActionPressed("Click") && !MovingEnt)
        {
            SelectedEnt = Editor.GetEntity(Editor.Camera.GetGlobalMousePosition()) as Entity ;
            if(SelectedEnt != null) 
                MovingEnt = true;  
        }
        else if(MovingEnt && !PlacingEnt && @event.IsActionPressed("Click") )
        {
            PlaceEntity();
        }

        // Placing entities
        if (PlacingEnt && @event.IsActionPressed("Click") && !MovingEnt) 
            PlaceEntity();
            

        // Scaling up the Entity with Shift Scroll wheel.
        if(PlacingEnt && @event.IsActionPressed("ZoomIn") && Input.IsActionPressed("Shift") )
        {
            SelectedEnt.Scale += new Vector2(0.15f,0.15f);
            ScaleBarBar.Value = SelectedEnt.Scale.x;    
        }
        if(PlacingEnt && @event.IsActionPressed("ZoomOut") && Input.IsActionPressed("Shift"))
        {
            SelectedEnt.Scale -= new Vector2(0.15f, 0.15f);
            ScaleBarBar.Value = SelectedEnt.Scale.x;
        }
        
        // Canceling
        if (PlacingEnt && @event.IsActionPressed("ui_cancel") || MovingEnt && @event.IsActionPressed("ui_cancel"))
            ClearSelected();

        // EditorMode est un controle dans project settings
        if (@event.IsActionPressed("RightClick")) 
        {
            PlacingEnt = false;
            ClearSelected();
            EditorMenu.PopupCenteredMinsize();
            EditorMenu.RectGlobalPosition = GetGlobalMousePosition();
        }

    }


    public override void _Process(float delta)
    {
        if(PlacingEnt)  (
            GetNode("Action") as Label).Text = "Placing : " + SelectedEnt.Name;
        else if(MovingEnt) 
            (GetNode("Action") as Label).Text = "Moving : " + SelectedEnt.Name;
        else (
            GetNode("Action") as Label).Text = "Idling";

        if (PlacingEnt || MovingEnt)
        {
            TargetPosition = Editor.Camera.GetGlobalMousePosition();
            if(Snapping)
            {
                int SnapX = (int)(TargetPosition.x - TargetPosition.x % 8);
                int SnapY = (int)(TargetPosition.y - TargetPosition.y % 8);
                SelectedEnt.GlobalPosition = new Vector2(SnapX, SnapY);
            }
                    
            SelectedEnt.Selected = true;
            ScaleBar.RectGlobalPosition = GetGlobalMousePosition();
        }
        ScaleBarBar.Visible = PlacingEnt;
    }


    private void MakeMenu()
    {
        EditorMenu.AddItem("Add new Entity", 0);
        EditorMenu.AddSeparator();
        EditorMenu.AddItem("Test");
        EditorMenu.AddItem("Test");
        EditorMenu.AddItem("Test");
        EditorMenu.AddItem("Test");
    }


    private void PlaceEntity()
    {
        var position = SelectedEnt.GlobalPosition;

        // Creating node
        if (SelectedEnt is Chest) {
            Chest chest = SelectedEnt.Duplicate() as Chest;
            chest.GlobalPosition = position;
            chest.Name = "Chest";
            chest.Selected = false;
            Editor.Entities.AddChild(chest);
        }
        else if(SelectedEnt is FireArea)
        {
            FireArea fireArea = SelectedEnt.Duplicate() as FireArea;
            fireArea.GlobalPosition = position;
            fireArea.Burning = true;
            fireArea.Name = "FireArea";
            fireArea.Selected = false;
            Editor.Entities.AddChild(fireArea);
        }
        else if (SelectedEnt is Oil)
        {
            Oil oilArea = SelectedEnt.Duplicate() as Oil; ;
            oilArea.GlobalPosition = position;
            oilArea.Name = "OilArea";
            oilArea.Selected = false;
            Editor.Entities.AddChild(oilArea);

            //(SelectedEnt as Oil).RandomSize();
            ScaleBarBar.Value = SelectedEnt.Scale.x;
        }
        else
        {
            Entity newEnt = SelectedEnt.Duplicate() as Entity;
            newEnt.GlobalPosition = position;
            newEnt.Name = "Tree";
            Editor.Entities.AddChild(newEnt);
            ScaleBarBar.Value = SelectedEnt.Scale.x;
        }

        if(MovingEnt) 
        {
            MovingEnt = false;
            PlacingEnt = false;
            SelectedEnt.QueueFree();
        }
        
    }


    private void ClearSelected()
    {
        if(SelectedEnt != null)
            SelectedEnt.QueueFree();

        SelectedEnt = null;
        PlacingEnt = false;
    }


	private void _on_EditorMenu_id_pressed(int ID)
	{
        switch (ID)
        {
            // Asset explorer
            case 0:
                EntExplorer.PopupCenteredMinsize();
                (EntExplorer.GetNode("ItemList") as ItemList).UnselectAll();
                break;
        }
	}


	private void EntExplorerSelected(int index)
	{
        ClearSelected();
        switch (index)
        {
            case 0:
                Chest chest = Chest.Instance() as Chest;
                Editor.Entities.AddChild(chest);
                SelectedEnt = chest;
                PlacingEnt = true;
                break;
            case 1:
                FireArea fireArea = FireArea.Instance() as FireArea;
                Editor.Entities.AddChild(fireArea);
                fireArea.Burning = false;
                SelectedEnt = fireArea;
                PlacingEnt = true;
                break;
            case 2:
                Oil oilArea = OilArea.Instance() as Oil;
                //oilArea.RandomSize();
                Editor.Entities.AddChild(oilArea);
                SelectedEnt = oilArea;
                PlacingEnt = true;
                break;
            case 3:
                Node2D tree = Tree.Instance() as Node2D;
                Editor.Entities.AddChild(tree);
                SelectedEnt = tree as Entity;
                PlacingEnt = true;
                break;
            case 4:
                Node2D rock = Rock.Instance() as Node2D;
                Editor.Entities.AddChild(rock);
                SelectedEnt = rock as Entity;
                PlacingEnt = true;
                break;
        }

        SelectedEnt.Selected = true;
        EntExplorer.Hide();

        SelectedEnt.SetProcess(false);
        SelectedEnt.SetPhysicsProcess(false);
    }


}