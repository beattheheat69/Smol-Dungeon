using Unity.VisualScripting;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    [SerializeField]
    CrossbowStats_SO baseStat; // Base stats of slimes
    /*private void OnTriggerEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            IDamageable hitObject = collision.gameObject.GetComponent<IDamageable>();
            hitObject.takeDamage(baseStat.power);
            Debug.Log("hit something");
        }

        Destroy(gameObject);
    }*/

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            Destroy(gameObject);
        }
    }
}
