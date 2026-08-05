using LoveApp;
using R = Raylib_cs.Raylib;
using Raylib_cs;


R.InitWindow(800, 600, "Love App");
R.SetTargetFPS(60);

bool enabledDrawFps = true;

Scene scene = new();
scene.AddUI(new UI());
scene.SelectScene(0);

while (!R.WindowShouldClose())
{
    R.BeginDrawing();
    R.ClearBackground(Color.SkyBlue);

    if (R.IsKeyPressed(KeyboardKey.F))
        enabledDrawFps = !enabledDrawFps;
    
    if (enabledDrawFps)
        R.DrawText(R.GetFPS().ToString(), 0, 0, 14, Color.Black);
    
    // R.DrawText(R.GetFrameTime().ToString(), 100, 0, 14, Color.Black);
    MouseTrail.UpdateAndDraw(R.GetMousePosition(), R.GetFrameTime());
    
    scene.Draw();

    R.EndDrawing();
}

R.CloseWindow();
