using System;
using UnityEngine;

namespace LD53 {
	public class PedalMechanism : MonoBehaviour {
		[SerializeField] protected RotatingThing  _rotatingPart;
		[SerializeField] protected AnimationCurve _usageOfLeftPedalCurve;
		[SerializeField] protected AnimationCurve _usageOfRightPedalCurve;

		public RotatingThing rotatingPart => _rotatingPart;

		public float leftPedalEfficiency  => _usageOfLeftPedalCurve.Evaluate(GetRotatingPartAngle());
		public float rightPedalEfficiency => _usageOfRightPedalCurve.Evaluate(GetRotatingPartAngle());

		private float GetRotatingPartAngle() => Vector3.SignedAngle(transform.forward, Vector3.forward, transform.right);
	}
}