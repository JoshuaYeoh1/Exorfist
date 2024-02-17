using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void RestartScene()
    {
        //Get the current active scene's index
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        //Reload the current scene
        SceneManager.LoadScene(sceneIndex);
    }
}
