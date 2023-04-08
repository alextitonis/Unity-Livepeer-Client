# Unity-Livepeer-Client

## How to run

Download the Repository and open it with unity (Built in 2021.3.17f1)
Install https://github.com/NexPlayer/NexPlayer_Unity_Plugin in the project
There is a Main Scene, where there is a sample setup.
The project consists of 2 main Prefabs
* The API, which handles the web requests
* The Video Player, which can be used to play Streams through playfab
 - Video Player is based on https://github.com/NexPlayer/NexPlayer_Unity_Plugin for HLS video streaming
 
## Documentation

* First in the API Script you will need to set your Livepeer API Key
* All web requests are built using Unity's WebRequest class
* Web Requests are based on the Original Livepeer Documentation (https://docs.livepeer.org/reference/api/create-asset)

### Create Stream
Create Stream is used to create a stream with the custom data.

#### How To Use
```
MainAPI.Instance.CreateStream(createStreamData, callback);
```

* CreateStreamData consists of these values:
```
    public string name;
    public StreamProfile[] profiles;
```

* Callback is called on completion, returning the Stream Response:
```
    public int lastSeen;
    public bool isActive;
    public bool record;
    public bool suspended;
    public float sourceSegments;
    public float transcodedSegments;
    public float sourceSegmentsDuration;
    public float sourceBytes;
    public float transcodedBytes;
    public StreamProfile[] profiles;
    public string name;
    public string kind;
    public string userId;
    public string id;
    public long createdAt;
    public string streamKey;
    public string playbackId;
    public string createdByTokenName;
```

### Get Stream
Get Stream is used to get the data from the stream, using its ID

#### How To Use
```
MainAPI.Instance.GetStream(streamId, callback);
```

* Callback is called on completion, returning the Stream Data if found:
```    
    public int lastSeen;
    public bool isActive;
    public bool record;
    public bool suspended;
    public float sourceSegments;
    public float transcodedSegments;
    public float sourceSegmentsDuration;
    public float sourceBytes;
    public float transcodedBytes;
    public StreamProfile[] profiles;
    public string name;
    public string kind;
    public string userId;
    public string id;
    public long createdAt;
    public string streamKey;
    public string playbackId;
    public string createdByTokenName;
```

### Get Stream List
Get Stream List is used to get all the available stream, based on the user's API Key

#### How To Use
```
MainAPI.Instance.GetStreamLeast(callback)
```

* Callback is called on completion, returning the list of the Streams
```
public List<StreamResponse> list
```
StreamResponse class, is consisted of the Data of each Stream, same with GetStream response

### Update Stream Data
Update Stream Data is used to change the current data of a stream using its ID

#### How To Use
```
MainAPI.Instance.UpdateStreamData(streamId, newData);
```

* newData is type of UpdateStreamDataObj
```
    public bool record;
    public bool suspended;
```

### Delete Stream
Delete Stream is used to delete a stream using its ID

#### How To Use
```
MainAPI.Instance.DeleteStream(streamId);
```

### Transcode Video
Transcode Video transcodes a video file and uploads the results to the specified storage service.
```
MainAPI.Instance.TranscodeVideo(data, callback);
```
Data Input:
```
{
   "input":{
      "url":"https://www.example.com/video.mp4"
   },
   "storage":{
      "type":"s3",
      "endpoint":"https://gateway.storjshare.io",
      "credentials":{
         "accessKeyId":"$ACCESS_KEY_ID",
         "secretAccessKey":"$SECRET_ACCESS_KEY"
      },
      "bucket":"mybucket"
   },
   "outputs":{
      "hls":{
         "path":"/samplevideo/hls"
      },
      "mp4":{
         "path":"/samplevideo/mp4"
      }
   },
   "profiles":[
      {
         "name":"480p",
         "bitrate":1000000,
         "fps":30,
         "width":854,
         "height":480
      },
      {
         "name":"360p",
         "bitrate":500000,
         "fps":30,
         "width":640,
         "height":360
      }
   ]
}
```

### Play HLS Stream

#### Option 1
* Add the Video Player Prefab
* Find the NexPlayer_Manager object (child of Video Player)
* Set URL value, tick the Is Live Stream and AutoPlay

#### Option 2
* Add the VideoManager.cs script in a game object in the scene
* Set the streamId and playback_base_url values
* Assign the NexPlayer script
* It is setup to play the stream On Start, otherwise PlayStream function can be called to play a stream on run-time
