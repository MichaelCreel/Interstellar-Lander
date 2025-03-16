using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject buttons, settingsMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        settingsMenu.SetActive(true);
        buttons.SetActive(false);
        foreach (Transform child in settingsMenu.transform)
        {
            IUpdateVisual updateVisualComponent = child.GetComponent<IUpdateVisual>();
            if (updateVisualComponent != null)
            {
                updateVisualComponent.UpdateVisual();
            }
        }
    }
}
