using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreIncrease : MonoBehaviour, IPoolObject
{
    [SerializeField] private  TMP_Text _text;
    [SerializeField] private Animator _anim;
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    public void Clean()
    {
    }
    public void startAnim(uint value)
    {
        _text.text = value.ToString();
        _anim.Play("ScoreUp", 0);
    }
    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.gameObject.SetActive(active);
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
       _text  = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
