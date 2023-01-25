using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics.CodeAnalysis;

namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

/// <summary>
/// Window class for HelloTriangle. While we can use <see cref="OpenTK.Windowing.Desktop.GameWindow" />
/// class provided directly by OpenTK,
/// we will use the native GLFW bindings to show how renderers work under the hood.
/// </summary>
public unsafe class HelloTriangleWindow : IDisposable
{
    public HelloTriangleWindow(int width, int height, string title)
    {
        // In this constructor, we perform multiple steps,
        // mainly,
        
        // 1. Initialize GLFW and check if initialization is successful
        // 2. Tell GLFW that window should not be resiazable.
        //    * As a side note, GLFW can also be used with other graphics API like DirectX 11/12 or Vulkan,
        //      though, GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi) needs to be passed on.
        //    * By Default, GLFW assumes the Client API is OpenGL, though we can also pass in the API explicitly by
        //      calling GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
        // 3. Create GLFW window, and check if creation is successful. If not, we terminate the program
        // 4. Tell GLFW to set our window to the current OpenGL Context
        
        // Initialize GLFW
        if (!GLFW.Init())
        {
            throw new ApplicationException("Cannot initialize GLFW Library!");
        }
        
        // These tell GLFW to initialize OpenGL Context to version 4.5
        GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
        GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
        
        // Tell GLFW to use Core profile, which is more restrictive but also allows
        // RenderDoc debugging.
        // In addition, Core does not include deprecated functions (e.g. glBegin/glEnd)
        GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

        GLFW.WindowHint(WindowHintBool.Resizable, false);
        
        // Create GLFW Window
        _window = GLFW.CreateWindow(width, height, title, null, null);

        if (_window == null)
        {
            GLFW.Terminate();
            throw new ApplicationException("Cannot create GLFW Window!");
        }
    }
    
    /// <summary>
    /// Checks if window is still active
    /// </summary>
    public bool IsActive => !GLFW.WindowShouldClose(_window);
    
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public void PollEvents() => GLFW.PollEvents();
    
    /// <summary>
    /// Set context to current for OpenGL
    /// </summary>
    ///
    /// <remarks>
    /// OpenGL uses a "state machine" pattern (in other words, GLFW keeps a global "context" reference)
    /// for all subsequent gl* function calls. We need to set our context (i.e. this._window) as the current one
    /// </remarks>
    public void SetAsCurrentContext() => GLFW.MakeContextCurrent(_window);
    
    /// <summary>
    /// Swap the buffer.
    /// </summary>
    ///
    /// <remarks>
    /// While it may look like when we are calling glDraw* functions directly to the screen, we are (often)
    /// drawing to a hidden frame buffer. The <see cref="GLFW.SwapBuffers" /> function takes that hidden
    /// buffer and shows it to the screen. Usually there are 1-2 hidden buffer, in addition to the buffer being
    /// currently presented.
    ///
    /// Note that this is a vast oversimplification of how swap chains work. In DirectX 11/12 or Vulkan, swap chains
    /// must be explicitly created (In DirectX, this is through IDXGISwapChain*) in which we can specify the number of
    /// buffers.
    /// </remarks>
    public void SwapBuffer() => GLFW.SwapBuffers(_window);

    // Dispose interface implementation
    // see https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    // for more information
    public void Dispose()
    {
        DisposeInternal();
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Internal function to dispose of all resources not managed by .NET garbage collector,
    /// in this case, GLFW's window handle
    /// </summary>
    private void DisposeInternal()
    {
        if (_disposed)
        {
            return;
        }

        if (_window != null)
        {
            GLFW.DestroyWindow(_window);
        }

        _window = null;
        
        GLFW.Terminate();

        _disposed = true;
    }
    
    // Finalizer code, only called if from the garbage collector.
    // This code is only called if the Dispose() function is not called, as we suppressed finalizer call
    // in the garbage collector
    ~HelloTriangleWindow()
    {
        DisposeInternal();
    }
    
    
    private bool _disposed = false;
    
    // raw pointer to our GLFW window.
    // Pointers in C# require "unsafe" mode. Normally, you would not need to do this as
    // many libraries, including OpenTK provides a windowing class.
    private Window* _window;
}

