using System.Collections;
using LD53.Data.Orders;
using LD53.Scenes.Game.Data;
using TMPro;
using UnityEngine;
using Utils.Extensions;

public class PhoneHudUi : MonoBehaviour {
	[Header("Score"), SerializeField]      protected TMP_Text       _scoreText;
	[SerializeField]                       protected TMP_Text       _gainedScoreText;
	[SerializeField]                       protected AnimationCurve _gainedScoreOpacityCurve;
	[SerializeField]                       protected float          _gainedScoreNotificationTime = 2;
	[Header("Time"), SerializeField]       protected TMP_Text       _timeText;
	[Header("Deliveries"), SerializeField] protected Transform      _newOrderNotification;
	[SerializeField]                       protected AnimationCurve _newOrderAnimationCurve;
	[SerializeField]                       protected float          _newOrderAnimationTime = 2;

	private float gainedScoreNotificationStart { get; set; } = -100;

	private void Start() {
		GameContext.onDeliveredAndGainedScore.AddListenerOnce(HandleGainedScore);
		DeliveryManager.onOrderCreated.AddListenerOnce(HandleNewOrderCreated);
		GameContext.onNewGame.AddListenerOnce(ResetUi);
		ResetUi();
	}

	private void ResetUi() {
		gainedScoreNotificationStart = -100;
		_scoreText.text = "0";
	}

	private void HandleNewOrderCreated(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUp) => StartCoroutine(PlayNewOrderAnimation());

	private IEnumerator PlayNewOrderAnimation() {
		_newOrderNotification.gameObject.SetActive(true);
		for (var t = 0f; t < 1; t += Time.deltaTime / _newOrderAnimationTime) {
			_newOrderNotification.localScale = Vector3.one * _newOrderAnimationCurve.Evaluate(t / _newOrderAnimationTime);
			yield return null;
		}
		_newOrderNotification.gameObject.SetActive(false);
	}

	private void HandleGainedScore(float gainedScore) {
		_gainedScoreText.text = $"+{gainedScore}";
		gainedScoreNotificationStart = Time.time;
	}

	private void Update() {
		_scoreText.text = $"{GameContext.score}";
		_timeText.text = $"{Mathf.FloorToInt(GameContext.remainingTime / 60f):00}:{GameContext.remainingTime % 60:00}";
		_gainedScoreText.color = new Color(1, 1, 1, _gainedScoreOpacityCurve.Evaluate((gainedScoreNotificationStart - Time.time) / _gainedScoreNotificationTime));
	}
}