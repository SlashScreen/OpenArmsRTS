using Brutal.ImGuiApi;
using Brutal.Strings;
using Raylib_cs;

namespace RTS.Engine.UI;

class UIManager: IDisposable {
    ImGuiContextPtr ctx = ImGui.CreateContext();

    public void Frame() {
        // TODO: Make this scriptable or some shit
        ImGui.NewFrame();

        {
            ImGui.Text("Hello World".GetUTF8Bytes());
        }

        ImGui.Render();

        var drawInstructions = ImGui.GetDrawData();
    }

    public void Dispose()
    {
        ImGui.DestroyContext(ctx);
    }
}
