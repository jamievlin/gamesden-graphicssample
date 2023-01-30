using OpenTK.Mathematics;

namespace TheGamesDen.GraphicsProgSample.RotatingCube;

/// <summary>
/// Stores data for 3D camera and for generation of transform matrix
/// </summary>
public struct Camera
{
    public Vector3 Position { get; set; }
    public Vector3 LookPosition { get; set; }
    public Vector3 UpVector { get; set; }

    public Matrix4 CreateTransform()
    {
        return Matrix4.LookAt(Position, LookPosition, UpVector);
    }
    
}