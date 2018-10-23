using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PieceManager
{
    private Game game;
    private System.Collections.Generic.List<Piece> pieces;

    public void setGame(Game game)
    {
        this.game = game;
    }

    public void setPieces(System.Collections.Generic.List<Piece> pieces)
    {
        this.pieces = pieces;
    }

    public System.Collections.Generic.List<object> createNewPieceBlocks(Piece piec, int row, int column)
    {
        Piece piece = new Piece(this.game);

        System.Random random = new System.Random();

        int randomNumber = random.Next(7);

        if (randomNumber == 0)
        {
            piece.setBlock1(row, column);
            piece.setBlock2(row + 1, column);
            piece.setBlock3(row + 2, column);
            piece.setBlock4(row + 3, column);
            piece.setKind("line");
        }
        else if(randomNumber == 1)
        {
            piece.setBlock1(row, column);
            piece.setBlock2(row, column + 1);
            piece.setBlock3(row + 1, column);
            piece.setBlock4(row + 1, column + 1);
            piece.setKind("square");
        }
        else if(randomNumber == 2)
        {
            piece.setBlock1(row, column);
            piece.setBlock2(row + 1, column);
            piece.setBlock3(row + 1, column + 1);
            piece.setBlock4(row + 2, column + 1);
            piece.setKind("rs");
        }
        else if (randomNumber == 3)
        {
            piece.setBlock1(row, column + 1);
            piece.setBlock2(row + 1, column + 1);
            piece.setBlock3(row + 2, column + 1);
            piece.setBlock4(row + 2, column);
            piece.setKind("la");
        }
        else if (randomNumber == 4)
        {
            piece.setBlock1(row, column + 1);
            piece.setBlock2(row + 1, column + 1);
            piece.setBlock3(row + 1, column);
            piece.setBlock4(row + 2, column);
            piece.setKind("ls");
        }
        else if (randomNumber == 5)
        {
            piece.setBlock1(row, column);
            piece.setBlock2(row + 1, column);
            piece.setBlock3(row + 2, column);
            piece.setBlock4(row + 2, column + 1);
            piece.setKind("ra");
        }
        else if (randomNumber == 6)
        {
            piece.setBlock1(row, column + 1);
            piece.setBlock2(row + 1, column);
            piece.setBlock3(row + 1, column + 1);
            piece.setBlock4(row + 1, column + 2);
            piece.setKind("hat");
        }
        piece.setPosition("up");

        System.Collections.Generic.List<object> list = new System.Collections.Generic.List<object>();
        list.Add(piece);
        list.Add(this.pieces);

        return list;
    }
}