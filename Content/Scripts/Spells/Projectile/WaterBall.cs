using Godot;
using System;

public class WaterBall : Node2D
{
    [Export] public float DieDistance;
    [Export] public PackedScene Impact;

    [Export] private float ProjectileSpeed = 50f;
    private Vector2 StartPosition;

    public override void _Ready()
        => StartPosition = this.GlobalPosition;

    public override void _PhysicsProcess(float delta)
    {
        CheckDistance();
        MoveLocalX(ProjectileSpeed * delta);
    }

    // Calculates the distance from the Initial position with the Current position
    // It calles Die() when the distance is Higher than the DieDistance set.
    private void CheckDistance()
    {
        if(DieDistance <= 0)
            return;
        if((this.GlobalPosition - this.StartPosition).Length() >= DieDistance)
            Die();
    }

    // Called a Body enters the hitZone.
    private void _on_Area2D_body_entered(object body)
    {
        if(body is Player)
            return;

        Die();
    }

    // Not used for now.
    private void _on_Area2D_body_exited(object body)
    {
    }

    private void Die()
    {
        if(Impact == null) return;

        var impact = Impact.Instance() as Node2D;
        impact.GlobalPosition = this.GlobalPosition;
        Game.Entities.AddChild(impact);

        (GetNode("Area2D") as Area2D).SetDeferred("monitoring", false);
        (GetNode("Area2D") as Area2D).SetDeferred("monitorable", false);

        QueueFree();
    }
}
