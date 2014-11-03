using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using bbproto;

public class EffectManager {

	private Dictionary<string, ParticleSystem> skillEffectPool = new Dictionary<string, ParticleSystem> ();
	private Dictionary<string, ParticleEffectItem> currentEffects = new Dictionary<string, ParticleEffectItem> ();

	private Dictionary<string, GameObjectEffItem> skillObjPool = new Dictionary<string, GameObjectEffItem> ();
	private Dictionary<string, EffectObjectItem> currEffObjs = new Dictionary<string, EffectObjectItem> ();

	private static EffectManager instance;
	public static EffectManager Instance {
		get{
			if(instance == null) {
				instance = new EffectManager();
			}
			return instance;
		}
	}
	
	private Transform EffectPoolRoot;
	private EffectManager() {
		EffectPoolRoot = GameObject.Find ("EffectPool").transform;
		EffectPoolRoot.gameObject.SetActive (false);
		GameInput.OnUpdate += OnUpdate;
	}

	private List<string> tobeRemoved = new List<string>();
	void OnUpdate(){
		string path;
		foreach (var psItem in currentEffects.Values) {
			path = psItem.pathAndId.Split('_')[0];
#if !UNITY_EDITOR
			if(psItem.ps == null){
				tobeRemoved.Add(psItem.pathAndId);
				if(psItem.callback != null){
					psItem.callback(psItem.ps);
					psItem.callback = null;
				}
			}else if(!psItem.ps.IsAlive()){
				if(skillEffectPool.ContainsKey(path)){
					if(skillEffectPool[path] != psItem.ps){
						GameObject.Destroy(psItem.ps.gameObject);
					}else{
						psItem.ps.transform.parent = EffectPoolRoot;
					}
				}
				if(psItem.callback != null){
					psItem.callback(psItem.ps);
					psItem.callback = null;
				}
				
				tobeRemoved.Add(psItem.pathAndId);
			}
#else
			if(!psItem.ps.IsAlive()){
				if(skillEffectPool.ContainsKey(path)){
					if(skillEffectPool[path] != psItem.ps){
						GameObject.Destroy(psItem.ps.gameObject);
					}else{
						psItem.ps.transform.parent = EffectPoolRoot;
					}
				}
				if(psItem.callback != null){
					psItem.callback(psItem.ps);
					psItem.callback = null;
				}
				
				tobeRemoved.Add(psItem.pathAndId);
			}
#endif
		}

		foreach (var eoItem in currEffObjs.Values) {
			path = eoItem.pathAndId.Split('_')[0];
			eoItem.item.currTime += Time.deltaTime;
			if(!eoItem.item.IsAlive()){
				if(skillObjPool.ContainsKey(path)){
					if(skillObjPool[path] != eoItem.item){
						GameObject.Destroy(eoItem.item.obj);
					}else{
						eoItem.item.obj.transform.parent = EffectPoolRoot;
					}
				}
				if(eoItem.callback != null){
					eoItem.callback(eoItem.item.obj);
					eoItem.callback = null;
				}
				tobeRemoved.Add(eoItem.pathAndId);
			}
		}
		foreach (var item in tobeRemoved) {
//			Debug.Log ("remove effect: " + );
			if(currEffObjs.ContainsKey(item)){
				currEffObjs.Remove(item);
			}else{
				currentEffects.Remove(item);
			}
		}
		tobeRemoved.Clear ();
	}

