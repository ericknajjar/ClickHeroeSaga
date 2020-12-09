using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClicksController : MonoBehaviour
{
    [SerializeField] Text _display;
    public static uint _totalClicks;
    public static string _userId;
    public static string _accessToken;


    void Start()
    {
    
        _display.text = _totalClicks.ToString();
    }

    public void Click()
    {
        ++_totalClicks;
        string toShow = _totalClicks.ToString();
        _display.text = toShow;
        StartCoroutine(SendUpdatedClicks());
   
    }

    IEnumerator SendUpdatedClicks()
    {
        var data = new Dictionary<string, object>
        {
            { "user_id", _userId },
            { "token",_accessToken},
            { "clicks",_totalClicks},
        };

        string toSend = JsonConvert.SerializeObject(data);
        byte[] bytesToSend = Encoding.UTF8.GetBytes(toSend);

        using (UnityWebRequest www = new UnityWebRequest("http://localhost:5000/set_clicks", UnityWebRequest.kHttpVerbPOST))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(bytesToSend);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            var text = www.downloadHandler.text;
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else if (www.responseCode != 200)
            {
                Debug.LogError($"{www.responseCode}: {text}");
            }
         
        }
    }

}
