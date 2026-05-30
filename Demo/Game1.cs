using System;
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
    private Vector3 _cameraTarget = Vector3.Zero;
    private float _cameraYaw;
    private float _cameraPitch = 0.32f;
    private float _cameraDistance = 15.8f;
    private MouseState _previousMouseState;
    private const float CameraRotationSpeed = 0.01f;
    private const float CameraZoomSpeed = 0.01f;
    private BodyHandle _firstDominoHandle;
    private KeyboardState _previousKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        LoadContent();

        _physicsSimulation = new PhysicsSimulation();

        // Add ground plane
        _physicsSimulation.AddStaticBox(
            new System.Numerics.Vector3(0, -0.5f, 0),
            System.Numerics.Quaternion.Identity,
            width: 50,
            height: 1,
            length: 100);

        // Add a ramp
        var rampRotation = System.Numerics.Quaternion.CreateFromAxisAngle(System.Numerics.Vector3.UnitX, -0.2f);
        _physicsSimulation.AddStaticBox(
            new System.Numerics.Vector3(0, 1f, -10f),
            rampRotation,
            width: 25000f,
            height: 1f,
            length: 1000f);

        // Add dominoes
        const float dominoWidth = 0.2f;
        const float dominoHeight = 1.0f;
        const float dominoLength = 0.55f;
        const float dominoSpacing = 5f;

        for (int i=0; i < 15; i++)
        {
            var position = new System.Numerics.Vector3(
                x: 0,
                y: dominoHeight / 2f + 0.5f,
                z: i * dominoSpacing);
            
            var handle = _physicsSimulation.AddDynamicBox(
                position,
                System.Numerics.Quaternion.Identity,
                dominoWidth,
                dominoHeight,
                dominoLength,
                mass: 1f);

            if (i == 0)
            {
                _firstDominoHandle = handle;
            }

            _entities.Add(new PhysicsEntity(_boxModel, handle));
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _boxModel = Content.Load<Model>("SM_Domino_00");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        UpdateCameraMouseState();
        UpdateCameraMatrices();
        
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
        {
            _physicsSimulation.ApplyImpulse(_firstDominoHandle, new System.Numerics.Vector3(0, 0, 50));
        }
        _previousKeyboardState = keyboardState;

        _physicsSimulation.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    private void UpdateCameraMouseState()
    {
        var mouseState = Mouse.GetState();

        if (mouseState.RightButton == ButtonState.Pressed)
        {
            var deltaX = mouseState.X - _previousMouseState.X;
            var deltaY = mouseState.Y - _previousMouseState.Y;

            _cameraYaw -= deltaX * CameraRotationSpeed;
            _cameraPitch = MathHelper.Clamp(
                _cameraPitch - deltaY * CameraRotationSpeed,
                -1.2f,
                1.2f);
        }

        var scrollDelta = mouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        if (scrollDelta != 0)
        {
            _cameraDistance = MathHelper.Clamp(
                _cameraDistance - scrollDelta * CameraZoomSpeed,
                4f,
                60f);
        }

        _previousMouseState = mouseState;
    }

    private void UpdateCameraMatrices()
    {

        // Add camera
        // _viewMatrix = Matrix.CreateLookAt(
        //     new Vector3(0, 5, 15),
        //     new Vector3(0, 0, 0),
        //     Vector3.Up);
        // _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
        //     MathHelper.ToRadians(45),
        //     _graphics.GraphicsDevice.Viewport.AspectRatio,
        //     0.1f,
        //     1000f);

        var cameraOffset = new Vector3(
            _cameraDistance * (float)Math.Cos(_cameraPitch) * (float)Math.Sin(_cameraYaw),
            _cameraDistance * (float)Math.Sin(_cameraPitch),
            _cameraDistance * (float)Math.Cos(_cameraPitch) * (float)Math.Cos(_cameraYaw));

        _viewMatrix = Matrix.CreateLookAt(
            _cameraTarget + cameraOffset,
            _cameraTarget,
            Vector3.Up);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45),
            GraphicsDevice.Viewport.AspectRatio,
            0.1f,
            1000f);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightGreen);

        GraphicsDevice.BlendState = BlendState.Opaque;
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        foreach (var entity in _entities)
        {
            entity.Draw(_physicsSimulation.Simulation, _viewMatrix, _projectionMatrix);
        }

        base.Draw(gameTime);
    }
}
