//this script detachs the object to the world immediately. Useful in bullets for example, 
//so they don't remain the child of the spaceship and do not move with it.

#pragma strict



function Start () {
transform.parent=null;

}

function Update () {

}