namespace RTS.Engine.Input;

interface IInputProvider {
    public delegate void InputEventDelegate(string name, InputEvent inputEvent, double delta);
    public event InputEventDelegate OnInput;

    void Poll(double delta);
}
