using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioClip[] levelMusic = new AudioClip[4];
    AudioSource source;

    int musicChoice;

	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
        musicChoice = Random.Range(0, 3);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(levelMusic[musicChoice]);
            source.loop = true;
        }
    }
}
