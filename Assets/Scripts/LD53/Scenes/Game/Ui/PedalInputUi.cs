using LD53.Data.Bikes;
using LD53.Inputs;
using UnityEngine;
using Utils.Extensions;

namespace LD53.Scenes.Game.Ui {
	public class PedalInputUi : MonoBehaviour {
		public static bool           visible        { get; set; }
		public static PedalMechanism pedalMechanism { get; set; }

		[SerializeField] protected KeyUi _leftPedalKey;
		[SerializeField] protected KeyUi _rightPedalKey;

		private void Start() {
			_leftPedalKey.key = GameInput.controls.Bike.LeftPedal.controls.GetSafe(0)?.shortDisplayName.ToUpper() ?? string.Empty;
			_rightPedalKey.key = GameInput.controls.Bike.RightPedal.controls.GetSafe(0)?.shortDisplayName.ToUpper() ?? string.Empty;
		}

		private void Update() {
			_leftPedalKey.opacity = pedalMechanism && visible ? pedalMechanism.leftPedalEfficiency : 0;
			_rightPedalKey.opacity = pedalMechanism && visible ? pedalMechanism.rightPedalEfficiency : 0;
		}
	}
}