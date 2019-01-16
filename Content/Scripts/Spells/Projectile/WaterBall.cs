using Godot;
using System;

public class WaterBall : Node2D
{
    [Export] public float DieDistance;
    [Export] public PackedScene Impact;

    private Vector2 StartPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StartPosition = this.GlobalPosition;
    }

    public override void _PhysicsProcess(float delta)
    {
        CheckDistance();

        MoveLocalX(100 * delta);
		//GD.Print("Heres my position: " + this.GlobalPosition.ToString());
    }

    private void CheckDistance()
    {
        if(DieDistance <= 0)
            return;
        if((this.GlobalPosition - this.StartPosition).Length() >= DieDistance)
            Die();
    }
    private void _on_Area2D_body_entered(object body)
    {
        if(body is Player)
            return;

        Die();
    }

    private void _on_Area2D_body_exited(object body)
    {
    }

    private void Die()
    {
        if(Impact == null)
            return;

        var impact = Impact.Instance() as Node2D;
        impact.GlobalPosition = this.GlobalPosition;
        Game.Entities.AddChild(impact);

        (GetNode("Area2D") as Area2D).SetDeferred("monitoring", false);
        (GetNode("Area2D") as Area2D).SetDeferred("monitorable", false);

        this.QueueFree();
        GD.Print(this.Name + "projectile destroyed");
    }
}
