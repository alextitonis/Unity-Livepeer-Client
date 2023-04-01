using UnityEngine;

public class Tests : MonoBehaviour
{
    private void Start()
    {
        TestCreateStream();
        TestGetStreamList();
    }

    [SerializeField] StreamResponse resp;

    void TestCreateStream()
    {
        CreateStreamData data = new CreateStreamData()
        {
            name = "First Stream",
            profiles = new StreamProfile[] {
                new StreamProfile() {
                    name = "720p",
                    bitrate = 2000000,
                    fps = 30,
                    width = 1280,
                    height=720
                },
                new StreamProfile(){
                    name = "480p",
                    bitrate = 1000000,
                    fps = 30,
                    width = 854,
                    height=480
                },
                new StreamProfile() {
                    name = "360p",
                    bitrate = 500000,
                    fps = 30,
                    width = 640,
                    height=360
                }
            }
        };

        MainAPI.getInstance.CreateStream(data, resp =>
        {
            TestGetStream(resp.id);
            TestSetRecording(resp.id, new UpdateStreamDataObj()
            {
                record = true,
                suspended = false
            });
            TestDeleteStream(resp.id);
        });
    }
    void TestGetStream(string streamId)
    {
        MainAPI.getInstance.GetStream(streamId, data => this.resp = data);
    }
    void TestGetStreamList()
    {
        MainAPI.getInstance.GetStreamsList(data => Debug.Log(data.Count));
    }
    void TestSetRecording(string streamId, UpdateStreamDataObj data)
    {
        MainAPI.getInstance.UpdateStreamData(streamId, data);
    }
    void TestDeleteStream(string streamId)
    {
        MainAPI.getInstance.DeleteStream(streamId);
    }
}