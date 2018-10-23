using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

public class Block
{
    public int Row;
    public int Column;
    public Texture2D texture;

    public Block(int row, int column)
    {
        Row = row;
        Column = column;
    }
}