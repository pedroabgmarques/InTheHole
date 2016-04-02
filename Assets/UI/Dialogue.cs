using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    #region Singletons

    private PlayerMovement player;

    #endregion

    #region Componentes

    private Text txtScore;
    private Text txtDialogue;

    #endregion

    #region Variáveis

    public GameObject ScoreText;

    public GameObject DialogueText;
    public GameObject DialogueBackground;

    public GameObject BlueScreen;

    private float timer;
    private bool hasCaughtDiamond = false;
    private bool hasNegativeScore = false;

    #endregion

    public void Awake()
    {
        txtDialogue = DialogueText.GetComponent<Text>();
        DialogueText.SetActive(false);
        DialogueBackground.SetActive(false);

        txtScore = ScoreText.GetComponent<Text>();
        txtScore.text = "000000";

        BlueScreen.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        player = PlayerMovement.Player;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.0f && timer < 5.5f)
        {
            txtDialogue.text = "Olá! Eu vou ajudar-te nesta grande aventura. Vamos a isso!";
            txtDialogue.color = Color.black;
            SetDialogue(1.0f, 5.0f);
        }
        else if (timer >= 5.5f && timer < 10.5f)
        {
            txtDialogue.text = "Apanha os Diamantes VERDES! Eles dão-te 5 pontos cada.";
            txtDialogue.color = Color.green;
            SetDialogue(5.5f, 10.0f);
        }
        else if (timer >= 10.5f && timer < 12.5f && player.Score > 0)
        {
            hasCaughtDiamond = true;
            txtDialogue.text = "Boa! É isso mesmo, continua!";
            txtDialogue.color = Color.black;
            SetDialogue(10.5f, 12.0f);

        }
        else if (timer >= 12.5f && timer < 18.5f  && hasCaughtDiamond)
        {
            txtDialogue.text = "Agora apanha esses Diamantes VERMELHOS! Eles são ESPECIAIS, e dão-te imensos pontos!";
            txtDialogue.color = Color.red;
            SetDialogue(12.5f, 18.0f);
        }
        else if (timer >= 18.5f && hasNegativeScore)
        {
            txtDialogue.text = "Hahahahahah! Temos pena...";
            txtDialogue.color = Color.black;
            SetDialogue(18.5f, 2000);
        }

        if (player.Score < 0)
            hasNegativeScore = true;
        else hasNegativeScore = false;

        if (player.HasFinished)
            BlueScreen.SetActive(true);

        UpdateScore();
    }

    #region Métodos Privados

    // Actuliza o score.
    private void UpdateScore()
    {
        if (player.Score > -10000 && player.Score <= -1000)
            txtScore.text = "-000" + Mathf.Abs(player.Score);
        else if (player.Score > -1000 && player.Score <= -100)
            txtScore.text = "-0000" + Mathf.Abs(player.Score);
        else if (player.Score > -100 && player.Score <= -10)
            txtScore.text = "-00000" + Mathf.Abs(player.Score);
        else if (player.Score > -10 && player.Score < 0)
            txtScore.text = "-00000" + Mathf.Abs(player.Score);
        else if (player.Score < 10)
            txtScore.text = "000000" + player.Score;
        else if (player.Score < 100)
            txtScore.text = "00000" + player.Score;
        else if (player.Score < 1000)
            txtScore.text = "0000" + player.Score;
    }

    private void SetDialogue(float startTime, float endTime)
    {
        if (timer >= startTime)
        {
            DialogueText.SetActive(true);
            DialogueBackground.SetActive(true);
        }
        if (timer > endTime)
        {
            DialogueText.SetActive(false);
            DialogueBackground.SetActive(false);
        }
    }

    #endregion

}