using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISetSelected : MonoBehaviour
{
    [SerializeField]
    GameObject UIElementToSelect = null;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (UIElementToSelect != null)
            EventSystem.current.SetSelectedGameObject(UIElementToSelect);
    }
    public void SetSelected(GameObject UIelement)
    {
        EventSystem.current.SetSelectedGameObject(UIelement);
    }
}
