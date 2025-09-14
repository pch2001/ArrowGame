using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MP4Player : MonoBehaviour
{
    public GameObject MP4Playerpanel;   // 패널 (영상 UI가 들어있는 오브젝트)
    public RawImage rawImage;           // 영상 출력할 RawImage
    public VideoPlayer videoPlayer;     // VideoPlayer 컴포넌트

    void Start()
    {
        Time.timeScale = 1f;
 
        MP4Playerpanel.SetActive(false);

        // VideoPlayer 초기 설정
        videoPlayer.playOnAwake = false;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
    }

    public void PlayFromStreamingAssets(string fileName)
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log("Video Path: " + fullPath);


        MP4Playerpanel.SetActive(true);
        StartCoroutine(PlayVideo(fullPath));
    }


    private IEnumerator PlayVideo(string fullPath)
    {
        videoPlayer.url = fullPath;       // 로컬 파일 경로
        videoPlayer.Prepare();            // 영상 준비

        // 준비될 때까지 대기
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // 준비되면 RawImage에 Texture 연결
        rawImage.texture = videoPlayer.texture;

        videoPlayer.Play();

        yield return new WaitForSeconds(4f);
        StopVideo();

    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        MP4Playerpanel.SetActive(false);
    }
}
