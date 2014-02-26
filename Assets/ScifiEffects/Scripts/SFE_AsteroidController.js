//despite the name, it controls everything that can be shot and does not have a shield.

#pragma strict

var impulseForce:float=10;
var HPmin:float=3;
var HPmax:float=6;
private var HP:float;
var explosion:GameObject;

function Start () {
HP=Random.Range(HPmin, HPmax);
}

function Update () {
}

function OnCollisionEnter(collision : Collision) {
if (collision.gameObject.GetComponent(SFE_BulletController))
HP-=collision.gameObject.GetComponent(SFE_BulletController).damage;

if (collision.gameObject.GetComponent(SFE_LaserController))
HP-=collision.gameObject.GetComponent(SFE_LaserController).damage;




if (HP<=0)
{
 Instantiate(explosion, transform.position, transform.rotation);
 Destroy (gameObject);

}

}