
using OpenTK.Mathematics;

namespace TheGamesDen.GraphicsProgSample.RotatingCube;

public static class ProgramMain
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var renderer = new Renderer(new Vector2i(1366, 768), "The Games Den Rotating Cube");
        renderer.Run();
    }
}