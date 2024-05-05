using System;
using System.Collections;
using System.Collections.Generic;
using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

public class DrawManager : MonoBehaviour
{
    [SerializeField] private Button[] _colors;


    private void Start()
    {
        foreach (Button b in _colors)
        {
            var c = b.GetComponent<Image>().color;
            b.onClick.AddListener(() => Drawable.Pen_Colour = c);
        }
        Drawable.Pen_Colour = Color.white;
        Drawable.Pen_Width = 0;
    }

    public void SetThickness(float t) => Drawable.Pen_Width = Mathf.RoundToInt(t);
}
