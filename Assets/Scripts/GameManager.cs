using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;

    public TextMeshProUGUI scoreText;

    public void IncreaseScore(){
        score ++;        
        scoreText.text = "" + score;
    }

    public void GameOver(){
        //Display menus to start new game
        Debug.Log("Game over baby!");
        Time.timeScale = 0f;
    }
}
