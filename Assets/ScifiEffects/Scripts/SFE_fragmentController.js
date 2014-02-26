//this controls the fragments that do not collide and eventually disappear.
#pragma strict

var impulseForce:float=10;
var impulseRot:float=10;
var shrinkTime:float=5;
private var lifetime:float=0;
private var rnd:float;

function Start () {
transform.rotation.x=Random.Range(-180, 180);
transform.rotation.y=Random.Range(-180, 180);
transform.rotation.z=Random.Range(-180, 180);


rnd=Random.Range(-impulseRot, impulseRot);
rigidbody.AddTorque(transform.up * rnd, ForceMode.Impulse);
rnd=Random.Range(-impulseRot, impulseRot);
rigidbody.AddTorque(transform.right * rnd, ForceMode.Impulse);
rnd=Random.Range(-impulseRot, impulseRot);
rigidbody.AddTorque(transform.forward * rnd, ForceMode.Impulse);


rigidbody.AddRelativeForce(Vector3(Random.Range(-impulseForce, impulseForce),Random.Range(-impulseForce, impulseForce), Random.Range(-impulseForce, impulseForce)), ForceMode.Impulse);



}

function Update () {
lifetime+=Time.deltaTime;

transform.localScale.x = 1 - (lifetime/shrinkTime);
transform.localScale.y = 1 - (lifetime/shrinkTime);
transform.localScale.z = 1 - (lifetime/shrinkTime);

if (shrinkTime<lifetime) Destroy(gameObject);


}