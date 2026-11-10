using UnityEngine;

public class ComplexEnemy : MonoBehaviour
{
    public float velocidad = 2f;               // velocidad al patrullar
    public float velocidadPersecucion = 3.5f;  // velocidad al perseguir
    public float distanciaPatrulla = 3f;       // cuánto patrulla desde su punto inicial
    public float rangoDeteccion = 6f;          // distancia a la que ve al jugador
    public float fuerzaSalto = 6f;             // salto al perseguir
    public float alturaLimite = -15f;          // si cae por debajo, vuelve al inicio

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform jugador;
    private Vector3 puntoInicio;
    private int direccion = 1;
    private bool enSuelo;
    private float tiempoSiguienteSalto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        puntoInicio = transform.position;

        // Busca al jugador por su script
        ProjectVideojuego scriptJugador = FindFirstObjectByType<ProjectVideojuego>();
        if (scriptJugador != null) jugador = scriptJugador.transform;
    }

    void Update()
    {
        bool persiguiendo = jugador != null &&
            Vector2.Distance(transform.position, jugador.position) < rangoDeteccion;

        if (persiguiendo)
        {
            // Persigue al jugador
            direccion = jugador.position.x > transform.position.x ? 1 : -1;
            rb.linearVelocity = new Vector2(direccion * velocidadPersecucion, rb.linearVelocity.y);

            // Salta si el jugador está más arriba
            if (enSuelo && jugador.position.y > transform.position.y + 1f
                && Time.time >= tiempoSiguienteSalto)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
                tiempoSiguienteSalto = Time.time + 1.5f;
            }
        }
        else
        {
            // Patrulla como el enemigo simple
            if (transform.position.x > puntoInicio.x + distanciaPatrulla) direccion = -1;
            else if (transform.position.x < puntoInicio.x - distanciaPatrulla) direccion = 1;

            rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
        }

        // Voltea el sprite según la dirección
        sr.flipX = direccion < 0;

        // Si cae al vacío, vuelve al inicio
        if (transform.position.y < alturaLimite)
        {
            transform.position = puntoInicio;
            rb.linearVelocity = Vector2.zero;
        }
    }

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

    void OnCollisionExit2D(Collision2D colision)
    {
        enSuelo = false;
    }
}
