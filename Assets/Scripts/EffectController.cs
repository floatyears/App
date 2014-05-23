using UnityEngine;
using System.Collections;


public class EffectController : MonoBehaviour {

    public Transform ObjectCache;
    protected XffectCache EffectCache;

	void Start () 
	{
        EffectCache = ObjectCache.GetComponent<XffectCache>();
		
	}

    protected Vector3 GetFaceDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    void OnEffect(string eftname)
    {
        Xffect xft;
        if (eftname == "lightning")
        {
            for (int i = 0; i < 10; i++)
            {
                xft = EffectCache.GetObject(eftname).GetComponent<Xffect>();
                Vector3 emitpoint = Vector3.zero;
                emitpoint.x = Random.Range(-2.2f, 2.3f);
                emitpoint.z = Random.Range(-2.1f, 2.1f);
                xft.SetEmitPosition(emitpoint);
                xft.Active();
            }
        }
        else if (eftname == "cyclone")
        {
            xft = EffectCache.GetObject(eftname).GetComponent<Xffect>();
            xft.SetDirectionAxis(GetFaceDirection().normalized);
            xft.Active();
        }
        else if (eftname == "crystal")
        {
            xft = EffectCache.GetObject("crystal_surround").GetComponent<Xffect>();
            xft.Active();

            xft = EffectCache.GetObject("crystal").GetComponent<Xffect>();
            xft.SetEmitPosition(new Vector3(0, 1.9f, 1.4f));
            xft.Active();

            xft = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
            xft.SetDirectionAxis(new Vector3(-1.5f, 1.8f, 0f));
            xft.Active();

            xft = EffectCache.GetObject("crystal").GetComponent<Xffect>();
            xft.SetEmitPosition(new Vector3(0, 1.5f, -1.2f));
            xft.Active();

            xft = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
            xft.SetDirectionAxis(new Vector3(1.4f, 1.4f, 0f));
            xft.Active();
        }
        else
        {
            xft = EffectCache.GetObject(eftname).GetComponent<Xffect>();
            xft.Active();
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 225), "Effect List");
        GUI.Label(new Rect(150, 0, 350, 25), "alt+left mouse button to rotation.  mouse wheel to zoom.");
        if (GUI.Button(new Rect(10, 20, 80, 20), "crystal"))
        {
            OnEffect("crystal");
        }
        if (GUI.Button(new Rect(10, 45, 80, 20), "rage_explode"))
        {
            OnEffect("rage_explode");
        }
        if ((GUI.Button(new Rect(10, 70, 80, 20), "cyclone")))
        {
            OnEffect("cyclone");
        }
        if ((GUI.Button(new Rect(10, 95, 80, 20), "lightning")))
        {
            OnEffect("lightning");
        }
        if ((GUI.Button(new Rect(10, 120, 80, 20), "hit")))
        {
            OnEffect("hit");
        }
        if ((GUI.Button(new Rect(10, 145, 80, 20), "firebody")))
        {
            OnEffect("firebody");
        }
        if ((GUI.Button(new Rect(10, 170, 80, 20), "explode")))
        {
            OnEffect("explode");
        }
        if ((GUI.Button(new Rect(10, 195, 80, 20), "rain")))
        {
            OnEffect("rain");
        }
    }
}
