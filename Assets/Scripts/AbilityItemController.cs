using UnityEngine;

public abstract class AbilityItemController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log("Found battery!");
            AddAbility(other.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void AddAbility(GameObject other);
}