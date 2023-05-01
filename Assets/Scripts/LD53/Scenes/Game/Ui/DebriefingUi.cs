using LD53.Scenes.Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD53.Scenes.Game.Ui {
	public class DebriefingUi : MonoBehaviour {
		private static DebriefingUi instance { get; set; }

		[SerializeField] protected GameObject     _processingPage;
		[SerializeField] protected GameObject     _offlinePage;
		[SerializeField] protected TMP_Text       _offlinePageText;
		[SerializeField] protected Button         _continueAfterOfflinePageButton;
		[SerializeField] protected GameObject     _highScorePage;
		[SerializeField] protected TMP_Text       _highScoreText;
		[SerializeField] protected TMP_InputField _highScoreNameInput;
		[SerializeField] protected Button         _saveScoreAndContinueButton;
		[SerializeField] protected Button         _skipScoreButton;
		[SerializeField] protected GameObject     _finalPage;
		[SerializeField] protected Button         _restartButton;
		[SerializeField] protected Button         _quitButton;

		public static string highScoreInputName => instance._highScoreNameInput.text;

		public static UnityEvent onContinueToFinalPageClicked { get; } = new UnityEvent();
		public static UnityEvent onSubmitScoreClicked         { get; } = new UnityEvent();
		public static UnityEvent onRestartClicked             { get; } = new UnityEvent();
		public static UnityEvent onExitClicked                { get; } = new UnityEvent();

		private static string saveOfflinePageTemplate   { get; set; }
		private static string saveHighScorePageTemplate { get; set; }

		public enum Page {
			Processing,
			Offline,
			HighScore,
			Final,
		}

		public void Awake() => instance = this;

		public void Start() {
			gameObject.SetActive(false);
			saveOfflinePageTemplate = _offlinePageText.text;
			saveHighScorePageTemplate = _highScoreText.text;
			_continueAfterOfflinePageButton.onClick.AddListenerOnce(onContinueToFinalPageClicked.Invoke);
			_saveScoreAndContinueButton.onClick.AddListenerOnce(onSubmitScoreClicked.Invoke);
			_skipScoreButton.onClick.AddListenerOnce(onContinueToFinalPageClicked.Invoke);
			_restartButton.onClick.AddListenerOnce(onRestartClicked.Invoke);
			_quitButton.onClick.AddListenerOnce(onExitClicked.Invoke);
		}

		public static void PopulateWithStats(GameStats stats) {
			foreach ((var component, var template) in new[] { (instance._offlinePageText, saveOfflinePageTemplate), (instance._highScoreText, saveHighScorePageTemplate) }) {
				var text = template;
				text = text.Replace("[SCORE]", $"{stats.score}");
				text = text.Replace("[DELIVERIES]", $"{stats.deliveries}");
				text = text.Replace("[CRASHES]", $"{stats.crashes}");
				text = text.Replace("[RANKING]", $"{stats.onlineRanking}");
				text = text.Replace("[EMPLOYEES]", $"{stats.entriesInDb}");
				component.text = text;
			}
		}

		public static void Show(Page page) {
			instance._processingPage.gameObject.SetActive(page == Page.Processing);
			instance._offlinePage.gameObject.SetActive(page == Page.Offline);
			instance._highScorePage.gameObject.SetActive(page == Page.HighScore);
			instance._finalPage.gameObject.SetActive(page == Page.Final);
			instance.gameObject.SetActive(true);
		}

		public static void Hide() {
			instance.gameObject.SetActive(false);
		}
	}
}