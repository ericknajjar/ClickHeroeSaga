using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClicksController : MonoBehaviour
{
    [SerializeField] Text _display;
    uint _totalClicks;

    void Start()
    {
        
    }

    public void Click()
    {
        ++_totalClicks;
        _display.text = _totalClicks.ToString();
    }

}
