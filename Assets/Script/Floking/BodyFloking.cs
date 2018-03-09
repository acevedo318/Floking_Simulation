using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Body floking.
/// Cada instancia tiene que decidir por sí mismo qué bandadas considerar como su entorno.
/// Los modelos básicos del comportamiento flocking son controlados por tres sencillas reglas:
//1. Separación - evitar aglomeración de vecinos (repulsión de corto alcance)
//2. Alineación - dirigir hacia un rubro promedio de vecinos.
//3. Cohesión - dirigir hacia la posición media de los vecinos (atracción de largo alcance)

//Se calcula la velocidad de interaccion
/// </summary>
public class BodyFloking : MonoBehaviour {

	[SerializeField]
	private GameObject Controller;
	[SerializeField]
	private bool inited = false;
	[SerializeField]
	private float minVelocity;
	[SerializeField]
	private float maxVelocity;
	private float randomness;//Multiplicador del vector velocidad
	[SerializeField]
	private GameObject chasee;



	// Use this for initialization
	void Start () {
		StartCoroutine (BodySteering());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Vector De Interaccion
	private Vector3 Calculateinteraction ()
	{
		//Obtengo Valores negativos y positivos en el vector de manera aleatoria
		Vector3 randomize = new Vector3 ((Random.value *2) -1, (Random.value * 2) -1, (Random.value * 2) -1);
		//Hacer el vector con magnitud 1 y conservando su direccion 
		randomize.Normalize();

		BodyController boidController = Controller.GetComponent<BodyController>();

		//Posicion central del rebaño
		Vector3 flockCenter = boidController.flockCenter;
		//Velocidad promedio del rebaño
		Vector3 flockVelocity = boidController.flockVelocity;
		//Posicion del objeto donde iran
		Vector3 follow = chasee.transform.localPosition;
		//Darle los valores recogidos
		flockCenter = flockCenter - transform.localPosition;
		flockVelocity = flockVelocity - this.GetComponent<Rigidbody>().velocity;

		//Ubicacion del objeto a llegar
		follow = follow - transform.localPosition;

		//Devuelve velocidad de interaccion
		return (flockCenter + flockVelocity + follow * 2 + randomize * randomness);
	}

	//Dirigidor del cuerpo en corrutina para estar haciendose continuamente
	IEnumerator BodySteering ()
	{
		while (true)
		{
			if (inited)
			{
				GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + Calculateinteraction () * Time.deltaTime;

				// enforce minimum and maximum speeds for the boids
				float speed = GetComponent<Rigidbody>().velocity.magnitude;
				if (speed > maxVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
				}
				else if (speed < minVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minVelocity;
				}
			}
			//Cada 0.3 segundos a 0.5 segundos
			float waitTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds (waitTime);
		}
	}

	/// <summary>
	/// Recibir la informacion del BodyController
	/// </summary>
	/// <param name="theController">The controller.</param>
	public void SetController (GameObject theController)
	{
		Controller = theController;
		BodyController boidController = Controller.GetComponent<BodyController>();
		minVelocity = boidController.minVelocity; //Obtengo la minima velocidad del rebaño
		maxVelocity = boidController.maxVelocity; //Obtengo la maxima velocidad del rebaño
		randomness = boidController.randomness;
		chasee = boidController.chasee;
		inited = true;
	}
}
