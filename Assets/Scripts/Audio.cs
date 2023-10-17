using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private bool soundEnabled = true;
    private AudioSource[] allAudioSources;
    
    void Start()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        foreach (var audioSource in allAudioSources)
        {
            audioSource.enabled = soundEnabled;
        }
    }
}
