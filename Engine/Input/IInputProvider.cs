namespace RTS.Engine.Input;

interface IInputProvider {
    public delegate void InputEventDelegate(string name, InputEvent inputEvent);
    public event InputEventDelegate OnInput;

    void Poll();
}
