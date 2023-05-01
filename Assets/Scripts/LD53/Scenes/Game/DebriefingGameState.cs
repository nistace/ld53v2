using System.Collections;
using LD53.Scenes.Game.Data;
using LD53.Scenes.Game.Ui;
using UnityEngine;
using Utils.Coroutines;
using Utils.Extensions;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class DebriefingGameState : GameState {
		public static GameState state { get; } = new DebriefingGameState();
		private DebriefingGameState() { }

		protected override void Enable() {
			DebriefingUi.Show(DebriefingUi.Page.Processing);
			CoroutineRunner.Run(GameContext.CompileStats(OnStatsCompiled));
		}

		protected override void Disable() => DebriefingUi.Hide();

		protected override IEnumerator Continue() {
			yield break;
		}

		protected override void SetListenersEnabled(bool enabled) {
			DebriefingUi.onContinueToFinalPageClicked.AddListenerOnce(ContinueToFinalPage);
			DebriefingUi.onSubmitScoreClicked.AddListenerOnce(HandleSubmitScore);
			DebriefingUi.onRestartClicked.AddListenerOnce(HandleRestart);
			DebriefingUi.onExitClicked.AddListenerOnce(Application.Quit);
		}

		private static void OnStatsCompiled(GameStats stats) {
			DebriefingUi.PopulateWithStats(stats);
			DebriefingUi.Show(stats.onlineDataGathered ? DebriefingUi.Page.HighScore : DebriefingUi.Page.Offline);
		}

		private static void HandleRestart() => ChangeState(InitGameState.state);

		private void HandleSubmitScore() {
			// TODO SUBMIT
			ContinueToFinalPage();
		}

		private static void ContinueToFinalPage() => DebriefingUi.Show(DebriefingUi.Page.Final);
	}
}