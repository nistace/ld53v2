using System;
using UnityEngine;

namespace LD53 {
	public class AlignForwardWithRoot : MonoBehaviour {
		private void Update() {
			transform.forward = transform.root.forward;
		}
	}
}