using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Reload the current active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
