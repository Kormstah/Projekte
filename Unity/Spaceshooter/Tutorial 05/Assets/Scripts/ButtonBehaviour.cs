using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            ResetStats();
            SceneManager.LoadScene(1);
        }
    }
    public void LoadLevelByIndex(int levelIndex)
    {
        ResetStats();
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevelByName(string levelName)
    {
        ResetStats();
        SceneManager.LoadScene(levelName);
    }

    public void ResetStats()
    {
        Player.score = 0;
        Player.lives = 3;
        Player.missed = 0;
        Player.killStreak = 0; // (P02,E01)
    }
}
