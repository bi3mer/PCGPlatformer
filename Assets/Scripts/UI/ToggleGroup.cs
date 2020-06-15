﻿using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class ToggleGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects = null;

    private Toggle toggle = null;

    private void Awake() 
    {
        toggle = GetComponent<Toggle>();

        Assert.IsNotNull(objects);
        Assert.IsNotNull(toggle);
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener(HandleValueChange);
    }

    private void HandleValueChange(bool val)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(val);
        }
    }
}