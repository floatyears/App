//This is responsible that the cursor follows the mouse, and that an effect plays if the user clicks
// also this script handles changing the effects.

var moveThis : GameObject; //this is an invisible "cursor" that is always there where the mouse points
var camRoot:GameObject;
var camPoint1:GameObject;
var camPoint2:GameObject;
private var camCurrent:int=1;
var spaceShip : GameObject;
private var hit : RaycastHit;
var createThis : GameObject[];
private var cooldown : float;
private var changeCooldown : float;
private var selected:int=0;
var writeThis:GUIText;
private var rndNr:float;
private var effect:GameObject;
var layermask:LayerMask;


function Start () {

selected=createThis.length-1;

writeThis.text=selected.ToString()+" "+createThis[selected].name;

}

function Update () {

if(cooldown>0){cooldown-=Time.deltaTime;}
if(changeCooldown>0){changeCooldown-=Time.deltaTime;}

var ray = Camera.main.ScreenPointToRay (Input.mousePosition);

if (Physics.Raycast (ray, hit, 1000, layermask)) {
moveThis.transform.position=hit.point;


if(Input.GetMouseButton(0)&&cooldown<=0){
effect=Instantiate(createThis[selected], spaceShip.transform.position, spaceShip.transform.rotation);
effect.transform.parent=spaceShip.transform;

cooldown=0.5;
}



}


if (Input.GetKeyDown(KeyCode.UpArrow) && changeCooldown<=0)
{
	selected+=1;
		if(selected>(createThis.length-1)) {selected=0;}
	
	writeThis.text=selected.ToString()+" "+createThis[selected].name;
	changeCooldown=0.1;
}

if (Input.GetKeyDown(KeyCode.DownArrow) && changeCooldown<=0)
{
	selected-=1;
		if(selected<0) {selected=createThis.length-1;}
	
		writeThis.text=selected.ToString()+" "+createThis[selected].name;
	changeCooldown=0.1;
}

if (Input.GetKeyDown(KeyCode.Space) && changeCooldown<=0)
{
if (camCurrent==1)
{
camCurrent=2;
camRoot.transform.position=camPoint2.transform.position;
camRoot.transform.rotation=camPoint2.transform.rotation;
}
else
{
camCurrent=1;
camRoot.transform.position=camPoint1.transform.position;
camRoot.transform.rotation=camPoint1.transform.rotation;
}


	changeCooldown=0.1;
}



}