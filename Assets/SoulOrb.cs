using UnityEngine;
using UnityEngine.Events;

public class SoulOrb : MonoBehaviour
{
    public GameObject SoulOrbVFXPrefab;
    public ParticleSystem SoulOrbCollectVFXPrefab;
    public UnityEvent OnCollect;

    public void Collect()
    {
        var vfx = Instantiate(SoulOrbVFXPrefab, transform.position, Quaternion.identity);

        var orbs = Instantiate(SoulOrbCollectVFXPrefab, PlayerGameManager.Instance.SoulCollectTransform.position, Quaternion.identity, PlayerGameManager.Instance.SoulCollectTransform);
        Vector3 targetPos = transform.position - orbs.transform.position;
        var shape = orbs.shape;
        shape.enabled = true;
        shape.position = targetPos;
        orbs.Play();

        Destroy(gameObject);
        Destroy(vfx, 5f);
        Destroy(orbs.gameObject, 5f);
        PlayerGameManager.Instance.CollectSoulOrb();

        OnCollect.Invoke();
    }
}
