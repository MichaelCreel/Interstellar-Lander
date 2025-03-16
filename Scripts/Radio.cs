using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Radio : MonoBehaviour
{
    [SerializeField] private AudioClip[] music;

    [SerializeField] private AudioSource source;

    [SerializeField] private TMP_Text info;
    // Start is called before the first frame update
    void Start()
    {
        info.enableVertexGradient = true;
        StartCoroutine(VolumeUpdateWait());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopCoroutine(MusicWait());
            source.Stop();
            SelectMusic();
        }
    }

    private void SelectMusic()
    {
        int selected = UnityEngine.Random.Range(0, music.Length);
        source.clip = music[selected];
        info.text = source.clip.name;
        StartCoroutine(MusicWait());
    }

    private IEnumerator MusicWait()
    {
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        source.Stop();
        SelectMusic();
    }

    public void UpdateVolume()
    {
        source.volume = FindObjectOfType<Settings>().volume / 100f;
    }

    private IEnumerator VolumeUpdateWait() {
        yield return new WaitForSeconds(0.1f);
        UpdateVolume();
        SelectMusic();
    }
}
