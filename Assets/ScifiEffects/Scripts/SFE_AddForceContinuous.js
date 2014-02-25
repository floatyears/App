#pragma strict

var force:float=10;

function Start () {

}

function Update () {
rigidbody.AddForce(transform.up * force*Time.deltaTime, ForceMode.Impulse);
}