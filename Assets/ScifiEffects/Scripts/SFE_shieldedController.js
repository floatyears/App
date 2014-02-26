// this script handles a space object's HP and Shield parameters.

#pragma strict

var impulseForce:float=10;
var HPmin:float=3;
var HPmax:float=6;

var shieldMin:float=5;
var shieldMax:float=5;

var shieldObject:GameObject;

var shieldGraphicsStuff:String="---------------------------------";

var normalShieldGfxPower:float=1;
var onHitShieldGfxPower:float=10;
var onHitShieldGfxCooldownSpeed:float=1;

private var power:float;

var onHitShieldGenerate:GameObject;
var onDestroyShieldGenerate:GameObject;

private var HP:float;
private var shield:float;
var explosion:GameObject;


function Start () {
//setting HP and shield parameters between random values

HP=Random.Range(HPmin, HPmax);
shield=Random.Range(shieldMin, shieldMax);




power=normalShieldGfxPower;
shieldObject.renderer.material.SetFloat("_AllPower", normalShieldGfxPower);
}

function Update () {

if (shieldObject)
{
shieldObject.renderer.material.SetFloat("_AllPower", power);
if (power>normalShieldGfxPower) power-=Time.deltaTime*onHitShieldGfxCooldownSpeed;
if (power<normalShieldGfxPower) power=normalShieldGfxPower;
}



}

function OnCollisionEnter(collision : Collision) {  

if (shieldObject) power=onHitShieldGfxPower;

/*
this basically does the following:
checks if there are shields remaining, if no, then the damage goes to HP
else it gets substracted from the shield, and if the shield is below 0, then destroys the shield


it is not super great as damage now doesn't "overbleed" through the shield; even if a 10 strength attack hits
an 1 strength shield, the shield still completely blocks it before being destroyed, but this whole
scene is just an effect demo package, so meh
*/

if (shield<=0)
{
if (collision.gameObject.GetComponent(SFE_BulletController))
HP-=collision.gameObject.GetComponent(SFE_BulletController).damage;

if (collision.gameObject.GetComponent(SFE_LaserController))
HP-=collision.gameObject.GetComponent(SFE_LaserController).damage;
}


if (shield>0)
{

if ((shield>0) && (onHitShieldGenerate)) {

      var contact = collision.contacts[0];
        var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        var pos = contact.point;
        Instantiate(onHitShieldGenerate, pos, rot);

}


if (collision.gameObject.GetComponent(SFE_BulletController))
shield-=collision.gameObject.GetComponent(SFE_BulletController).damage;

if (collision.gameObject.GetComponent(SFE_LaserController))
shield-=collision.gameObject.GetComponent(SFE_LaserController).damage;

if (shield<=0) {
Destroy(shieldObject);
if (onDestroyShieldGenerate) Instantiate(onDestroyShieldGenerate, transform.position, transform.rotation);
}



}





if (HP<=0)  //yep, if the object does not have any HP left, it gets destroyed, and an explosion is created
{
 Instantiate(explosion, transform.position, transform.rotation);
 Destroy (gameObject);

}



}