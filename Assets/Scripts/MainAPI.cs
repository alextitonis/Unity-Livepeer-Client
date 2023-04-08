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
    public void GetStreamsList(StreamListCallback callback)
    {
        StartCoroutine(GetStreamsListRoutine(callback));
    }
    public void UpdateStreamData(string streamId, UpdateStreamDataObj data)
    {
        StartCoroutine(UpdateStreamDataResponse(streamId, data));
    }

    public void CreateAsset(string name, System.Action<CreateAssetResponse> callback)
    {
        StartCoroutine(CreateAssetRoutine(name, callback));
    }

    public void CreateAssetViaUrl(string url, string name, System.Action<CreateAssetResponse> callback)
    {
        StartCoroutine(CreateAssetViaURLRoutine(url, name, callback));
    }

    public void CreateSigningKey(System.Action<SigningKeyResponse> callback)
    {
        StartCoroutine(CreateSigningKeyRoutine(callback));
    }

    public void CreateMultiStream(string name, string url, System.Action<MultistreamTargetResponse> callback)
    {
        StartCoroutine(CreateMultiStreamTargetRoutine(name, url, callback));
    }

    public void CreateWebhook(WebhookRequestData data, System.Action<WebhookResponse> callback)
    {
        StartCoroutine(CreateWebhookRoutine(data, callback));
    }

    public void DeleteStream(string streamId)
    {
        StartCoroutine(DeleteRequest("stream/", streamId));
    }
    public void DeleteAsset(string assetId)
    {
        StartCoroutine(DeleteRequest("asset/", assetId));
    }
    public void DeleteMultiStream(string streamId)
    {
        StartCoroutine(DeleteRequest("multistream/target/", streamId));
    }
    public void DeleteSigningKey(string keyId)
    {
        StartCoroutine(DeleteRequest("access-control/signing-key/", keyId));
    }

    public void GetStream(string streamId, System.Action<StreamResponse> callback)
    {
        StartCoroutine(RetrieveRequest("stream/", streamId, callback));
    }
    public void GetAsset(string assetId, System.Action<CreateAssetResponse> callback)
    {
        StartCoroutine(RetrieveRequest("asset/", assetId, callback));
    }
    public void GetMultiStream(string streamId, System.Action<MultistreamTargetResponse> callback)
    {
        StartCoroutine(RetrieveRequest("multistream/target/", streamId, callback));
    }
    public void GetMultistreamTargets(System.Action<List<MultistreamTargetResponse>> callback)
    {
        StartCoroutine(RetrieveRequest("multistream/target", "", callback));
    }
    public void GetPlaybackInfo(string playbackId, System.Action<PlaybackInfoResponse> callback)
    {
        StartCoroutine(RetrieveRequest("/playback/", playbackId, callback));
    }
    public void GetRecordedSessions(string parentId, int record, System.Action<List<RecordedSessionsResponse>> callback)
    {
        StartCoroutine(RetrieveRequest("stream/", parentId + "/sessions?record=" + record, callback));
    }
    public void GetRecordedSession(string sessionId, System.Action<RecordedSessionsResponse> callback)
    {
        StartCoroutine(RetrieveRequest("session/", sessionId, callback));
    }
    public void GetSigningKeys(System.Action<SigningKeyResponse> callback)
    {
        StartCoroutine(RetrieveRequest("access-control/signing-key", "", callback));
    }
    public void GetTask(string taskId, System.Action<TaskResponse> callback)
    {
        StartCoroutine(RetrieveRequest("task/", taskId, callback));
    }
    public void GetTasks(System.Action<List<TaskResponse>> callback)
    {
        StartCoroutine(RetrieveRequest("task", "", callback));
    }
    public void GetWebHook(string webhookId, System.Action<List<WebhookResponse>> callback)
    {
        StartCoroutine(RetrieveRequest("webhook/", webhookId, callback));
    }

    public void UpdateWebhook(string id, WebhookRequestData data, System.Action<bool> callback)
    {
        StartCoroutine(UpdateWebhookRoutine(id, data, callback));
    }
    public void PatchRecording(string id, bool recording, System.Action<bool> callback)
    {
        StartCoroutine(PatchRecordingRoutine(id, recording, callback));
    }
    public void UpdateMultistreamTarget(string id, string name, string mUrl, bool disabled, System.Action<bool> callback)
    {
        StartCoroutine(UpdateMultistreamTargetRoutine(id, name, mUrl, disabled, callback));
    }
    public void UpdateSigningKey(string id, string name, bool disabled, System.Action<bool> callback)
    {
        StartCoroutine(UpdateSigningKeyRoutine(id, name, disabled, callback));
    }
    public void UpdateAsset(string id, string name, string storageJson, System.Action<bool> callback)
    {
        StartCoroutine(UpdateAssetRoutine(id, name, storageJson, callback));
    }

    public void TranscodeVideo(TranscodeVideoData data, System.Action<TranscodeVideoResponse> callback)
    {
        StartCoroutine(TranscodeVideoRoutine(data, callback));
    }
    #endregion

    #region Routines
    private IEnumerator CreateStreamRoutine(CreateStreamData data, StreamCallback callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "stream", "POST");
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

    private IEnumerator GetStreamsListRoutine(StreamListCallback callback)
    {
        string url = Constants.API_URL + "stream" + "/?streamsonly=1";
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
        string url = Constants.API_URL + "stream" + "/" + streamId + "/record";
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

    private IEnumerator CreateAssetRoutine(string name, System.Action<CreateAssetResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "asset/request-upload", "POST");
        string b = "{\"name\": \"" + name + "\"}";

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
            CreateAssetResponse respObj = JsonConvert.DeserializeObject<CreateAssetResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator CreateAssetViaURLRoutine(string url, string name, System.Action<CreateAssetResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "asset/request-upload", "POST");
        string b = "{\"url\": \"" + url + "\", \"name\": \"" + name + "\"}";

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
            CreateAssetResponse respObj = JsonConvert.DeserializeObject<CreateAssetResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator CreateSigningKeyRoutine(System.Action<SigningKeyResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "access-control/signing-key", "POST");
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
            SigningKeyResponse respObj = JsonConvert.DeserializeObject<SigningKeyResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    public IEnumerator CreateMultiStreamTargetRoutine(string name, string url, System.Action<MultistreamTargetResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "multistream/target", "POST");
        string b = "{\"url\": \"" + url + "\", \"name\": \"" + name + "\"}";

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
            MultistreamTargetResponse respObj = JsonConvert.DeserializeObject<MultistreamTargetResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    public IEnumerator CreateWebhookRoutine(WebhookRequestData data, System.Action<WebhookResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "multistream/target", "POST");
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
            WebhookResponse respObj = JsonConvert.DeserializeObject<WebhookResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator DeleteRequest(string api, string id)
    {
        string url = Constants.API_URL + api + id;
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

    private IEnumerator RetrieveRequest<T>(string api, string id, System.Action<T> callback)
    {
        string url = Constants.API_URL + api + id;
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
            var respObj = JsonConvert.DeserializeObject<T>(resp);
            callback(respObj);
        }
        web.Dispose();
    }

    private IEnumerator UpdateWebhookRoutine(string id, WebhookRequestData data, System.Action<bool> callback)
    {
        string url = Constants.API_URL + "webhook/" + id;
        var web = new UnityWebRequest(url, "PATCH");
        string b = JsonConvert.SerializeObject(data);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            callback(true);
        }
        else
        {
            callback(false);
            Debug.Log(web.error);
        }

        web.Dispose();
    }

    private IEnumerator PatchRecordingRoutine(string id, bool recording, System.Action<bool> callback)
    {
        string url = Constants.API_URL + $"stream/{id}/record";
        var web = new UnityWebRequest(url, "PATCH");
        string b = "{\"record\": " + recording + "}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            callback(true);
        }
        else
        {
            callback(false);
            Debug.Log(web.error);
        }

        web.Dispose();
    }

    private IEnumerator UpdateMultistreamTargetRoutine(string id, string name, string mUrl, bool disabled, System.Action<bool> callback)
    {
        string url = Constants.API_URL + $"multistream/target/{id}";
        var web = new UnityWebRequest(url, "PATCH");
        string b = "{\"name\": \"" + name + "\",\n\"url\": \"" + mUrl + "\",\n\"disabled\": " + disabled + "}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            callback(true);
        }
        else
        {
            callback(false);
            Debug.Log(web.error);
        }

        web.Dispose();
    }

    private IEnumerator UpdateSigningKeyRoutine(string id, string name, bool disabled, System.Action<bool> callback)
    {
        string url = Constants.API_URL + $"access-control/signing-key/${id}";
        var web = new UnityWebRequest(url, "PATCH");
        string b = "{\"name\": \"" + name + "\",\n\"disabled\": " + disabled + "}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            callback(true);
        }
        else
        {
            callback(false);
            Debug.Log(web.error);
        }

        web.Dispose();
    }

    private IEnumerator UpdateAssetRoutine(string id, string name, string storageJson, System.Action<bool> callback)
    {
        string url = Constants.API_URL + $"asset/{id}";
        var web = new UnityWebRequest(url, "PATCH");
        string b = "{\"name\": \"" + name + "\",\n " + storageJson + "}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(b);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);

        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            callback(true);
        }
        else
        {
            callback(false);
            Debug.Log(web.error);
        }

        web.Dispose();
    }

    private IEnumerator TranscodeVideoRoutine(TranscodeVideoData data, System.Action<TranscodeVideoResponse> callback)
    {
        var web = new UnityWebRequest(Constants.API_URL + "transcode", "POST");
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
            TranscodeVideoResponse respObj = JsonConvert.DeserializeObject<TranscodeVideoResponse>(resp);
            callback(respObj);
        }
        web.Dispose();
    }
    #endregion
}