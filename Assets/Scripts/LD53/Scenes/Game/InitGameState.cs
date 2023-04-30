using System.Collections;
using LD53.Inputs;
using LD53.Scenes.Game.Data;
using LD53.Scenes.Game.Ui;
using Utils.GameStates;

namespace LD53.Scenes.Game {
	public class InitGameState : GameState {
		public static GameState state { get; } = new InitGameState();
		private InitGameState() { }
		protected override void Enable() { }
		protected override void Disable() { }

		protected override IEnumerator Continue() {
			GameData.InstantiateBike(GameData.gameEnvironment.GetDefaultSpawnPosition());
			GameData.deliveryManager.Setup();
			yield return null;
			ChangeState(CyclingGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}