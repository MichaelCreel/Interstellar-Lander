using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject buttons, playMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        playMenu.SetActive(true);
        buttons.SetActive(false);
        FindObjectOfType<HighScoreLoader>().LoadHighscores();
    }
}
