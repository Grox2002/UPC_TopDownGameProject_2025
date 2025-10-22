using UnityEngine;

public class Rehen : MonoBehaviour
{
    public bool estaSalvado = false;
    public float distanciaInteraccion = 2f;

    private Transform jugador;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (estaSalvado) return;

        float distancia = Vector2.Distance(jugador.position, transform.position);
        if (distancia < distanciaInteraccion && Input.GetKeyDown(KeyCode.E))
        {
            Salvar();
        }
    }

    private void Salvar()
    {
        estaSalvado = true;
        SalvarRehenes.Instance.RehenSalvado();
        
        gameObject.SetActive(false);
    }
}
