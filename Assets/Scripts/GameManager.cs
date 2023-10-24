using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
	private int introSceneBuildIndex = 1;
	public int GetIntroSceneBuildIndex => introSceneBuildIndex;

	[SerializeField]
	private int townSceneBuildIndex = 2;
	public int GetTownSceneBuildIndex => townSceneBuildIndex;

	[SerializeField]
	private int bakerySceneBuildIndex = 3;
	public int GetBakerySceneBuildIndex => bakerySceneBuildIndex;

	[SerializeField]
	private int bakeryBattleSceneBuildIndex = 4;
	public int GetBakeryBattleSceneBuildIndex => bakeryBattleSceneBuildIndex;
	
	[SerializeField]
	private int dragBarSceneBuildIndex = 5;
	public int GetDragBarSceneBuildIndex => dragBarSceneBuildIndex;
	
	[SerializeField]
	private int dragBarBattleSceneBuildIndex = 6;
	public int GetDragBarBattleSceneBuildIndex => dragBarBattleSceneBuildIndex;

	[SerializeField]
	private int outroSceneBuildIndex = 7;
	public int GetOutroSceneBuildIndex => outroSceneBuildIndex;
	
	[Header("Scene Transition Options")]
	[SerializeField]
	private Image blackoutScreen;
	[SerializeField]
	private float blackoutSpeed = 1f;
	
	
	//Positional Data
	private Vector3 lastPlayerPositionInTown;

	//Game Progress values
	private bool bFinishedBakery = false;
	public bool FinishedBakery => bFinishedBakery;	
	
	private bool bFinishedBakeryBattle = false;
	public bool FinishedBakeryBattle => bFinishedBakeryBattle;
	
	
	private bool bFinishedDrag = false;
	public bool FinishedDrag => bFinishedDrag;	
	
	private bool bFinishedDragBarBattle = false;
	public bool FinishedDragBattle => bFinishedDragBarBattle;
	
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

	public bool IsBakeryFinished()
	{
		return bFinishedBakery;
	}


	public void SetBakeryFinished(bool bvalue){
		bFinishedBakery = bvalue;
	}

	public void SetDragFinished(bool bvalue){
		bFinishedDrag = bvalue;
	}
	public bool IsDragFinished()
	{
		return bFinishedDrag;
	}

	public bool IsBakeryAndDragFinished()
	{
		return bFinishedBakery && bFinishedDrag;
	}


	
	#region Data Management

	private void SaveData(int fromBuildIndex, int toBuildIndex)
	{
		Debug.Log("Saving data...");
		
		//When transitioning out of town scene.
		if (fromBuildIndex == townSceneBuildIndex &&  toBuildIndex != mainMenuBuildIndex)
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

		//Progress - Finished battle Scenes
		if (fromBuildIndex == bakeryBattleSceneBuildIndex && toBuildIndex == bakerySceneBuildIndex)
		{
			bFinishedBakeryBattle = true;
		}
		if (fromBuildIndex == dragBarBattleSceneBuildIndex && toBuildIndex == dragBarSceneBuildIndex)
		{
			bFinishedDragBarBattle = true;
		}
		
		//Progress - Finished entire interactions
		if (fromBuildIndex == bakerySceneBuildIndex && toBuildIndex == townSceneBuildIndex)
		{
			bFinishedBakery = true;
		}
		if (fromBuildIndex == dragBarSceneBuildIndex && toBuildIndex == townSceneBuildIndex)
		{
			bFinishedDrag = true;
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

		if (toBuildIndex == townSceneBuildIndex)
		{
			AudioManager.Instance.PlayTravelingTheme();
		}
		else if (toBuildIndex == bakeryBattleSceneBuildIndex || toBuildIndex == dragBarBattleSceneBuildIndex)
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

	public void LoadIntroScene()
	{
		StartCoroutine(TransitionScene(introSceneBuildIndex));
	}

	public void LoadMainMenuScene()
	{
		StartCoroutine(TransitionScene(mainMenuBuildIndex));
	}

	public void LoadTownScene()
	{
		StartCoroutine(TransitionScene(townSceneBuildIndex));
	}

	public void LoadBakeryBattleScene()
	{
		StartCoroutine(TransitionScene(bakeryBattleSceneBuildIndex));
	}
	
	public void LoadBakeryScene()
	{
		StartCoroutine(TransitionScene(bakerySceneBuildIndex));
	}

	public void LoadDragBarBattleScene()
	{
		StartCoroutine(TransitionScene(dragBarBattleSceneBuildIndex));
	}
	
	public void LoadDragBarScene()
	{
		StartCoroutine(TransitionScene(dragBarSceneBuildIndex));
	}

	public void LoadOutroScene()
	{
		StartCoroutine(TransitionScene(outroSceneBuildIndex));
	}
	
	#endregion


	public AudioManager GetAudioManager(){
		return AudioManager.Instance;
	}
}
