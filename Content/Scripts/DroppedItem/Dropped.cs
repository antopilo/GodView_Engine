
using Godot;
using System;

public class Dropped : Node2D
{
    private int IndexRS;
    private Spell RandomSpell;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GenerateSpell();
        (this.GetNode("AnimatedSprite") as AnimatedSprite).Animation = RandomSpell.Name;
    }

    public void GenerateSpell()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        IndexRS = rng.RandiRange(0,1);
        RandomSpell = Game.Hand.AllSpell[IndexRS];
    }

    public void Interact()
    {
        //RandomSpell = Game.Hand.AllSpell[GenerateSpell()];
        //Game.Hand.ChangeSpell(0, RandomSpell);
    }

}
