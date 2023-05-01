using System;
using System.Collections;
using System.Text;
using LD53.Data.Orders;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Utils.Events;
using Utils.Extensions;

namespace LD53.Scenes.Game.Data {
	public static class GameContext {
#if UNITY_EDITOR
		private const  int    gameTime = 1 * 60;
		private static string apiUrlRoot => "localhost/nathanistace/gamejams/ludumdare53/api/";
#else
		private const int gameTime = 5 * 60;
		private static string apiUrlRoot => "https://nathanistace.be/gamejams/ludumdare50/api/";
#if UNITY_EDITOR
#else
#endif
#endif
		public static  int   score        { get; private set; }
		private static int   deliveries   { get; set; }
		public static  int   crashes      { get; set; }
		private static float endTime      { get; set; }
		private static bool  timerRunning { get; set; }

		public static float remainingTime => timerRunning ? Mathf.Clamp(endTime - Time.time, 0, gameTime) : gameTime;

		public static UnityEvent onNewGame                 { get; } = new UnityEvent();
		public static FloatEvent onDeliveredAndGainedScore { get; } = new FloatEvent();

		public static void SetupNewGame() {
			score = 0;
			deliveries = 0;
			crashes = 0;
			timerRunning = false;
			DeliveryManager.onOrderDelivered.AddListenerOnce(HandleOrderDelivered);
			onNewGame.Invoke();
		}

		private static void HandleOrderDelivered(DeliveryOrder order, DeliveryPoint delivery, PickUpPoint pickUp) {
			var gainedScore = order.GetScoreNow();
			score += gainedScore;
			deliveries++;
			onDeliveredAndGainedScore.Invoke(gainedScore);
		}

		public static void StartTimer() {
			endTime = Time.time + gameTime;
			timerRunning = true;
		}

		public static IEnumerator CompileStats(UnityAction<GameStats> callback) {
			GetScoreResult requestResult = null;
			/*	using (var webRequest = new UnityWebRequest($"{apiUrlRoot}scores", "GET")) {
					webRequest.downloadHandler = new DownloadHandlerBuffer();
	
					yield return webRequest.SendWebRequest();
					if (webRequest.result == UnityWebRequest.Result.Success) {
						requestResult = JsonUtility.FromJson<GetScoreResult>(webRequest.downloadHandler.text);
					}
				}*/

			yield return null;
			callback.Invoke(new GameStats {
				score = score,
				deliveries = deliveries,
				crashes = crashes,
				onlineDataGathered = requestResult != null,
				onlineRanking = requestResult?.onlineRanking ?? -1,
				entriesInDb = requestResult?.entriesInDb ?? -1
			});
		}

		public static IEnumerator SendHighScore(string name) {
			/*	using (var webRequest = new UnityWebRequest($"{apiUrlRoot}scores", "GET")) {
					var data = new PostScoreData { name = name, score = score, deliveries = deliveries, crashes = crashes };
					webRequest.SetRequestHeader("Content-Type", "application/json");
					webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
					webRequest.downloadHandler = new DownloadHandlerBuffer();
					yield return webRequest.SendWebRequest();
				}*/
			yield return null;
		}

		[Serializable] public class GetScoreResult {
			public int onlineRanking;
			public int entriesInDb;
		}

		[Serializable] public class PostScoreData {
			public int    score;
			public int    crashes;
			public int    deliveries;
			public string name;
		}
	}
}