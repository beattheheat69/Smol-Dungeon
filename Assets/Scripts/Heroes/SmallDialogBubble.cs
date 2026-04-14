using System.Collections;
using UnityEngine;

public class SmallDialogBubble : MonoBehaviour
{
    public string dialog;
	MeshRenderer textMesh;

	private void Start()
	{
		textMesh = GetComponent<MeshRenderer>();
		StartCoroutine(PlayDialog());
	}

	IEnumerator PlayDialog()
	{
		yield return new WaitForSeconds(0.5f);
		textMesh.enabled = true;
		yield return new WaitForSeconds(2.5f);
		textMesh.enabled = false;
		this.gameObject.SetActive(false);
	}
}
