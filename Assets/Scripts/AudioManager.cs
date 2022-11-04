using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance ;

    [SerializeField]
    private AudioClip deathSound,eatSound;
    void Start()
    {
        createInstance();
    }

    void createInstance()
    {
        if (!Instance)
            Instance = this;
    }

    // Update is called once per frame
    public void playSound(string soundName)
    {
        if (soundName == "death")
            AudioSource.PlayClipAtPoint(deathSound, new Vector3(0f, 0f, 0f));
        if (soundName == "eat")
            AudioSource.PlayClipAtPoint(eatSound, new Vector3(0f, 0f, 0f));
    }
}
