using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using bbproto;
using LitJson;

public class TaskAndAchieveModel : ProtobufDataBase {

	private Dictionary<int,TaskConf> oriAchieveList;
	private Dictionary<int,TaskConf> oriTaskList;

	private List<TaskConf> achieveNotCompList;
	private List<TaskConf> achieveCompList;

	private List<TaskConf> taskList;

	private bool isDataChange = false; //1-task ,2-achieve

	public override void Init (object data)
	{
		oriAchieveList = new Dictionary<int,TaskConf> ();
		oriTaskList = new Dictionary<int,TaskConf> ();
		achieveNotCompList = new List<TaskConf> ();
		achieveNotCompList = new List<TaskConf> ();
		taskList = new List<TaskConf> ();


		JsonData jsonData = JsonMapper.ToObject ((string)data);
		
		for (int i = 0; i < jsonData.Count; i++){
			
			TaskConf ins = new TaskConf ();
			
			ins.taskID = (int)jsonData[i]["taskID"];
			ins.taskType = (ETaskType)(int)jsonData[i]["taskType"];
			ins.taskDesc = (string)jsonData[i]["taskDesc"];
			ins.taskGoal = (string)jsonData[i]["taskGoal"];
			ins.goalCnt = (int)jsonData[i]["goalCnt"];
			ins.TaskState = TaskStateEnum.NONE;

			JsonData jd = jsonData[i]["giftItem"];
			for (int j = 0; j < jd.Count; j++ ) {
				GiftItem gi = new GiftItem();
				gi.content = (int)jd[j]["content"];
				gi.count = (int)jd[j]["count"];
				ins.giftItem.Add(gi);
			}

			if(ins.taskType == ETaskType.ACHIEVEMENT){
				oriAchieveList.Add(ins.taskID,ins);
			}else if(ins.taskType == ETaskType.DAILYTASK){
				oriTaskList.Add(ins.taskID,ins);
			}
		}
		HttpRequestManager.Instance.AddProtoListener (ProtocolNameEnum.RspTaskList, OnTaskList);
	}

	void OnTaskList(object data){
		RspTaskList rsp = data as RspTaskList;
		TaskConf ti;
		foreach (var item in rsp.achieve.achieved) {
			ti = oriAchieveList [item.taskId];
			if(!achieveCompList.Contains(ti)){
				if(item.bonusId != 0){
					ti.TaskState = TaskStateEnum.TaskComp;
				}else{
					ti.TaskState = TaskStateEnum.TaskBonusComp;
				}

				achieveCompList.Add(ti);
				isDataChange = true;
			}else{
				if(item.bonusId != 0){
					if(oriAchieveList[item.taskId].TaskState != TaskStateEnum.TaskComp){
						oriAchieveList[item.taskId].TaskState = TaskStateEnum.TaskComp;
						isDataChange = true;
					}
				}else{
					if(oriAchieveList[item.taskId].TaskState != TaskStateEnum.TaskBonusComp){
						oriAchieveList[item.taskId].TaskState = TaskStateEnum.TaskBonusComp;
						isDataChange = true;
					}
				}
			}
			if(achieveNotCompList.Contains(ti)){
				achieveNotCompList.Remove(ti);
				isDataChange = true;
			}
		}
		if (isDataChange) {
			isDataChange = false;
			MsgCenter.Instance.Invoke(CommandEnum.AchieveDataChange);
		}
		foreach (var item in rsp.achieve.unAchieved) { //
			ti = oriAchieveList [item.taskId];
			ti.TaskState = TaskStateEnum.NotComp;
			if(achieveCompList.Contains(ti)){
				achieveCompList.Remove(ti);
				isDataChange = true;
			}
			if(!achieveNotCompList.Contains(ti)){
				achieveNotCompList.Add(ti);
				isDataChange = true;
			}
		}
		taskList.Clear ();
		foreach (var item in rsp.dailyTask.achieved) {
			ti = oriTaskList [item.taskId];
			if(item.bonusId == 0){
				Debug.Log("Task Err: completed task will not show, id:" + item.taskId );
			}else{
				if(ti.TaskState != TaskStateEnum.TaskComp){
					ti.TaskState = TaskStateEnum.TaskComp;
					isDataChange = true;
				}
			}
			taskList.Add(ti);
		}
		foreach (var item in rsp.dailyTask.unAchieved) {
			ti = oriTaskList [item.taskId];
			if(ti.TaskState != TaskStateEnum.NotComp){
				ti.TaskState = TaskStateEnum.NotComp;
				isDataChange = true;
			}
			taskList.Add(ti);
		}
		if (isDataChange) {
			isDataChange = false;
			MsgCenter.Instance.Invoke(CommandEnum.TaskDataChange);
		}
	}

	public List<TaskConf> GetAchieveCompList(){
		return achieveCompList;
	}

	public List<TaskConf> GetAchieveNotCompList(){
		return achieveNotCompList;
	}

	public List<TaskConf> GetTaskList(){
		return taskList;
	}

}
