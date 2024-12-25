using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneChanger : MonoBehaviour
{
    // Method to load a scene by index
    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
