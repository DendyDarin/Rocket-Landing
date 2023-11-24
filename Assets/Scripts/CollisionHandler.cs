using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in editor
    [SerializeField] float invokeDelay = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    // CACHE - e.g. references for readability or speed
    AudioSource audios;

    // STATE - private instances (member) variables
    bool isTransitioning = false;

    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision other) {
        if(isTransitioning) { return; }

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
        isTransitioning = true;
        audios.Stop(); // to stop other audio upon crash
        // TODO: add sfx
        audios.PlayOneShot(deathSound);
        // TODO: add particles when crashed
        GetComponent<Movement>().enabled = false; // so player can't move when crash
        Invoke("ReloadLevel", invokeDelay); // call method after delay x seconds
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audios.Stop();
        // TODO: add sfx
        audios.PlayOneShot(successSound);
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
