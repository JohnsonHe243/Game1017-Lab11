using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_Text timertext;
    public static Game Instance { get; private set; } // Static object of the class.
    public SoundManager SOMA;

    private float startTime; // We're going to use Time.time;

    private void Awake() // Ensure there is only one instance.
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Will persist between scenes.
            Initialize();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances.
        }
    }

    private void Initialize()
    {
        SOMA = new SoundManager();
        SOMA.Initialize(gameObject);
        SOMA.AddSound("Jump", Resources.Load<AudioClip>("jump"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Roll", Resources.Load<AudioClip>("roll"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("StillDre", Resources.Load<AudioClip>("StillDre"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("I_Ran", Resources.Load<AudioClip>("I_Ran"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("I_Ran");

        startTime = Time.time; // Gives us a number of seconds.
        StartCoroutine("UpdateTimer"); // You'll need a way to stop the timer.
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            float elapsedTime = Time.time - startTime;
            timertext.text = "Time: " + elapsedTime.ToString("F3") + "s";
            yield return null;
        }
    }
}
