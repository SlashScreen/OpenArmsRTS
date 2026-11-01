using rlImGui_cs;
using ImGuiNET;

namespace RTS.Engine.UI;

class ImGuiProvider: IUIProvider {
    public void Init() {
        rlImGui.Setup(true);
    }

    public void Frame()
    {
        // TODO: Make this scriptable or some shit
        rlImGui.Begin();
        {
            if (ImGui.Begin("Simple Test", ImGuiWindowFlags.NoMove))
            {
                ImGui.TextUnformatted("helloooooo");
            }
        }
        ImGui.End();
        rlImGui.End();
    }

    public void Dispose()
    {
        rlImGui.Shutdown();
    }
}
