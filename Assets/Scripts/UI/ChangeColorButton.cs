using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorButton : MonoBehaviour
{
    [HideInInspector]
    public Action<ChangeColorButton> OnButtonClicked;

    [SerializeField] private Color _color;
    [SerializeField] private GameObject _selectedOverlay;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        OnButtonClicked?.Invoke(this);
    }

    public void SetSelectedState(bool state)
    {
        _selectedOverlay.SetActive(state);
    }

    public Color GetColor()
    {
        return _color;
    }
}
