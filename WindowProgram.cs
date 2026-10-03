using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Scratch;


namespace ScratchNET;

public class WindowProgram : GameWindow {
    public static WindowProgram? Instance;

    public ScratchNet scratch;

    public List<Sprite> sprites;

    public WindowProgram() : base
        (GameWindowSettings.Default,
        new NativeWindowSettings()
        {
            Title = "ScratchNET | scratch .NET 10",
            WindowBorder = WindowBorder.Resizable,
            StartVisible = false,
            StartFocused = true,
            WindowState = WindowState.Normal,
            API = ContextAPI.OpenGL,
            Profile = ContextProfile.Core
        }
        )
    {
        Instance = this;

        scratch = new ScratchNet();
        ClientSize = new Vector2i(scratch.Settings.Width, scratch.Settings.height);
        CenterWindow();

        ErrorType status = scratch.Open("./test.sb3");

        if (status != ErrorType.OK)
            Console.WriteLine($"ERROR TYPE: {status}");

        scratch.Start();

    }

    protected override void OnLoad()
    {
        IsVisible = true;
        base.OnLoad();


        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.ClearColor(Color4.Black);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        scratch.Draw();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        scratch.WindowResize(e.Width, e.Height);
    }
}