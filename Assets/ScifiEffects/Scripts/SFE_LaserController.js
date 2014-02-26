//this laser controller script checks if the laser hits something, adds collision, deals with damage, impact effect, etc.
//it is needed once per laser effect in the demo scene
// it does not actually draw a laser beam

var boxCollider:BoxCollider;
var damage:float=3;

var impactEffect:GameObject;
var muzzleEffect:GameObject;






function Start()
{	
Destroy(boxCollider, 0.08);  //don't ask.... just to make extra sure the box collider doesn't stay out too long, even if... eh it is complicated, this is needed.
if (muzzleEffect) Instantiate(muzzleEffect, transform.position, transform.rotation);


var direction = transform.TransformDirection(Vector3.up);
var hit : RaycastHit;
 
	if (Physics.Raycast (transform.position, direction, hit)) 
	{ // if it hits something, this happens
	boxCollider.size.y=hit.distance;  //I set up a box collider to be along the laser...
	boxCollider.center.y+=hit.distance/2;
	if (impactEffect) Instantiate(impactEffect, hit.point, hit.transform.rotation);
	}
	else   //if the raycast hits nothing
	{
	Destroy(boxCollider); //if the laser didn't hit anything, then we have to get rid of the box collider, else it just stays there...
	}




}

function Update() 
{


}

function OnCollisionEnter(collision : Collision) { //we just want to hit it once, so after a collision we destroy the collider...
Destroy(boxCollider);
}
  