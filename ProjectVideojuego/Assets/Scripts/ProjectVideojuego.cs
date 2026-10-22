using UnityEngine;
using TMPro;

public class ProjectVideojuego : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float alturaLimite = -10f; // si cae por debajo de esta altura, reaparece

    public TextMeshProUGUI contadorJuego; // aquí se arrastra el texto ContadorJuego

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private bool enSuelo;
    private Vector3 puntoInicio;

    private int cake, chicken, coffe, jam, cookie;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        puntoInicio = transform.position; // guarda donde empezó
        ActualizarContador();
    }

    void Update()
    {
        // Movimiento en el eje X (flechas o A/D)
        float movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        // Voltear el sprite según la dirección
        if (movimiento > 0) sr.flipX = false;
        else if (movimiento < 0) sr.flipX = true;

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

        // Actualizar los parámetros del Animator
        animator.SetBool("Caminando", movimiento != 0);
        animator.SetBool("EnSuelo", enSuelo);
    }

    void Reaparecer()
    {
        transform.position = puntoInicio;
        rb.linearVelocity = Vector2.zero; // detiene la caída
    }

    // Detecta cuando el jugador toca un objeto coleccionable
    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Collectible")) return;

        switch (otro.gameObject.name)
        {
            case "Cake": cake++; break;
            case "Chicken": chicken++; break;
            case "Coffe": coffe++; break;
            case "Jam": jam++; break;
            case "Cookie": cookie++; break;
        }

        Destroy(otro.gameObject); // el objeto desaparece
        ActualizarContador();
    }

    void ActualizarContador()
    {
        if (contadorJuego == null) return;

        contadorJuego.text = "Cake: " + cake + " | Chicken: " + chicken +
                             " | Coffe: " + coffe + " | Jam: " + jam +
                             " | Cookie: " + cookie;
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