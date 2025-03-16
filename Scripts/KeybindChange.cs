using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public interface IUpdateVisual
{
    void UpdateVisual();
}

public class KeybindChange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IUpdateVisual
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int bindNumber;
    [SerializeField] private string buttonString;
    private bool downed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator WaitForPress()
    {
        text.text = buttonString + ":\nWaiting...";
        bool keyAssigned = false;
        while (!keyAssigned)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keyCode))
                    {
                        FindObjectOfType<Settings>().keybinds[bindNumber] = keyCode;
                        FindObjectOfType<Settings>().SaveKeybinds();
                        text.text = buttonString + ":\n" + keyCode.ToString();
                        keyAssigned = true;
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (downed)
        {
            StartCoroutine(WaitForPress());
        }
    }

    public void UpdateVisual()
    {
        text.text = buttonString + ":\n" + FindObjectOfType<Settings>().keybinds[bindNumber].ToString();
    }
}