using System.Collections.Generic;
using LD53.Data.Orders;
using LD53.Scenes.Game.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Utils.Types;

namespace LD53.Scenes.Game.Ui {
	public class PhoneMapUi : MonoBehaviour {
		[SerializeField] protected RectTransform               _mapImage;
		[SerializeField] protected Image                       _playerIcon;
		[SerializeField] protected PhoneInteractionAreaTokenUi _deliveryTokenPrefab;
		[SerializeField] protected PhoneInteractionAreaTokenUi _pickUpTokenPrefab;
		[SerializeField] protected RectTransform               _tokensParent;

		private Queue<PhoneInteractionAreaTokenUi>              inactiveDeliveryTokens { get; } = new Queue<PhoneInteractionAreaTokenUi>();
		private Queue<PhoneInteractionAreaTokenUi>              inactivePickUpTokens   { get; } = new Queue<PhoneInteractionAreaTokenUi>();
		private Map<PhoneInteractionAreaTokenUi, DeliveryPoint> activeDeliveryTokens   { get; } = new Map<PhoneInteractionAreaTokenUi, DeliveryPoint>();
		private Map<PhoneInteractionAreaTokenUi, PickUpPoint>   activePickUpTokens     { get; } = new Map<PhoneInteractionAreaTokenUi, PickUpPoint>();

		private void Start() {
			DeliveryManager.onOrderRemoved.AddListenerOnce(HandleOrderRemoved);
			DeliveryManager.onOrderCreated.AddListenerOnce(HandleOrderCreated);
			DeliveryManager.onOrderPickedUp.AddListenerOnce(HandleUpdatePickedUp);
		}

		private void HandleOrderCreated(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUp) {
			if (pickUp.IsInteractable()) AddToken(activePickUpTokens, inactivePickUpTokens, pickUp, _pickUpTokenPrefab);
			if (delivery.IsInteractable()) AddToken(activeDeliveryTokens, inactiveDeliveryTokens, delivery, _deliveryTokenPrefab);
		}

		private void HandleUpdatePickedUp(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUp) {
			RemoveToken(activePickUpTokens, inactivePickUpTokens, pickUp);
			if (delivery.IsInteractable()) AddToken(activeDeliveryTokens, inactiveDeliveryTokens, delivery, _deliveryTokenPrefab);
		}

		private void HandleOrderRemoved(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUp) {
			RemoveToken(activeDeliveryTokens, inactiveDeliveryTokens, delivery);
			RemoveToken(activePickUpTokens, inactivePickUpTokens, pickUp);
		}

		private static void RemoveToken<E>(Map<PhoneInteractionAreaTokenUi, E> activeTokenMap, Queue<PhoneInteractionAreaTokenUi> inactiveTokenQueue, E target) {
			if (activeTokenMap.ContainsRight(target)) {
				var token = activeTokenMap.LeftOf(target);
				token.gameObject.SetActive(false);
				inactiveTokenQueue.Enqueue(token);
				activeTokenMap.RemoveRight(target);
			}
		}

		private void AddToken<E>(Map<PhoneInteractionAreaTokenUi, E> activeTokenMap, Queue<PhoneInteractionAreaTokenUi> inactiveTokenQueue, E target, PhoneInteractionAreaTokenUi prefab)
			where E : IInteractablePoint {
			if (activeTokenMap.ContainsRight(target)) return;
			if (inactiveTokenQueue.Count == 0) {
				inactiveTokenQueue.Enqueue(Instantiate(prefab, _tokensParent));
			}
			var token = inactiveTokenQueue.Dequeue();
			token.gameObject.SetActive(true);
			token.SetMapPosition(GameData.gameEnvironment.GetPositionRelativeToBounds(target.worldPosition));
			token.HideArrow();
			activeTokenMap.Set(token, target);
		}

		private void Update() {
			_playerIcon.enabled = GameData.currentBike && !GameData.currentBike.crashed;
			if (!GameData.currentBike || GameData.currentBike.crashed) return;
			var playerOffset = GameData.gameEnvironment.GetPositionRelativeToBounds(GameData.currentBike.transform.position);
			Debug.Log(playerOffset);
			_mapImage.pivot = playerOffset;
			_mapImage.anchoredPosition = Vector2.zero;
			_playerIcon.transform.rotation = Quaternion.Euler(0, 0, -GameData.currentBike.transform.rotation.eulerAngles.y);
			// TODO Position the tokens on the map
		}
	}
}