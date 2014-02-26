//this script plays a sound. The trick is that the sound stays played even if the object is destroyed while playing the script.
//the idea is that the script generates a temporarly object that plays the sound.

#pragma strict
    
var soundEffect:AudioClip;
var minPitch:float=0.9;
var maxPitch:float=1.1;


function Start () {


PlayClipAt(soundEffect, transform.position);

}

function Update () {

}

function PlayClipAt(clip: AudioClip, pos: Vector3): AudioSource {
  var tempGO = GameObject("TempAudio"); 
  tempGO.transform.position = pos; 
  var aSource = tempGO.AddComponent(AudioSource); 
  aSource.clip = soundEffect; 
  aSource.pitch=Random.Range(minPitch, maxPitch);
// soundEffect.pitch = 1.5;
  
  aSource.Play(); 
  Destroy(tempGO, clip.length); 
  return aSource; 
}