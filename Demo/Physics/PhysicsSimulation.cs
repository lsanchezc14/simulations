using System;
using System.Collections.Concurrent;
using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities;
using BepuUtilities.Memory;

namespace Demo.Physics
{
    public class PhysicsSimulation : IDisposable
    {
        public Simulation Simulation { get; private set; }
        public NarrowPhaseCallbacks Callbacks;
        private readonly BufferPool _bufferPool;
        private readonly ThreadDispatcher _threadDispatcher;

        public PhysicsSimulation()
        {
            _bufferPool = new BufferPool();
            var targetThreadCount = Math.Max(1, Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1);

            _threadDispatcher = new ThreadDispatcher(targetThreadCount);
            Callbacks = new NarrowPhaseCallbacks();
            Simulation = Simulation.Create(
                _bufferPool,
                Callbacks,
                new PoseIntegratorCallbacks(gravity: new Vector3(0, -9.81f, 0)),
                new SolveDescription(8, 1));
        }

        public ConcurrentQueue<(CollidableReference, CollidableReference)> GetAndClearCollisionEvents => Callbacks.CollisionEvents;    

        public void Update(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            Simulation.Timestep(deltaTime, _threadDispatcher);
        }

        public BodyHandle AddDynamicBox(
            Vector3 position,
            Quaternion orientation,
            float width,
            float height,
            float length,
            float mass)
        {
            var pose = new RigidPose(position, orientation);
            var shape = new Box(width, height, length);
            var inertia = shape.ComputeInertia(mass);
            var description = BodyDescription.CreateDynamic(
                pose,
                inertia,
                Simulation.Shapes.Add(shape),
                new BodyActivityDescription(0.01f));

            return Simulation.Bodies.Add(description);
        }

        public StaticHandle AddStaticBox(
            Vector3 position,
            Quaternion orientation,
            float width,
            float height,
            float length)
        {
            var shape = new Box(width, height, length);
            var description = new StaticDescription(
                position,
                orientation,
                Simulation.Shapes.Add(shape));

            return Simulation.Statics.Add(description);
        }

        public void ApplyImpulse(BodyHandle bodyHandle, Vector3 impulse)
        {
            var bodyReference = Simulation.Bodies.GetBodyReference(bodyHandle);
            bodyReference.Awake = true;
            bodyReference.ApplyImpulse(impulse, new Vector3(0, -1, 0));
        }

        public void Dispose()
        {
            Simulation.Dispose();
            _threadDispatcher.Dispose();
            _bufferPool.Clear();
        }
    }
}