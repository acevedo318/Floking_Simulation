using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Body controller.
/// Recopilar Informacion,instanciar e ubicar las instancias 
/// </summary>
public class BodyController : MonoBehaviour {

	public float minVelocity = 5;
	public float maxVelocity = 20;
	public float randomness = 1;
	public int flockSize = 20; // tamaño del rebaño
	public GameObject prefabButerfly;
	public GameObject chasee;//Persecucion de target
	private Collider colliderBoid;//Colisionador De Agente


	public Vector3 flockCenter; //Ubicacion del centro del rebaño para que la camara lo siga
	public Vector3 flockVelocity;// Velocidad del movimiento

	private GameObject[] bodys;

	// Use this for initialization
	void Start () {

		bodys = new GameObject[flockSize];// Crear tamaño del rebaño
		colliderBoid = GetComponent<Collider>();
		(colliderBoid as SphereCollider).radius = flockSize*1.45f;// para tener mas espacio cuando se crean 
		InstancesFirstposition();
	}
	
	// Update is called once per frame
	void Update () {

		AverageInformation ();// Recopilar y actualizar informacion
		
	}
		
	/// <summary>
	/// Instanceses the firstposition.
	/// Ubico en una posicion aleatoria cada cuerpo y lo instancion
	/// Utiliza el colisionador como punto de generacion
	/// </summary>
	private void InstancesFirstposition(){

		//creo una posicion aleatoria
		for (var i=0; i<flockSize; i++)
		{
			Vector3 position = new Vector3 (
				                   Random.value * colliderBoid.bounds.size.x,
				                   Random.value * colliderBoid.bounds.size.y,
				                   Random.value * colliderBoid.bounds.size.z
			)- colliderBoid.bounds.extents ;

			//Instancio el cuerpo y lo ubico ademas lo guardo en el arreglo
			GameObject boid = Instantiate(prefabButerfly, transform.position, transform.rotation) as GameObject;
			boid.transform.parent = transform;// Se Hace Hijo del padre del script
			boid.transform.localPosition = position;
			boid.GetComponent<BodyFloking>().SetController(gameObject);
			bodys[i] = boid;
		}

	}

	//Recopilar informacion sobre los cuerpos,utilizando su rigibody y su ubicacion
	private void AverageInformation(){

		Vector3 theCenter = Vector3.zero;
		Vector3 theVelocity = Vector3.zero;

		foreach (GameObject boid in bodys)
		{
			theCenter = theCenter + boid.transform.localPosition;
			theVelocity = theVelocity + boid.GetComponent<Rigidbody>().velocity;
		}

		flockCenter = theCenter/(flockSize);
		flockVelocity = theVelocity/(flockSize);

	}
}
