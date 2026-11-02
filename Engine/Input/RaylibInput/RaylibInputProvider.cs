using Raylib_cs;

namespace RTS.Engine.Input.RaylibInput;

class RaylibInputProvider(Dictionary<RaylibInputProvider.IInputKind, string> InputMap): IInputProvider {
    public event IInputProvider.InputEventDelegate? OnInput;

    public void Poll()
    {
        foreach (var (action, actionName) in InputMap)
        {
            switch (action)
            {
                case IsKeyJustPressed kp:
                    {
                        if (Raylib.IsKeyPressed(kp.Key)) OnInput?.Invoke(actionName, new KeyPressed());
                        break;
                    }
                case IsKeyDown kp:
                    {
                        if (Raylib.IsKeyPressedRepeat(kp.Key)) OnInput?.Invoke(actionName, new KeyDown());
                        break;
                    }
            }
        }

        if (Raylib.WindowShouldClose()) OnInput?.Invoke(Engine.QUIT_ACTION_NAME, new Quit());
    }

    public interface IInputKind;
    public record struct IsKeyJustPressed(KeyboardKey Key) : IInputKind;
    public record struct IsKeyDown(KeyboardKey Key) : IInputKind;
    public record struct MouseButtonInput(int Button) : IInputKind;

    public record class KeyPressed() : InputEvent;
    public record class KeyDown() : InputEvent;
}
