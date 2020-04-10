using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that is attached to a trigger area. Destroys pickupable item that is dropped in that area.
/// </summary>
public class ItemDestroyer : MonoBehaviour
{
    //Fields------------------------------------------------------------------------------------------

    [SerializeField]
    private float fadeOutTime = 2f;

    private float alpha;
    private Shader transparent;


    //Methods-----------------------------------------------------------------------------------------

    private void Awake()
    {
        transparent = Shader.Find("UI/Lit/Transparent");
    }


    /// <summary>
    /// Gets pickupable from parent.
    /// If found, destroys collided gameobject and children after fade out time.
    /// </summary>
    /// <param name="collider">Collider that enters area that this script is attached to.</param>
    private void OnTriggerStay(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            GameObject item = parent.gameObject;

            if (item.TryGetComponent(out Pickupable pickupable))
            {
                if (pickupable.Carried == false)
                {
                    pickupable.RemoveFromPlayer();
                    Destroy(pickupable);

                    MeshRenderer[] rends = SetTransparentWithChildren(item);

                    foreach (MeshRenderer rend in rends)
                    {
                        Color startColor = rend.material.color;
                        Color destColor = new Color(startColor.r, startColor.g, startColor.b, 0);
                        StartCoroutine(LerpMeshRendererColor(rend, fadeOutTime, startColor, destColor));
                        StartCoroutine(DestroyAfterTime(item, fadeOutTime));
                    }
                }
            }
        }
    }


    /// <summary>
    /// Finds all MeshRenderers in given object's children.
    /// Sets transparent renderer to their materials and to the given object.
    /// </summary>
    /// <param name="obj">Gameobject to set transparent.</param>
    /// <returns>Arry of found meshrenderers in children and given GameObject</returns>
    private MeshRenderer[] SetTransparentWithChildren(GameObject obj)
    {
        List<MeshRenderer> rends = new List<MeshRenderer>();

        foreach (Transform child in obj.transform)
        {
            MeshRenderer rend = child.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                rends.Add(rend);
                rend.material.shader = transparent;
                Color childColor = rend.material.color;
            }
        }
        MeshRenderer meshRend = obj.GetComponent<MeshRenderer>();

        if (meshRend != null)
        {
            rends.Add(meshRend);
            meshRend.material.shader = transparent;
            Color childColor = meshRend.material.color;
        }
        return rends.ToArray();
    }


    /// <summary>
    /// Lerps given MeshRenderer's color to to given color from given color in given time.
    /// </summary>
    /// <param name="meshRend">MeshRenderer</param>
    /// <param name="lerpDuration">Lerp deration</param>
    /// <param name="startColor">start color</param>
    /// <param name="destColor">destination color</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator LerpMeshRendererColor(MeshRenderer meshRend,
        float lerpDuration, Color startColor, Color destColor)
    {
        SetTransparentWithChildren(meshRend.gameObject);

        float lerpStartTime = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStartTime;
            if (meshRend != null)
            {
                meshRend.material.color = Color.Lerp(startColor, destColor, lerpProgress / lerpDuration);
            }
            else
            {
                lerping = false;
            }

            if (lerpProgress >= lerpDuration)
            {
                lerping = false;
            }
        }
        yield return true;
    }


    /// <summary>
    /// Destroys item and its children after given time.
    /// </summary>
    /// <param name="obj">GameObject to be destroyed.</param>
    /// <param name="time">time to be waited</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        foreach (Transform child in obj.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(obj);
    }
}
