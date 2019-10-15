using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxisRaw ("1D-Pad Vertical") != 0 && buttonSelected == false) 
		{
			if((!SceneManager.GetActiveScene().name.Equals("MainScene")) || (SceneManager.GetActiveScene().name.Equals("MainScene") && Time.timeScale == 0f)){
				eventSystem.SetSelectedGameObject(selectedObject);
				buttonSelected = true;
			}
		}
	}

	private void OnDisable()
	{
		buttonSelected = false;
	}

	//public void OnPointerEnter(PointerEventData eventData){
	//	if (!buttonSelected) {
	//		eventSystem.SetSelectedGameObject(selectedObject);
	//		buttonSelected = true;
	//	}
	//}
}