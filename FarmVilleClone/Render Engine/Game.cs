using System;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using FarmVilleClone.Textures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace FarmVilleClone.Render_Engine
{
    public class Game : GameWindow
    {
        private readonly ModelLoader _loader;
        private readonly Renderer _renderer;
        private readonly StaticShader _shader;
        private ModelTexture _texture;
        private TexturedModel _texturedModel;
        private RawModel _model;
        private Entity _entity;
        private Camera _camera;
        private Light light;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, 0, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            _loader = new ModelLoader();
            _shader = new StaticShader();
            _renderer = new Renderer(_shader);
        }

        protected override void OnLoad(EventArgs e)
        {
            _renderer.Prepare();
            _model = ObjLoader.LoadModel("./../../Resources/obj/stall.obj", _loader);
            _texture = new ModelTexture(_loader.LoadTexture("./../../Resources/textures/stallTexture.png"));
            _texturedModel = new TexturedModel(_model, _texture);

            var texture = _texturedModel.GetModelTexture();
            texture.SetShineDamper(10);
            texture.SetReflectivity(1);
            
            _entity = new Entity(_texturedModel, new Vector3(0, -2.5f, -20), 0, 0, 0, 1);

            light = new Light(new Vector3(0, -2.5f, -2.5f), new Vector3(1, 1, 1));
            
            _camera = new Camera();

            _entity.Rotate(0, 180, 0);
            
            base.OnLoad(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // _entity.Rotate(0, 1, 0);
            // _entity.Translate(0,0,-.2f);
            _camera.Move();
            _renderer.Prepare();
            _shader.Start();
            _shader.LoadLight(light);
            _shader.LoadViewMatrix(_camera);
            _renderer.Render(_entity, _shader);
            _shader.Stop();

            GL.Flush();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _shader.CleanUp();
            _loader.CleanUp();
            base.OnClosed(e);
        }

        public override void Exit()
        {
            _loader.CleanUp();
            base.Exit();
        }
    }
}
