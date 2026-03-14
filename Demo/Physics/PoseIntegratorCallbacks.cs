using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BepuPhysics;
using BepuUtilities;

namespace Demo.Physics
{
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public AngularIntegrationMode AngularIntegrationMode => throw new NotImplementedException();

        public bool AllowSubstepsForUnconstrainedBodies => throw new NotImplementedException();

        public bool IntegrateVelocityForKinematics => throw new NotImplementedException();

        public PoseIntegratorCallbacks(Vector3 gravity)
        {
            
        }

        public void Initialize(Simulation simulation)
        {

        }

        public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
        {

        }

        public void PrepareForIntegration(float dt)
        {

        }
    }
}