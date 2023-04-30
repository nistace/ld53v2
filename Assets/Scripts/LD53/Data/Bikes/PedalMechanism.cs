using LD53.Data.Common;
using UnityEngine;

namespace LD53.Data.Bikes {
	public class PedalMechanism : MonoBehaviour {
		[SerializeField] protected RotatingThing  _rotatingPart;
		[SerializeField] protected AnimationCurve _curve;
		[SerializeField] protected float          _leftOffset;
		[SerializeField] protected float          _rightOffset;

		public RotatingThing rotatingPart => _rotatingPart;

		public float leftPedalEfficiency  => _curve.Evaluate((GetRotatingPartAngle() + _leftOffset) % 1);
		public float rightPedalEfficiency => _curve.Evaluate((GetRotatingPartAngle() + _rightOffset) % 1);

		private float GetRotatingPartAngle() => (Vector3.SignedAngle(transform.forward, transform.parent.forward, transform.right) + 180) / 360;
	}
}