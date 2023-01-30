using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using TheGamesDen.GraphicsProgSample.HelloTriangle;

namespace TheGamesDen.GraphicsProgSample.RotatingCube;


/// <summary>
/// Our main class for the GameWindow.
/// </summary>
///
/// <remarks>
/// One thing you might notice here is that unlike in the Hello Triangle Project, we do not have a main loop structure.
/// The reason is that we are using OpenTK's <see cref="GameWindow"/> class which has an underlying main loop
/// implementation (similar to the one that we talked about or the one in the HelloTriangle).
///
/// Nevertheless, we can still implement our own main loop if we want to.
/// </remarks>
public sealed class Renderer : GameWindow
{
    private const float FovYDegrees = 60.0f;
    
    private static NativeWindowSettings ConfigureNativeWindowSettings(Vector2i resolution, string title) =>
        new()
        {
            API = ContextAPI.OpenGL,
            APIVersion = new Version(4, 5),
            WindowState = WindowState.Normal,
            Profile = ContextProfile.Core,
            Size = resolution,
            WindowBorder = WindowBorder.Fixed,
            Title = title
        };

    public Renderer(Vector2i resolution, string title) :
        base(GameWindowSettings.Default, ConfigureNativeWindowSettings(resolution, title))
    {
        _utils = new Utils(Assembly.GetExecutingAssembly());
        _perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(FovYDegrees),
            ((float) resolution.X) / resolution.Y,
            0.3f,
            1000.0f
        );
    }
    
    /// <summary>
    /// Loads Shaders and creates the main Shader program
    /// </summary>
    private void CreateShaders()
    {
        _mainShaderProgramNumber = GL.CreateProgram();
        
        GL.AttachShader(
            _mainShaderProgramNumber,
            _utils.CreateShaderInOpenGl("RCMainVertexShader.glsl", ShaderType.VertexShader));
        
        GL.AttachShader(
            _mainShaderProgramNumber,
            _utils.CreateShaderInOpenGl("RCMainFragmentShader.glsl", ShaderType.FragmentShader));

        GL.LinkProgram(_mainShaderProgramNumber);
    }
    
    /// <summary>
    /// Initialize buffers, like in the Hello Triangle project.
    /// </summary>
    private void InitBuffers()
    {
        _cubeVAONumber = GL.GenVertexArray();

        var returnBufferNumbers = new int[2];
        GL.CreateBuffers(2, returnBufferNumbers);
        
        _cubeVertBufferNumber = returnBufferNumbers[0];
        _cubeIndexBufferNumber = returnBufferNumbers[1];
    }
    
    /// <summary>
    /// Loads data into the GPU buffer
    /// </summary>
    private void LoadData()
    {
        GL.NamedBufferData(
            _cubeVertBufferNumber,
            Cube.CubeModelVertices.Count * Vertex3.SizeInBytes,
            Cube.CubeModelVertices.ToArray(),
            BufferUsageHint.StaticDraw
        );

        GL.NamedBufferData(
            _cubeIndexBufferNumber,
            Cube.CubeIndices.Count * Marshal.SizeOf<uint>(),
            Cube.CubeIndices.ToArray(),
            BufferUsageHint.StaticDraw
        );
    }
    
    /// <summary>
    /// Loads uniform's shader location so the CPU can upload data to that uniform slot.
    /// </summary>
    private void LoadUniformBindings()
    {
        _projViewMatrixUnifNumber = GL.GetUniformLocation(_mainShaderProgramNumber, "projViewModelMatrix");
    }
    
    /// <summary>
    /// Initializes Vertex array attribute.
    /// Notice here, we are using an explicit layout(position=X) attribute in the GLSL shader
    /// </summary>
    private void InitializeVertexArrayAttrib()
    {
        var shaderVertexAttributes = new Dictionary<string, int>()
        {
            {"vtxInPos", 0},
            {"vtxInNormal", 1} // Specified in the shader
        };
        // 0 maps to vtxInPos
        // 1 maps to vtxInNormal
        
        GL.BindVertexArray(_cubeVAONumber);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _cubeVertBufferNumber);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _cubeIndexBufferNumber);

        GL.VertexAttribPointer(
            index: shaderVertexAttributes["vtxInPos"],
            size: 3,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: Vertex3.SizeInBytes,
            offset: 0);
        
        GL.VertexAttribPointer(
            shaderVertexAttributes["vtxInNormal"],
            3,
            VertexAttribPointerType.Float,
            false,
            Vertex3.SizeInBytes,
            Vertex3.PositionOffsetInBytes
        );
        
        GL.EnableVertexAttribArray(shaderVertexAttributes["vtxInPos"]);
        GL.EnableVertexAttribArray(shaderVertexAttributes["vtxInNormal"]);
    }
    
    /// <summary>
    /// This method is optional and allows us to process debug messages (including errors)
    /// from OpenGL.
    ///
    /// Here, we are setting the debug message callback so that every time OpenGL reports an error,
    /// it calls our function, which prints to the standard error and if <see cref="throwOnError"/> is set to true,
    /// also raises an exception.
    /// </summary>
    /// <param name="throwOnError">Whether to throw an error on OpenGL call errors</param>
    /// <exception cref="ApplicationException">
    /// Thrown with OpenGL failure messagee if <see cref="throwOnError"/>
    /// is true.
    /// </exception>
    private static void InitDebugMessageCallback(bool throwOnError = false)
    {
        GL.Enable(EnableCap.DebugOutput);

        if (throwOnError)
        {
            GL.Enable(EnableCap.DebugOutputSynchronous);
        }
        
        GL.DebugMessageCallback((src, type, id, sev, len, message, userParam) =>
        {
            Console.Error.WriteLine("---- GLCallback ----");

            if (type == DebugType.DebugTypeError)
            {
                Console.Error.WriteLine("*** GL Error ***");
            }

            var msgString = Marshal.PtrToStringAnsi(message, len);
            Console.Error.WriteLine($"SEV={sev.ToString()}, Message:");
            Console.Error.WriteLine(msgString);
            
            Console.WriteLine("---- End GLCallabck ----");

            if (throwOnError && type == DebugType.DebugTypeError)
            {
                throw new ApplicationException($"OpenGL Error: Message: {msgString}");
            }

        }, nint.Zero);
    }
    
    /// <summary>
    /// This method gets called before the main loop starts
    /// </summary>
    protected override void OnLoad()
    {
#if DEBUG
        InitDebugMessageCallback(true);
#endif
        CreateShaders();
        LoadUniformBindings();
        InitBuffers();
        LoadData();
        InitializeVertexArrayAttrib();
        
        _camera = new()
        {
            Position = (5, 5, 5),
            UpVector = Vector3.UnitZ,
            LookPosition = (0, 0, 0)
        };
        
        // This call is important since when we are drawing Triangles - if we only have the "output" target,
        // OpenGL has no idea if what we are drawing is "behind" what's already drawn or not.
        
        // To work around this problem, we maintain another render target - the "depth buffer" which records
        // the depth of objects drawn. By comparing the depth value in the buffer and the depth of what we are
        // drawing, we can determine if a triangle is "behind" other previously drawn triangles or not, and if so,
        // skips the drawing on that pixel. This is how we maintain a "reasonable" picture when drawing objects.
        GL.Enable(EnableCap.DepthTest);
        
        GL.ClearColor(Color.Black);
    }
    
    /// <summary>
    /// Called every frame before the frame draw.
    /// </summary>
    /// <param name="e">Event arguments - also contains the delta time value in seconds.</param>
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        // Here, delta means how much time has elapsed since the last time this method was called.
        // We can use the delta time (e.Time value) to determine how much to change our state - in this case,
        // the rotation angle.
        _rotationAngle = (_rotationAngle + ((float) e.Time * 0.5f)) % MathHelper.TwoPi;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
   
        GL.UseProgram(_mainShaderProgramNumber);

        GL.BindVertexArray(_cubeVAONumber);

        var modelMatrix = Matrix4.CreateRotationZ(_rotationAngle);
        var uploadMatrix =  modelMatrix * _camera.CreateTransform() * _perspectiveMatrix;
        GL.UniformMatrix4(_projViewMatrixUnifNumber, false, ref uploadMatrix);
        GL.UniformMatrix4(_projViewMatrixUnifNumber, false, ref uploadMatrix);
        

        GL.DrawElements(PrimitiveType.Triangles, Cube.CubeIndices.Count, DrawElementsType.UnsignedInt, 0);

        Context.SwapBuffers();
    }
    // Utils for loading assembly
    private readonly Utils _utils;
    
    // fields
    private int _mainShaderProgramNumber;
    private int _cubeVAONumber;
    private int _cubeVertBufferNumber;
    private int _cubeIndexBufferNumber;
    
    // Transformations
    private Camera _camera;
    private readonly Matrix4 _perspectiveMatrix;
    private float _rotationAngle = 0;
    
    // Uniforms
    private int _projViewMatrixUnifNumber;
}