#pragma strict

var rnd:float=10;

function Start () {
rigidbody.AddForce(Random.Range(-rnd, rnd), Random.Range(-rnd, rnd), Random.Range(-rnd, rnd), ForceMode.Impulse);
}

function Update () {

}