	/// <summary>
	/// Play the effect.
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="pos">Position.</param>
	/// <param name="effectEndCallback">Effect end callback.</param>
	public void PlayEffect(string path, Transform parent, Vector3 pos = default(Vector3), DataListener effectEndCallback = null){
		if (string.IsNullOrEmpty (path)) {
			return;		
		}
//		Debug.Log ("Effect Play: [[[---" + path + "---]]]");
		string pid = path + "_" + parent.GetInstanceID();
		ParticleSystem ps = null;
		GameObjectEffItem ei = null;
		if (currentEffects.ContainsKey (pid)) {
			Debug.Log("effect playing: " + pid);
			ParticleEffectItem pItem = currentEffects[pid];
			pItem.ps.Clear();
			if(pItem.callback != null){
				pItem.callback(pItem.ps);
			}
			pItem.ps.transform.localPosition = pos;
			pItem.ps.Play();
		}else if(currEffObjs.ContainsKey(pid)){
			Debug.Log("effect playing: " + pid);
			EffectObjectItem eItem = currEffObjs[pid];
			if(eItem.callback != null){
				eItem.callback(eItem.item.obj);
			}
			eItem.item.obj.transform.localPosition = pos;
			eItem.item.Play();
		}else{
			if (skillEffectPool.ContainsKey (path)) { //particle system effect
				if(skillEffectPool[path].IsAlive()){
					ps = GameObject.Instantiate(skillEffectPool[path]) as ParticleSystem;
				}else{
					ps = skillEffectPool[path];
				}
				ps.transform.parent = parent;
				ps.transform.localPosition = pos;
				ps.Clear();
				currentEffects.Add(pid,new ParticleEffectItem(pid,ps,effectEndCallback));
				ps.Play();
				
			}else if(skillObjPool.ContainsKey(path)){ //gameobject effect
				ei = skillObjPool[path];
				GameObject effObj;
				if(ei.currTime < ei.callbackTime){
					effObj = GameObject.Instantiate(ei.obj) as GameObject;
				}else{
					effObj = ei.obj;
				}
				effObj.transform.parent = parent;
				effObj.transform.localPosition = pos;
				currEffObjs.Add(pid,new EffectObjectItem(pid,ei,effectEndCallback));
				ei.Play();
			}else{
				ResourceManager.Instance.LoadLocalAsset("Effect/effect/" + path, o => {
					if(o != null) {
						GameObject obj = GameObject.Instantiate(o) as GameObject;
						obj.transform.parent = parent;
						obj.transform.localScale = Vector3.one;
						obj.transform.localPosition = pos;
						ps = obj.GetComponent<ParticleSystem>();
						if(ps == null){
							Debug.LogWarning("The Effect isnt't ParticleSystem!");
							ei = new GameObjectEffItem(obj,1.5f);
							skillObjPool.Add(path,ei);
							ei.Play();
							currEffObjs.Add(pid,new EffectObjectItem(pid,ei,effectEndCallback));
						}else{
							skillEffectPool.Add(path,ps);
							currentEffects.Add(pid,new ParticleEffectItem(pid,ps,effectEndCallback));
						}

					}
				});
			}
		}

	}

	/// <summary>
	/// Plaies the effect.
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="pos">Position.</param>
	/// <param name="keyTimeCallback">Key time callback. The num of the params must be even, first is time, second is the callback</param>
	void PlayEffect(string path, Transform parent, Vector3 pos = default(Vector3), params object[] keyTimeCallback){
		if (keyTimeCallback.Length % 2 != 0) {
			throw new ArgumentException("The Num of the Parameters Must Be Even!");
			return;
		}
	}

	/// <summary>
	/// Stops the effect.
	/// </summary>
	/// <param name="path">Path.</param>
	/// <param name="parent">Parent.</param>
	public void StopEffect(string path, Transform parent){
		string pid = path + "_" + parent.GetInstanceID();
		if (currentEffects.ContainsKey (pid)) {
			ParticleEffectItem pItem = currentEffects[pid];
			if(pItem.callback != null){
				pItem.callback(pItem.ps);
			}
			pItem.ps.Stop();
			pItem.ps.Clear();
			currentEffects.Remove(pid);
			
			if(skillEffectPool.ContainsKey(path)){
				if(skillEffectPool[path] != pItem.ps){
					GameObject.Destroy(pItem.ps.gameObject);
				}else{
					pItem.ps.transform.parent = EffectPoolRoot;
				}
			}
		}
	}

	public void ClearCache() {
		foreach (var item in skillEffectPool) {
			GameObject.Destroy(item.Value.gameObject);
		}
		foreach (var item in currentEffects) {
			GameObject.Destroy(item.Value.ps.gameObject);
			item.Value.callback = null;
		}
		foreach (var item in skillObjPool) {
			GameObject.Destroy(item.Value.obj);
		}
		foreach (var item in currEffObjs) {
			GameObject.Destroy(item.Value.item.obj);
			item.Value.callback = null;
		}
		currEffObjs.Clear ();
		skillObjPool.Clear ();
		currentEffects.Clear ();
		skillEffectPool.Clear ();
	}
}
 
class ParticleEffectItem{
	public string pathAndId;
	public ParticleSystem ps;
	public DataListener callback;

	public ParticleEffectItem(string pathAndId, ParticleSystem  ps, DataListener callback,GameObject effObj = null){
		this.pathAndId = pathAndId;
		this.ps = ps;
		this.callback = callback;

	}
}

class EffectObjectItem{
	public string pathAndId;
	public GameObjectEffItem item;
	public DataListener callback;

	public EffectObjectItem(string pathAndId, GameObjectEffItem item, DataListener callback){
		this.pathAndId = pathAndId;
		this.callback = callback;
		this.item = item;
	}
}

class GameObjectEffItem{
	public GameObject obj;
	public float callbackTime;

	public float currTime;

	public GameObjectEffItem(GameObject obj,float effectTime){
		this.obj = obj;
		callbackTime = effectTime;
		currTime = 0f;
	}

	public void Play(){
		obj.SetActive (false);
		currTime = 0f;
		obj.SetActive (true);
	}

	public bool IsAlive(){
		return currTime < callbackTime;
	}
}
