using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreenSizer : MonoBehaviour {

	private RectTransform pos;
	private VerticalLayoutGroup layout;

	void Start ()
	{
		layout = transform.FindChild ("Layout").gameObject.GetComponent<VerticalLayoutGroup>();
		pos = layout.GetComponent<RectTransform> ();
		Resize ();
	}

	void Resize()
	{
	}

}
