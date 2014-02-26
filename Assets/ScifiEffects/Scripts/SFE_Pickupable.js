#pragma strict


var onPickupGenerate:GameObject;



function Start () {

}

function Update () {

}

function OnCollisionEnter(collision : Collision) {

if (onPickupGenerate) Instantiate(onPickupGenerate, transform.position, transform.rotation);

Destroy(gameObject);

}