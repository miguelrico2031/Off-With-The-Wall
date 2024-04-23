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
    }
}
