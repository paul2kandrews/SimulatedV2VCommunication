using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public bool isEnd;
	public GameObject node;
	public GameObject? connection = null;
	public Renderer rend;

	private void Awake()
	{
		if (gameObject.name == "end")
		{
			isEnd = true;
		}
		else { isEnd = false; }

		rend = GetComponent<Renderer>();
		rend.enabled = false;
	}
}
