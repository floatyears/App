using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaWindowView : UIComponentUnity {

    List<UIButton> btnList;
    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
        base.ShowUI ();
    }
    
    public override void HideUI () {
        base.HideUI ();
    }
    
    public override void DestoryUI () {
        base.DestoryUI ();
    }
    
    private void InitUI() {
       
    }

}
