using System.Collections;
using LD53.Data.Bikes;
using LD53.Scenes.Game.Data;
using UnityEngine;
using Utils.Audio;
using Utils.Coroutines;
using Utils.GameStates;
using Utils.Loading;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class WatchCrashGameState : GameState {
		public static WatchCrashGameState state { get; } = new WatchCrashGameState();
		private WatchCrashGameState() { }

		private Vector3 collisionForce { get; set; }

		public void Prepare(Vector3 collision) {
			collisionForce = collision;
		}

		protected override void Enable() {
			GameContext.crashes++;
			GameData.currentBike.Crash(collisionForce);
			GameData.camera.target = GameData.currentBike.deliverer.headPosition;
			GameData.camera.strategy = FollowCam.Strategy.WatchTarget;
		}

		protected override void Disable() { }

		protected override IEnumerator Continue() {
			AudioManager.Music.loop = false;
			AudioManager.Music.ChangeClip("musicStop", true);
			yield return new WaitForSeconds(6f);
			yield return CoroutineRunner.Run(LoadingCanvas.instance.DoFadeIn());
			ChangeState(RespawnGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}