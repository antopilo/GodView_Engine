using Godot;
using System;

public class Entity : Node2D
{
    public float Height = 16;
    public float Width = 16;
    public bool Selected = false;
    public bool Active = true;

    public Vector2 Size
    {
        get { return new Vector2(Width, Height); }
    }

    public override void _Ready()
    {
        if (this.HasNode("Shadow"))
        {
            GetSize(); // Get the size of the Sprite.

            var shadow = GetNode("Shadow") as Sprite;
            if(shadow.Material == null)
                return;
            (shadow.Material as ShaderMaterial).SetShaderParam("Height", Width);
            (shadow.Material as ShaderMaterial).SetShaderParam("Width", Height);
            //GD.Print("Detected size of " + this.Name + " is X:" + Width + " Y:" + Height);
        }
    }

    private void GetSize()
    {
        if (this.HasNode("Sprite"))
        {
            if (GetNode("Sprite") is AnimatedSprite)
                return;

            Sprite sprite = GetNode("Sprite") as Sprite;
            Height = sprite.Texture.GetHeight() * sprite.Scale.y;
            Width = sprite.Texture.GetWidth() * sprite.Scale.x;

            GD.Print("detected height : " + Height + " Detected Width: " + Width);
        }
    }
    public override void _Draw()
    {
        if (!Selected)
            return;

        if (this.HasNode("Sprite"))
        {
            if (GetNode("Sprite") is AnimatedSprite)
                return;

            Sprite sprite = GetNode("Sprite") as Sprite;
            Height = sprite.Texture.GetHeight() * sprite.Scale.y;
            Width = sprite.Texture.GetWidth() * sprite.Scale.x;

            GD.Print("detected height : " + Height + " Detected Width: " + Width);
        }
        var position = new Vector2(-Width / 2, 0);
        var rectangle = new Rect2(position, new Vector2(Width, -Height));
        DrawRect(rectangle, new Color("E50000"), false);
    }

    public bool AtPosition(Vector2 pPosition)
    {
        return (pPosition.x > this.GlobalPosition.x  - Width / 2 && pPosition.y > this.GlobalPosition.y - Height &&
            pPosition.x < this.GlobalPosition.x + Width / 2 && pPosition.y < this.GlobalPosition.y);
    }
}
