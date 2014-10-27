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

	private Dictionary<EAchieveType,List<TaskConf>> achieveNotCompDic;

//	private bool isDataChange = false; //1-task ,2-achieve

	private int achieveBonusCount;
	private int taskBonusCount;

	private List<int> tempAwards;

	private bool isFirstData = true;

	public override void Init (object data)
	{
		oriAchieveList = new Dictionary<int,TaskConf> ();
		oriTaskList = new Dictionary<int,TaskConf> ();
		achieveCompList = new List<TaskConf> ();
		achieveNotCompList = new List<TaskConf> ();
		taskList = new List<TaskConf> ();
		achieveNotCompDic = new Dictionary<EAchieveType, List<TaskConf>>();

		JsonData jsonData = JsonMapper.ToObject ((string)data);
		isFirstData = true;
		
		for (int i = 0; i < jsonData.Count; i++){
			
			TaskConf ins = new TaskConf ();
			
			ins.taskID = (int)jsonData[i]["taskID"];
			ins.taskType = (ETaskType)(int)jsonData[i]["taskType"];
			ins.taskDesc = (string)jsonData[i]["taskDesc"];
			ins.taskGoal = (string)jsonData[i]["taskGoal"];
			ins.goalCnt = (int)jsonData[i]["goalCnt"];
			ins.achieveType = (EAchieveType)(int)jsonData[i]["achieveType"];
			ins.goToSence = (string)jsonData[i]["goToSence"];
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
			if(!achieveNotCompDic.ContainsKey(ins.achieveType)){
				achieveNotCompDic.Add(ins.achieveType,new List<TaskConf>());
			}
			achieveNotCompDic[ins.achieveType].Add(ins);
		}
		foreach (var item in achieveNotCompDic.Values) {
			item.Sort((x1,x2)=>{
				return (int)x1.goalCnt - (int)x2.goalCnt;
			});
		}
		HttpRequestManager.Instance.AddProtoListener (ProtocolNameEnum.RspTaskList, OnTaskList);
	}

	void OnTaskList(object data){
		RspTaskList rsp = data as RspTaskList;
		TaskConf ti;
		int aCount = 0;
		int tCount = 0;
		foreach (var item in rsp.achieve.achieved) {
			ti = oriAchieveList [item.taskId];
			ti.BonusID = item.bonusId;
			if(!achieveCompList.Contains(ti)){
				if(item.bonusId != 0){
					aCount++;
					ti.TaskState = TaskStateEnum.TaskComp;
				}else{
					ti.TaskState = TaskStateEnum.TaskBonusComp;
				}
				achieveCompList.Add(ti);
				if(!isFirstData)
					ModuleManager.Instance.ShowModule(ModuleEnum.AchieveTipModule,"data",ti);
			}else{
				if(item.bonusId != 0){
					aCount++;
					ti.TaskState = TaskStateEnum.TaskComp;
				}else{
					ti.TaskState = TaskStateEnum.TaskBonusComp;
				}
			}
			if(achieveNotCompDic.ContainsKey(ti.achieveType) && achieveNotCompDic[ti.achieveType].Contains(ti)){
				achieveNotCompDic[ti.achieveType].Remove(ti);
				if(achieveNotCompDic[ti.achieveType].Count <= 0){
					achieveNotCompDic.Remove(ti.achieveType);
				}
			}
		}
		achieveCompList.Sort((x1,x2)=>{
			return (int)x1.achieveType - (int)x2.achieveType;
		});

		foreach (var item in rsp.achieve.unAchieved) { //
			ti = oriAchieveList [item.taskId];
			ti.BonusID = 0;
			ti.CurrGoalCount = item.currentCnt;
			ti.TaskState = TaskStateEnum.NotComp;

			if(achieveCompList.Contains(ti)){
				achieveCompList.Remove(ti);
			}
		}
		achieveNotCompList.Clear ();
		foreach (var item in achieveNotCompDic.Values) {
			achieveNotCompList.Add(item[0]);
		}
		MsgCenter.Instance.Invoke(CommandEnum.AchieveDataChange);

		taskList.Clear ();
		foreach (var item in rsp.dailyTask.achieved) {
			ti = oriTaskList [item.taskId];
			ti.BonusID = item.bonusId;
			if(item.bonusId == 0){
				Debug.Log("Task Err: completed task will not show, id:" + item.taskId );
			}else{
				tCount++;
				ti.TaskState = TaskStateEnum.TaskComp;
			}
			taskList.Add(ti);
		}
		foreach (var item in rsp.dailyTask.unAchieved) {
			ti = oriTaskList [item.taskId];
			ti.BonusID = 0;
			ti.TaskState = TaskStateEnum.NotComp;
			taskList.Add(ti);
		}
		MsgCenter.Instance.Invoke(CommandEnum.TaskDataChange);

		if (aCount != achieveBonusCount) {
			achieveBonusCount = aCount;
			MsgCenter.Instance.Invoke(CommandEnum.AchieveBonusChange);
		}
		if (tCount != taskBonusCount) {
			taskBonusCount = tCount;
			MsgCenter.Instance.Invoke(CommandEnum.TaskBonusChange);
		}

		isFirstData = false;
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

	public int AchieveBonusCount{
		get{
			return achieveBonusCount;
		}
		set{
			if(achieveBonusCount != value){
				achieveBonusCount = value;
				MsgCenter.Instance.Invoke(CommandEnum.AchieveBonusChange);
			}
		}
	}

	public int TaskBonusCount{
		get{
			return taskBonusCount;
		}
		set{
			if(taskBonusCount != value){
				taskBonusCount = value;
				MsgCenter.Instance.Invoke(CommandEnum.TaskBonusChange);
			}
		}
	}

	public void TakeAwardTemp(TaskConf data){
		if (tempAwards == null) {
			tempAwards = new List<int>();
		}
		tempAwards.Add (data.BonusID);
		data.TaskState = TaskStateEnum.TaskBonusComp;
		if (data.taskType == ETaskType.ACHIEVEMENT) {
			MsgCenter.Instance.Invoke(CommandEnum.AchieveDataChange);
			AchieveBonusCount--;	
		}else{
			taskList.Remove(data);
			MsgCenter.Instance.Invoke(CommandEnum.TaskDataChange);
			TaskBonusCount--;

		}
	}

	public void SendTempAward(){
		if (tempAwards != null && tempAwards.Count > 0) {
			BonusController.Instance.TakeTaskBonus (null, tempAwards);
			tempAwards.Clear();
		}
			
	}
}
