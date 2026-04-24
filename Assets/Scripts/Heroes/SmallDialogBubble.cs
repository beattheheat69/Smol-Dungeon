using System.Collections;
using UnityEngine;

public class SmallDialogBubble : MonoBehaviour
{
    public string[] dialog;
	MeshRenderer textMesh;
	TextMesh text;
	int randomDialog;

	private void Start()
	{
		randomDialog = Random.Range(0, dialog.Length);
		textMesh = GetComponent<MeshRenderer>();
		text = GetComponent<TextMesh>();
		text.text = dialog[randomDialog];
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

	public void StartCutscene()
	{
		this.gameObject.SetActive(true);
		randomDialog = Random.Range(0, dialog.Length);
		text.text = dialog[randomDialog];
		StartCoroutine(PlayDialog());
	}
}
