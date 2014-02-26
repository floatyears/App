//controls a bullet. Adds speed, detaches stuff when the bullet is destroyed, for example particle systems that still
//need to live a while after the bullet is destroyed.
#pragma strict

var impulseForce:float=10;
var muzzleFire:GameObject;
var explosion:GameObject;
var damage:float;
var detachOnDeath:GameObject[];

function Start () {
if (muzzleFire) Instantiate(muzzleFire, transform.position, transform.rotation);
rigidbody.AddForce(transform.up * impulseForce, ForceMode.Impulse);

}

function Update () {

}

function OnCollisionEnter(collision : Collision) {

Instantiate(explosion, transform.position, transform.rotation);

if (detachOnDeath) {
	for(var i=0;i < detachOnDeath.length; i++)
	{
	detachOnDeath[i].transform.parent=null;
	var PS : ParticleSystem;  
	PS = detachOnDeath[i].GetComponent(ParticleSystem);
	PS.enableEmission=false;
	Destroy(detachOnDeath[i], 5);
	}
}


Destroy (gameObject);

}