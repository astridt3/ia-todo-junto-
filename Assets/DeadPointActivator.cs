using UnityEngine;

public class DeadPointActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] walls;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            ActivateAll();
            activated = true;
        }
    }

    private void ActivateAll()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }

        foreach (GameObject wall in walls)
        {
            if (wall != null)
                wall.SetActive(true);
        }
    }
}