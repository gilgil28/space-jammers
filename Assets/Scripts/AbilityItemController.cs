using UnityEngine;

public abstract class AbilityItemController : MonoBehaviour
{
    [SerializeField] private CollectTrigger _collectTrigger;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            _collectTrigger.Trigger();
            AddAbility(other.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void AddAbility(GameObject other);
}