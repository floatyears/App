using UnityEngine;
using System.Collections;

public class RuleOfCardCreation 
{
	public static int OnRule()
	{
		// 生成一个随机值,根据概率返回生成卡片的ID
		float probability=Random.value;
		if(probability<=0.25f)
			return 1;
		if(probability<=0.55f&&probability>0.25f)
			return 2;
		if(probability<=0.80f&&probability>0.55f)
			return 3;
		if(probability<=1.00f&&probability>0.80f)
			return 4;
		else
			return 0;
	}
}
