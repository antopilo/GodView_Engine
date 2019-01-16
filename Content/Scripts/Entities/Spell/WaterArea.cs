using Godot;
using System;

// Splash of water on the ground.
public class WaterArea : Node2D
{
    private void _on_HitZone_area_entered(object body)
    {
        if(body is null || (body as Node2D).GetParent() is null)
            return;
        if((body as Node2D).GetParent() is FireArea && ((body as Node2D).GetParent() as FireArea).Burning)
            ((body as Node2D).GetParent() as FireArea).Extinguish();   
        
    }

    // private void _on_HitZone_body_entered(object body)
    // {
    //     if(body is FireArea && (body as FireArea).Burning)
    //         (body as FireArea).Extinguish();
    // }
}
