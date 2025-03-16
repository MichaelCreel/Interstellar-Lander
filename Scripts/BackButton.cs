using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject closeMenu, openMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        closeMenu.SetActive(false);
        openMenu.SetActive(true);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.HotloadKeybinds();
        }

        Radio radio = FindObjectOfType<Radio>();
        if (radio != null)
        {
            radio.UpdateVolume();
        }
    }
}
