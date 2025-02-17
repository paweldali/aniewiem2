using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    // private AchievementManager achievementManager;
    public Vector2 lastCheckPointPos;

    public Vector2 startPos;

    public bool isLevelCompleted = false;

    public float levelTime;

    public int levelNumber = 1;

    public int numberOfLevels = 15;

    private Timer timer;


    // Start is called before the first frame update, keeping one instance of gamemaster
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        levelNumber = SaveManager.instance.LastPlayedLevel;
        // achievementManager = GameObject.FindGameObjectWithTag("AM").GetComponent<AchievementManager>();
        //timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelNumber != SaveManager.instance.LastPlayedLevel)
        {
            SaveManager.instance.LastPlayedLevel = levelNumber;
            SaveManager.instance.Save();
        }

        if (isLevelCompleted)
        {
            Debug.Log("level completed");
            timer = GameObject.Find("Main Camera").GetComponent<Timer>(); //after player death scene is destroyed, so this cant be in start()
            levelTime = timer.getCurrentTime();
            // Debug.Log("level" + levelNumber +  "completed with time:" + levelTime.ToString());

            //save time
            if ((levelTime < SaveManager.instance.bestTimes[levelNumber - 1]) || SaveManager.instance.bestTimes[levelNumber - 1] == 0f)
            {
                SaveManager.instance.bestTimes[levelNumber - 1] = levelTime;
                SaveManager.instance.Save();
                // Debug.Log(levelNumber + " level, new best time saved to file " + SaveManager.instance.bestTimes[levelNumber - 1]);
            }

            AchievementManager.instance.SetLevelTime(levelTime);
            AchievementManager.instance.CheckAchievementCompletion();

            isLevelCompleted = false;
            SceneManager.LoadScene("FinishedLevel");
        }
    }
}