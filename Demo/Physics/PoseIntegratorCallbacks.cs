using System.Numerics;
using BepuPhysics;
using BepuUtilities;

namespace Demo.Physics
{
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        private Vector3 _gravity;
        private Vector3Wide _gravityWideDt;

        public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        public bool AllowSubstepsForUnconstrainedBodies => false;

        public bool IntegrateVelocityForKinematics => false;

        public PoseIntegratorCallbacks(Vector3 gravity)
        {
            _gravity = gravity;
            _gravityWideDt = default;
        }

        public void Initialize(Simulation simulation)
        {

        }

        public void IntegrateVelocity(
            Vector<int> bodyIndices,
            Vector3Wide position,
            QuaternionWide orientation,
            BodyInertiaWide localInertia,
            Vector<int> integrationMask,
            int workerIndex,
            Vector<float> dt,
            ref BodyVelocityWide velocity)
        {
            velocity.Linear.X += _gravityWideDt.X;
            velocity.Linear.Y += _gravityWideDt.Y;
            velocity.Linear.Z += _gravityWideDt.Z;
        }

        public void PrepareForIntegration(float dt)
        {
            Vector3Wide.Broadcast(_gravity * dt, out _gravityWideDt);
        }
    }
}