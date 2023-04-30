using UnityEngine;
using UnityEngine.UI;

namespace LD53.Scenes.Game.Ui {
	public class PhoneInteractionAreaTokenUi : MonoBehaviour {
		[SerializeField] protected Transform     _arrowRotator;
		[SerializeField] protected Image         _arrowImage;
		[SerializeField] protected RectTransform _rectTransform;

		public bool showArrow {
			get => _arrowImage.enabled;
			set => _arrowImage.enabled = value;
		}

		public object rectTransform { get; set; }

		public void ShowArrow(float angle) {
			_arrowImage.enabled = true;
			_arrowRotator.rotation = Quaternion.Euler(0, 0, angle);
		}

		public void HideArrow() => _arrowImage.enabled = false;

		public void SetMapPosition(Vector2 position) {
			_rectTransform.anchorMin = position;
			_rectTransform.anchorMax = position;
			_rectTransform.anchoredPosition = Vector2.zero;
		}
	}
}