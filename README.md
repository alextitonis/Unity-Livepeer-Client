# Unity-Livepeer-Client

## How to run

Download the Repository and open it with unity (Built in 2021.3.17f1)
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