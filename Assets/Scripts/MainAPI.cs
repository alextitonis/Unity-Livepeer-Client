using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//https://livepeer.studio/blog/api-calls-postman-livepeer-studio
//https://livepeer.studio/blog/streaming-with-rtmp-api

public class MainAPI : MonoBehaviour
{
    public static MainAPI getInstance;
    void Awake() { getInstance = this; }

    [SerializeField] string apiKey = "0e742ec3-5e7c-4206-8c1a-f48f3fb4073c";

    public delegate void StreamCallback(StreamResponse resp);
    public delegate void StreamListCallback(List<StreamResponse> resp);

    #region Functions
    public void CreateStream(CreateStreamData data, StreamCallback callback)
    {
        StartCoroutine(CreateStreamRoutine(data, callback));
    }
    public void GetStream(string streamId, StreamCallback callback)
    {
        StartCoroutine(GetStreamRoutine(streamId, callback));
    }
    public void GetStreamsList(StreamListCallback callback)
    {
        StartCoroutine(GetStreamsListRoutine(callback));
    }
    public void UpdateStreamData(string streamId, UpdateStreamDataObj data)
    {
        StartCoroutine(UpdateStreamDataResponse(streamId, data));
    }
    public void DeleteStream(string streamId)
    {
        StartCoroutine(DeleteStreamRoutine(streamId));
    }
    #endregion

    #region Routines
    private IEnumerator CreateStreamRoutine(CreateStreamData data, StreamCallback callback)
    {
        var web = new UnityWebRequest(Constants.STREAM_URL, "POST");
        string b = JsonConvert.SerializeObject(data);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError
           || web.responseCode != 201)
        {
            Debug.LogError(web.error + " - " + web.responseCode);
        }
        else
        {
            string resp = web.downloadHandler.text;
            StreamResponse respObj = JsonConvert.DeserializeObject<StreamResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator GetStreamRoutine(string streamId, StreamCallback callback)
    {
        string url = Constants.STREAM_URL + "/" + streamId;
        var web = new UnityWebRequest(url, "GET");

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError
           || web.responseCode != 200)
        {
            Debug.LogError(web.error + " - " + web.responseCode);
        }
        else
        {
            string resp = web.downloadHandler.text;
            StreamResponse respObj = JsonConvert.DeserializeObject<StreamResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator GetStreamsListRoutine(StreamListCallback callback)
    {
        string url = Constants.STREAM_URL + "/?streamsonly=1";
        var web = new UnityWebRequest(url, "GET");

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError
           || web.responseCode != 200)
        {
            Debug.LogError(web.error + " - " + web.responseCode);
        }
        else
        {
            string resp = web.downloadHandler.text;
            List<StreamResponse> respObj = JsonConvert.DeserializeObject<List<StreamResponse>>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator UpdateStreamDataResponse(string streamId, UpdateStreamDataObj data)
    {
        string b = JsonConvert.SerializeObject(data);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        string url = Constants.STREAM_URL + "/" + streamId + "/record";
        var web = new UnityWebRequest(url, "PATCH");

        web.downloadHandler = new DownloadHandlerBuffer();
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError
           || web.responseCode != 204)
        {
            Debug.LogError(web.error + " - " + web.responseCode);
        }
        web.Dispose();
    }

    private IEnumerator DeleteStreamRoutine(string streamId)
    {
        string url = Constants.STREAM_URL + "/" + streamId;
        var web = new UnityWebRequest(url, "DELETE");

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError
           || web.responseCode != 204)
        {
            Debug.LogError(web.error + " - " + web.responseCode);
        }

        web.Dispose();
    }
    #endregion
}