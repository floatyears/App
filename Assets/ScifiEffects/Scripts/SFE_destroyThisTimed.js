//this very basic script destroys its gameobject in a pre-set amount of time. I use it all the time
//so effect holders and various stuffs do not remain in the scene.
var destroyTime:float=5;

function Start () {
Destroy (gameObject, destroyTime);
}
function Update () {
}