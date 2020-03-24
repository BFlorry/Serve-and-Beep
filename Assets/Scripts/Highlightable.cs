﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private float outlineWidth = 0.05f;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        UnHighlight();
    }

    public void Highlight()
    {
        mat.SetFloat("_FirstOutlineWidth", outlineWidth);
    }

    public void UnHighlight()
    {
        mat.SetFloat("_FirstOutlineWidth", 0f);
    }

}