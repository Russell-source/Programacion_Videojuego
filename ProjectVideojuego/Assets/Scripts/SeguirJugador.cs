using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (jugador == null) return;

        Vector3 destino = new Vector3(jugador.position.x, jugador.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }
}
