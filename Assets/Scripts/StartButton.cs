using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private RectTransform rectTransform;
    private AudioSource audioSource;
    private int stage = 0;

    public void OnClick()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        switch (stage)
        {
            case 0:
                rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                if (!audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 0.8f;
                audioSource.Play();
                break;
            case 1:
                rectTransform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                if (!audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 0.9f;
                audioSource.pitch = 1.2f;
                audioSource.timeSamples = 60;
                audioSource.Play();
                break;
            case 2:
                rectTransform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                if (!audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 1.0f;
                audioSource.pitch = 2.1f;
                audioSource.Play();
                break;
            case 3:
                rectTransform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
                if (!audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 1.0f;
                audioSource.pitch = 0.8f;
                audioSource.Play();
                break;
            case 4:
                rectTransform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                if (!audioSource.isPlaying) audioSource.Stop();
                audioSource.volume = 1.0f;
                audioSource.pitch = 1.5f;
                audioSource.Play();
                break;
            case 5:
                SceneManager.LoadScene(1);
                break;
        }
        stage++;
    }
}
