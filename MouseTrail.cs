using System.Numerics;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace LoveApp;

public static class MouseTrail
{
    private const float MovementThreshold = 0.5f;
    private const float TrailSpacing = 10f;
    private const float FallingSpawnInterval = 0.09f;
    private const int MaxHearts = 300;

    private static readonly List<Heart> Hearts = [];
    private static readonly Random Random = new();
    private static readonly Color[] BurstColors =
    [
        Color.Red,
        Color.Pink,
        Color.Maroon,
        Color.Gold,
        Color.White
    ];

    private static Vector2 _previousMousePosition;
    private static bool _hasPreviousPosition;
    private static float _fallingSpawnTimer;

    public static void UpdateAndDraw(Vector2 mousePosition, float dt)
    {
        dt = Math.Min(dt, 0.05f);

        if (!_hasPreviousPosition)
        {
            _previousMousePosition = mousePosition;
            _hasPreviousPosition = true;
        }

        Vector2 mouseMovement = mousePosition - _previousMousePosition;
        bool isMoving = mouseMovement.LengthSquared() >
                        MovementThreshold * MovementThreshold;

        if (isMoving)
        {
            EmitTrail(_previousMousePosition, mousePosition);
            _fallingSpawnTimer = 0f;
        }
        else
        {
            _fallingSpawnTimer += dt;
            while (_fallingSpawnTimer >= FallingSpawnInterval)
            {
                EmitFallingHeart(mousePosition);
                _fallingSpawnTimer -= FallingSpawnInterval;
            }
        }

        UpdateHearts(dt);
        DrawHearts();
        _previousMousePosition = mousePosition;
    }

    public static void EmitHeartBurst(Vector2 position, int heartCount = 140)
    {
        for (int i = 0; i < heartCount; i++)
        {
            float angle = RandomRange(0f, MathF.Tau);
            float speed = RandomRange(100f, 360f);
            Vector2 direction = new(MathF.Cos(angle), MathF.Sin(angle));
            Vector2 spawnOffset = direction * RandomRange(0f, 24f);

            AddHeart(new Heart(
                position + spawnOffset,
                RandomRange(3f, 10f),
                direction * speed,
                RandomRange(1.2f, 2.8f),
                RandomRange(110f, 210f),
                BurstColors[Random.Next(BurstColors.Length)]));
        }
    }

    private static void EmitTrail(Vector2 from, Vector2 to)
    {
        float distance = Vector2.Distance(from, to);
        int count = Math.Max(1, (int)MathF.Ceiling(distance / TrailSpacing));

        for (int i = 0; i < count; i++)
        {
            float amount = (i + 1f) / count;
            Vector2 position = Vector2.Lerp(from, to, amount);
            position += new Vector2(RandomRange(-2f, 2f), RandomRange(-2f, 2f));

            AddHeart(new Heart(
                position,
                RandomRange(3.5f, 7f),
                new Vector2(RandomRange(-12f, 12f), RandomRange(-20f, -5f)),
                RandomRange(0.45f, 0.8f),
                35f,
                Color.Red));
        }
    }

    private static void EmitFallingHeart(Vector2 mousePosition)
    {
        Vector2 position = mousePosition +
                           new Vector2(RandomRange(-12f, 12f), RandomRange(4f, 10f));

        AddHeart(new Heart(
            position,
            RandomRange(4f, 8f),
            new Vector2(RandomRange(-35f, 35f), RandomRange(15f, 45f)),
            RandomRange(1.5f, 2.5f),
            RandomRange(150f, 230f),
            Color.Red));
    }

    private static void AddHeart(Heart heart)
    {
        if (Hearts.Count >= MaxHearts)
            Hearts.RemoveAt(0);

        Hearts.Add(heart);
    }

    private static void UpdateHearts(float dt)
    {
        for (int i = Hearts.Count - 1; i >= 0; i--)
        {
            Heart heart = Hearts[i];
            heart.Update(dt);

            if (heart.IsDead ||
                heart.Position.Y > R.GetScreenHeight() + heart.Radius * 2f)
            {
                Hearts.RemoveAt(i);
            }
            else
            {
                Hearts[i] = heart;
            }
        }
    }

    private static void DrawHearts()
    {
        foreach (Heart heart in Hearts)
            heart.Draw();
    }

    private static float RandomRange(float min, float max)
    {
        return min + Random.NextSingle() * (max - min);
    }
}
