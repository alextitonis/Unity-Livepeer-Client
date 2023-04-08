using System.Collections.Generic;

[System.Serializable]
public class CreateStreamData
{
    public string name;
    public StreamProfile[] profiles;
}

[System.Serializable]
public class StreamProfile
{
    public string name;
    public long bitrate;
    public int fps;
    public int width;
    public int height;
}

[System.Serializable]
public class StreamResponse
{
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
}

[System.Serializable]
public class UpdateStreamDataObj
{
    public bool record;
    public bool suspended;
    public UpdateStreamDataMultistream[] multistream;
}

[System.Serializable]
public class UpdateStreamDataMultistream
{
    public string id;
    public string profile;
    public bool videoOnly;
}

[System.Serializable]
public class CreateAssetResponse
{
    public string url;
    public string tusEndpoint;
    public AssetObject asset;
    public string name;
}

[System.Serializable]
public class AssetObject
{
    public string id;
    public string playbackId;
    public string userId;
    public long createdAt;
    public Status status;
}

[System.Serializable]
public class Status
{
    public string phase;
    public long updatedAt;
}

[System.Serializable]
public class SigningKeyResponse
{
    public string id;
    public string name;
    public string userId;
    public long createdAt;
    public string privateKey;
    public string publicKey;
}

[System.Serializable]
public class MultistreamTargetResponse
{
    public string id;
    public string userId;
    public string name;
    public string url;
}

[System.Serializable]
public class WebhookRequestData
{
    public string[] events;
    public string url;
    public string name;
}

[System.Serializable]
public class WebhookResponse
{
    public long createdAt;
    public string[] events;
    public string id;
    public string kind;
    public string name;
    public string url;
    public string userId;
}

[System.Serializable]
public class PlaybackInfoResponse
{
    public string type;
    public PlaybackInfoMeta[] meta;
}

[System.Serializable]
public class PlaybackInfoMeta
{
    public PlaybackInfoSource[] source;
}

[System.Serializable]
public class PlaybackInfoSource
{
    public string hrn;
    public string type;
    public string url;
    public long size;
    public int width;
    public int height;
    public long bitrate;
}

[System.Serializable] 
public class RecordedSessionsResponse
{
    public float sourceSegmentsDuration;
    public string id;
    public bool record;
    public string parentId;
    public string recordingStatus;
    public string recordingUrl;
    public string mp4URL;
}

[System.Serializable]
public class Hash
{
    public string hash;
    public string algorithm;
}

[System.Serializable]
public class Track
{
    public int fps;
    public string type;
    public string codec;
    public int width;
    public int height;
    public int bitrate;
    public double duration;
    public string pixelFormat;
    public int? channels;
    public int? sampleRate;
}

[System.Serializable]
public class VideoSpec
{
    public string format;
    public List<Track> tracks;
    public double duration;
}

[System.Serializable]
public class AssetSpec
{
    public List<Hash> hash;
    public string name;
    public long size;
    public string type;
    public VideoSpec videoSpec;
}

[System.Serializable]
public class Import
{
    public AssetSpec assetSpec;
    public string videoFilePath;
    public string metadataFilePath;
}

[System.Serializable]
public class Output
{
    public Import import;
}

[System.Serializable]
public class ImportParams
{
    public string url;
}

[System.Serializable]
public class Params
{
    public ImportParams import;
}

[System.Serializable]
public class TaskStatus
{
    public string phase;
    public long updatedAt;
}

[System.Serializable]
public class TaskResponse
{
    public string id;
    public string type;
    public Output output;
    public Params @params;
    public Status status;
    public string userId;
    public long createdAt;
    public string outputAssetId;
}

[System.Serializable]
public class Input
{
    public string url;
}

[System.Serializable]
public class Credentials
{
    public string accessKeyId;
    public string secretAccessKey;
}

[System.Serializable]
public class Storage
{
    public string type;
    public string endpoint;
    public Credentials credentials;
    public string bucket;
}

[System.Serializable]
public class Hls
{
    public string path;
}

[System.Serializable]
public class Mp4
{
    public string path;
}

[System.Serializable]
public class Outputs
{
    public Hls hls;
    public Mp4 mp4;
}

[System.Serializable]
public class Profile
{
    public string name;
    public int bitrate;
    public int fps;
    public int width;
    public int height;
}

[System.Serializable]
public class TranscodeVideoData
{
    public Input input;
    public Storage storage;
    public Outputs outputs;
    public List<Profile> profiles;
}

[System.Serializable]
public class TranscodeFile
{
    public Input input;
    public Storage storage;
    public Outputs outputs;
}

[System.Serializable]
public class TranscodeVideoParams
{
    public TranscodeFile transcode_file;
}

[System.Serializable]
public class TranscodeVideoResponse
{
    public string id;
    public long createdAt;
    public string type;
    public string userId;
    public TranscodeVideoParams @params;
    public Status status;
}
