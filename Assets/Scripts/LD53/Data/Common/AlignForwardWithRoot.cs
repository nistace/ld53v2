using UnityEngine;

namespace LD53.Data.Common {
	public class AlignForwardWithRoot : MonoBehaviour {
		private void Update() {
			transform.forward = transform.root.forward;
		}
	}
}