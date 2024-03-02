using Raylib_cs;

namespace DVar.Checkers.Entities;

public class Piece
{
    public int PosX { get; set; }
    public int PosY { get; private set; }
    public bool IsWhite { get; set; }
    public bool IsQueen { get; set; } 
    private Color Color { get; set; }

    public Piece(Color color, bool isWhite)
    {
        Color = color;
        IsWhite = isWhite;
    }
    
    public void Draw(int radius, int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
        Raylib.DrawCircle(posX, posY, radius, Color);
    }
}