using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrengh = 100f;
    [SerializeField] float rotationStrengh = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem rightEngineParticle;
    [SerializeField] ParticleSystem leftEngineParticle;

    Rigidbody rigidbody;
    AudioSource audioSource;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust() {
        if(thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }


    private void StartThrusting()
    {
        rigidbody.AddRelativeForce(Vector3.up * thrustStrengh * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    private void ProcessRotation() {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }


    private void RotateRight()
    {
        ApplyRotation(rotationStrengh);
        if (!rightEngineParticle.isPlaying)
        {
            leftEngineParticle.Stop();
            rightEngineParticle.Play();
        }
    }

     private void RotateLeft()
    {
        ApplyRotation(-rotationStrengh);
        if (!leftEngineParticle.isPlaying)
        {
            rightEngineParticle.Stop();
            leftEngineParticle.Play();
        }
    }

    private void StopRotating()
    {
        leftEngineParticle.Stop();
        rightEngineParticle.Stop();
    }

    private void ApplyRotation (float rotationThisFrame) {
        rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rigidbody.freezeRotation = false;
    }
}
