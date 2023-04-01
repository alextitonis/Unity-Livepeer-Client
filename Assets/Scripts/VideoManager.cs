using NexPlayerSample;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public static VideoManager getInstance;
    void Awake() { getInstance = this; }

    [SerializeField] string streamId;
    [SerializeField] string playback_base_url;
    [SerializeField] NexPlayer player;

    private void Start()
    {
        PlayStream(playback_base_url, streamId);   
    }

    public void PlayStream(string baseUrl, string streamId)
    {
        player.streamURI = CreateStreamId(baseUrl, streamId);
        player.PlayAsync();
    }

    string CreateStreamId(string playback_base_url, string playback_id)
    {
        return playback_base_url + "/" + playback_id + "/index.m3u8";
    }
}