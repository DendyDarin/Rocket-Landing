using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float mainRotate = 10f;
    Rigidbody rb;
    AudioSource audios;

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
        if(Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if(!audios.isPlaying)
            {
                audios.Play();
            }
        }
        else
        {
            audios.Stop();
        }
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(mainRotate);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-mainRotate);
        }
    }

    void ApplyRotation(float rotateThisFrame)
    {
        rb.freezeRotation = true; // freeze rotation so can manually rotate
        transform.Rotate(Vector3.forward * rotateThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
