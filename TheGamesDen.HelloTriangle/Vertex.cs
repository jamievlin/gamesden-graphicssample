using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace TheGamesDen.GraphicsProgSample.HelloTriangle;

/// <summary>
/// Struct for Vertex
/// </summary>
[Serializable, StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    private readonly Vector2 _position;
    private readonly Color4 _color;

    public Vertex(Vector2 position, Color4 color)
    {
        _position = position;
        _color = color;
    }

    public static unsafe int GetSize()
    {
        return sizeof(Vertex);
    }

    public static unsafe int GetColorByteOffset()
    {
        return sizeof(Vector2);
    }
}