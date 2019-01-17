using Godot;
using System;

public class Hand : Position2D
{
    public Spell[] SpellPool;

    public Spell[] AllSpell;

    // Called when the node is loaded.
    public override void _Ready()
    {
        SetAllSpell();

        SpellPool = new Spell[2];

        // Ajouter les 2 seuls spell lol
        SpellPool[0] = this.GetNode("Water") as Spell;
        SpellPool[1] = this.GetNode("Fire") as Spell;

        SpellPool[0].Unlocked = true;
        SpellPool[0].Equipped = true;

        SpellPool[1].Unlocked = true;
    }

    // Called every frame.
    override public void _Process(float delta)
    {
        if(Input.IsActionJustPressed("Switch"))
            SwapSpell();

        (Game.FirstSpellSlot.GetNode("AnimatedSprite") as AnimatedSprite).Animation = SpellPool[0].Name;
        (Game.SecondSpellSlot.GetNode("AnimatedSprite") as AnimatedSprite).Animation = SpellPool[1].Name;
    }

    // Unlock a spell.
    public void UnlockSpell(Spell pSpell)
    {
        pSpell.Unlocked = true;
    }

    //Set all spell.
    public void SetAllSpell()
    {
        AllSpell = new Spell[2];

        AllSpell[0] = this.GetNode("Water") as Spell;
        AllSpell[1] = this.GetNode("Fire") as Spell;
    }

    // Change an equipped spell for a new one.
    public void ChangeSpell(int idx, Spell spell){
        SpellPool[idx] = spell as Spell;
    }

    // Swaps the two current spells
    public void SwapSpell(){
        SpellPool[0].Visible = false;

        Spell temp = SpellPool[0];
        SpellPool[0] = SpellPool[1];
        SpellPool[1] = temp;

        SpellPool[0].Visible = true;
        SpellPool[0].Equipped = true;
        SpellPool[1].Equipped = false;
    }
}
