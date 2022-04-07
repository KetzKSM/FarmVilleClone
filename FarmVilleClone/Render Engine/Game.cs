using System;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace FarmVilleClone.RenderEngine
{
    public class Game : GameWindow
    {
        private ModelLoader loader;
        private Renderer renderer;
        private Vector3[] buffer;
        private StaticShader shader;

        private int[] indices;
        private RawModel model;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, 0, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            this.loader = new ModelLoader();
            this.renderer = new Renderer();
            this.shader = new StaticShader();
        }

        protected override void OnLoad(EventArgs e)
        {
            renderer.Prepare();

            buffer = new Vector3[]
            {
                //Must declare CCW
                new Vector3(-0.5f, 0.5f, 0f),
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
            };

            indices = new int[]
            {
                0,1,3,3,1,2
            };

            model = loader.LoadToVAO(buffer, indices);

            base.OnLoad(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            renderer.Prepare();
            shader.Start();
            renderer.Render(model);
            shader.Stop();

            GL.Flush();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            shader.CleanUp();
            loader.CleanUp();
            base.OnClosed(e);
        }

        public override void Exit()
        {
            loader.CleanUp();
            base.Exit();
        }
    }
}
