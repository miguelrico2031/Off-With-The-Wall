using System;
using UnityEngine;

public class ServicesBootstrapper : MonoBehaviour
{
    //Inicializa los managers antes que cualquier otro Awake se llame (esta configurado en los project settings asi)
    public void Bootstrap()
    {
        GameManager.Instance.Register<IPeopleService>(new PeopleManager());
        GameManager.Instance.Register<IBuildingService>(new BuildingManager());
        GameManager.Instance.Register<IHUDService>(FindObjectOfType<HUDManager>());
        GameManager.Instance.Register<IAIService>(FindObjectOfType<AIManager>());
        GameManager.Instance.Register<IPopUpService>(FindObjectOfType<PopUpManager>());
        GameManager.Instance.Register<IDialogueService>(FindObjectOfType<DialogueManager>());
        GameManager.Instance.Register<IEventSpawnService>(FindObjectOfType<EventSpawner>());
        GameManager.Instance.Register<IEventService>(FindObjectOfType<EventManager>());
        GameManager.Instance.Register<IAudioService>(FindObjectOfType<AudioManager>());
        GameManager.Instance.Register<IStartLoseUIService>(FindObjectOfType<StartLoseUIManager>());
        GameManager.Instance.Register<IMultUIService>(FindObjectOfType<MultUIManager>());
        GameManager.Instance.Register<IScoreIncreaseUIService>(FindObjectOfType<ScoreIncreaseUIManager>());


    }
}
