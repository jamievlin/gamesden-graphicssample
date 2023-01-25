using System.Reflection;
using System.Resources;
using OpenTK.Graphics.OpenGL4;

namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

/// <summary>
/// These are utilities functions
/// </summary>
public static class Utils
{
    private static readonly Assembly Asm = Assembly.GetExecutingAssembly();
    
    /// <summary>
    /// Helper function to load shader. Throws <see cref="MissingManifestResourceException"/> if resource cannot be
    /// found.
    /// </summary>
    /// <param name="name">Name of the shader to pass to the Resource Reader</param>
    /// <returns>Shader data as text</returns>
    private static string LoadShaderAsText(string name)
    {
        // We use EmbeddedResource for .NET Core to store and load shaders.
        using var vertexInputStream = Asm.GetManifestResourceStream($"{Asm.GetName().Name}.Shaders.{name}");

        if (vertexInputStream == null)
        {
            throw new MissingManifestResourceException($"Cannot find shader {name}!");
        }
        
        using var vtxReader = new StreamReader(vertexInputStream);
        return vtxReader.ReadToEnd();
    }

    /// <summary>
    /// Helper function to create a shader in OpenGL.
    /// Requires OpenGL context to be initialized
    /// </summary>
    /// <param name="name">File name of the Shader to load from Resource Assembly</param>
    /// <param name="shaderType">Type of shader to create</param>
    /// <returns>GL Shader Number</returns>
    /// <exception cref="ApplicationException">Throws if shader compilation fails</exception>
    public static int CreateShaderInOpenGl(string name, ShaderType shaderType)
    {
        var text = LoadShaderAsText(name);

        var shaderNumber = GL.CreateShader(shaderType);
        GL.ShaderSource(shaderNumber, text);
        GL.CompileShader(shaderNumber);
        
        // Optional step, but helpful in checking that shader compilation happened successfully
        GL.GetShader(shaderNumber, ShaderParameter.CompileStatus, out var shaderCompileStatus);
        if (shaderCompileStatus == 0)
        {
            GL.GetShaderInfoLog(shaderNumber, out var shaderErrorText);

            if (shaderErrorText != null)
            {
                Console.Error.WriteLine($"Cannot compile shader {name}!");
                Console.Error.WriteLine("Shader compile error message:");
                Console.Error.Write(shaderErrorText);
                Console.Error.WriteLine("---------------------------");
            }

            throw new ApplicationException($"Cannot compile shader {name}");
        }

        return shaderNumber;
    }
}