using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameManager : MonoBehaviour
{
    private static PlayerGameManager _instance;

    public static PlayerGameManager Instance { get { return _instance; } }

    [Header("UI")]
    public SoulCounterUI SoulCounterUI;
    public TMPro.TextMeshProUGUI GunText;

    private int numSoulOrbsCollected = 0;

    public Transform SoulCollectTransform;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void CollectSoulOrb()
    {
        numSoulOrbsCollected++;
        // update UI
        SoulCounterUI.ShowSoulCount(numSoulOrbsCollected);
        // update gun UI
        GunText.text = SoulCounterUI.ToRoman(numSoulOrbsCollected);
    }

    public int GetNumSoulOrbsCollected()
    {
        return numSoulOrbsCollected;
    }
}
