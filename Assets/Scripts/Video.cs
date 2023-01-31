using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Video : Interactable
{
    private VideoPlayer videoPlayerThirdPerson;
    private VideoSource videoSource;
    
    private VideoPlayer videoPlayerFirstPerson;

    //Audio
    private AudioSource audioSource;

    private GameObject firstPersonVideo;
    private GameObject firstPersonVideoCamera;
    public static bool VideoCameraActive;

    // Use this for initialization
    void Start()
    {
        // set first person camera
        VideoCameraActive = false;
        firstPersonVideo = gameObject.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonVideo.name);
        firstPersonVideoCamera = firstPersonVideo.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonVideoCamera.name);
        firstPersonVideoCamera.SetActive(false);

        Application.runInBackground = true;
        playVideo();
    }

    private void playVideo()
    {
        SetUpVideo(ref videoPlayerThirdPerson, gameObject);
        SetUpVideo(ref videoPlayerFirstPerson, firstPersonVideo);
        StartCoroutine(playSelectedVideo(videoPlayerThirdPerson));
        StartCoroutine(playSelectedVideo(videoPlayerFirstPerson));
    }

    private void SetUpVideo(ref VideoPlayer videoPlayer, GameObject obj)
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = obj.AddComponent<VideoPlayer>();

        //Add AudioSource
        // audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        //   audioSource.playOnAwake = false;
        //    audioSource.Pause();

        //We want to play from video clip from url
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = $"https://{NetworkConstants.IpAddress}/videos/VirtualReality2.mp4";

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        //videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();
    }

    private IEnumerator playSelectedVideo(VideoPlayer videoPlayer)
    {
        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            //Debug.Log("Preparing Video");
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Play Video
        videoPlayer.Play();

        //Play Sound
        //audioSource.Play();

        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

    public override void CmdInteractF(GameObject obj)
    {
        if(videoPlayerThirdPerson != null)
            
        if(videoPlayerThirdPerson.isPlaying)
        {
            Debug.Log("Pause video");
            videoPlayerThirdPerson.Pause();
            videoPlayerFirstPerson.Pause();
        }
    }

    public override void CmdInteractE(GameObject obj)
    {
        if (videoPlayerThirdPerson.isPaused)
        {
            Debug.Log("Play video");
            videoPlayerThirdPerson.Play();
            videoPlayerFirstPerson.Play();
        }
    }

    public override void CmdInteractI(GameObject gameObject)
    {
        if (VideoCameraActive)
        {
            firstPersonVideoCamera.SetActive(false);
        }
        else
        {
            firstPersonVideoCamera.SetActive(true);
        }
        VideoCameraActive = !VideoCameraActive;
    }
}
