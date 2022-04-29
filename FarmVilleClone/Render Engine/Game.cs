using System;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Terrains;
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

        private readonly MasterRenderer _masterRenderer;
        private ModelTexture _texture;
        private TexturedModel _texturedModel;
        private RawModel _model;
        private Entity _entity;
        private Camera _camera;
        private Light _light;

        private Terrain _terrain;
        private Terrain _terrain2;

        private MousePointer _mouse;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, 0, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            _loader = new ModelLoader();
            _masterRenderer = new MasterRenderer();
        }

        protected override void OnLoad(EventArgs e)
        {
            _model = ObjLoader.LoadModel("./../../Resources/obj/stall.obj", _loader);
            _texture = new ModelTexture(_loader.LoadTexture("./../../Resources/textures/stallTexture.png"));
            _texturedModel = new TexturedModel(_model, _texture);

            var texture = _texturedModel.GetModelTexture();
            texture.SetShineDamper(10);
            texture.SetReflectivity(.2f);
            
            _camera = new Camera();
            _mouse = new MousePointer(_camera, _masterRenderer.GetProjectionMatrix());
            
            _entity = new Entity(_texturedModel, _mouse.GetCurrentTerrainPoint(), 0, 180, 0, 1);

            _light = new Light(new Vector3(0, 5f, -2.5f), new Vector3(1, 1, 1));

            _terrain = new Terrain(0, 0, _loader, new ModelTexture(_loader.LoadTexture("./../../Resources/textures/grass.png")));
            _terrain2 = new Terrain(1, 0, _loader, new ModelTexture(_loader.LoadTexture("./../../Resources/textures/grass.png")));

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
            _camera.Move();
            _mouse.Update();
            _entity.SetPosition(_mouse.GetCurrentTerrainPoint());

            _masterRenderer.ProcessTerrain(_terrain);
            _masterRenderer.ProcessTerrain(_terrain2);
            _masterRenderer.ProcessEntity(_entity);
            _masterRenderer.Render(_light, _camera);
            
            GL.Flush();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _masterRenderer.CleanUp();
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
