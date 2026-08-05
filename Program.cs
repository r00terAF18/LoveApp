using LoveApp;
using R = Raylib_cs.Raylib;
using Raylib_cs;


R.InitWindow(1280, 720, "Love App");
R.SetTargetFPS(60);

Color backgroundTop = new(255, 218, 233, 255);
Color backgroundBottom = new(181, 224, 255, 255);

Scene scene = new();
scene.AddUI(new DateQuestionUI());
scene.AddUI(new LoveLetterUI());
scene.AddUI(new ComplimentJarUI());
scene.AddUI(new LoveMeterUI());
scene.SelectScene(0);

while (!R.WindowShouldClose())
{
    R.BeginDrawing();
    R.ClearBackground(backgroundTop);
    R.DrawRectangleGradientV(
        0,
        0,
        R.GetScreenWidth(),
        R.GetScreenHeight(),
        backgroundTop,
        backgroundBottom);

    MouseTrail.UpdateAndDraw(R.GetMousePosition(), R.GetFrameTime());

    scene.Draw();

    R.EndDrawing();
}

R.CloseWindow();
