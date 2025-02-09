using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyNewMine;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private CubeRenderer _cube; 
    private Matrix _world = Matrix.CreateTranslation(0,0,0);
    private Matrix _view = Matrix.CreateLookAt(new Vector3(0,10,10),Vector3.Zero, Vector3.Up);
    private Matrix _projection;
    private Player _player;

    private List<Chunks> _blocks = new List<Chunks>();

    //ToDo: Bugfix: , texturas de bloques, personaje.
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4, 
            GraphicsDevice.Viewport.AspectRatio, 0.1f, 500f);
        _player = new Player(new Vector3(3,6,-10), 
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height);
        _cube = new CubeRenderer(GraphicsDevice);
        base.Initialize();
    }

    protected override void LoadContent(){
        try{
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            }
        catch(Exception ex){
                Debug.WriteLine($"Error in LoadContent: {ex.Message}");    
        }
    }
    
    private void DrawBlocks(){
        
        foreach (var block in _blocks)
        {
            foreach (var mesh in block.blockModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(block.Position);
                    effect.View = _view;
                    effect.Projection = _projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        try {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            _player.Update(gameTime);
            base.Update(gameTime);
        }
        catch (Exception ex) {
                Debug.WriteLine($"error in update: {ex.Message}");
                Exit();
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        try{
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _view = _player.GetViewMatrix();
            _cube.Draw(_view, _projection);
            DrawBlocks();
            base.Draw(gameTime);
        }
        catch(Exception ex) {
            Debug.WriteLine($"Error in Draw: {ex.Message}");
            Exit();
        }
    }
}