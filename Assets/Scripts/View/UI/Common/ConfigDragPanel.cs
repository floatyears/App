using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigDragPanel{
	public static Dictionary<string, object> OthersDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> UnitListDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> CatalogDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> LevelUpDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> PartyListDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> FriendListDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> HelperListDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> StoryStageDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> OnSaleUnitDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> QuestSelectDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> LevelUpBaseDragPanelArgs = new Dictionary<string, object>();
	public static Dictionary<string, object> LevelUpMaterialDragPanelArgs = new Dictionary<string, object>();

	public ConfigDragPanel(){
		Config();
	}

	private void Config(){
		ConfigOthersDragPanel();
		ConfigUnitListDragPanel();
		ConfigCatalogDragPanel();
		ConfigLevelUpDragPanel();
		ConfigPartyListDragPanel();
		ConfigFriendListDragPanel();
		ConfigHelperListDragPanel();
		ConfigStoryStageDragPanel();
		ConfigOnSaleUnitDragPanel();
		ConfigQuestSelectDragPanel();
		ConfigLevelUpBaseDragPanel();
		ConfigLevelUpMaterialDragPanel();
	}

	private void ConfigUnitListDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), UnitListDragPanelArgs...");
		UnitListDragPanelArgs.Add("scrollerLocalPos",				220 * Vector3.up								);
		UnitListDragPanelArgs.Add("position",							Vector3.zero									);
		UnitListDragPanelArgs.Add("clipRange",						new Vector4(0, -210, 640, 600)			);
		UnitListDragPanelArgs.Add("gridArrange",						UIGrid.Arrangement.Vertical			);
		UnitListDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -540, 0)				);
		UnitListDragPanelArgs.Add("cellWidth",							140												);
		UnitListDragPanelArgs.Add("cellHeight",						140												);
		UnitListDragPanelArgs.Add("maxPerLine",						 4													);
	}

	private void ConfigLevelUpDragPanel() {
		//Debug.Log("ConfigDragPanel.Config(), LevelUpDragPanelArgs...");
		LevelUpDragPanelArgs.Add("scrollerLocalPos",				-240 * Vector3.up							);
		LevelUpDragPanelArgs.Add("position",							Vector3.zero									);
		LevelUpDragPanelArgs.Add("clipRange",						new Vector4(0, 0, 640, 200)				);
		LevelUpDragPanelArgs.Add("gridArrange",					UIGrid.Arrangement.Horizontal		);
		LevelUpDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -120, 0)				);
		LevelUpDragPanelArgs.Add("cellWidth",						130												);
		LevelUpDragPanelArgs.Add("cellHeight",						130												);
		LevelUpDragPanelArgs.Add("maxPerLine",						 0													);
	}

	private void ConfigFriendListDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), FriendListDragPanelArgs...");
		FriendListDragPanelArgs.Add("scrollerLocalPos",			 220 * Vector3.up							);
		FriendListDragPanelArgs.Add("position",						 Vector3.zero									);
		FriendListDragPanelArgs.Add("clipRange",						 new Vector4(0, -210, 640, 600)		);
		FriendListDragPanelArgs.Add("gridArrange",					 UIGrid.Arrangement.Vertical			);
		FriendListDragPanelArgs.Add("scrollBarPosition",			 new Vector3(-320, -540, 0)				);
		FriendListDragPanelArgs.Add("cellWidth",						 140												);
		FriendListDragPanelArgs.Add("cellHeight",						 140												);
		FriendListDragPanelArgs.Add("maxPerLine",					  4													);
	}

	
	private void ConfigHelperListDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), HelperListDragPanelArgs...");
		HelperListDragPanelArgs.Add("scrollerLocalPos",			 -85* Vector3.up								);
		HelperListDragPanelArgs.Add("position", 						 Vector3.zero									);
		HelperListDragPanelArgs.Add("gridArrange", 				 UIGrid.Arrangement.Horizontal		);
		HelperListDragPanelArgs.Add("scrollMovement", 			 UIScrollView.Movement.Vertical		);
		HelperListDragPanelArgs.Add("maxPerLine", 					 1													);
		HelperListDragPanelArgs.Add("clipRange", 					 new Vector4(0, 62, 640, 813)			);
		HelperListDragPanelArgs.Add("scrollBarPosition",			 new Vector3(1280, 1350, 0)				);
		HelperListDragPanelArgs.Add("cellWidth", 					 0													);
		HelperListDragPanelArgs.Add("cellHeight", 					 120												);

	}

	
	private void ConfigLevelUpBaseDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), LevelUpBaseDragPanelArgs...");
		LevelUpBaseDragPanelArgs.Add("scrollerLocalPos",		 -28 * Vector3.up							);
		LevelUpBaseDragPanelArgs.Add("position", 					 Vector3.zero									);
		LevelUpBaseDragPanelArgs.Add("clipRange", 				 new Vector4(0, -120, 640, 400)		);
		LevelUpBaseDragPanelArgs.Add("gridArrange",				 UIGrid.Arrangement.Vertical			);
		LevelUpBaseDragPanelArgs.Add("scrollBarPosition",		 new Vector3(-320, -315, 0)				);
		LevelUpBaseDragPanelArgs.Add("cellWidth", 				 120												);
		LevelUpBaseDragPanelArgs.Add("cellHeight",				 110												);
		LevelUpBaseDragPanelArgs.Add("maxPerLine",				  3													);
	}

	private void ConfigLevelUpMaterialDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), LevelUpMaterialDragPanelArgs...");
		LevelUpMaterialDragPanelArgs.Add("scrollerLocalPos",	  -28 * Vector3.up							);
		LevelUpMaterialDragPanelArgs.Add("position", 				  Vector3.zero									);
		LevelUpMaterialDragPanelArgs.Add("clipRange", 			  new Vector4(0, -120, 640, 400)		);
		LevelUpMaterialDragPanelArgs.Add("gridArrange", 		  UIGrid.Arrangement.Vertical			);
		LevelUpMaterialDragPanelArgs.Add("scrollBarPosition",	  new Vector3(-320, -315, 0)			);
		LevelUpMaterialDragPanelArgs.Add("cellWidth", 			  110												);
		LevelUpMaterialDragPanelArgs.Add("cellHeight",			  110												);
		LevelUpMaterialDragPanelArgs.Add("maxPerLine",			   3												);
	}

	private void ConfigOthersDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), OthersDragPanelArgs...");
		OthersDragPanelArgs.Add("scrollerLocalPos" ,				 -190*Vector3.up							);
		OthersDragPanelArgs.Add("position", 							 Vector3.zero									);
		OthersDragPanelArgs.Add("clipRange", 						 	 new Vector4(0, 0, 640, 200)				);
		OthersDragPanelArgs.Add("gridArrange", 					 	 UIGrid.Arrangement.Horizontal		);
		OthersDragPanelArgs.Add("scrollBarPosition", 				 new Vector3(-320,-120,0)				);
		OthersDragPanelArgs.Add("cellWidth", 						 	 150												);
		OthersDragPanelArgs.Add("cellHeight",						 	 130												);
		OthersDragPanelArgs.Add("maxPerLine", 						  0													);
	}

	private void ConfigPartyListDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), PartyListDragPanelArgs...");
		PartyListDragPanelArgs.Add("scrollerLocalPos",				  Vector3.zero									);
		PartyListDragPanelArgs.Add("position", 							 Vector3.zero									);
		PartyListDragPanelArgs.Add("clipRange", 						 new Vector4(0, -120, 640, 400)		);
		PartyListDragPanelArgs.Add("gridArrange", 					 UIGrid.Arrangement.Vertical			);
		PartyListDragPanelArgs.Add("scrollBarPosition",				 new Vector3(-320, -315, 0)				);
		PartyListDragPanelArgs.Add("cellWidth", 						 100												);
		PartyListDragPanelArgs.Add("cellHeight",						 100												);
		PartyListDragPanelArgs.Add("maxPerLine",					 3													);
	}

	private void ConfigStoryStageDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), StoryStageDragPanelArgs...");
		StoryStageDragPanelArgs.Add("scrollerLocalPos", 				215 * Vector3.up							);
		StoryStageDragPanelArgs.Add("position", 							Vector3.zero								);
		StoryStageDragPanelArgs.Add("clipRange", 						new Vector4(0, 0, 640, 200)			);
		StoryStageDragPanelArgs.Add("gridArrange",					UIGrid.Arrangement.Horizontal	);
		StoryStageDragPanelArgs.Add("scrollBarPosition", 				new Vector3(-320, -120, 0)			);
		StoryStageDragPanelArgs.Add("cellWidth", 						230											);
		StoryStageDragPanelArgs.Add("cellHeight", 						150											);
		StoryStageDragPanelArgs.Add("maxPerLine", 					0												);
	}

	
	private void ConfigQuestSelectDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), QuestSelectDragPanelArgs...");
		QuestSelectDragPanelArgs.Add("scrollerLocalPos",				-60 * Vector3.up							);
		QuestSelectDragPanelArgs.Add("position",							Vector3.zero								);
		QuestSelectDragPanelArgs.Add("clipRange",						new Vector4(0, 0, 640, 200)			);
		QuestSelectDragPanelArgs.Add("gridArrange",					UIGrid.Arrangement.Horizontal	);
		QuestSelectDragPanelArgs.Add("maxPerLine",					0												);
		QuestSelectDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -120, 0)			);
		QuestSelectDragPanelArgs.Add("cellWidth",						125											);
		QuestSelectDragPanelArgs.Add("cellHeight",						125											);
	}

	
	private void ConfigOnSaleUnitDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), OnSaleUnitDragPanelArgs...");
		OnSaleUnitDragPanelArgs.Add("scrollerLocalPos",				-240 * Vector3.up						);
		OnSaleUnitDragPanelArgs.Add("position", 						Vector3.zero								);
		OnSaleUnitDragPanelArgs.Add("clipRange", 						new Vector4(0, -120, 640, 400)		);
		OnSaleUnitDragPanelArgs.Add("gridArrange", 					UIGrid.Arrangement.Vertical		);
		OnSaleUnitDragPanelArgs.Add("scrollBarPosition",				new Vector3(-320, -340, 0)			);
		OnSaleUnitDragPanelArgs.Add("cellWidth", 						120											);
		OnSaleUnitDragPanelArgs.Add("cellHeight",						120											);
		OnSaleUnitDragPanelArgs.Add("maxPerLine",					3												);
	}

	private void ConfigCatalogDragPanel(){
		//Debug.Log("ConfigDragPanel.Config(), CatalogDragPanelArgs...");
		CatalogDragPanelArgs.Add("scrollerLocalPos",					280 * Vector3.up							);
		CatalogDragPanelArgs.Add("position", 								Vector3.zero								);
		CatalogDragPanelArgs.Add("clipRange", 							new Vector4(0, -235, 640, 640)		);
		CatalogDragPanelArgs.Add("gridArrange", 						UIGrid.Arrangement.Vertical		);
		CatalogDragPanelArgs.Add("scrollBarPosition",					new Vector3(-320, -565, 0)			);
		CatalogDragPanelArgs.Add("cellWidth", 							120											);
		CatalogDragPanelArgs.Add("cellHeight",							120											);
		CatalogDragPanelArgs.Add("maxPerLine",							5												);
	}

}
