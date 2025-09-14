using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MP4Player : MonoBehaviour
{
    public GameObject MP4Playerpanel;   // �г� (���� UI�� ����ִ� ������Ʈ)
    public RawImage rawImage;           // ���� ����� RawImage
    public VideoPlayer videoPlayer;     // VideoPlayer ������Ʈ

    void Start()
    {
        Time.timeScale = 1f;
 
        MP4Playerpanel.SetActive(false);

        // VideoPlayer �ʱ� ����
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
        videoPlayer.url = fullPath;       // ���� ���� ���
        videoPlayer.Prepare();            // ���� �غ�

        // �غ�� ������ ���
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // �غ�Ǹ� RawImage�� Texture ����
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
