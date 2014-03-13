using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;

namespace FrozenCore.Components
{
    [RequiredComponent(typeof(Transform))]
    public class Mover : Component, ICmpUpdatable
    {
        private abstract class Operation
        {
            protected float _movingSpeed;
            protected float _rotationSpeed;

            internal Operation(float inMovingSpeed, float inRotationSpeed)
            {
                _movingSpeed = inMovingSpeed;
                _rotationSpeed = inRotationSpeed;
            }

            internal abstract void DoTransform(float inSecondsPast, Transform inTransform);
        }

        private class Rotate : Operation
        {
            private float _origin { get; set; }
            private float _target { get; set; }

            internal Rotate(float inRotationSpeed, float inOriginAngle, float inTargetAngle)
                : base(0, inRotationSpeed)
            {
                _origin = inOriginAngle;
                _target = inTargetAngle;
            }

            internal override void DoTransform(float inSecondsPast, Transform inTransform)
            {
                
            }
        }

        private class Move : Operation
        {
            internal Vector3 Target { get; set; }

            internal Move(float inMovingSpeed, Vector3 inTargetPosition)
                : base(inMovingSpeed, 0)
            {
                Target = inTargetPosition;
            }

            internal override void DoTransform(float inSecondsPast, Transform inTransform)
            {
                
            }
        }

        [NonSerialized]
        private List<Operation> _operations;

        [NonSerialized]
        private Operation _currentOperation;

        public Mover()
        {
            _operations = new List<Operation>();
        }

        void ICmpUpdatable.OnUpdate()
        {
            float secondsPast = Time.LastDelta / 1000;

            if (_operations.Count > 0)
            {
                _currentOperation = _operations[0];
                _operations.RemoveAt(0);
            }

            if (_currentOperation != null)
            {
                _currentOperation.DoTransform(secondsPast, this.GameObj.Transform);
            }
        }

        public void MoveTo(Vector2 inTarget, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, new Vector3(inTarget, 0)));
        }

        public void MoveTo(Vector3 inTarget, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, inTarget));
        }

        public void RotateTo(float inTarget, float inSpeed)
        {
            _operations.Add(new Rotate(inSpeed, inTarget));
        }

        public void MoveBy(Vector2 inAmount, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, this.GameObj.Transform.Pos + new Vector3(inAmount, 0)));
        }

        public void MoveBy(Vector3 inAmount, float inSpeed)
        {
            _operations.Add(new Move(inSpeed, this.GameObj.Transform.Pos + inAmount));
        }

        public void RotateBy(float inAmount, float inSpeed)
        {
            _operations.Add(new Rotate(inSpeed, this.GameObj.Transform.Angle + inAmount));
        }
    }
}
