using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationCaller : MonoBehaviour
{
    public void startText()
    {
        print("kk");

        GameManager.Instance.Get<IDialogueService>().TypePhraseAnim();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}