namespace RTS.Engine.Systems;

interface ISystem
{
    /// <summary> Initialize this system, allowing hooks to be subscribed to. </summary>
    void Init(Engine engine);
}
