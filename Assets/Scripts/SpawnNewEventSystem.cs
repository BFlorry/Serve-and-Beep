using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnNewEventSystem : MonoBehaviour
{
    [SerializeField]
    GameObject eventSystem;

    [SerializeField]
    GameObject UIElementToSelect = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        SpawnEventSystem();
    }

    public void SpawnEventSystem()
    {
        Debug.Log("Spawning eventsystem");
        GameObject es = Instantiate(eventSystem);
        Debug.Log("Spawned eventsystem: " + es);
        es.GetComponent<EventSystem>().firstSelectedGameObject = UIElementToSelect;
        es.GetComponent<UISetSelected>().SetSelected(UIElementToSelect);
        es.SetActive(false);
        es.SetActive(true);
    }
}
