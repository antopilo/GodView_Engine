using Godot;
using System;

public class Dropped : Node2D
{
    private int IndexRS;
    private Spell RandomSpell;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RandomSpell = Game.Hand.AllSpell[GenerateSpell()];
        (this.GetNode("AnimatedSprite") as AnimatedSprite).Animation = RandomSpell.Name;
    }

    public int GenerateSpell()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        IndexRS = rng.RandiRange(0,1);
        return IndexRS;
    }

    public void Interact()
    {
        Game.Hand.ChangeSpell(0, RandomSpell);
    }
}
