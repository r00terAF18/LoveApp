using System.Numerics;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace LoveApp;

public sealed class LoveLetterUI : UI
{
    private static readonly string[] Lines =
    [
        "You make ordinary moments feel special.",
        "Your smile is my favorite kind of sunshine.",
        "Being around you makes everything feel lighter.",
        "And somehow, I like you more every single day."
    ];

    public override string Name => "A Note for You";

    public override void Draw()
    {
        Rectangle card = DrawCard(720f, 390f);
        DrawOrbitingHearts(card);

        DrawCenteredText("Dear Mohadeseh,", (int)card.Y + 48, 34, Color.Maroon);

        for (int i = 0; i < Lines.Length; i++)
        {
            DrawCenteredText(
                Lines[i],
                (int)card.Y + 125 + i * 48,
                24,
                new Color(103, 48, 70, 255));
        }

        DrawCenteredText("With lots of love, <3", (int)card.Y + 330, 25, Color.Maroon);
    }

    private static void DrawOrbitingHearts(Rectangle card)
    {
        float time = (float)R.GetTime();
        Vector2 center = new(card.X + card.Width / 2f, card.Y + card.Height / 2f);

        for (int i = 0; i < 10; i++)
        {
            float angle = time * 0.45f + i * MathF.Tau / 10f;
            Vector2 position = center + new Vector2(
                MathF.Cos(angle) * (card.Width / 2f + 26f),
                MathF.Sin(angle) * (card.Height / 2f + 18f));

            Heart heart = new(
                position,
                7f + MathF.Sin(time * 2f + i) * 2f,
                Vector2.Zero,
                1f,
                0f,
                i % 2 == 0 ? Color.Pink : Color.Red);
            heart.Draw();
        }
    }
}

public sealed class ComplimentJarUI : UI
{
    private static readonly string[] Compliments =
    [
        "Your laugh could improve absolutely any day.",
        "You have a beautiful way of making people feel seen.",
        "Your kindness is one of my favorite things about you.",
        "You are effortlessly adorable, Mohadeseh.",
        "Talking to you is always the best part of my day.",
        "You make my heart do a tiny happy dance.",
        "You are even more wonderful than you realize."
    ];

    private int _complimentIndex;

    public override string Name => "Compliment Jar";

    public override void Draw()
    {
        Rectangle card = DrawCard(760f, 350f);
        DrawCenteredText("Mohadeseh's Compliment Jar", (int)card.Y + 45, 34, Color.Maroon);
        DrawCenteredText(
            Compliments[_complimentIndex],
            (int)card.Y + 145,
            24,
            new Color(103, 48, 70, 255));

        Rectangle button = new(
            card.X + card.Width / 2f - 130f,
            card.Y + 245f,
            260f,
            58f);

        if (DrawColoredButton(
                button,
                "Pick another",
                new Color(236, 112, 154, 255),
                new Color(219, 83, 128, 255),
                new Color(183, 57, 99, 255)))
        {
            int previousIndex = _complimentIndex;
            while (_complimentIndex == previousIndex)
                _complimentIndex = Random.Shared.Next(Compliments.Length);

            MouseTrail.EmitHeartBurst(
                new Vector2(button.X + button.Width / 2f, button.Y),
                36);
        }
    }
}

public sealed class LoveMeterUI : UI
{
    private float _loveLevel = 0.35f;
    private bool _celebratedMaximum;

    public override string Name => "Love-O-Meter";

    public override void Draw()
    {
        Rectangle card = DrawCard(700f, 380f);
        DrawCenteredText("Love-O-Meter for Mohadeseh", (int)card.Y + 45, 34, Color.Maroon);

        string meterText = _loveLevel >= 1f
            ? "OFF THE CHARTS! <3"
            : $"Love level: {(int)(_loveLevel * 100f)}%";
        DrawCenteredText(meterText, (int)card.Y + 120, 27, Color.Maroon);

        Rectangle meterBounds = new(card.X + 90f, card.Y + 180f, card.Width - 180f, 42f);
        R.DrawRectangleRounded(meterBounds, 0.5f, 12, new Color(242, 210, 222, 255));

        Rectangle fillBounds = new(
            meterBounds.X + 5f,
            meterBounds.Y + 5f,
            (meterBounds.Width - 10f) * _loveLevel,
            meterBounds.Height - 10f);
        R.DrawRectangleRounded(fillBounds, 0.5f, 12, new Color(225, 65, 113, 255));

        Rectangle button = new(
            card.X + card.Width / 2f - 120f,
            card.Y + 270f,
            240f,
            58f);

        if (DrawColoredButton(
                button,
                _loveLevel >= 1f ? "Still more love!" : "Add more love",
                new Color(225, 65, 113, 255),
                new Color(202, 48, 96, 255),
                new Color(164, 38, 77, 255)))
        {
            _loveLevel = Math.Min(1f, _loveLevel + 0.13f);
            int burstSize = _loveLevel >= 1f ? 120 : 24;
            MouseTrail.EmitHeartBurst(
                new Vector2(button.X + button.Width / 2f, button.Y),
                burstSize);

            if (_loveLevel >= 1f)
                _celebratedMaximum = true;
        }

        if (_celebratedMaximum)
            DrawCenteredText("The meter was never big enough anyway.", (int)card.Y + 340, 20, Color.Maroon);
    }
}
