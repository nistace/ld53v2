using System;
using UnityEngine;

namespace LD53.Scenes.Game.Ui {
	public class PhoneSystemUi : MonoBehaviour {
		private static PhoneSystemUi instance { get; set; }

		[SerializeField] protected RectTransform _smallTransform;
		[SerializeField] protected RectTransform _fullScreenTransform;
		[SerializeField] protected PhoneUi       _phoneUi;

		public enum Position {
			Pocket,
			FullScreen
		}

		private void Awake() {
			instance = this;
		}

		public static void ShowPhone(Position position) {
			if (!instance) return;
			instance._phoneUi.MoveTo(instance.GetPositionRect(position));
		}

		private RectTransform GetPositionRect(Position position) {
			switch (position) {
				case Position.Pocket:     return _smallTransform;
				case Position.FullScreen: return _fullScreenTransform;
				default:                  throw new ArgumentOutOfRangeException(nameof(position), position, null);
			}
		}
	}
}