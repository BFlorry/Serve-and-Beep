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
    private Shader standardShader;
    private Material transparentMat;
    private AudioSource audio;


    //Methods-----------------------------------------------------------------------------------------

    private void Awake()
    {
        standardShader = Shader.Find("Standard");
        audio = this.GetComponentInParent<AudioSource>();
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
                    audio.PlayOneShot(audio.clip);

                    MeshRenderer[] rends = SetTransparentWithChildren(item);

                    foreach (MeshRenderer rend in rends)
                    {
                        Color startColor = rend.material.color;
                        Color destColor = new Color(startColor.r, startColor.g, startColor.b, 0);
                        StartCoroutine(LerpMeshRendererColor(rend, fadeOutTime, startColor, destColor));
                    }
                    StartCoroutine(DestroyAfterTime(item, fadeOutTime));
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
                rend.material.shader = standardShader;
                rend.material = ToFadeMode(rend.material);
                Color childColor = rend.material.color;
            }
        }

        MeshRenderer meshRend = obj.GetComponent<MeshRenderer>();

        if (meshRend != null)
        {
            rends.Add(meshRend);
            meshRend.material.shader = standardShader;
            meshRend.material = ToFadeMode(meshRend.material);
            Color childColor = meshRend.material.color;
        }
        return rends.ToArray();
    }


    /// <summary>
    /// Makes copy of given material and changes its render mode to Fade.
    /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/StandardShaderGUI.cs#L359
    /// </summary>
    /// <param name="mat">Material</param>
    /// <returns>Copy of given material with render mode changed to fade.</returns>
    private Material ToFadeMode(Material mat)
    {
        Material material = new Material(mat);
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        return material;
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

        Destroy(obj);
    }
}
