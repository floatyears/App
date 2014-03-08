module EUnitType
  UALL = 0
  UFIRE = 1
  UWATER = 2
  UWIND = 3
  ULIGHT = 4
  UDARK = 5
  UNONE = 6
  UHeart = 7
end

module EUnitRace
  ALL = 0
  HUMAN = 1
  UNDEAD = 2
  MYTHIC = 3
  BEAST = 4
  MONSTER = 5
  LEGEND = 6
  SCREAMCHEESE = 7
end

class UnitInfo
  include Beefcake::Message
end

class PowerType
  include Beefcake::Message
end

class HelperRequire
  include Beefcake::Message
end

class EvolveInfo
  include Beefcake::Message
end

class UnitInfo
  UNITTYPE = { "UALL" => 0 , "UFIRE" => 1, "UWATER" => 2, "UWIND" => 3, "ULIGHT" => 4 ,"UDARK" => 5,"UNONE" => 6,"UHeart" => 7 }
  UNITRACE = { "ALL" => 0 , "HUMAN" => 1, "UNDEAD" =>2 , "MYTHIC" => 3 , "BEAST" => 4, "MONSTER" => 5 ,"LEGEND" => 6, "SCREAMCHEESE" => 7 }
    
  required :id, :uint32, 1
  optional :name, :string, 2
  optional :race, EUnitRace, 3
  optional :type, EUnitType, 4
  optional :rare, :int32, 5
  optional :skill1, :int32, 6
  optional :skill2, :int32, 7
  optional :leaderSkill, :int32, 8
  optional :activeSkill, :int32, 9
  optional :passiveSkill, :int32, 10
  optional :maxLevel, :int32, 11
  optional :profile, :string, 12
  optional :powerType, PowerType, 13
  optional :evolveInfo, EvolveInfo, 14
  optional :cost, :int32, 15
  optional :saleValue, :int32, 16
  optional :devourValue, :int32, 17
  
  def self.create_with_params(params)
    power_type = PowerType.new(attackType:  params[:attackType].to_i,hpType: params[:hpType].to_i,expType: params[:expType].to_i)
    hepler_require = HelperRequire.new(level: params[:level].to_i,race: params[:helper_race].to_i,type: params[:helper_type].to_i)
    materialUnitId = build_materialUnitId(params[:materialUnitId1],params[:materialUnitId2],params[:materialUnitId3])
    envolve_info = EvolveInfo.new(evolveUnitId: params[:evolveUnitId].to_i ,materialUnitId: materialUnitId,helperRequire: hepler_require)
    unit_info =  UnitInfo.new(
    id: params[:id].to_i,
    name: params[:name],
    race: params[:race].to_i,
    type: params[:type].to_i,
    rare: params[:rare].to_i,
    skill1: params[:skill1].to_i,
    skill2: params[:skill2].to_i,
    leaderSkill: params[:leaderSkill].to_i,
    activeSkill: params[:activeSkill].to_i,
    passiveSkill: params[:passiveSkill].to_i,
    maxLevel: params[:maxLevel].to_i,
    profile: params[:profile],
    powerType: power_type,
    evolveInfo: envolve_info,
    cost: params[:cost].to_i,
    saleValue: params[:saleValue].to_i,
    devourValue: params[:devourValue].to_i
    )
  end
  
  def self.build_materialUnitId(p1,p2,p3)
    materialUnitId = []
    materialUnitId << p1.to_i if p1 != "请选择卡牌信息"
    materialUnitId << p2.to_i if p2 != "请选择卡牌信息"
    materialUnitId << p3.to_i if p3 != "请选择卡牌信息"
    materialUnitId.uniq
  end
  
  def save_to_file
    File.open(Rails.root.join("public/unit/X_UNIT_#{self["id"]}"), "wb") { | file|  file.write(self.encode) } 
  end
  
  def save_to_redis
    $redis.set "X_UNIT_"+self["id"].to_s,self.encode
  end
end

class PowerType
  optional :attackType, :int32, 1
  optional :hpType, :int32, 2
  optional :expType, :int32, 3
end

class HelperRequire
  optional :level, :int32, 1
  optional :race, EUnitRace, 2
  optional :type, EUnitType, 3
end

class EvolveInfo
  required :evolveUnitId, :int32, 1
  repeated :materialUnitId, :int32, 2
  optional :helperRequire, HelperRequire, 3
end
