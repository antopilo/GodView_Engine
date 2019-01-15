using Godot;
using System.Collections.Generic;

public class Oil : Entity
{
    private bool PlayerPresent = false;
    private bool Ignited = false;
    private bool Triggered = false;
    [Export] float DamagePerSecond = 5;
    [Export] float IgniteDelay = 0.2f;

    [Export] Vector2 SizeReduction = new Vector2(1.00001f, 1.00001f);
    [Export] Color OilIgnitedColor = new Color(255,255,255);
    private Player _Player = null;
    private Timer _Timer;
    private List<Node2D> Colliding = new List<Node2D>();
    RandomNumberGenerator rbg = new RandomNumberGenerator();

    public override void _Ready()
    {
        base._Ready();
        rbg.Randomize();
        
        foreach (var areas in (GetNode("HitZone") as Area2D).GetOverlappingAreas())
        {
            Colliding.Add((areas as Area2D).GetParent() as Node2D);
        }
    }

    public void RandomSize()
    {
        float rng = rbg.RandfRange(0.5f, 1.5f);
        this.Scale = new Vector2(rng, rng);
    }
    public override void _PhysicsProcess(float delta)
    {
        (GetNode("Flames") as Particles2D).Emitting = Ignited;
        if (Ignited == true)
        {
            (this as CanvasItem).Modulate = OilIgnitedColor;
            this.Scale -= SizeReduction * delta / 2;
            (GetNode("Sprite") as CanvasItem).Modulate = new Color(1,1,1, (GetNode("Sprite") as CanvasItem).Modulate.a - SizeReduction.x * delta);
            if ((GetNode("Sprite") as CanvasItem).Modulate.a <= 0 )
                this.QueueFree();
            if (PlayerPresent && _Player != null)
            {
                _Player.HurtPlayer(DamagePerSecond / 60);
                _Player.Hurting = true;
            }
            else if(_Player != null)
                _Player.Hurting = false;
        }
        
        if(Triggered == false)
            CheckFlame();

    }

    private void CheckFlame()
    {
        foreach (Node2D area in Colliding)
        {
            if (area is Oil && (area as Oil).Ignited)
            {
                StartIgnite();
            }
            else if (area is FireArea)
            {
                StartIgnite();
            }
        }
    }

    private void StartIgnite()
    {
        if (Triggered)
            return;


        if (IgniteDelay <= 0 && !Ignited && !Triggered)
        {
            //Ignite();
            //Triggered = true;
            return;
        }
        if (!Ignited && !Triggered)
        {
            Triggered = true;

            var Timer = new Timer();
            Timer.WaitTime = IgniteDelay;
            _Timer = Timer;
            AddChild(Timer);
            Timer.Connect("timeout", this, "Ignite");
            Timer.Start();
        }
    }

    private void Ignite()
    {
        _Timer.Stop();
        _Timer.QueueFree();
        Ignited = true;
    }

    // Signals

    //Area Entering
    private void _on_HitZone_area_entered(object area)
    {
        var parent = (area as Node2D).GetParent() as Node2D;
        if( parent is FireArea || parent is Oil)
        {
            Colliding.Add(parent);
        }
    }


    private void _on_HitZone_area_exited(object area)
    {
        var parent = (area as Node2D).GetParent() as Node2D;
        if (parent is FireArea || parent is Oil)
        {
            Colliding.Remove(parent);
        }
    }

    // Body Entering
    private void _on_HitZone_body_entered(object body)
    {
        if (body is Player)
        {
            PlayerPresent = true;
            _Player = body as Player;
        }
    }

    private void _on_HitZone_body_exited(object body)
    {
        if (body is Player)
        {
            PlayerPresent = false;
        }
    }
}







