using UnityEngine;

public class Fragmento : MonoBehaviour
{
    [SerializeField] private LayerMask camadaPlayer;
    private void OnCollisionEnter2D(Collision2D collision) // Detecta colisão com fragmentos de tempo (verifica se a camada da colisão está na camada de fragmentos)
    {
        if (((1 << collision.gameObject.layer) & camadaPlayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
