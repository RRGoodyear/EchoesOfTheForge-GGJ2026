using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    private VideoPlayer video;
    private bool runOnce = false;

    [SerializeField] private string videoFilename;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !runOnce)
        {
            PlayVideo();
            runOnce = true;
        }
    }

    public void PlayVideo()
    {
        if (video)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFilename);
            video.url = videoPath;
            video.Play();
        }
    }
}
