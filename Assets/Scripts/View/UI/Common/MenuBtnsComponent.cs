using UnityEngine;
using System.Collections;

public class MenuBtnsComponent : ConcreteComponent, IUICallback {
    public static bool enable = true;

    public static void SetEnable(bool newState){
        enable = newState;
    }
	public MenuBtnsComponent (string uiName) : base(uiName) {

	}

	public override void CreatUI () {
		base.CreatUI ();

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

	public void Callback (object data)
	{
        if (!MenuBtnsComponent.enable){
            return;
        }
		try {
			SceneEnum se = (SceneEnum)data;
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}
}
