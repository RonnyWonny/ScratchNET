using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ScratchNET.Common;

public class Shader : IDisposable
{
    private bool DisposedValue = false;

    private int Handle;

    public Shader(string VertexPath, string FragPath)
    {
        string VertexSource = File.ReadAllText(VertexPath);
        string FragSource = File.ReadAllText(FragPath);

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexSource);
        CompileShader(VertexShader);

        int FragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragShader, FragSource);
        CompileShader(FragShader);

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int HandleSuccess);

        if (HandleSuccess == 0)
        {
            string log = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(log);
        }

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragShader);
        GL.DeleteShader(FragShader);
        GL.DeleteShader(VertexShader);
    }

    ~Shader()
    {
        if (DisposedValue == false)
        {
            Console.WriteLine("GPU resource leak..");
        }
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int FragSuccess);

        if (FragSuccess == 0)
        {
            string log = GL.GetShaderInfoLog(shader);
            Console.WriteLine(log);
        }
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public void SetVector3(string attribName, Vector3 vector3)
    {
        GL.UseProgram(Handle);
        int location = GL.GetUniformLocation(Handle, attribName);
        GL.Uniform3(location, ref vector3);
    }

    public void SetVector4(string attribName, Vector4 vector4)
    {
        GL.UseProgram(Handle);
        int location = GL.GetUniformLocation(Handle, attribName);
        GL.Uniform4(location, ref vector4);
    }

    public void SetMatrix4(string attribName, Matrix4 matrix)
    {
        GL.UseProgram(Handle);
        int location = GL.GetUniformLocation(Handle, attribName);
        GL.UniformMatrix4(location, true, ref matrix);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GL.DeleteProgram(Handle);

            DisposedValue = true;
        }
    }

}