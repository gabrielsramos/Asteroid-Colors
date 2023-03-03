using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [HideInInspector]
    public Action<Color> OnColorSelected;

    [SerializeField] private List<ChangeColorButton> _changeColorButtons;
    [SerializeField] private Heart[] _hearts;
    [SerializeField] private GameObject _victoryPopup;
    [SerializeField] private GameObject _defeatPopup;
    [SerializeField] private TextMeshProUGUI _levelText;

    private Color _selectedColor;
    private int _livesCounter;

    private void Awake()
    {
        foreach (ChangeColorButton button in _changeColorButtons)
        {
            button.OnButtonClicked += ChangeColor;
        }
        _livesCounter = _hearts.Length;
    }

    private void Start()
    {
        ChangeColor(_changeColorButtons[0]);
    }

    private void OnDestroy()
    {
        foreach (ChangeColorButton button in _changeColorButtons)
        {
            button.OnButtonClicked -= ChangeColor;
        }
    }

    private void ChangeColor(ChangeColorButton button)
    {
        _selectedColor = button.GetColor();
        DeselectAllButtons();
        button.SetSelectedState(true);
        OnColorSelected?.Invoke(_selectedColor);
    }

    private void DeselectAllButtons()
    {
        foreach (ChangeColorButton button in _changeColorButtons)
        {
            button.SetSelectedState(false);
        }
    }

    public void SetLevel(int level)
    {
        _levelText.text = $"Level {level}";
    }

    public void LoseLive()
    {
        _hearts[_livesCounter - 1].SetFilledState(false);
        _livesCounter--;
    }

    public void ShowVictoryPopup()
    {
        _victoryPopup.SetActive(true);
    }

    public void ShowDefeatPopup()
    {
        _defeatPopup.SetActive(true);
    }
}
