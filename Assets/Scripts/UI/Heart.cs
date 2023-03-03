using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private GameObject _fill;

    public void SetFilledState(bool state)
    {
        _fill.SetActive(state);
    }
}
