using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PlayManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string difficulty;

    [SerializeField] private GameObject loadingText;

    public void OnPointerClick(PointerEventData eventData)
    {
        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Game.data"))
        {
            writer.WriteLine(difficulty);
            writer.Close();
        }
        loadingText.SetActive(true);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
