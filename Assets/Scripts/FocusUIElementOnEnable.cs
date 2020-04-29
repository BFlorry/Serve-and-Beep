using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FocusUIElementOnEnable : MonoBehaviour
{
    [SerializeField]
    GameObject elementToFocus = null;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(elementToFocus);
    }
}
