namespace RTS.Engine.UI;

interface IUIProvider: IDisposable {
    void Init();
    void Frame();
}
