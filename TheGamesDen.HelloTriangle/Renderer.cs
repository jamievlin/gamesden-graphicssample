using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection;

namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

public class Renderer
{
    public Renderer(HelloTriangleWindow window)
    {
        _utils = new Utils(Assembly.GetExecutingAssembly());
        
        _window = window;
        _window.SetAsCurrentContext();
        
        InitializeOpenGl();
        InitializeShaders();
        InitializeVertexBuffer();
        InitializeVertexInputLayout();

        LoadData();
    }
    
    /// <summary>
    /// Runs main loop for the program
    /// </summary>
    public void RunMainLoop()
    {
        while (_window.IsActive)
        {
            _window.PollEvents();
            DrawFrame();
            _window.SwapBuffer();
        }
    }
    /// <summary>
    /// Initializes OpenGL. This function is OpenTK-specific
    /// </summary>
    private void InitializeOpenGl()
    {
        // This is OpenTK-specific, but this step initializes OpenGL functions.
        GL.LoadBindings(new GLFWBindingsContext());
    }
    
    /// <summary>
    /// Links all loaded shaders together into a shader program
    /// </summary>
    private void InitializeShaders()
    {
        _mainShaderProgramNumber = GL.CreateProgram();
        
        GL.AttachShader(
            _mainShaderProgramNumber,
            _utils.CreateShaderInOpenGl("MainVertexShader.glsl", ShaderType.VertexShader));
        
        GL.AttachShader(
            _mainShaderProgramNumber,
            _utils.CreateShaderInOpenGl("MainFragmentShader.glsl", ShaderType.FragmentShader));

        GL.LinkProgram(_mainShaderProgramNumber);
    }

    /// <summary>
    /// Initializes OpenGL Vertex Array Object and Buffer
    /// </summary>
    private void InitializeVertexBuffer()
    {
        _vertexArrayNumber = GL.GenVertexArray();
        _vertexBufferNumber = GL.GenBuffer();
    }
    
    /// <summary>
    /// Initializes Vertex Layout and binds to the vertex buffer object
    /// </summary>
    private void InitializeVertexInputLayout()
    {
        GL.BindVertexArray(_vertexArrayNumber);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferNumber);
        
        // Here, we are binding the number 0 and 1 to "vtxInPos" and "vtxInColor" respectively.
        // Note that these numbers are arbitrary, which means we can use any number we want (e.g. index 123)
        // to bind an attribute location
        GL.BindAttribLocation(_mainShaderProgramNumber, 0, "vtxInPos"); 
        GL.BindAttribLocation(_mainShaderProgramNumber, 1, "vtxInColor");

        GL.VertexAttribPointer(
            index: 0,
            size: 2,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: Vertex.GetSize(),
            offset: 0);
        
        GL.VertexAttribPointer(
            1,
            4,
            VertexAttribPointerType.Float,
            false,
            Vertex.GetSize(),
            Vertex.GetColorByteOffset()
        );
        
        
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
    }
    
    /// <summary>
    /// Helper function to load all data
    /// </summary>
    private void LoadData()
    {
        var vertices = new List<Vertex>
        {
            new(new Vector2(-0.5f, -0.5f), Color4.Red),
            new(new Vector2(0.0f, 0.5f), Color4.Green),
            new(new Vector2(0.5f, -0.5f), Color4.Blue)
        };
        
        GL.NamedBufferData(
            buffer: _vertexBufferNumber,
            size: vertices.Count * Vertex.GetSize(),
            data: vertices.ToArray(),
            usage: BufferUsageHint.StaticDraw
        );
    }

    /// <summary>
    /// The "Bread and Butter" of the class.
    /// This function calls all relevant OpenGL Functions to draw frame
    /// </summary>
    private void DrawFrame()
    {
        // sets clear color to black
        GL.ClearColor(Color4.Black);
        
        // Clears the buffer bit function
        GL.Clear(ClearBufferMask.ColorBufferBit);
                
        // draw call
        GL.BindVertexArray(_vertexArrayNumber);

        // Use our shader
        GL.UseProgram(_mainShaderProgramNumber);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    // Window dependency, supplied by the constructor.
    private readonly HelloTriangleWindow _window;
    
    // GL resource numbers 
    private int _vertexArrayNumber = 0;
    private int _vertexBufferNumber = 0;
    private int _mainShaderProgramNumber = 0;

    private readonly Utils _utils;
    
    ~Renderer()
    {
        if (_vertexArrayNumber != 0)
        {
            GL.DeleteVertexArray(_vertexArrayNumber);
        }
    }
}