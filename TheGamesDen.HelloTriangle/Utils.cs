using System.Reflection;
using System.Resources;

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
    public static string LoadShaderAsText(string name)
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
}