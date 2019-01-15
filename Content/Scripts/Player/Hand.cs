using Godot;
using System;

public class Hand : Position2D
{
    private Spell[] AllSpell;
    private Spell[] SpellPool;

    // Called when the node is loaded.
    public override void _Ready()
    {
        SpellPool = new Spell[2];

        // Ajouter les 2 seuls spell lol
        SpellPool[0] = GetNode("Water") as Spell;
        SpellPool[1] = GetNode("Fire") as Spell;

        SpellPool[0].Unlocked = true;
        SpellPool[0].Equipped = true;

        SpellPool[1].Unlocked = true;
    }

    // Called every frame.
    override public void _Process(float delta)
    {
        if(Input.IsActionJustPressed("Switch"))
        {
            SwapSpell();
        }
    }

    // Unlock a spell.
    public void UnlockSpell(Spell pSpell)
    {
        pSpell.Unlocked = true;
    }

    // Change an equipped spell for a new one.
    public void ChangeSpell(int idx, Spell spell){
        SpellPool[idx] = spell as Spell;
    }

    // Swaps the two current spells
    public void SwapSpell(){
        Spell temp = SpellPool[0];
        SpellPool[0] = SpellPool[1];
        SpellPool[1] = temp;

        SpellPool[0].Equipped = true;
        SpellPool[1].Equipped = false;
    }
}
