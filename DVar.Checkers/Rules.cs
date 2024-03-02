using DVar.Checkers.Entities;

namespace DVar.Checkers;

public static class Rules
{
    public static bool WhiteTurn { get; set; } = true;

    public static bool IsValidMove(int startX, int startY, int endX, int endY, bool isQueen)
    {
        int deltaX = Math.Abs(endX - startX);
        int deltaY = endY - startY;
        
        if (isQueen)
            return deltaX == Math.Abs(deltaY);

        if (deltaX != 1)
            return false;

        if (deltaY != 1 && !WhiteTurn)
            return false;

        if (deltaY != -1 && WhiteTurn)
            return false;

        return true;
    }

    public static bool IsAnyPieceOnCell(Piece?[,] pieces, int tileX, int tileY) =>
        pieces[tileY, tileX] != null;

    public static bool IsOpponentPieceOnCell(Piece?[,] pieces, int tileX, int tileY)
    {
        Piece? piece = pieces[tileY, tileX];
        return piece != null && piece.IsWhite != WhiteTurn;
    }

    public static bool CanCapture(int startX, int endX, bool isQueen)
    {
        int deltaX = Math.Abs(endX - startX);

        if (isQueen)
            return true;
        
        if (deltaX == 2)
        {
            return true;
        }

        return false;
    }

    public static bool CheckPieceAhead(Piece?[,] pieces, int startX, int endX)
    {
        if (pieces[endX - 1, startX - 1] != null)
        {
            DestroyPiece(pieces, startX - 1, endX - 1);
            return true;
        }

        if (pieces[endX + 1, startX + 1] != null)
        {
            DestroyPiece(pieces, startX + 1, endX + 1);
            return true;
        }

        return false;
    }

    public static void DestroyPiece(Piece?[,] pieces, int tileX, int tileY) =>
        pieces[tileX, tileY] = null;
}