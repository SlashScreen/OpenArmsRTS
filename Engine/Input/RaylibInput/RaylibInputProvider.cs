using Raylib_cs;

namespace RTS.Engine.Input.RaylibInput;

class RaylibInputProvider(Dictionary<RaylibInputProvider.IInputKind, string> InputMap): IInputProvider {
    public event IInputProvider.InputEventDelegate? OnInput;

    public void Poll(double delta)
    {
        foreach (var (action, actionName) in InputMap)
        {
            switch (action)
            {
                case IsKeyDown kp:
                    {
                        if (Raylib.IsKeyPressed(kp.Key)) OnInput?.Invoke(actionName, new KeyPressed(), delta);
                        if (Raylib.IsKeyDown(kp.Key)) OnInput?.Invoke(actionName, new KeyDown(), delta);
                        break;
                    }
            }
        }

        if (Raylib.WindowShouldClose()) OnInput?.Invoke(Engine.QUIT_ACTION_NAME, new Quit(), delta);
    }

    public interface IInputKind;
    public record struct IsKeyDown(KeyboardKey Key) : IInputKind;
    public record struct MouseButtonInput(int Button) : IInputKind;

    public record class KeyPressed() : InputEvent;
    public record class KeyDown() : InputEvent;
}
