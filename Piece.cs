using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Piece
{
    public Block block1, block2, block3, block4;
    public string kind;
    public string position;
    private Game Game;

    public Piece(Game game)
    {
        Game = game;
    }

    public void setKind(string kind)
    {
        this.kind = kind;
    }

    public void setPosition(string position)
    {
        this.position = position;
    }
    
    public void setBlock1(int row, int column)
    {
        block1 = new Block(row, column);
        block1.texture = Game.Content.Load<Texture2D>("block");
    }

    public void setBlock2(int row, int column)
    {
        block2 = new Block(row, column);
        block2.texture = Game.Content.Load<Texture2D>("block");
    }

    public void setBlock3(int row, int column)
    {
        block3 = new Block(row, column);
        block3.texture = Game.Content.Load<Texture2D>("block");
    }

    public void setBlock4(int row, int column)
    {
        block4 = new Block(row, column);
        block4.texture = Game.Content.Load<Texture2D>("block");
    }
}