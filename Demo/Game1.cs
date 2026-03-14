using System.Collections.Generic;
using System.Numerics;
using Demo.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;
using BepuPhysics;

namespace Demo;

public class Game1 : Game
{
    private List<PhysicsEntity> _entities = new List<PhysicsEntity>();
    private Model _boxModel;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    private PhysicsSimulation _physicsSimulation;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _physicsSimulation = new PhysicsSimulation();
        _physicsSimulation.AddStaticBox(
            new System.Numerics.Vector3(0, -1, 0),
            System.Numerics.Quaternion.Identity,
            width: 50,
            height: 1,
            length: 50);

        // Add a ramp
        var rampRotation = System.Numerics.Quaternion.CreateFromAxisAngle(System.Numerics.Vector3.UnitX, -0.2f);
        _physicsSimulation.AddStaticBox(
            new System.Numerics.Vector3(0, 1f, -10f),
            rampRotation,
            width: 10,
            height: 1,
            length: 20);

        // Add dominoes
        const float dominoWidth = 0.2f;
        const float dominoHeight = 1.0f;
        const float dominoLength = 0.6f;
        const float dominoSpacing = 0.55f;

        _boxModel = Content.Load<Model>("Box");

        for (int i=0; i < 15; i++)
        {
            var position = new System.Numerics.Vector3(
                x: 0,
                y: dominoHeight / 2f + 0.5f,
                z: i * dominoSpacing);
            
            var handle = _physicsSimulation.AddDynamicBox(
                position,
                dominoWidth,
                dominoHeight,
                dominoLength,
                mass: 1f);

            _entities.Add(new PhysicsEntity(_boxModel, handle));
        }

        // Add camera
        _viewMatrix = Matrix.CreateLookAt(
            new Vector3(0, 5, 15),
            new Vector3(0, 0, 0),
            Vector3.Up);
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45),
            _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _boxModel = Content.Load<Model>("Box");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        GraphicsDevice.BlendState = BlendState.Opaque;
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        foreach (var entity in _entities)
        {
            entity.Draw(_physicsSimulation.Simulation, _viewMatrix, _projectionMatrix);
        }

        base.Draw(gameTime);
    }
}
