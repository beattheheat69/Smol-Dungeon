using System.Collections;
using UnityEngine;

public class BossKnockback : MonoBehaviour
{
	float thrust = 10f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero"))
		{
			Debug.Log("Knockback time");
			Vector2 difference = other.transform.position - GetComponentInParent<Transform>().position;
			difference = difference.normalized * thrust;
			other.GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse);
			StartCoroutine(ResetKnockback(other.GetComponent<Rigidbody2D>()));
		}
	}

	IEnumerator ResetKnockback(Rigidbody2D rb)
	{
		yield return new WaitForSeconds(0.3f);
		rb.linearDamping = 0f;
		rb.linearVelocity = Vector2.zero;
	}
}
