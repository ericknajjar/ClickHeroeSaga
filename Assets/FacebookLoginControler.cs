using Facebook.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FacebookLoginControler : MonoBehaviour
{
    bool _facebookInited = false;

    private void Start()
    {
        FB.Init(() => {
            if(FB.IsInitialized)
            {
                FB.ActivateApp();
                _facebookInited = true;
            }
            
        });
    }

    public void OnLogin()
    {
        FB.LogInWithPublishPermissions(null,(result) => 
        {
            if (FB.IsLoggedIn)
            {
               
                var aToken = AccessToken.CurrentAccessToken;
                Debug.Log(aToken.UserId);
          
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
            }
            else
            {
                Debug.Log("User cancelled login");
            }

        });

    }

    /* IEnumerator LoginCorrotine()
     {
         var data = new Dictionary<string, string>
         {
             { "email", _email.text },
             { "password",_password.text},
         };

         string toSend = JsonConvert.SerializeObject(data);
         byte[] bytesToSend = Encoding.UTF8.GetBytes(toSend);

         Debug.Log($"Sending... {toSend}");
         using (UnityWebRequest www = new UnityWebRequest("http://localhost:5000/pior_login_do_mundo", UnityWebRequest.kHttpVerbPOST))
         {
             www.SetRequestHeader("Content-Type", "application/json");
             www.uploadHandler = new UploadHandlerRaw(bytesToSend);
             www.downloadHandler = new DownloadHandlerBuffer();

             yield return www.SendWebRequest();

             if (www.isNetworkError || www.isHttpError)
             {
                 Debug.Log(www.error);
             }
             else
             {
                 var text = www.downloadHandler.text;
                 if (www.responseCode == 200)
                 {
                     var result = JObject.Parse(text);

                     ClicksController._userId = result["user_id"].Value<string>();
                     ClicksController._accessToken = result["token"].Value<string>();
                     ClicksController._totalClicks = result["clicks"].Value<uint>();
                     Debug.Log(result);

                     SceneManager.LoadScene(1);


                 }
                 else
                 {
                     Debug.LogError($"{www.responseCode}: {text}");
                 }
             }
         }
     }*/
}
