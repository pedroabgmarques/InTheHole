using UnityEngine;
using System.Collections;
using System.Net.Mime;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Player;

    #region Componentes

    public GameObject HurtImage;

    private RawImage hurtImage;
    private Rigidbody body;

    #endregion

    #region Variáveis

    /* Variáveis Públicas */
    public Transform CameraTransform;
    public float Speed;
    public int Score;
    public bool HasFinished;

    /* Variáveis Privadas */
    private Vector2 movement;
    private Vector3 direction;
    private float moveHorizontal;
    private float moveVertical;
    private float moveForce;

    public Color HurtColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private Vector3 startingPosition;

    private AudioSource DieSound;

    #endregion

    #region Métodos MonoBehaviour

    // Awake
    public void Awake()
    {
        Player = this;

        body = GetComponent<Rigidbody>();
        hurtImage = HurtImage.GetComponent<RawImage>();
        hurtImage.color = HurtColor;

        
        DieSound = GetComponent<AudioSource>();
        DieSound.playOnAwake = false;

        startingPosition = transform.position;

        
    }

    // Update
    public void Update()
    {
        hurtImage.color = HurtColor;

        if (transform.position.y < -20.0f)
        {
            transform.position = startingPosition;
            body.velocity = Vector3.zero;
        }
    }

    // Fixed Update
    public void FixedUpdate()
    {
        Move();
    }

    private int count = 0;

    // On Trigger Enter
    public void OnTriggerEnter(Collider other)
    {
        DieSound.Play();
        HasFinished = true;
        
        Invoke("LoadRoomScene", 4.0f);
    }

    #endregion

    #region Métodos Públicos

    #endregion

    #region Métodos Privados

    // Move a bola dependendo da direcção da câmara.
    private void Move()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical);
        moveForce = Speed * movement.magnitude;

        direction = moveVertical * CameraTransform.forward + moveHorizontal * CameraTransform.right;

        body.AddForce(direction * Speed);
    }

    private void LoadRoomScene()
    {
        SceneManager.LoadScene("Room");
    }

    #endregion
}
