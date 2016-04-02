using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Player;

    #region Componentes

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

    #endregion

    #region Métodos MonoBehaviour

    // Awake
    public void Awake()
    {
        Player = this;

        body = GetComponent<Rigidbody>();
    }

    // Fixed Update
    public void FixedUpdate()
    {
        Move();
    }

    // On Trigger Enter
    public void OnTriggerEnter(Collider other)
    {
        HasFinished = true;
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

    #endregion
}
