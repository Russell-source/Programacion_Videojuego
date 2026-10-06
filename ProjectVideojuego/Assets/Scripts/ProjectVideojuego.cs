using UnityEngine;

public class ProjectVideojuego : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float alturaLimite = -10f; 

    private Rigidbody2D rb;
    private bool enSuelo;
    private Vector3 puntoInicio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoInicio = transform.position; 
    }

    void Update()
    {
        
        float movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
        }

        
        if (transform.position.y < alturaLimite)
        {
            Reaparecer();
        }
    }

    void Reaparecer()
    {
        transform.position = puntoInicio;
        rb.linearVelocity = Vector2.zero; 
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
            Debug.Log("Tocando el suelo"); 
        }
    }
}