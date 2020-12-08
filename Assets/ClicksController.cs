using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClicksController : MonoBehaviour
{
    [SerializeField] Text _display;
    uint _totalClicks;
    const string KEY = "ClicksController._totalClicks";

    void Start()
    {
        string clickString = PlayerPrefs.GetString(KEY, "0");

        _totalClicks = uint.Parse(clickString);
        _display.text = clickString;
    }

    public void Click()
    {
        ++_totalClicks;
        string toShow = _totalClicks.ToString();
        _display.text = toShow;
        PlayerPrefs.SetString(KEY, toShow);
    }

}
