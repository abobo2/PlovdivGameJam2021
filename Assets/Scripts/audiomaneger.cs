﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audiomaneger : MonoBehaviour
{
    public sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Start()
    {
        Play("prototype3.0");
    }


    public void Play(string name)
    {
       
    }
}
