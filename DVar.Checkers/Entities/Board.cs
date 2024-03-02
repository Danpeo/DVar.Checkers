using System.Reflection.Emit;
using DVar.Checkers.Params;
using Raylib_cs;

namespace DVar.Checkers.Entities;

public class Board
{
    private int _posX;
    private int _posY;
    private int _cols;
    private int _rows;
    private Piece?[,] _pieces;
    public Color _cellColor;

    private int _currentlySelectedX = -1;
    private int _currentlySelectedY = -1;
    private bool _captured;
    private bool _moved;
    private bool _isQueen;

    public Board(int cols, int rows, int posX, int posY)
    {
        _cols = cols;
        _rows = rows;
        _posX = posX;
        _posY = posY;
        _pieces = new Piece[rows, cols];

        InitPieces();
    }

    public void Update()
    {
        if (!Raylib.IsMouseButtonPressed(MouseButton.Left))
            return;

        int mouseX = Raylib.GetMouseX();
        int mouseY = Raylib.GetMouseY();

        int tileX = (mouseX - _posX) / BoardParam.TileSize;
        int tileY = (mouseY - _posY) / BoardParam.TileSize;

        if (!isWithinBoard())
            return;

        Console.WriteLine($"{tileX}:{tileY}");
        Console.WriteLine($"{_currentlySelectedX}:{_currentlySelectedY}");
        _cellColor = Color.Orange;

        Piece? clickedPiece = _pieces[tileY, tileX];
        if (clickedPiece != null && clickedPiece.IsWhite == Rules.WhiteTurn)
        {
            _currentlySelectedX = tileX;
            _currentlySelectedY = tileY;

            _isQueen = clickedPiece.IsQueen;
            DrawTile(tileX, tileY, _posX, _posY);
        }
        else
        {
            if (Rules.IsValidMove(_currentlySelectedX, _currentlySelectedY, tileX, tileY, _isQueen) &&
                !Rules.IsAnyPieceOnCell(_pieces, tileX, tileY) && !_captured)
            {
                MovePiece(tileY, tileX);
                switchTurn();
            }
            else if (canCapture())
            {
                if (tryCapture())
                {
                    _currentlySelectedX = tileX;
                    _currentlySelectedY = tileY;
                    _captured = true;
                }
                return;
            }
            else
            {
                _captured = false;
                switchTurn();
            }

            _currentlySelectedX = -1;
            _currentlySelectedY = -1;
        }

        return;

        void switchTurn()
        {
            if (_moved)
            {
                Rules.WhiteTurn = !Rules.WhiteTurn;
                _moved = false;
            }
        }

        bool tryCapture()
        {
            int midX = (_currentlySelectedX + tileX) / 2;
            int midY = (_currentlySelectedY + tileY) / 2;

            if (TryDestroyPiece(midY, midX))
            {
                MovePiece(tileY, tileX);
                return true;
            }

            return false;
        }

        bool canCapture() =>
            IsSelected() && Rules.CanCapture(_currentlySelectedX, tileX, _isQueen) &&
            !Rules.IsAnyPieceOnCell(_pieces, tileX, tileY);

        bool isWithinBoard() =>
            tileX >= 0 && tileX < _cols && tileY >= 0 && tileY < _rows;
    }

    private void MovePiece(int tileY, int tileX)
    {
        _moved = true;
        _pieces[tileY, tileX] = _pieces[_currentlySelectedY, _currentlySelectedX];
        DestroyPiece(_currentlySelectedY, _currentlySelectedX);
    }

    private bool TryDestroyPiece(int tileY, int tileX)
    {
        Piece? piece = _pieces[tileY, tileX];
        if (piece != null)
        {
            _pieces[tileY, tileX] = null;
            return true;
        }
        return false;
    }

    private void DestroyPiece(int tileY, int tileX) => _pieces[tileY, tileX] = null;

    public void Draw(Color colorEven, Color colorOdd)
    {
        const int tileSize = BoardParam.TileSize;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                _cellColor = (x + y) % 2 == 0 ? colorEven : colorOdd;

                if (_currentlySelectedX == x && _currentlySelectedY == y)
                {
                    _cellColor = Color.Orange;
                }

                DrawTile(x, y, _posX, _posY);

                if (_pieces[y, x] != null)
                {
                    _pieces[y, x]!.Draw(tileSize / 3, x * tileSize + _posX + tileSize / 2,
                        y * tileSize + _posY + tileSize / 2);
                }
            }
        }
    }

    private void DrawTile(int x, int y, int posX, int posY)
    {
        const int tileSize = BoardParam.TileSize;
        Raylib.DrawRectangle(x * tileSize + posX, y * tileSize + posY, tileSize, tileSize, _cellColor);
    }

    private void InitPieces()
    {
        int rows = (_rows - 2) / 2;

        for (int y = 0; y < rows; y++)
        {
            init(y, Color.Lime, white: false);
        }

        for (int y = _rows - 1; y >= _rows - rows; y--)
        {
            init(y, Color.Red, white: true);
        }

        return;

        void init(int y, Color color, bool white)
        {
            for (int x = (y % 2 == 0) ? 1 : 0; x < _cols; x += 2)
            {
                _pieces[y, x] = new Piece(color, white);
            }
        }
    }

    private bool IsSelected() => _currentlySelectedX != -1 && _currentlySelectedY != -1;
}