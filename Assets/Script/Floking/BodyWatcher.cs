using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWatcher : MonoBehaviour {


	private Transform transformBodyController; // Transform del objeto controlador
	[SerializeField]
	private BodyController bodyController;

	// Use this for initialization
	void Start () {

		if(bodyController == null){
			Debug.LogWarning ("Transform no asignado");
			bodyController = (BodyController)GameObject.FindObjectOfType (typeof(BodyController));
		}
		transformBodyController = bodyController.gameObject.GetComponent<Transform> ();

	}
	
	//Se llama a LateUpdate después de haber llamado a todas las funciones de actualización. Esto es útil para ordenar la ejecución de scripts. Por ejemplo, una cámara follow debería implementarse siempre en LateUpdate porque rastrea los objetos que podrían haberse movido dentro de Update.
	void LateUpdate ()
	{
		//Si no es nulo realizar la tarea de seguimiento
		if (bodyController)
		{
			Vector3 watchPoint = bodyController.flockCenter;
			transform.LookAt(watchPoint+transformBodyController.position);

		}
	}
}
