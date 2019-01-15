using Godot;
using System;

public class Game : Node
{
    // Refs
    public static Control FirstSpellSlot;
    public static Control SecondSpellSlot;

    // Maybe later...
    //public static Hand CurrentLevel;
    //public static Player Entities;

    public override void _Ready()       
    {
        if (!GetTree().GetRoot().HasNode("Game"))
            return;
        FirstSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SelectedSpell") as Control;
        SecondSpellSlot = GetTree().GetRoot().GetNode("Game/UI/Screen/SecondSpell") as Control;
    }
}
