using System;
using System.Collections;
using System.Collections.Generic;
using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

public class DrawUI : MonoBehaviour
{
    [SerializeField] private Button[] _colors;
    [SerializeField] private GameObject _draw;

    private CanvasGroup _group;
    private Action _callback;
    
    private void Start()
    {
        _group = GetComponent<CanvasGroup>();
        _group.alpha = 0;
        _group.blocksRaycasts = false;
        _draw.SetActive(false);
        foreach (Button b in _colors)
        {
            var c = b.GetComponent<Image>().color;
            b.onClick.AddListener(() => Drawable.Pen_Colour = c);
        }
        Drawable.Pen_Colour = Color.white;
        Drawable.Pen_Width = 0;
        FindAnyObjectByType<Drawable>(FindObjectsInactive.Include).ResetCanvas();
    }

    public void Display(Action callback)
    {
        _group.alpha = 1;
        _group.blocksRaycasts = true;
        _draw.SetActive(true);
        _callback = callback;
    }
    public void SetThickness(float t) => Drawable.Pen_Width = Mathf.RoundToInt(t);

    public void DrawDone()
    {
        _group.alpha = 0;
        _group.blocksRaycasts = false;
        _draw.SetActive(false);
        _callback();
        AudioManager.Instance.PlayClick1();

    }

}
