using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using bbproto;

public class EffectManager {

	private Dictionary<string, ParticleSystem> skillEffectPool = new Dictionary<string, ParticleSystem> ();
	private Dictionary<string, ParticleEffectItem> currentEffects = new Dictionary<string, ParticleEffectItem> ();

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
		foreach (var psItem in currentEffects.Values) {
			string path = psItem.pathAndId.Split('_')[0];
#if !UNITY_EDITOR
			if(psItem.ps == null){
				tobeRemoved.Add(psItem);
				if(psItem.callback != null){
					psItem.callback(psItem.ps);
					psItem.callback = null;
				}
			}else if(!psItem.ps.IsAlive() || psItem.ps.isStopped){
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
			if(!psItem.ps.IsAlive() || psItem.ps.isStopped){
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

		foreach (var item in tobeRemoved) {
			Debug.Log ("remove effect: " + currentEffects.Remove(item));
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
		string pid = path + "_" + parent.GetInstanceID();
		ParticleSystem ps;
		if (currentEffects.ContainsKey (pid)) {
			Debug.Log("effect playing: " + pid);
			ParticleEffectItem pItem = currentEffects[pid];
			pItem.ps.Clear();
			if(pItem.callback != null){
				pItem.callback(pItem.ps);
			}
			pItem.ps.Play();
		}else{
			if (skillEffectPool.ContainsKey (path)) {
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
				
			}else{
				ResourceManager.Instance.LoadLocalAsset("Effect/effect/" + path, o => {
					if(o != null) {
						GameObject obj = GameObject.Instantiate(o) as GameObject;
						obj.transform.parent = parent;
						obj.transform.localScale = Vector3.one;
						obj.transform.localPosition = pos;
						ps = obj.GetComponent<ParticleSystem>();
						if(ps == null){
							throw new ArgumentException("The Effect isnt't ParticleSystem!");
							return;
						}
						skillEffectPool.Add(path,ps);
						currentEffects.Add(pid,new ParticleEffectItem(pid,ps,effectEndCallback));
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
		skillEffectPool.Clear ();
	}
}
 
class ParticleEffectItem{
	public string pathAndId;
	public ParticleSystem ps;
	public DataListener callback;

	public ParticleEffectItem(string pathAndId, ParticleSystem  ps, DataListener callback){
		this.pathAndId = pathAndId;
		this.ps = ps;
		this.callback = callback;
	}
}
