using System;
using System.Collections.Generic;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Textures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Terrain = FarmVilleClone.Entities.Terrain;

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
        private Entity _entity2;
        private Camera _camera;
        private Light _light;
        private MousePointer _mouse;

        private Terrain _grassTile;

        private bool _mouseClicked;
        private Entity _movableEntity;
        private readonly TerrainLoader _terrainLoader;

        private List<Terrain> _terrainField;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, 0, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {
            _loader = new ModelLoader();
            _masterRenderer = new MasterRenderer();
            _terrainLoader = new TerrainLoader(_masterRenderer);
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
            
            _entity = new Entity(_texturedModel, new Vector3(-10, 0, -20), 0, 180, 0, 1);
            _entity2 = new Entity(_texturedModel, new Vector3(-20, 0, -5), 0, 180, 0, 1);
            _light = new Light(new Vector3(0, 20f, 0f), new Vector3(1, 1, 1));
            // _terrain = new Terrain(0, 0, _loader, new ModelTexture(_loader.LoadTexture("./../../Resources/textures/grass.png")));
            // _terrain2 = new Terrain(1, 0, _loader, new ModelTexture(_loader.LoadTexture("./../../Resources/textures/grass.png")));

            var grassTileModel = ObjLoader.LoadModel("./../../Resources/obj/grass_tile.obj", _loader);
            var grassTexture = new ModelTexture(_loader.LoadTexture("./../../Resources/textures/grassTileTexture.png"));
            var grassTexturedModel = new TexturedModel(grassTileModel, grassTexture);
            _grassTile = new Terrain(grassTexturedModel, new Vector3(0, -1f,  0), 1);

            _terrainField = _terrainLoader.InitializeTerrain(_grassTile);

            _mouseClicked = false;
            
            base.OnLoad(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Update the World
            
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            
            _camera.Move();
            _mouse.Update();

            if (input.IsKeyDown(Key.Z))
            {
                _movableEntity = _mouse.FindClosestEntityByRay(_masterRenderer.GetEntityDictionary());
                if (_movableEntity != null) 
                { 
                    _mouseClicked = true;
                }
            }

            if (input.IsKeyDown(Key.X))
            {
                _movableEntity = null;
                _mouseClicked = false;
            }

            if (_mouseClicked)
            {
                _movableEntity?.SetPosition(_mouse.GetCurrentTerrainPoint());
            }
            
            _masterRenderer.ProcessTerrain(_terrainField);
            _masterRenderer.ProcessEntity(_entity);
            _masterRenderer.ProcessEntity(_entity2);

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Render the World

            Console.WriteLine(_masterRenderer.GetEntityDictionary().Values.Count);
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
            _masterRenderer.CleanUp();
            _loader.CleanUp(); 
            base.Exit();
        }
    }
}
