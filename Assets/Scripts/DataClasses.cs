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
}