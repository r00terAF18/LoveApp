using System.Numerics;
using Raylib_cs;
using R = Raylib_cs.Raylib;

namespace LoveApp;

public struct Heart
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Radius;
    public float Life;
    public float MaxLife;
    public float Gravity;
    public Color Color;

    public readonly bool IsDead => Life <= 0f || Radius <= 0.25f;

    public Heart(
        Vector2 position,
        float radius,
        Vector2 velocity,
        float life,
        float gravity,
        Color color)
    {
        Position = position;
        Radius = radius;
        Velocity = velocity;
        Life = life;
        MaxLife = life;
        Gravity = gravity;
        Color = color;
    }

    public void Update(float dt)
    {
        Velocity.Y += Gravity * dt;
        Position += Velocity * dt;
        Life -= dt;
    }

    public readonly void Draw()
    {
        float alpha = Math.Clamp(Life / MaxLife, 0f, 1f);
        Color fadedColor = R.Fade(Color, alpha);

        // Two round lobes and a pointed lower half form a simple scalable heart.
        float lobeRadius = Radius * 0.58f;
        float lobeY = Position.Y - Radius * 0.25f;
        R.DrawCircleV(
            new Vector2(Position.X - Radius * 0.46f, lobeY),
            lobeRadius,
            fadedColor);
        R.DrawCircleV(
            new Vector2(Position.X + Radius * 0.46f, lobeY),
            lobeRadius,
            fadedColor);
        R.DrawTriangle(
            new Vector2(Position.X - Radius, Position.Y),
            new Vector2(Position.X, Position.Y + Radius * 1.35f),
            new Vector2(Position.X + Radius, Position.Y),
            fadedColor);
    }
}
