using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEndGame : MonoBehaviour
{
    [SerializeField] private LayerMask camadaPlayer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & camadaPlayer) != 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
