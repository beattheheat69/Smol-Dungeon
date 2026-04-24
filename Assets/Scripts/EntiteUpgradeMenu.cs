using UnityEngine;

public class EntiteUpgradeMenu : MonoBehaviour
{
    [SerializeField]
    int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (EvilXPCount.upgrades[id])
        {
            gameObject.SetActive(false);
        }
    }
}
