using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float invokeDelay = 1f;

    void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("it safe");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Isi bensin");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        // TODO: add sfx
        // TODO: add particles when crashed
        GetComponent<Movement>().enabled = false; // so player can't move when crash
        Invoke("ReloadLevel", invokeDelay); // call method after delay x seconds
    }

    void StartSuccessSequence()
    {
        // TODO: add sfx
        // TODO: add effect when success!
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", invokeDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
