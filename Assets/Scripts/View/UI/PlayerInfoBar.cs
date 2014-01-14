using UnityEngine;
using System.Collections;

public class PlayerInfoBar : UIBaseUnity
{
	private UILabel labTextRank;
	private UILabel labVauleRank;
	
	private UILabel labVaulePlayerName;
	
	private UILabel labValueMoney;
	
	private UILabel labVauleCurStamina;
	private UILabel labVauleLine;
	private UILabel labVauleTotalStamina;
	
	private UILabel labVauleChipNum;
	
	void Start()
	{
		Init("PlayerInfoBar");
	}
	public override void Init (string name)
	{
//		base.Init (name);
//		//Find out Labels
//		labTextRank = FindChild<UILabel>("Lab_T_Rank");
//		labTextRank.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labVauleRank = FindChild<UILabel>("Lab_V_Rank");
//		labVauleRank.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labVaulePlayerName = FindChild<UILabel>("Lab_V_PlayerName");
//		labVaulePlayerName.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labValueMoney = FindChild<UILabel>("Lab_V_Icon");
//		labValueMoney.trueTypeFont= ViewManager.Instance.DynamicFont;
//		
//		labVauleCurStamina = FindChild<UILabel>("Lab_V_CurStamina");
//		labVauleCurStamina.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labVauleLine = FindChild<UILabel>("Lab_T_Line");
//		labVauleLine.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labVauleTotalStamina = FindChild<UILabel>("Lab_V_TotalStamina");
//		labVauleTotalStamina.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		labVauleChipNum = FindChild<UILabel>("Lab_V_ChipNum");
//		labVauleChipNum.trueTypeFont = ViewManager.Instance.DynamicFont;
//		
//		//Give Vaules from Config
//		labTextRank.text = UIConfig.Lab_T_Rank;
//		labVauleRank.text = UIConfig.Lab_V_Rank;
//		
//		labVaulePlayerName.text = UIConfig.Lab_V_PlayerName;
//		
//		labValueMoney.text = UIConfig.Lab_V_Money;
//		
//		labVauleCurStamina.text = UIConfig.Lab_V_CurStamina;
//		labVauleLine.text = UIConfig.Lab_T_Line;
//		labVauleTotalStamina.text = UIConfig.Lab_V_TotalStamina;
//		
//		labVauleChipNum.text = UIConfig.Lab_V_ChipNum;

	}
	
}
