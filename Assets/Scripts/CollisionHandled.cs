using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandled : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip collisionBreak;
    [SerializeField] AudioClip colissionFinish;
    [SerializeField] ParticleSystem sucessParticle;
    [SerializeField] ParticleSystem crashParticle;

    AudioSource audioSource;

    bool isControlled = true;
    bool isColidder = true;

    private void Start() {
        audioSource = GetComponent<AudioSource>();    
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame) {
            NextLevel();
        } else if (Keyboard.current.cKey.wasPressedThisFrame) {
            isColidder = false;
        }
    }

    private void OnCollisionEnter(Collision other) {

        if (!isControlled || !isColidder) { return; }

        switch (other.gameObject.tag) 
        {
            case "Friendly":
                Debug.Log("Opa!");
                break;
            case "Finish":
                StartNextLevel();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence() {
        isControlled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(collisionBreak);
        crashParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void StartNextLevel() {
        isControlled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(colissionFinish);
        sucessParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("NextLevel", levelLoadDelay);
    }

    private void ReloadLevel() {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }
    
    private void NextLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if(nextScene == SceneManager.sceneCountInBuildSettings) {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

}
