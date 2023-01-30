using OpenTK.Mathematics;

namespace TheGamesDen.GraphicsProgSample.RotatingCube;

/// <summary>
/// Utility class for storing geometry information for a unit cube.
/// </summary>
///
/// <remarks>
/// In a more complex renderer, we would normally load models from an external source (for example, FBX or OBJ files
/// or a custom 3D model format). The reason we hardcode in the geometry into a class file is so we do not have to
/// write a specialized procedure for loading models.
/// </remarks>
public static class Cube
{
    private static readonly Vector3 TopFrontLeft = (1, -1, 1);
    private static readonly Vector3 TopBackLeft = (1, 1, 1);
    private static readonly Vector3 TopFrontRight = (-1, -1, 1);
    private static readonly Vector3 TopBackRight = (-1, 1, 1);
    private static readonly Vector3 BottomFrontLeft = (1, -1, -1);
    private static readonly Vector3 BottomBackLeft = (1, 1, -1);
    private static readonly Vector3 BottomFrontRight = (-1, -1, -1);
    private static readonly Vector3 BottomBackRight = (-1, 1, -1);

    public static readonly List<Vertex3> CubeModelVertices = new()
    {
        // top face
        new() { Position = TopFrontLeft, Normal = Vector3.UnitZ},
        new() { Position = TopBackLeft, Normal = Vector3.UnitZ},
        new() { Position = TopFrontRight, Normal = Vector3.UnitZ},
        new() { Position = TopBackRight, Normal = Vector3.UnitZ},
        
        // bottom face
        new() { Position = BottomFrontLeft, Normal = -Vector3.UnitZ},
        new() { Position = BottomBackLeft, Normal = -Vector3.UnitZ},
        new() { Position = BottomFrontRight, Normal = -Vector3.UnitZ},
        new() { Position = BottomBackRight, Normal = -Vector3.UnitZ},
        
        // front face
        new() { Position = TopFrontLeft, Normal = -Vector3.UnitY},
        new() { Position = TopFrontRight, Normal = -Vector3.UnitY},
        new() { Position = BottomFrontLeft, Normal = -Vector3.UnitY},
        new() { Position = BottomFrontRight, Normal = -Vector3.UnitY},

        // front face
        new() { Position = TopBackLeft, Normal = Vector3.UnitY},
        new() { Position = TopBackRight, Normal = Vector3.UnitY},
        new() { Position = BottomBackLeft, Normal = Vector3.UnitY},
        new() { Position = BottomBackRight, Normal = Vector3.UnitY},
        
        // left face
        new() { Position = TopFrontLeft, Normal = Vector3.UnitX},
        new() { Position = BottomFrontLeft, Normal = Vector3.UnitX},
        new() { Position = TopBackLeft, Normal = Vector3.UnitX},
        new() { Position = BottomBackLeft, Normal = Vector3.UnitX},

        // Right face
        new() { Position = TopFrontRight, Normal = -Vector3.UnitX},
        new() { Position = BottomFrontRight, Normal = -Vector3.UnitX},
        new() { Position = TopBackRight, Normal = -Vector3.UnitX},
        new() { Position = BottomBackRight, Normal = -Vector3.UnitX},
    };

    public static readonly List<uint> CubeIndices = new()
    {
        0, 1, 2,
        1, 2, 3,
        
        4, 5, 6,
        5, 6, 7,
        
        8, 9, 10,
        9, 10, 11,
        
        12, 13, 14,
        13, 14, 15,
        
        16, 17, 18,
        17, 18, 19,
        
        20, 21, 22,
        21, 22, 23,
    };
}