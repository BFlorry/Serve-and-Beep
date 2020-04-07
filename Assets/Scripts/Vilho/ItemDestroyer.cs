using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 2f;

    private float alpha;

    private void OnTriggerEnter(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            GameObject item = parent.gameObject;

            Pickupable pickupable = item.GetComponent<Pickupable>();

            if (pickupable != null)
            {
                if (pickupable.Carried == false)
                {
                    pickupable.Carried = true;
                    MeshRenderer meshRend = collider.gameObject.GetComponent<MeshRenderer>();
                    Color startColor = meshRend.material.color;
                    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
                    alpha = startColor.a;
                    StartCoroutine(DestroyAfterTime(fadeOutTime));
                    //StartCoroutine(FadeOut(meshRend, fadeOutTime, startColor, endColor));
                }
            }
        }
    }


    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }


    //Not in use yet.
    private IEnumerator FadeOut(MeshRenderer target_MeshRender,
        float lerpDuration, Color startLerp, Color targetLerp)
    {
        while (target_MeshRender.material.color.a >= 0)
        {
            target_MeshRender.material.color = new Color(startLerp.r, startLerp.g, startLerp.b, this.alpha);
            this.alpha = this.alpha - 0.1f;
            yield return null;
        }
        Destroy(this.gameObject);
    }


    //Not in use yet.
    private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender,
        float lerpDuration, Color startLerp, Color targetLerp)
    {
        float lerpStart_Time = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStart_Time;
            if (target_MeshRender != null)
            {
                target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
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
        Destroy(this.gameObject);
        yield break;
    }
}