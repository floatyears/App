//The point of this script is to generate a prefab.


var createThis:GameObject; 
var detachToWorld:boolean=true;

function Start () {
var justCreated:GameObject=Instantiate(createThis, transform.position, transform.rotation);  //create the prefab
if (detachToWorld==false) justCreated.transform.parent=transform;
}


function Update () {
}

function OnDrawGizmosSelected () {
Gizmos.color = Color (1,1,1,.5);
Gizmos.DrawSphere (transform.position, 0.3);
}

function OnDrawGizmos () {
Gizmos.color = Color (1,1,1,.1);
Gizmos.DrawSphere (transform.position, 0.3);
}