using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public float launchDelay = 5f;
    public float fadeDuration = 1f;
    public AudioSource launchAudio;
    public CanvasGroup fadeCanvasGroup;

    GameObject presser;
    AudioSource sound;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0,0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.localPosition = new Vector3(0, 1, 2);
        sphere.AddComponent<Rigidbody>();
    }

    public void loadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void StartLaunchSequence()
    {
        if(launchAudio != null && fadeCanvasGroup != null)
        {
            StartCoroutine(LaunchSequenceCoroutine());
        }
        else
        {
            Debug.LogError("Launch Audio or Fade CanvasGroup not assigned in the inspector");
            loadNext();
        }
    }

    IEnumerator LaunchSequenceCoroutine()
    {
       

        // Play audio
        launchAudio.Play();

        //Wait for specified launch delay.
        yield return new WaitForSeconds(launchDelay);
        

        fadeCanvasGroup.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }


        loadNext();
    }

}
