using System.Collections;
using LD53.Scenes.Game.Data;
using Utils.GameStates;

namespace LD53.Scenes.Game {
	public class RespawnGameState : GameState {
		public static GameState state { get; } = new RespawnGameState();
		private RespawnGameState() { }
		protected override void Enable() { }
		protected override void Disable() { }

		protected override IEnumerator Continue() {
			GameData.InstantiateBike(GameData.gameEnvironment.GetClosestSpawnPosition(GameData.currentBike.transform.position));
			yield return null;
			// TODO transition
			ChangeState(CyclingGameState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}