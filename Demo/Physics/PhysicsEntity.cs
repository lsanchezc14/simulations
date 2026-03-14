using BepuPhysics;
using BepuUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Demo.Physics
{
    public class PhysicsEntity
    {
        public Model Model {get;}
        public BodyHandle BodyHandle {get;}

        private readonly Matrix[] _boneTransforms;

        public PhysicsEntity(Model model, BodyHandle bodyHandle)
        {
            Model = model;
            BodyHandle = bodyHandle;

            _boneTransforms = new Matrix[model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
        }

        public void Draw(Simulation simulation, Matrix view, Matrix projection)
        {
            var bodyDescription = simulation.Bodies.GetDescription(BodyHandle);
            var physicsPose = bodyDescription.Pose;

            var renderPosition = new Vector3(physicsPose.Position.X, physicsPose.Position.Y, physicsPose.Position.Z);
            var renderOrientation = new Quaternion(physicsPose.Orientation.X, physicsPose.Orientation.Y, physicsPose.Orientation.Z, physicsPose.Orientation.W);

            Matrix worldMatrix = Matrix.CreateFromQuaternion(renderOrientation) * Matrix.CreateTranslation(renderPosition);

            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = _boneTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}