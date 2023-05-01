using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD53.Scenes.Game.Ui {
	public class TutorialUi : MonoBehaviour {
		private static TutorialUi instance { get; set; }

		[SerializeField] protected Button       _button;
		[SerializeField] protected TMP_Text     _buttonLabel;
		[SerializeField] protected GameObject[] _pages;
		[SerializeField] protected string[]     _buttonsLabels;

		public static int pageCount => instance._pages.Length;

		public static UnityEvent onEndTutorialClicked { get; } = new UnityEvent();

		public void Awake() => instance = this;

		public void Start() {
			_button.onClick.AddListenerOnce(onEndTutorialClicked.Invoke);
			Hide();
		}

		public static void Show(int page) {
			instance.gameObject.SetActive(true);
			for (var index = 0; index < instance._pages.Length; index++) {
				instance._pages[index].gameObject.SetActive(page == index);
			}
			instance._buttonLabel.text = instance._buttonsLabels[page];
		}

		public static void Hide() {
			instance.gameObject.SetActive(false);
		}
	}
}