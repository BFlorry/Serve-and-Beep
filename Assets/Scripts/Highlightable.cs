using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private float outlineWidth = 0.05f;

    Material[] mats;

    void Start()
    {
        if(TryGetComponent(out Renderer rend))
        {
            mats = new Material[1];
            mats[0] = rend.material;
        }
        if (mats == null)
        {
            List<Material> materialList = new List<Material>();
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            mats = new Material[rends.Length];
            for(int i = 0; i < rends.Length; i++)
            {
                for (int u = 0; u < rends[i].materials.Length; u++)
                {
                    materialList.Add(rends[i].materials[u]);
                }
            }
            mats = materialList.ToArray();
        }
        
        UnHighlight();
    }

    public void Highlight()
    {
        foreach (Material mat in mats)
        {
            if (mat.HasProperty("_FirstOutlineWidth"))
                mat.SetFloat("_FirstOutlineWidth", outlineWidth);
        }
    }

    public void UnHighlight()
    {
        foreach(Material mat in mats)
        {
            if(mat.HasProperty("_FirstOutlineWidth"))
                mat.SetFloat("_FirstOutlineWidth", 0f);
        }
    }
}
