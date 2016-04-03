using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class backgroundScript : MonoBehaviour {

    private Image background;
    private Text title;
    private Button newGame;

    void Awake()
    {
        background = GetComponentInChildren<Image>();
        background.canvasRenderer.SetAlpha(0.0f);
        title = GetComponentInChildren<Text>();
        title.canvasRenderer.SetAlpha(0.0f);
        newGame = GetComponentInChildren<Button>();
        newGame.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        
        background.CrossFadeAlpha(1f, 3f, false);
        title.CrossFadeAlpha(1f, 6f, false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > 6f)
        {
            newGame.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
