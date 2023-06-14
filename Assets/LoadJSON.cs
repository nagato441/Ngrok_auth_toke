using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;


public class LoadJSON : MonoBehaviour
{
    [Serializable]
    public class wayspotAnchorsData
    {
        public string[] Payloads;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetJSON());
    }

    IEnumerator GetJSON()
    {
        string awsUrl = "https://whtls.s3.ap-northeast-2.amazonwas.com/JSON/AnchorJSON.json";
        UnityWebRequest request = UnityWebRequest.Get(awsUrl);

        yield return request.SendWebRequest();
        while(!request.isDone)
        {
            yield return null;
        }
        if(UnityWebRequest.Result.ConnectionError == request.result)
        {
            // add "Not connected" error message

        } else
        {
            if(request.isDone)
            {
                wayspotAnchorsData data = JsonUtility.FromJson<wayspotAnchorsData>(request.downloadHandler.text);
                string savePath = System.IO.Path.Combine(Application.persistentDataPath, "AnchortJSON.json");
                yield return new WaitForSeconds(1);
            }
        }
    }
}
