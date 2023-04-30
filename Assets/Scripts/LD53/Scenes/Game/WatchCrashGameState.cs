using System.Collections;
using LD53.Data.Bikes;
using UnityEngine;
using Utils.GameStates;

namespace LD53.Scenes.Game {
	public class WatchCrashGameState : GameState {
		public static WatchCrashGameState state { get; } = new WatchCrashGameState();
		private WatchCrashGameState() { }

		private Vector3 collisionForce { get; set; }

		public void Prepare(Vector3 collision) {
			collisionForce = collision;
		}

		protected override void Enable() {
			GameData.currentBike.Crash(collisionForce);
			GameData.camera.target = GameData.currentBike.deliverer.headPosition;
			GameData.camera.strategy = FollowCam.Strategy.WatchTarget;
		}

		protected override void Disable() { }

		protected override IEnumerator Continue() {
			yield return new WaitForSeconds(10f);
			ChangeState(RespawnGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}