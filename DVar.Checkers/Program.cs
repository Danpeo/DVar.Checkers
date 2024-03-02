using DVar.Checkers.Entities;
using DVar.Checkers.Params;
using Raylib_cs;
using static Raylib_cs.Raylib;

InitWindow(Param.ScreenWidth, Param.ScreenHeight, "CHECKERS");
SetTargetFPS(Param.TargetFPS);

var board = new Board(BoardParam.Cols, BoardParam.Rows,
    Param.ScreenWidth / 2 - BoardParam.Cols * BoardParam.TileSize / 2,
    Param.ScreenHeight / 2 - BoardParam.Cols * BoardParam.TileSize / 2);


while (!WindowShouldClose())
{
    BeginDrawing();
    {
        ClearBackground(Color.Black);
        board.Draw(Color.White, Color.DarkGray);
        board.Update();
    }
    EndDrawing();
}

CloseWindow();