using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInitializer : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPrefab;


    private void Awake()
    {
        var buildingSprites = GetComponentsInChildren<SpriteRenderer>();
        print(buildingSprites.Length);
        foreach (var sprite in buildingSprites)
        {
               var building = 
                    Instantiate(_buildingPrefab, sprite.transform.position, Quaternion.identity, transform);
                building.GetComponentInChildren<SpriteRenderer>().sprite = sprite.sprite;
            sprite.gameObject.SetActive(false);
       }
    }
}
