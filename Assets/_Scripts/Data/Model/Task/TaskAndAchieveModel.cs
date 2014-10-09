using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using bbproto;
using LitJson;

public class TaskAndAchieveModel : ProtobufDataBase {

	private List<TaskConf> achieveList;

	private List<TaskConf> achieveListComplete;

	private List<TaskConf> taskList;

	public override void Init (object data)
	{
		achieveList = new List<TaskConf> ();
		achieveListComplete = new List<TaskConf> ();
		taskList = new List<TaskConf> ();

		JsonData jsonData = JsonMapper.ToObject ((string)data);
		
		for (int i = 0; i < jsonData.Count; i++){
			
			TaskConf ins = new TaskConf ();
			
			ins.taskID = (int)jsonData[i]["taskID"];
			ins.taskType = (ETaskType)(int)jsonData[i]["taskType"];
			ins.taskDesc = (string)jsonData[i]["taskDesc"];
			ins.taskGoal = (string)jsonData[i]["taskGoal"];
			ins.goalCnt = (int)jsonData[i]["goalCnt"];

			JsonData jd = jsonData[i]["giftItem"];
			ins.giftItem = new List<GiftItem>();
			for (int j = 0; j < jd.Count; j++ ) {
				GiftItem gi = new GiftItem();
				gi.content = jd[j]["content"];
				gi.count = jd[j]["count"];
				ins.giftItem.Add(gi);
			}

			if(ins.taskType == ETaskType.ACHIEVEMENT){
				achieveList.Add(ins);
			}else if(ins.taskType == ETaskType.DAILYTASK){
				taskList.Add(ins);
			}

		}
		
//		DataCenter.Instance.SetData (ModelEnum.DragPanelConfig, uiInsData);
	}
}
