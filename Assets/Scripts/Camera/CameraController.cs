using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    #region Variáveis

    /* Variáveis Públicas */
    public Transform Player;
    public float Speed;
    public Vector3 Offset;

    public float SmoothPosition;
    public float SmoothRotation;

    /* Variáveis Privadas */

    // Posição
    private Vector3 currentPosition;
    private Vector3 nextPosition;
    // Rotação
    public Quaternion CurrentRotation;
    public Quaternion NextRotation;


    #endregion

    #region Métodos MonoBehaviour

    // Awake
    public void Awake()
    {
        currentPosition = transform.position;
        CurrentRotation = transform.rotation;
    }

    // Update
    public void Update()
    {
        ChasePlayer();
    }

    #endregion

    #region Métodos Privados

    private void ChasePlayer()
    {
        // Posição
        nextPosition = Player.position + Offset;
        transform.position = Vector3.Lerp(currentPosition, nextPosition, Time.deltaTime * SmoothPosition);
        currentPosition = transform.position;

        // Rotação
        transform.LookAt(Player);
        
        Offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Speed, Vector3.up) * Offset;
    }

    #endregion

}
