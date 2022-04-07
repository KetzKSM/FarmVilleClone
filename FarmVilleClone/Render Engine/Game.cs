using System;
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
        private RawModel model;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            this.loader = new ModelLoader();
            this.renderer = new Renderer();
        }

        protected override void OnLoad(EventArgs e)
        {
            renderer.Prepare();

            buffer = new Vector3[6]
            {
            // First Triangle
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3(0.5f, -0.5f, 0f),
            // Second Triangle
            new Vector3(0.5f, -0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f),
            new Vector3(-0.5f, 0.5f, 0f),
            };

            model = loader.LoadToVAO(buffer);

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
            renderer.Render(model);

            GL.Flush();
            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
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
