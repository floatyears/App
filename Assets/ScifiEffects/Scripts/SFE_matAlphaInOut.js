
var alphaIn:float=2;
var alphaStay:float=1;
var alphaOut:float=3;

var otherColors:float=0.5;
private var time:float=0;
private var alpha:float=0;

var killObjectOnEnd:boolean=true;

function Start () {

if (alphaIn<=0)  //debug hack...
{
alphaIn=0.1;
Debug.Log("Please don't set AlphaIn to zero or below...(matAlphaInOut script)");
}

if (alphaOut<=0)  //debug hack...
{
alphaOut=0.1;
Debug.Log("Please don't set AlphaOut to zero or below...(matAlphaInOut script)");
}


renderer.material.SetColor("_TintColor", Color(otherColors, otherColors, otherColors ,alpha));

}


function Update () {

time+=Time.deltaTime;

if (time<alphaIn)  //fading in
{
alpha=time/alphaIn;

}


if (time>=alphaIn && time<(alphaIn+alphaStay))  //staying
{
alpha=1;

}


if (time>=alphaIn+alphaStay && time<(alphaIn+alphaStay+alphaOut))  //fading out
{
alpha=1-((time-(alphaIn+alphaStay))/alphaOut);

}



renderer.material.SetColor("_TintColor", Color(otherColors, otherColors, otherColors ,alpha));

if (time>=(alphaIn+alphaStay+alphaOut) && killObjectOnEnd==true) 
{
Destroy(gameObject);
//Debug.Log("Destroyed.,,");
}


}