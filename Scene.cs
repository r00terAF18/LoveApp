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
        UIs.Remove(ui);
    }

    public void SelectScene(int index)
    {
        if (index < 0 || index >= UIs.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        SelectedUIIndex = index;
        ActiveUI = UIs[SelectedUIIndex];
    }

    public void Draw()
    {
        ActiveUI?.Draw();
    }
}
