using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public UnityEvent transitioningScenesEvent;
	
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	[Header("Scene Build Data")] 
	
	[SerializeField]
	private int mainMenuBuildIndex = 0;
	public int GetMainMenuSceneBuildIndex => mainMenuBuildIndex;

	[SerializeField]
	private int townSceneBuildIndex = 1;
	public int GetTownSceneBuildIndex => townSceneBuildIndex;

	[SerializeField]
	private int bakerySceneBuildIndex = 2;
	public int GetBakerySceneBuildIndex => bakerySceneBuildIndex;

	[SerializeField]
	private int dragBarSceneBuildIndex = 3;
	public int GetDragBarSceneBuildIndex => dragBarSceneBuildIndex;
	
	[Header("Scene Transition Options")]
	[SerializeField]
	private Image blackoutScreen;
	[SerializeField]
	private float blackoutSpeed = 1f;
	
	
	//Positional Data
	private Vector3 lastPlayerPositionInTown;

	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			
			//Make persistent
			DontDestroyOnLoad(gameObject);
		}
	}

	#region Data Management

	private void SaveData(int fromBuildIndex, int toBuildIndex)
	{
		Debug.Log("Saving data...");
		
		//When transitioning out of town scene.
		if (fromBuildIndex == townSceneBuildIndex && (toBuildIndex == bakerySceneBuildIndex || toBuildIndex == dragBarSceneBuildIndex))
		{
			//Save player location data in town to load back when coming back to TownScene
			GameObject player = GameObject.FindWithTag("Player");
			if (player)
			{
				lastPlayerPositionInTown = player.transform.position;
			}
			else
			{
				Debug.LogWarning("Couldn't save player location data. No player found. Check for instance made or `Player` tag set.");
			}
		}
		
		
	}

	private void LoadData(int fromBuildIndex, int toBuildIndex)
	{
		Debug.Log("Loading data...");
		
		//When transitioning into town scene (not from main menu)
		if (toBuildIndex == townSceneBuildIndex && fromBuildIndex != mainMenuBuildIndex)
		{
			//Load player location data
			GameObject player = GameObject.FindWithTag("Player");
			if (player)
			{
				player.transform.position = lastPlayerPositionInTown;
			}
			else
			{
				Debug.LogWarning("Couldn't load player location data. No player found. Check for instance made or `Player` tag set.");
			}
		}

		if (toBuildIndex == townSceneBuildIndex )
		{
			AudioManager.Instance.PlayTravelingTheme();
		}
		else if (toBuildIndex == bakerySceneBuildIndex || toBuildIndex == dragBarSceneBuildIndex)
		{
			AudioManager.Instance.PlayBattleTheme();
		}
		
	}

	#endregion //Data Management


	#region Scene Transitioning
	
	//This method handles transitioning scenes.
	//It will save certain data and load data once the desired scene is loaded.
	private IEnumerator TransitionScene(int toBuildIndex)
	{
		transitioningScenesEvent?.Invoke();
		blackoutScreen.gameObject.SetActive(true);
		
		int fromBuildIndex = SceneManager.GetActiveScene().buildIndex;
		Debug.Log("Transitioning from Scene " + fromBuildIndex + " to Scene " + toBuildIndex);

		//Save Data
		SaveData(fromBuildIndex, toBuildIndex);
		
		
		//Transition effects to block scene
		float blackoutTVal = 0f;
		Color originalColor = blackoutScreen.color;
		while (blackoutScreen.color != Color.black)
		{
			blackoutTVal += Time.deltaTime * blackoutSpeed;
			blackoutScreen.color = Color.Lerp(originalColor, Color.black, blackoutTVal);
			yield return null;
		}
		
		
		//Load Data
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(toBuildIndex);
		LoadData(fromBuildIndex, toBuildIndex);
		yield return new WaitForSeconds(1f);
		
		
		//Transition effects to show scene
		blackoutTVal = 0f;
		while (blackoutScreen.color != originalColor)
		{
			blackoutTVal += Time.deltaTime * blackoutSpeed;
			blackoutScreen.color = Color.Lerp(Color.black, originalColor, blackoutTVal);
			yield return null;
		}
		
		blackoutScreen.gameObject.SetActive(false);
	}

	public void LoadMainMenuScene()
	{
		StartCoroutine(TransitionScene(mainMenuBuildIndex));
	}

	public void LoadTownScene()
	{
		StartCoroutine(TransitionScene(townSceneBuildIndex));
	}

	public void LoadBakeryScene()
	{
		StartCoroutine(TransitionScene(bakerySceneBuildIndex));
	}

	public void LoadDragBarScene()
	{
		StartCoroutine(TransitionScene(dragBarSceneBuildIndex));
	}
	
	#endregion

}
