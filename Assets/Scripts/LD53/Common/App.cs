using System;
using LD53.Inputs;
using UnityEngine;
using Utils.Libraries;
using Utils.Loading;

public class App : MonoBehaviour {
	private static bool initialized { get; set; }

	[SerializeField] protected LoadingCanvas    _loadingScreen;
	[SerializeField] protected LibraryBuildData _libraryBuildData;

	private void Awake() {
		if (initialized) {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		_libraryBuildData.Build();
		LoadingCanvas.instance = _loadingScreen;
		_loadingScreen.gameObject.SetActive(true);
		initialized = true;
	}

	private void OnEnable() {
		GameInput.controls.System.Enable();
	}

}