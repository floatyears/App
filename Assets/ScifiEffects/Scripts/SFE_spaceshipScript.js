//this script controls the spaceship. It is very basic and a bit hacky.

#pragma strict

var myTarget:GameObject;

function Start () {

}

function Update () {

transform.position.x=myTarget.transform.position.x;

}