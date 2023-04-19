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
* All API calls are stored in `Scripts/MainAPI.cs` and Data Classes are stored in `Scripts/DataClasses.cs`

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

### Create Asset
To upload an asset, your first need to request for a direct upload URL and only then actually upload the contents of the asset.

This also allows your users to upload files since the upload URL is pre-signed with your credentials and is accessible from the browser. The initial API call to get the upload URL must be done from a backend though, from which an API token is available.

#### How To Use
```
MainAPI.Instance.CreateAsset(name, callback);
```

* callback is of type Action<CreateAssetResponse>
Where response has:
```
 public string url;
 public string tusEndpoint;
 public AssetObject asset;
 public string name;
```
AssetObject:
```
 public string id;
 public string playbackId;
 public string userId;
 public Status status;
```

### Create Asset Via URL
Same like Create asset, but uses external url for the video.

#### How To Use
 ```
 MainAPI.Instance.CreateAssetViaUrl(url, name, callback)
 ```
 URL is the url of the asset to be created with
 The callback is the same with the CreateAsset

### Create Signing Key
A signing key is used to sign JWTs and are necessary to make a stream private.

When you create a signing key, the request returns a base64 encoded pem keypair, where

The publicKey is a representation of the public key, encoded as base 64 and is passed as a string
The privateKey is displayed only on creation. This is the only moment where the client can save the private key, otherwise it will be lost. Remember to decode your string when signing JWTs

Up to 10 signing keys can be generated, after that you must delete at least one signing key to create a new one.

#### How To Use
```
 MainAPI.Instance.CreateSigningKey(callback);`
```
 Callback returns object of type SigningKeyResponse
 ```
    public string id;
    public string name;
    public string userId;
    public long createdAt;
    public string privateKey;
    public string publicKey;
 ```
 
### Create Multistream
Creates a multistream target object.

#### How To Use
```
 MainAPI.Instance.CreateMultiStream(name url, callback);`
```
 Callback returns object of type MultistreamTargetResponse
 ```
    public string id;
    public string userId;
    public string name;
    public string url;
 ```
 
### Create Webhook
To create a new webhook, you need to make an API call with the events you want to listen for and the URL that will be called when those events occur.

#### How To Use
```
 MainAPI.Instance.CreateWebhook(data, callback);
```
 Data is of type WebhookRequestData
 ```
    public string[] events;
    public string url;
    public string name;
 ```
 While callback returns object of type WebhookResponse
 ```
    public long createdAt;
    public string[] events;
    public string id;
    public string kind;
    public string name;
    public string url;
    public string userId;
 ```
 
### Delete Asset
This will archive an existing asset. This means that deleted assets can still be accessed through their download and playback URLs.

#### How To Use
```
 MainAPI.Instance.DeleteAsset(assetId);
```

### Delete Signing Key
Deletes an existing signing key object.

#### How To Use
```
 MainAPI.Instance.DeleteSignkingKey(keyId);
```
 
### Delete Multistream
Deletes an existing multistream target object. Make sure to remove any references to the target on existing streams before actually deleting it from the API.

#### How To Use

```
 MainAPI.Instance.DeleteMultistream(streamId);
```
 
### Get Asset
Retrieves the details of an Asset that has previously been imported or uploaded. Supply the unique Asset ID that was returned from your previous request, and Livepeer will return the corresponding Asset information.

#### How To Use
```
 MainAPI.Instance.GetAsset(assetId, callback);
```
 Callback returns object of type CreateAssetResponse

### Get Multistream
You can retrieve a multistream target with its ID

#### How To Use
```
 MainAPI.Instance.GetMultiStream(streamId, callback);
```
 Callback returns object of type MultistreamTargetResponse

### Get Playback Info
You can retrieve the playback information by calling the GET playback endpoint with the playbackId.

#### How To Use
```
 MainAPI.Instance.GetPlaybackInfo(playbackId, callback);
```
 Callback returns object of type PlaybackInfoResponse
