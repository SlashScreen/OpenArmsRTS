using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class RaylibRenderer: IRenderer {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    public void Init()
    {
        Raylib.InitWindow(800, 400, "Hello World");
    }

    public void Render()
    {
        Raylib.BeginDrawing();
        {
            Raylib.ClearBackground(bgColor);

            Raylib.DrawText("Hello Raylib!", 12, 12, 20, Raylib_cs.Color.Black);
        }
        Raylib.EndDrawing();
    }

    public void Shutdown()
    {
        Raylib.CloseWindow();
    }
}
