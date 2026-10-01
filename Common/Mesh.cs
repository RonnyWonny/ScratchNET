using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Scratch;
using System.Drawing;


namespace ScratchNET.Common;

public class Mesh
{
    public float[] Vertices = [];
    public uint[] Indices = [];

    private readonly int VertexBufferObject;
    private readonly int VertexArrayObject;
    private readonly int ElementBufferObject;

    private readonly Shader GeneralShader;

    public Matrix4 Translate = Matrix4.Identity;

    public Mesh(float[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

        GeneralShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        GeneralShader.Use();

        int vertexLocation = GeneralShader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        int TexCordLocation = GeneralShader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(TexCordLocation);
        GL.VertexAttribPointer(TexCordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
    }

    public void BindVertexArrayObject()
    {
        GL.BindVertexArray(VertexArrayObject);
    }
    private void UseElements()
    {
        GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Draw()
    {
        GeneralShader.Use();
        BindVertexArrayObject();

        GeneralShader.SetMatrix4("projection", Matrix4.CreateOrthographicOffCenter(
            0, WindowProgram.Instance.ClientSize.X,
            WindowProgram.Instance.ClientSize.Y, 0,
            -1.0f, 1.0f)
        );

        GeneralShader.SetMatrix4("transform", Translate);

        UseElements();
    }
}