```
 public string type;
 public PlaybackInfoMeta[] meta;
```
PlaybackInfoMeta
```
  public PlaybackInfoSource[] source;
```
PlaybackInfoSource
```
 public string hrn;
 public string type;
 public string url;
 public long size;
 public int width;
 public int height;
 public long bitrate;
```
 
### Get Recorded Sessions
To a return list of recorded sessions with the same parentId.

#### How To Use
```
 MainAPI.Instance.GetRecordedSessions(parentId, record, callback);
```
Callback returns object of type RecordedSessionsResponse
```
 public float sourceSegmentsDuration;
 public string id;
 public bool record;
 public string parentId;
 public string recordingStatus;
 public string recordingUrl;
 public string mp4URL;
```
 
### Get Signing Keys
You can list all the signing keys in your account.

#### How To Use
```
 MainAPI.Instance.GetSigningKeys(callback);
```
Callback returns object of type SigningKeyResponse
```
 public string id;
 public string name;
 public string userId;
 public long createdAt;
 public string privateKey;
 public string publicKey;
```
 
### Get Task
Get a tsk from the account, using its ID.

#### How To Use
```
 MainAPI.Instance.GetTask(taskId, callback);
```
Callback returns object of type TaskResponse
```
 public string id;
 public string type;
 public Output output;
 public Params @params;
 public Status status;
 public string userId;
 public long createdAt;
 public string outputAssetId;
```
 
### Get Tasks
Does the same with Get Task, but returns an array of Task Responses, with the latest task first.

#### How To Use
```
 MainAPI.Instance.GetTasks(callback);
```
Callback returns an array of objects of type TaskResponse
 
### Get Webhook
You can retrieve the webhooks by calling the GET webhook endpoint with the webhookId.

#### How To Use
```
 MainAPI.Instance.GetWebHook(webhookId, callback);
```
Callback returns object of type WebhookResponse
```    
 public long createdAt;
 public string[] events;
 public string id;
 public string kind;
 public string name;
 public string url;
 public string userId;
```
 
### Update Webhook
You can update a webhook by calling the PATCH webhook endpoint with the webhookId. It is a single endpoint that can update any of the mutable properties of a webhook. Specifically:
* name
* url
* events

#### How To Use
```
 MainAPI.Instance.UpdateWebhook(id, data, callback);
```
Data is type of WebhookRequestData
Callback returns object of type Boolean

### Patch Recording
PATCH /stream/{id}/record can only modify a stream object. You cannot modify the record value on a session object or the historic stream objects with a parentId (stream objects representing a single live stream session).

The record value is inherited by all future child session objects. Child session objects are read-only.

#### How To Use
```
 MainAPI.Instance.PatchRecording(id, recording, callback);
```
Callback returns object of type Boolean

### Update Multistream Target
PATCH /multistream/target/{id} updates an existing multistream target object. You can change any user-defined field in the current object, specifically name, url and disabled.

All fields are optional, and if any field is not included in the request payload it will be kept unchanged from the saved object.

You cannot access the current url field for updating it since we redact it from GET responses. If you want to update the URL you have to build it from scratch. You can also omit it from the PATCH payload to keep it as is.

A 204 No Content status response indicates the multistream target was successfully updated.

#### How To Use
```
 MainAPI.Instance.UpdateMultistreamTarget(id, name, mUrl, disabled, callback);
```
Callback returns object of type Boolean

### Update Signing Key
PATCH /access-control/signing-key/{KEY_ID} updates an existing signing key. It is a single endpoint that can update any of the mutable properties of a stream. Specifically:
* name
* disabled

All fields are optional, and if any field is not included in the request payload it will be kept unchanged from the saved object.

#### How To Use
```
 MainAPI.Instance.UpdateSigningKey(id, name, disabled, callback);
```
Callback returns object of type Boolean

### Update Asset
PATCH /asset/:id -d { } can be used to modify any asset that was uploaded with On Demand. It is a single endpoint that can update only the following mutable properties of an asset:
* name
* storage

#### How To Use
```
 MainAPI.Instance.UpdateAsset(id, name, storageJson, callback);
```
Callback returns object of type Boolean

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
