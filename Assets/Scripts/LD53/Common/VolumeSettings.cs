using LD53.Inputs;
using UnityEngine;
using Utils.Audio;

namespace LD53.Common {
	public class VolumeSettings : MonoBehaviour {
		[SerializeField] protected float _speed;

		private void Update() {
			AudioManager.masterVolume += GameInput.controls.System.Volume.ReadValue<float>() * _speed;
		}
	}
}