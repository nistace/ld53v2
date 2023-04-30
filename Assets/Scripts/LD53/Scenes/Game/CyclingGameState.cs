using System.Collections;
using LD53.Data.Orders;
using LD53.Scenes.Game.Ui;
using UnityEngine;
using Utils.Extensions;
using Utils.GameStates;

namespace LD53.Scenes.Game {
	public class CyclingGameState : GameState {
		public static GameState state { get; } = new CyclingGameState();

		private CyclingGameState() { }

		protected override void Enable() {
			PedalInputUi.pedalMechanism = GameData.currentBike.pedalMechanism;
			PedalInputUi.visible = true;
			GameData.currentBike.SetMovementEnabled(true);
			GameData.currentBike.SetLookAroundEnabled(true);
		}

		protected override void Disable() {
			PedalInputUi.visible = false;
			GameData.currentBike.SetMovementEnabled(false);
			GameData.currentBike.SetLookAroundEnabled(false);
		}

		protected override IEnumerator Continue() {
			while (enabled) {
				yield return null;
			}
		}

		protected override void SetListenersEnabled(bool enabled) {
			GameData.currentBike.onStoppedInInteractionArea.SetListenerActive(HandleStoppedInInteractionArea, enabled);
			GameData.currentBike.onCollisionHappened.SetListenerActive(HandleBikeCollided, enabled);
		}

		private static void HandleBikeCollided(Vector3 collisionForce) {
			WatchCrashGameState.state.Prepare(collisionForce);
			ChangeState(WatchCrashGameState.state);
		}

		private static void HandleStoppedInInteractionArea(BuildingInteractionArea interactionArea) => GameData.deliveryManager.HandleInteraction(interactionArea, GameData.inventory);
	}
}