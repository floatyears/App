//#if UNITY_EDITOR
using UnitTesting;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static test. Only for static functions
/// </summary>
public class StaticTest {

    [SetUp]
    public void HandleSetup() {
    }
    
    
    [TearDown]
    public void HandleTearDown() {
    }

    /// <summary>
    /// Tests the UUID. UUID should be unique
    /// </summary>
    [Test]
    public void TestUUID()
    {
        int TEST_COUNT = 100000;
        List <string> uuidList = new List<string>();
        for (int i = 0; i < TEST_COUNT; i++){
            uuidList.Add(GenerationHelper.NewUUID());
        }
        HashSet<string> uuidSet = new HashSet<string>(uuidList);

        int j = 0;
        foreach (string uuid in uuidSet){
            j++;
        }
        Assert.Approx(j, TEST_COUNT, 0);
		ModelManager.Instance.InitData ();
		List<AttackImageUtility> temp = null;
		BattleUseData bud = new BattleUseData ();
		for (int i = 0; i < 2; i++) {
			temp = bud.CaculateFight (1, 1);
		}
		for (int i = 0; i < 2; i++) {
			temp = bud.CaculateFight (1, 2);
		}

		for (int i = 0; i < 3; i++) {
			temp = bud.CaculateFight (1, 3);
		}

		List<AttackImageUtility> temp1 = null;
		for (int i = 0; i < 2; i++) {
			temp1 = bud.CaculateFight (2, 1);
		}
		for (int i = 0; i < 2; i++) {
			temp1 = bud.CaculateFight (2, 2);
		}
		
		for (int i = 0; i < 3; i++) {
			temp1 = bud.CaculateFight (2, 3);
		}

		MsgCenter.Instance.Invoke (CommandEnum.StartAttack, null);

//		temp = bud.CaculateFight (1, 1);
//		temp = bud.CaculateFight (1, 2);
//		temp = bud.CaculateFight (1, 3);
//		temp = bud.CaculateFight (1, 4);
//		temp = bud.CaculateFight (1, 5);
		ConfirmationData (temp);
		ConfirmationData (temp1);
    }

	void ConfirmationData (List<AttackImageUtility> temp) {
		if (temp == null) {
			Debug.Log(" temp is null ");		
		}
		foreach (var item in temp) {
			TNormalSkill tns = GlobalData.tempNormalSkill[item.skillID] as TNormalSkill;
			Debug.Log("attackProperty : " + item.attackProperty + "-- userProperty : " + item.userProperty +"-- skill name : " + tns.GetName());
		}
	}
}

//#endif
