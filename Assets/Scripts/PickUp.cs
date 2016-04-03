using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class PickUp : MonoBehaviour
{
    #region Componentes

    private MeshRenderer meshRenderer;
    private Rigidbody body;
    private AudioSource sfxPickUp;

    /* Singletons */
    private PlayerMovement player;

    #endregion

    #region Variáveis

    public int Points;
    public float Speed;

    #endregion

    #region Métodos MonoBehaviour

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        body = GetComponent<Rigidbody>();
        sfxPickUp = GetComponent<AudioSource>();
    }

    // Start
    public void Start()
    {
        player = PlayerMovement.Player;
    }

    // Update
    public void Update()
    {
        Rotate();
    }

    // On Collision Enter
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnPickUp();
    }

    #endregion

    #region Métodos Privados

    // Roda consecutivamente o pickup.
    private void Rotate()
    {
        transform.Rotate(new Vector3(0, Speed, 0)*Time.deltaTime);
    }

    private void OnPickUp()
    {
        player.Score += Points;
        sfxPickUp.Play();

        if (Points < 0)
            player.HurtColor.a += 0.3f;

        body.detectCollisions = false;
        meshRenderer.enabled = false;

        if (!sfxPickUp.isPlaying)
            Destroy(gameObject);
    }

    #endregion
}
