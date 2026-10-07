using UnityEngine;

public class ProjectVideojuego : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float alturaLimite = -10f; // si cae por debajo de esta altura, reaparece

    private Rigidbody2D rb;
    private bool enSuelo;
    private Vector3 puntoInicio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoInicio = transform.position; // guarda donde empezó
    }

    void Update()
    {
        // Movimiento en el eje X (flechas o A/D)
        float movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        // Salto con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
        }

        // Si cae al vacío, vuelve al inicio
        if (transform.position.y < alturaLimite)
        {
            Reaparecer();
        }
    }

    void Reaparecer()
    {
        transform.position = puntoInicio;
        rb.linearVelocity = Vector2.zero; // detiene la caída
    }

    // Mientras toque algo por debajo, está en el suelo
    void OnCollisionStay2D(Collision2D colision)
    {
        foreach (ContactPoint2D contacto in colision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                enSuelo = true;
                return;
            }
        }
    }

    // Al dejar de tocar, ya no está en el suelo
    void OnCollisionExit2D(Collision2D colision)
    {
        enSuelo = false;
    }
}