using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class RouletteUI : MonoBehaviour
{
   public enum Result {Crit, Success, Fail}

   [SerializeField] private Rigidbody2D _needleRb;
   [SerializeField] private Image _critImg, _successImg;
   [SerializeField] private Button _startButton, _stopButton;

   private CanvasGroup _group;
   private float _critAngle, _successAngle;
   private Action<Result> _onResolved;


   private void Start()
   {
      _group = GetComponent<CanvasGroup>();
      _group.alpha = 0;
      _group.blocksRaycasts = false;
   }
   
   
   public void Display(RouletteEvent rouletteEvent, Action<Result> onResolved)
   {
      _onResolved = onResolved;
      
      _group.alpha = 1;
      _group.blocksRaycasts = true;
      _startButton.enabled = true;
      _stopButton.enabled = false;

      _critImg.fillAmount = rouletteEvent.CritChance;
      _critAngle = rouletteEvent.CritChance * 360f;
      _successImg.fillAmount = rouletteEvent.SucessChance + rouletteEvent.CritChance;
      _successAngle = _successImg.fillAmount * 360f;
   }

   public void StartSpin()
   {
        AudioManager.Instance.IniciaRuleta();

        _startButton.enabled = false;
      _stopButton.enabled = true;
      _needleRb.angularVelocity = -GameManager.Instance.GameInfo.RouletteSpinSpeed;
   }

   public void StopSpin()
   {
      _stopButton.enabled = false;
      _needleRb.angularVelocity = 0f;
      var angle = Mathf.Abs(_needleRb.rotation % 360);
      var result = Result.Fail;
      if (angle <= _critAngle) result = Result.Crit;
      else if (angle <= _successAngle) result = Result.Success;
        AudioManager.Instance.FinalizaRuleta(result);

        StartCoroutine(WaitAndHide(result));
   }

   private IEnumerator WaitAndHide(Result r)
   {
      yield return new WaitForSeconds(GameManager.Instance.GameInfo.RouletteHideDelay);
      _group.alpha = 0;
      _group.blocksRaycasts = false;
      _onResolved(r);
   }
   
}
