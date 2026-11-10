using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float velocidad = 2f;          // velocidad al patrullar
    public float distanciaPatrulla = 3f;  // cuánto se aleja del punto inicial
    public float alturaLimite = -15f;     // si cae por debajo, vuelve al inicio

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 puntoInicio;
    private int direccion = 1; // 1 = derecha, -1 = izquierda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        puntoInicio = transform.position;
    }

    void Update()
    {
        // Cambia de dirección al llegar al límite de la patrulla
        if (transform.position.x > puntoInicio.x + distanciaPatrulla) direccion = -1;
        else if (transform.position.x < puntoInicio.x - distanciaPatrulla) direccion = 1;

        // Se mueve en el eje X
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);

        // Voltea el sprite según la dirección
        sr.flipX = direccion < 0;

        // Si cae al vacío, vuelve al inicio
        if (transform.position.y < alturaLimite)
        {
            transform.position = puntoInicio;
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Si choca con una pared, da la vuelta
    void OnCollisionEnter2D(Collision2D colision)
    {
        foreach (ContactPoint2D contacto in colision.contacts)
        {
            if (Mathf.Abs(contacto.normal.x) > 0.5f)
            {
                direccion *= -1;
                break;
            }
        }
    }
}
