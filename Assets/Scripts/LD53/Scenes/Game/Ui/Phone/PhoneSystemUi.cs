using System;
using UnityEngine;

namespace LD53.Scenes.Game.Ui {
	public class PhoneSystemUi : MonoBehaviour {
		private static PhoneSystemUi instance { get; set; }

		[SerializeField] protected RectTransform       _smallTransform;
		[SerializeField] protected PhoneMapUi.Strategy _smallMapStrategy;
		[SerializeField] protected RectTransform       _fullScreenTransform;
		[SerializeField] protected PhoneMapUi.Strategy _fullScreenMapStrategy;
		[SerializeField] protected PhoneUi             _phoneUi;

		public enum Position {
			Pocket,
			FullScreen
		}

		private void Awake() {
			instance = this;
		}

		public void Start() => ShowPhone(Position.Pocket);

		public static void ShowPhone(Position position) {
			if (!instance) return;
			instance._phoneUi.MoveTo(instance.GetPositionRect(position), instance.GetMapStrategy(position));
		}

		private PhoneMapUi.Strategy GetMapStrategy(Position position) {
			switch (position) {
				case Position.Pocket:     return _smallMapStrategy;
				case Position.FullScreen: return _fullScreenMapStrategy;
				default:                  throw new ArgumentOutOfRangeException(nameof(position), position, null);
			}
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