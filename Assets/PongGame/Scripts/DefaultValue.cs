using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.Assertions;
using System;

[RequireComponent(typeof(TMP_InputField))]
public class DefaultValue : MonoBehaviour
{
	[SerializeField]
	private PongMenuManager.FIELD field = PongMenuManager.FIELD.HOST_NAME;

	private PongMenuManager menu;

	private TMP_InputField inputField;
	// Start is called before the first frame update
	void Start()
	{
		menu = FindObjectOfType<PongMenuManager>();
		Assert.IsNotNull(menu, "PongMenuManager not found in scene");

		inputField = GetComponent<TMP_InputField>();
		inputField.text = GetFieldValue();
	}

	private string GetFieldValue()
	{
		// get field value from PongMenuManager
		return menu.GetField(field);
	}
}
