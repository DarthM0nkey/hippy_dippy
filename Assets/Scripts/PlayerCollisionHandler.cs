using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object the player collided with is a laser
        if (other.CompareTag("Bad"))
        {
            // Reset the game by reloading the current scene
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene to restart the game
        SceneManager.LoadScene(currentScene.name);
    }
}

