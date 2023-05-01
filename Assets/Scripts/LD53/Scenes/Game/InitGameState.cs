using System.Collections;
using LD53.Scenes.Game.Data;
using Utils.Audio;
using Utils.Coroutines;
using Utils.GameStates;
using Utils.Loading;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class InitGameState : GameState {
		public static GameState state { get; } = new InitGameState();
		private InitGameState() { }
		protected override void Enable() { }
		protected override void Disable() { }

		protected override IEnumerator Continue() {
			GameData.InstantiateBike(GameData.gameEnvironment.GetDefaultSpawnPosition());
			GameContext.SetupNewGame();
			GameData.deliveryManager.Setup();
			AudioManager.Music.loop = true;
			AudioManager.Music.ChangeClip("music", true);
			yield return CoroutineRunner.Run(LoadingCanvas.instance.DoFadeOut());
			if (TutorialGameState.alreadySawTutorial) {
				GameContext.StartTimer();
				ChangeState(CyclingGameState.state);
			}
			else {
				ChangeState(TutorialGameState.state);
			}
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}