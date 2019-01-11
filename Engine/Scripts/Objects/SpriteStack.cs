using Godot;
using System;

/// <summary>
/// This is a class to represent a stack of sprite.
/// </summary>
public class SpriteStack : Node2D
{
    const int MaxSpriteCount = 20;
    public Sprite[] Layers = new Sprite[MaxSpriteCount];

    /// <summary>
    /// Stack the sprites
    /// </summary>
    public void MakeStackFromPath(string path)
    {
        // Set the name of the stack to the name of the loaded folder.
        string[] path_ = path.Split("\\");
        this.Name = path_[path_.Length - 1];

        int offset = 0;
        foreach (var image in GetSpritesFromPath(path) )
        {
            // Loading Image
            var image_ = new Image();
            image_.Load(image);

            // Making ImageTexture from image.
            var texture = new ImageTexture();
            texture.CreateFromImage(image_);
            texture.SetData(image_);

            // Adding the layer
            var sprite = new Sprite();
            sprite.SetTexture(texture);
            sprite.Position -= new Vector2(0, offset);
            sprite.Name = offset.ToString();

            Layers[Layers.Length] = sprite;
            this.AddChild(sprite);

            // Increasing the offset for next layer.
            offset += 1;
        }
    }

    /// <summary>
    /// Stack the sprites, but with a custom spacing between each layer.
    /// </summary>
    public void MakeStackFromPath(string path, int offsetSize) 
    {
        int offset = 0;
        foreach (var image in GetSpritesFromPath(path)) 
        {
            // Loading Image
            var image_ = new Image();
            image_.Load(image);
            
            // Making ImageTexture from image.
            var texture = new ImageTexture();
            texture.CreateFromImage(image_);
            texture.SetData(image_);

            // Adding the layer
            var sprite = new Sprite();
            sprite.SetTexture(texture);
            sprite.Position -= new Vector2(0, offset * offsetSize);
            sprite.Name = offset.ToString();

            Layers[Layers.Length] = sprite;
            AddChild(sprite);

            // Increasing the offset for next layer.
            offset += 1;
        }
    }

    /// <summary>
    /// Returns an array of all the files in a specified Folder.
    /// </summary>
    private string[] GetSpritesFromPath(string path)
    {
        string[] files = new string[20];
        var folder = new Directory();
        folder.Open(path);

        // Start listing the files
        folder.ListDirBegin();

        while (true)
        {
            var file = folder.GetNext();
            if (file == "")
                break;
            else if ( !file.BeginsWith(".") && file.EndsWith(".png") ) // making sure its a png
                files[files.Length] = file;
        }

        // Close the listing
        folder.ListDirEnd(); 

        return files;
    }

    /// <summary>
    /// Rotate the stack X amount in degrees.
    /// </summary>
    public void RotateStack(float pRotation)
    {
        foreach(var sprite in Layers)
            sprite.RotationDegrees += pRotation;
    }
}
