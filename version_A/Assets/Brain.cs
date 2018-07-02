using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	private int DNALength = 6; // 2 genes; 'what should I do when I can see the platform' and 'what should I do when I can't see the platform'
	public float timeAlive;
	public float distanceTravelled;
	public float timeWalking;
	public DNA dna;
	public GameObject eyes; // Link to eyes because eyes tell us whether we still see the platform or not
	bool alive = true;
	Vector3 startPosition;
	Quaternion spreadAngleRight;
	Quaternion spreadAngleLeft;
	Vector3 leftrayVector;
	Vector3 rightrayVector;
	public int rayAngle = 45;
	bool mCollide = true;
	bool lCollide = true;
	bool rCollide = true;

	//public GameObject ethanPrefab;
	//GameObject ethan;

//	void OnDestroy()
//	{
//		Destroy(ethan);
//	}

	void OnCollisionEnter(Collision obj)
	{
		if(obj.gameObject.tag == "wall")
		{
			alive = false;
			timeWalking = 0;
			distanceTravelled = distanceTravelled/2;
		}
	}

	public void Init()
	{
		/*initialise DNA
		  0 forward
		  1 rotate*/
		dna = new DNA(DNALength, 2);
		timeAlive = 0;
		timeWalking = 0;
		alive = true;
		distanceTravelled = 0;
		startPosition = this.transform.position;

		//ethan = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
		//ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
	}

	private void Update()
	{
		if(!alive) return;

		spreadAngleRight = Quaternion.AngleAxis(rayAngle, new Vector3(0, 1, 0));
		spreadAngleLeft = Quaternion.AngleAxis(-rayAngle, new Vector3(0, 1, 0));
		rightrayVector = spreadAngleRight * eyes.transform.forward;
		leftrayVector = spreadAngleLeft * eyes.transform.forward;

		Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 0.2f, Color.red, 10);
		Debug.DrawRay(eyes.transform.position, leftrayVector * 0.2f, Color.red, 10);
		Debug.DrawRay(eyes.transform.position, rightrayVector * 0.2f, Color.red, 10);
		mCollide = false;
		rCollide = false;
		lCollide = false;
		RaycastHit midHit;
		RaycastHit leftHit;
		RaycastHit rightHit;
		if(Physics.Raycast(eyes.transform.position, eyes.transform.forward * 0.2f, out midHit))
		{
			if(midHit.collider.gameObject.tag == "wall")
			{
				mCollide = true;
			}
		}
		if(Physics.Raycast(eyes.transform.position, leftrayVector * 0.2f, out leftHit))
		{
			if(leftHit.collider.gameObject.tag == "wall")
			{
				lCollide = true;
			}
		}
		if(Physics.Raycast(eyes.transform.position, rightrayVector * 0.2f, out rightHit))
		{
			if(rightHit.collider.gameObject.tag == "wall")
			{
				rCollide = true;
			}
		}

		timeAlive = PopulationManager.elapsed;

		//read DNA
		float h = 0;
		float v = 1;
		/*	if(seeGround)
			{
				 //make v relative to character and always move forward
				if(dna.GetGene(0) == 0) {v = 1; timeWalking += 1;}
				else if(dna.GetGene(0) == 1) h = -90;
				else if(dna.GetGene(0) == 2) h = 90;
			}
			else
			{
				if(dna.GetGene(1) == 0) {v = 1; timeWalking += 1;}
				else if(dna.GetGene(1) == 1) h = -90;
				else if(dna.GetGene(1) == 2) h = 90;
			}
		*/
		if(lCollide && mCollide && rCollide)
		{
			v= 0.5f;
			if(dna.GetGene(0) == 0) h = -10;
			else if(dna.GetGene(0) == 1) ;
		}
		else if(rCollide && mCollide)
		{
			if(dna.GetGene(1) == 0) ;
			else if(dna.GetGene(1) == 1) h = -10;
		}
		else if(lCollide && mCollide)
		{
			if(dna.GetGene(2) == 0) ;
			else if(dna.GetGene(2) == 1) h = 10;
		}
		else if(lCollide)
		{
			if(dna.GetGene(3) == 0) ;
			else if(dna.GetGene(3) == 1) h = 10; 
		}
		else if(rCollide)
		{
			if(dna.GetGene(4) == 0) ;
			else if(dna.GetGene(4) == 1) h = -10;
		}
		else
		{
			if(dna.GetGene(5) == 0) ;
			else if(dna.GetGene(5) == 1) ; 
		}
		
		this.transform.Translate(0, 0, v*0.1f);
		this.transform.Rotate(0, h, 0);
		distanceTravelled += Vector3.Distance(startPosition, this.transform.position);
	}
	
}