using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in editor
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float mainRotate = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    // CACHE - e.g. references for readability or speed
    Rigidbody rb;
    AudioSource audios;

    // STATE - private instances (member) variables

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audios.isPlaying)
        {
            audios.PlayOneShot(mainEngine);
        }
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    void StopThrusting()
    {
        audios.Stop();
        mainBooster.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(mainRotate);
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-mainRotate);
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    void StopRotating()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }

    void ApplyRotation(float rotateThisFrame)
    {
        rb.freezeRotation = true; // freeze rotation so can manually rotate
        transform.Rotate(Vector3.forward * rotateThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
