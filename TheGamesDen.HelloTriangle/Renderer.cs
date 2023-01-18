using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

public class Renderer
{
    public Renderer(HelloTriangleWindow window)
    {
        _window = window;
        _window.SetAsCurrentContext();
        
        InitializeOpenGl();
    }
    
    public void RunMainLoop()
    {
        while (_window.IsActive)
        {
            _window.PollEvents();
            DrawFrame();
            _window.SwapBuffer();
        }
    }

    private void InitializeOpenGl()
    {
        // This is OpenTK-specific, but this step initializes OpenGL functions.
        GL.LoadBindings(new GLFWBindingsContext());
    }

    /// <summary>
    /// The "Bread and Buffer" of the class.
    /// </summary>
    private void DrawFrame()
    {
        GL.ClearColor(Color4.Black);
    }

    private HelloTriangleWindow _window;
}