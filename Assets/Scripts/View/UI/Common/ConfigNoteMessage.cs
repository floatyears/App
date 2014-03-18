using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigNoteMessage
{
	public static Dictionary<string,string> searchFriendNotExist = new Dictionary<string, string>();
	public static Dictionary<string,string> inputIDEmpty = new Dictionary<string, string>();
	public static Dictionary<string,string> friendUpdateSubmit = new Dictionary<string, string>();
	public static Dictionary<string,string> refuseAllFriendIn = new Dictionary<string, string>();
	public static Dictionary<string,string> alreadyFriend = new Dictionary<string, string>();

	public ConfigNoteMessage(){
		Config();
	}

	private void Config(){
		searchFriendNotExist.Add("title", "Search Error");
		searchFriendNotExist.Add("content", "The Friend you search not exist!");
		
		inputIDEmpty.Add("title", "Input Error");
		inputIDEmpty.Add("content", "Could not Input empty ID!");
		
		friendUpdateSubmit.Add("title", "Friend Update");
		friendUpdateSubmit.Add("content", "Are you sure to update friend list?");
		
		refuseAllFriendIn.Add("title", "Refuse Apply");
		refuseAllFriendIn.Add("content", "Are you sure to refuse all friend apply?");

		alreadyFriend.Add("title", "Search Note");
		alreadyFriend.Add("content", "This user you search is already friend!");
	}




}

