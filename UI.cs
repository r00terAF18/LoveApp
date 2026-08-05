using System.Numerics;
using RayGuiSharp;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace LoveApp;

public class UI
{
    private const string Question = "Would you go out on a date with me?";
    private const float ButtonGap = 28f;
    private const float ResizeFactor = 0.85f;

    private float _yesWidth = 150f;
    private float _yesHeight = 56f;
    private float _noWidth = 150f;
    private float _noHeight = 56f;
    private bool _accepted;

    public void Draw()
    {
        int screenWidth = R.GetScreenWidth();
        int screenHeight = R.GetScreenHeight();

        if (_accepted)
        {
            DrawCenteredText("It's a date! <3", screenHeight / 2 - 24, 40, Color.Maroon);
            return;
        }

        DrawCenteredText(Question, screenHeight / 2 - 115, 30, Color.Maroon);

        float totalWidth = _yesWidth + ButtonGap + _noWidth;
        float startX = (screenWidth - totalWidth) / 2f;
        float buttonCenterY = screenHeight / 2f + 25f;

        Rectangle yesBounds = new(
            startX,
            buttonCenterY - _yesHeight / 2f,
            _yesWidth,
            _yesHeight);

        Rectangle noBounds = new(
            startX + _yesWidth + ButtonGap,
            buttonCenterY - _noHeight / 2f,
            _noWidth,
            _noHeight);

        if (DrawColoredButton(
                yesBounds,
                "Yes",
                new Color(46, 204, 113, 255),
                new Color(39, 174, 96, 255),
                new Color(30, 132, 73, 255)))
        {
            Vector2 burstPosition = new(
                yesBounds.X + yesBounds.Width / 2f,
                yesBounds.Y + yesBounds.Height / 2f);
            MouseTrail.EmitHeartBurst(burstPosition);
            _accepted = true;
        }

        if (DrawColoredButton(
                noBounds,
                "No",
                new Color(231, 76, 60, 255),
                new Color(192, 57, 43, 255),
                new Color(146, 43, 33, 255)))
        {
            ShrinkNoAndGrowYes(screenWidth, screenHeight);
        }
    }

    private void ShrinkNoAndGrowYes(int screenWidth, int screenHeight)
    {
        _noWidth = Math.Max(48f, _noWidth * ResizeFactor);
        _noHeight = Math.Max(28f, _noHeight * ResizeFactor);

        float growFactor = 2f - ResizeFactor;
        _yesWidth = Math.Min(screenWidth * 0.55f, _yesWidth * growFactor);
        _yesHeight = Math.Min(screenHeight * 0.25f, _yesHeight * growFactor);
    }

    private static bool DrawColoredButton(
        Rectangle bounds,
        string text,
        Color normal,
        Color focused,
        Color pressed)
    {
        int previousNormal = RayGui.GuiGetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorNormal);
        int previousFocused = RayGui.GuiGetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorFocused);
        int previousPressed = RayGui.GuiGetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorPressed);
        int previousTextSize = RayGui.GuiGetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize);

        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorNormal,
            R.ColorToInt(normal));
        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorFocused,
            R.ColorToInt(focused));
        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorPressed,
            R.ColorToInt(pressed));
        RayGui.GuiSetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize,
            24);

        bool wasPressed = RayGui.GuiButton(bounds, text).HasFlag(GuiResult.Pressed);

        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorNormal,
            previousNormal);
        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorFocused,
            previousFocused);
        RayGui.GuiSetStyle(
            GuiControl.Button,
            GuiControlProperty.BaseColorPressed,
            previousPressed);
        RayGui.GuiSetStyle(
            GuiControl.Default,
            GuiDefaultProperty.TextSize,
            previousTextSize);

        return wasPressed;
    }

    private static void DrawCenteredText(string text, int y, int fontSize, Color color)
    {
        int width = R.MeasureText(text, fontSize);
        R.DrawText(text, (R.GetScreenWidth() - width) / 2, y, fontSize, color);
    }
}
