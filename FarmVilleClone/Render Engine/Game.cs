using System;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using FarmVilleClone.Textures;
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
        private StaticShader shader;
        private ModelTexture texture;
        private TexturedModel texturedModel;
        private RawModel model;

        private Vector3[] buffer;
        private int[] indices;
        private Vector2[] textureCoords;


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
                new Vector3(-0.5f, 0.5f, 0f),  // V0
                new Vector3(-0.5f, -0.5f, 0f), // V1
                new Vector3(0.5f, -0.5f, 0f),  // V2
                new Vector3(0.5f, 0.5f, 0f),   // V3
            };

            indices = new int[]
            {
                0,1,3, // TOP TRIANGLE
                3,1,2  // BOTTOM TRAINGLE
            };

            textureCoords = new Vector2[]
            {
                new Vector2(0, 0), // V0
                new Vector2(0, 1), // V1
                new Vector2(1, 1), // V2
                new Vector2(1, 0), // V3
            };

            model = loader.LoadToVAO(buffer, textureCoords, indices);
            texture = new ModelTexture(loader.LoadTexture("./../../Resources/smiley.png"));
            texturedModel = new TexturedModel(model, texture);

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
            renderer.Render(texturedModel);
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
