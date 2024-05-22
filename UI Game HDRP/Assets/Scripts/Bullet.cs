using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	//Time before bullet gets destroyed
	float bulletTravelTime = 0.5f;

	//Setting the bullet to be destroyed after X time
	private void Start()
	{
		Destroy(gameObject,bulletTravelTime);
	}

}
