using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace TheGamesDen.GraphicsProgSample.RotatingCube;

/// <summary>
/// Primitive struct for Vertex containing Position and Normal vectors
/// </summary>
[Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Vertex3
{
    /// <summary>
    /// Size in Bytes, for passing in to the stride value for OpenGL
    /// </summary>
    public static readonly int SizeInBytes = Marshal.SizeOf<Vertex3>();
    
    /// <summary>
    /// Position offset - i.e. how many bytes to offset when looking for the Normal vector
    /// </summary>
    public static readonly int PositionOffsetInBytes = 0 + Marshal.SizeOf<Vector3>();
    
    /// <summary>
    /// World position for the vertex.
    /// </summary>
    ///
    /// <remarks>
    /// Notice how we supply the coordinates into the vertex as the "world"
    /// coordinates and not the OpenGL view coordinates. The reason we can do this is because we perform the
    /// transformation into view space in the Vertex Shader.
    /// </remarks>
    public Vector3 Position { get; init; }
    
    /// <summary>
    /// Normal vector for the vertex.
    /// This vector is for determining which "direction" is the surface perpendicular to.
    /// </summary>
    ///
    /// <remarks>
    /// In particular, a normal vector in mathematics mean a vector that is perpendicular to the tangent plane
    /// of a particular surface. One reason we have an arbitrary settable normal vector is to allow
    /// "smooth" shading - in other words, normals can be interpolated to give an illusion of a "smooth" surface
    /// with much less polygons needed.
    /// <br />
    /// See <a href="https://en.wikipedia.org/wiki/Normal_(geometry)">here</a> for more information.
    /// </remarks>
    public Vector3 Normal { get; init; }
}