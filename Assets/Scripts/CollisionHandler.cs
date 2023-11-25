using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in editor
    [SerializeField] float invokeDelay = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    // CACHE - e.g. references for readability or speed
    AudioSource audios;

    // STATE - private instances (member) variables
    bool isTransitioning = false;
    bool collisionDisabled = false; // optional: add debug key to disable collision

    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    // Add Cheat/DebugKey
    void Update()
    {
        RespondToDebugKey();
    }

    // Optional: add debug key to load next level when press L
    void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C)) // condition for debug key
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(isTransitioning || collisionDisabled) { return; } // add debug key condition

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
        crashParticles.Play();
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
        successParticles.Play();
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
