using Godot;
using System;

public class Entity : Node2D
{
    [Export] public float Height = 16, Width = 16;
    public bool Selected = false, Active = true;

    // Gets the size
    public Vector2 Size
    {
        get { return new Vector2(Width, Height); }
    }

    public override void _Ready()
    {
        if (this.HasNode("Shadow"))
        {
            //GetSize(); // Get the size of the Sprite.

            // Settings correct height and width for the Shadow Shader.
            var shadow = GetNode("Shadow") as Sprite;
            if(shadow.Material == null) return;
            (shadow.Material as ShaderMaterial).SetShaderParam("Height", Width);
            (shadow.Material as ShaderMaterial).SetShaderParam("Width", Height);
        }
    }

    // Gets the size of the sprite in pixels. Useful for drawing DebugFrame
    private void GetSize()
    {
        if (this.HasNode("Sprite")) // If node has Sprite node.
        {
            if (GetNode("Sprite") is AnimatedSprite) return;
            // Gettings the size in pixels of the Sprite node.
            Sprite sprite = GetNode("Sprite") as Sprite; 
            Height = sprite.Texture.GetHeight() * sprite.Scale.y;
            Width = sprite.Texture.GetWidth() * sprite.Scale.x;
        }
    }

    public override void _Process(float delta)
    {
        Update();
    }

    // Drawing a rectangle around the entity if Selected
    // The rectangle size is determined by the Height and 
    // Width of the sprite Node.
    public override void _Draw()
    {
        if (!Selected)  return;
           
        if (this.HasNode("Sprite"))
        {
            if (GetNode("Sprite") is AnimatedSprite) return;
            Sprite sprite = GetNode("Sprite") as Sprite;
            Height = sprite.Texture.GetHeight() * sprite.Scale.y;
            Width = sprite.Texture.GetWidth() * sprite.Scale.x;
        }

        var position = new Vector2(-Width / 2, 0);
        var rectangle = new Rect2(position, new Vector2(Width, -Height));

        DrawRect(rectangle, new Color("E50000"), false);
        DrawLine(new Vector2(-3, 0), new Vector2(3, 0), new Color(1, 1, 0), 1.25f);
        DrawLine(new Vector2(0, -3), new Vector2(0, 3), new Color(1, 1, 0), 1.25f);
    }

    // Input a global position and returns if that position is located
    // inside the Height and Width of the entity. Returns true.
    // Useful in the editor when selecting objects by clicking on them.
    public bool AtPosition(Vector2 pPosition)
    {
        return (pPosition.x > this.GlobalPosition.x  - Width / 2 && 
            pPosition.y > this.GlobalPosition.y - Height &&
            pPosition.x < this.GlobalPosition.x + Width / 2 
            && pPosition.y < this.GlobalPosition.y);
    }
}
