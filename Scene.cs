using RayGuiSharp;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace LoveApp;

public class Scene
{
    public List<UI> UIs { get; } = [];
    public int SelectedUIIndex { get; private set; }
    public UI? ActiveUI { get; private set; }

    public void AddUI(UI ui)
    {
        UIs.Add(ui);
    }

    public void RemoveUI(UI ui)
    {
        int removedIndex = UIs.IndexOf(ui);
        if (removedIndex < 0)
            return;

        UIs.RemoveAt(removedIndex);

        if (UIs.Count == 0)
        {
            SelectedUIIndex = 0;
            ActiveUI = null;
            return;
        }

        SelectScene(Math.Min(SelectedUIIndex, UIs.Count - 1));
    }

    public void SelectScene(int index)
    {
        if (index < 0 || index >= UIs.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        SelectedUIIndex = index;
        ActiveUI = UIs[SelectedUIIndex];
    }

    public void SelectNext()
    {
        if (UIs.Count == 0)
            return;

        SelectScene((SelectedUIIndex + 1) % UIs.Count);
    }

    public void SelectPrevious()
    {
        if (UIs.Count == 0)
            return;

        SelectScene((SelectedUIIndex - 1 + UIs.Count) % UIs.Count);
    }

    public void Draw()
    {
        HandleKeyboardNavigation();
        ActiveUI?.Draw();
        DrawNavigation();
    }

    private void HandleKeyboardNavigation()
    {
        if (R.IsKeyPressed(KeyboardKey.Left))
            SelectPrevious();
        else if (R.IsKeyPressed(KeyboardKey.Right))
            SelectNext();
    }

    private void DrawNavigation()
    {
        if (ActiveUI is null || UIs.Count < 2)
            return;

        int screenWidth = R.GetScreenWidth();
        int screenHeight = R.GetScreenHeight();
        const float buttonWidth = 145f;
        const float buttonHeight = 42f;
        float buttonY = screenHeight - buttonHeight - 18f;

        R.DrawRectangle(
            0,
            screenHeight - 76,
            screenWidth,
            76,
            new Color(255, 255, 255, 115));

        int previousTextSize = RayGui.GuiGetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize);
        RayGui.GuiSetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize,
            20);

        bool previousPressed = RayGui.GuiButton(
            new Rectangle(22f, buttonY, buttonWidth, buttonHeight),
            "< Previous").HasFlag(GuiResult.Pressed);
        bool nextPressed = RayGui.GuiButton(
            new Rectangle(screenWidth - buttonWidth - 22f, buttonY, buttonWidth, buttonHeight),
            "Next >").HasFlag(GuiResult.Pressed);

        RayGui.GuiSetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize,
            previousTextSize);

        if (previousPressed)
            SelectPrevious();

        if (nextPressed)
            SelectNext();

        string label = $"{SelectedUIIndex + 1} / {UIs.Count}  -  {ActiveUI.Name}";
        int labelWidth = R.MeasureText(label, 20);
        R.DrawText(
            label,
            (screenWidth - labelWidth) / 2,
            screenHeight - 49,
            20,
            Color.Maroon);
    }
}
