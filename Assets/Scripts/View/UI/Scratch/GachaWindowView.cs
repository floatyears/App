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
        btnFriendGacha = FindChild<UIButton>("bottom_panel/1");
        btnRareGacha = FindChild<UIButton>("bottom_panel/2");
        btnEventGacha = FindChild<UIButton>("bottom_panel/3");
        
        UIEventListener.Get(btnFriendGacha.gameObject).onClick = OnClickFriendGacha;
        UIEventListener.Get(btnRareGacha.gameObject).onClick = OnClickRareGacha;
        UIEventListener.Get(btnEventGacha.gameObject).onClick = OnClickEventGacha;
        
        friendGachaTimesParent = FindChild<UISprite>("bottom_panel/1/TimesParent");
        rareGachaTimesParent = FindChild<UISprite>("bottom_panel/2/TimesParent");
        eventGachaTimesParent = FindChild<UISprite>("bottom_panel/3/TimesParent");
        
        friendGachaTimes = FindChild<UILabel>("bottom_panel/1/TimesParent/Times");
        rareGachaTimes = FindChild<UILabel>("bottom_panel/2/TimesParent/Times");
        eventGachaTimes = FindChild<UILabel>("bottom_panel/3/TimesParent/Times");
    }

}
