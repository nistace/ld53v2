using System.Collections;
using LD53.Data.Bikes;
using LD53.Data.Orders;
using LD53.Inputs;
using LD53.Scenes.Game.Data;
using LD53.Scenes.Game.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Audio;
using Utils.Extensions;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class CyclingGameState : GameState {
		public static GameState state { get; } = new CyclingGameState();

		private CyclingGameState() { }

		protected override void Enable() {
			PedalInputUi.pedalMechanism = GameData.currentBike.pedalMechanism;
			PedalInputUi.visible = true;
			DeliveryManager.creationEnabled = true;
		}

		protected override void Disable() {
			PedalInputUi.visible = false;
			DeliveryManager.creationEnabled = false;
			PhoneSystemUi.ShowPhone(PhoneSystemUi.Position.Pocket);
		}

		protected override IEnumerator Continue() {
			while (enabled) {
				if (Mathf.Approximately(GameContext.remainingTime, 0)) {
					ChangeState(DebriefingGameState.state);
					yield break;
				}
				yield return null;
			}
		}

		protected override void SetListenersEnabled(bool enabled) {
			GameData.currentBike.onStoppedInInteractionArea.SetListenerActive(HandleStoppedInInteractionArea, enabled);
			GameData.currentBike.onCollisionHappened.SetListenerActive(HandleBikeCollided, enabled);
			GameInput.controls.Bike.GrabPhone.SetAnyListenerOnce(HandleGrabPhoneChanged, enabled);
			GameInput.controls.Bike.DringDring.SetPerformListenerOnce(HandleDringDring, enabled);
			DeliveryManager.onOrderCreated.AddListenerOnce(HandleOrderCreated);
			DeliveryManager.onOrderPickedUp.AddListenerOnce(HandleOrderPickedUp);
			DeliveryManager.onOrderDelivered.AddListenerOnce(HandleOrderDelivered);
			GameInput.controls.Bike.DringDring.SetEnabled(enabled);
			Bike.SetMovementEnabled(enabled);
			Bike.SetLookAroundEnabled(enabled);
			GameInput.controls.Bike.GrabPhone.SetEnabled(enabled);
		}

		private static void HandleDringDring(InputAction.CallbackContext obj) => AudioManager.Sfx.Play("dringDring");
		private static void HandleOrderDelivered(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUpPoint) => AudioManager.Sfx.Play("success");
		private static void HandleOrderCreated(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUpPoint) => AudioManager.Sfx.Play("notification");
		private static void HandleOrderPickedUp(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUpPoint) => AudioManager.Sfx.Play("orderReady");

		private static void HandleGrabPhoneChanged(InputAction.CallbackContext obj) => PhoneSystemUi.ShowPhone(obj.performed ? PhoneSystemUi.Position.FullScreen : PhoneSystemUi.Position.Pocket);

		private static void HandleBikeCollided(Vector3 collisionForce) {
			WatchCrashGameState.state.Prepare(collisionForce);
			ChangeState(WatchCrashGameState.state);
		}

		private static void HandleStoppedInInteractionArea(BuildingInteractionArea interactionArea) => GameData.deliveryManager.HandleInteraction(interactionArea);
	}
}