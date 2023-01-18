namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

public static class ProgramMain
{
    private static void Main(string[] args)
    {
        using var window = new HelloTriangleWindow(1366, 768, "The Games Den Hello Triangle");
        var renderer = new Renderer(window);
        
        renderer.RunMainLoop();
    }
}
