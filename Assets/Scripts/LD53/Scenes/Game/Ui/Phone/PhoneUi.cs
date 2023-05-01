using UnityEngine;
using Utils.Extensions;

namespace LD53.Scenes.Game.Ui {
	public class PhoneUi : MonoBehaviour {
		[SerializeField] protected RectTransform _rectTransform;
		[SerializeField] protected float         _transitionSpeed = .5f;

		public void MoveTo(RectTransform parent) {
			transform.SetParent(parent);
			_rectTransform.MoveAnchorsKeepPosition(Vector2.zero, Vector2.one);
		}

		private void Update() {
			_rectTransform.offsetMin = Vector2.MoveTowards(_rectTransform.offsetMin, Vector2.zero, Time.deltaTime * _transitionSpeed);
			_rectTransform.offsetMax = Vector2.MoveTowards(_rectTransform.offsetMax, Vector2.zero, Time.deltaTime * _transitionSpeed);
		}
	}
}