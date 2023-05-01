using System.Collections;
using LD53.Scenes.Game.Data;
using Utils.Audio;
using Utils.Coroutines;
using Utils.GameStates;
using Utils.Loading;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class RespawnGameState : GameState {
		public static GameState state { get; } = new RespawnGameState();
		private RespawnGameState() { }
		protected override void Enable() { }
		protected override void Disable() { }

		protected override IEnumerator Continue() {
			GameData.InstantiateBike(GameData.gameEnvironment.GetClosestSpawnPosition(GameData.currentBike.transform.position));
			yield return CoroutineRunner.Run(LoadingCanvas.instance.DoFadeOut());
			AudioManager.Music.loop = true;
			AudioManager.Music.ChangeClip("music", true);
			ChangeState(CyclingGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}