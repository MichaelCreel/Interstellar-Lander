using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject gameUI, pauseUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<PlayerController>().ChangePauseState(false);
    }
}
