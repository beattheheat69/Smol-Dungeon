using Unity.VisualScripting;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    [SerializeField]
    CrossbowStats_SO baseStat; // Base stats of slimes

    //If hit damageable object does damage dans gets destroyed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            IDamageable hitObject = collision.gameObject.GetComponent<IDamageable>();
            hitObject.takeDamage(baseStat.power);

        }
        if (collision.tag != "Room")
        {
            Destroy(gameObject);
        }
        
    }

    //If arrow leaves room it's destroyed
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            Destroy(gameObject);
        }
    }
}
