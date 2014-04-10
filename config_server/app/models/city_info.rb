## Generated from quest.proto for 
require "beefcake"


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

module EQuestState
  QS_NEW = 0
  QS_QUESTING = 1
  QS_CLEARED = 2
end

module ETrapType
  Move = 0
  StateException = 1
  ChangeEnvir = 2
  Injured = 3
end

module EQuestGridType
  Q_NONE = 0
  Q_TREATURE = 1
  Q_ENEMY = 2
  Q_TRAP = 3
  Q_KEY = 4
  Q_QUESTION = 5
  Q_EXCLAMATION = 6
end

module EGridStar
  GS_EMPTY = 0
  GS_STAR_1 = 1
  GS_STAR_2 = 2
  GS_STAR_3 = 3
  GS_STAR_4 = 4
  GS_STAR_5 = 5
  GS_STAR_6 = 6
  GS_KEY = 7
  GS_QUESTION = 8
  GS_EXCLAMATION = 9
end

module QuestBoostType
  QB_BOOST_NONE = 0
  QB_BOOST_MONEY = 1
  QB_BOOST_EXP = 2
  QB_BOOST_DROPRATE = 3
  QB_BOOST_DROPPLUS = 4
end

module QuestType
  E_QUEST_STORY = 0
  E_QUEST_EVENT = 1
  E_QUEST_EVOLVE = 2
end

class QuestStatus
  include Beefcake::Message
end

class NumRange
  include Beefcake::Message
end

class ColorPercent
  include Beefcake::Message
end

class StarConfig
  include Beefcake::Message
end

class QuestFloorConfig
  include Beefcake::Message
end

class QuestConfig
  include Beefcake::Message
end

class EnemyInfo
  include Beefcake::Message
end

class EnemyInfoConf
  include Beefcake::Message
end

class DropUnit
  include Beefcake::Message
end

class QuestGrid
  include Beefcake::Message
end

class QuestFloor
  include Beefcake::Message
end

class QuestDungeonData
  include Beefcake::Message
end

class Position
  include Beefcake::Message
end

class QuestBoost
  include Beefcake::Message
end

class QuestInfo
  include Beefcake::Message
end

class StageInfo
  include Beefcake::Message
end

class CityInfo
  include Beefcake::Message
end

class WorldMapInfo
  include Beefcake::Message
end

class QuestStatus
  optional :questId, :uint32, 1
  repeated :playTime, :uint32, 2
end

class NumRange
  optional :min, :int32, 1
  optional :max, :int32, 2
end

class ColorPercent
  optional :color, EUnitType, 1
  optional :percent, :float, 2
end

class StarConfig
  optional :repeat, :int32, 1
  optional :star, EGridStar, 2
  optional :coin, NumRange, 3
  repeated :enemyPool, :uint32, 4
  optional :enemyNum, NumRange, 5
  repeated :trap, :uint32, 6
end

class QuestFloorConfig
  optional :version, :int32, 1
  optional :treasureNum, :int32, 2
  optional :trapNum, :int32, 3
  optional :enemyNum, :int32, 4
  optional :keyNum, :int32, 5
  repeated :stars, StarConfig, 6
end

class QuestConfig
  optional :questId, :uint32, 1
  repeated :boss, EnemyInfoConf, 2
  repeated :enemys, EnemyInfoConf, 3
  repeated :colors, ColorPercent, 4
  repeated :floors, QuestFloorConfig, 5
end

#Quest Data
class EnemyInfo
  optional :enemyId, :uint32, 1
  optional :unitId, :uint32, 2
  optional :type, EUnitType, 3
  optional :hp, :int32, 4
  optional :attack, :int32, 5
  optional :defense, :int32, 6
  optional :nextAttack, :int32, 7
end

class EnemyInfoConf
  optional :enemy, EnemyInfo, 1
  optional :dropUnitId, :uint32, 2
  optional :dropUnitLevel, :int32, 3
  optional :dropRate, :float, 4
  optional :addRate, :float, 5
end

class DropUnit
  optional :dropId, :uint32, 1
  optional :unitId, :uint32, 2
  optional :level, :int32, 3
  optional :addHp, :int32, 4
  optional :addAttack, :int32, 5
  optional :addDefence, :int32, 6
end

class QuestGrid
  optional :position, :int32, 1
  optional :star, EGridStar, 2
  optional :color, :int32, 3
  optional :type, EQuestGridType, 4
  repeated :enemyId, :uint32, 5
  optional :dropId, :uint32, 6
  optional :dropPos, :int32, 7
  optional :coins, :int32, 8
  optional :trapId, :uint32, 9
end

class QuestFloor
  repeated :gridInfo, QuestGrid, 1
end

class QuestDungeonData
  optional :questId, :uint32, 1
  repeated :boss, EnemyInfo, 2
  repeated :enemys, EnemyInfo, 3
  optional :colors, :bytes, 4
  repeated :drop, DropUnit, 5
  repeated :floors, QuestFloor, 6
end

#QuestMapData
class Position
  optional :x, :int32, 1
  optional :y, :int32, 2
end

class QuestBoost
  optional :type, QuestBoostType, 1
  optional :value, :int32, 2
end

class QuestInfo
  required :id, :uint32, 1
  optional :state, EQuestState, 2
  optional :no, :int32, 3
  optional :name, :string, 4
  optional :story, :string, 5
  optional :stamina, :int32, 6
  optional :floor, :int32, 7
  optional :rewardExp, :int32, 8
  optional :rewardMoney, :int32, 9
  repeated :bossId, :uint32, 10
  repeated :enemyId, :uint32, 11
  
end

class StageInfo
  optional :version, :int32, 1
  optional :cityId, :uint32, 2
  optional :id, :uint32, 3
  optional :state, EQuestState, 4
  optional :type, QuestType, 5
  optional :stageName, :string, 6
  optional :description, :string, 7
  optional :startTime, :uint32, 8
  optional :endTime, :uint32, 9
  optional :boost, QuestBoost, 10
  optional :pos, Position, 11
  repeated :quests, QuestInfo, 12
end

class CityInfo
    
  QUEST_TYPE = { "E_QUEST_STORY" => 0 , "E_QUEST_EVENT" => 1 , "E_QUEST_EVOLVE" => 2 }
  EQUEST_STATE = { "QS_NEW" => 0 , "QS_QUESTING" => 1 , "QS_CLEARED" => 2 }  
  QUEST_BOOST_TYPE = { "QB_BOOST_NONE" => 0, "QB_BOOST_MONEY" => 1 , "QB_BOOST_EXP" => 2 , "QB_BOOST_DROPRATE" => 3, "QB_BOOST_DROPPLUS" => 4}
  E_GRID_STAR = { "GS_EMPTY" => 0 , "GS_STAR_1" => 1, "GS_STAR_2" => 2 , "GS_STAR_3" => 3 , "GS_STAR_4" => 4 , "GS_STAR_5" => 5 , "GS_STAR_6" => 6, "GS_KEY" => 7 , "GS_QUESTION" => 8 , "GS_EXCLAMATION" => 9 }
  EUNIT_TYPE    = { "UALL" => 0 , "UFIRE" => 1, "UWATER" => 2, "UWIND" => 3, "ULIGHT" => 4 ,"UDARK" => 5,"UNONE" => 6,"UHeart" => 7 }
  optional :version, :int32, 1
  optional :id, :uint32, 2
  optional :state, :int32, 3
  optional :cityName, :string, 4
  optional :description, :string, 5
  optional :pos, Position, 6
  repeated :stages, StageInfo, 7
  
  def self.create_with_params(params)
    begin 
      stages =  []
      city = JSON.parse(params["city"])
      city["stages"].each do |stage|
        quests = []
        stage["quests"].each do |quest|
          create_config(quest["quest_config"])
          quests << QuestInfo.new(id: params_to_i(quest["quest_id"]),state: params_to_i(quest["quest_state"]),no: params_to_i(quest["no"]),name:  quest["name"].to_s,story:  quest["story"].to_s,stamina:  params_to_i(quest["stamina"]),floor:  params_to_i(quest["floor"]),rewardExp: params_to_i(quest["rewardExp"]),rewardMoney: params_to_i(quest["rewardMoney"]),bossId: quest["bossId"].uniq,enemyId: quest["enemyId"].uniq)
        end
        boost = QuestBoost.new(type: params_to_i(stage["boost_type"]),value: params_to_i(stage["value"]))
        pos = Position.new(x: params_to_i(stage["stage_x"]),y: params_to_i(stage["stage_y"]))
        s =  StageInfo.new(version: params_to_i(stage["stage_version"]),cityId: params_to_i(stage["cityId"]),id: params_to_i(stage["stageInfoId"]),state: params_to_i(stage["state"]),type: params_to_i(stage["type"]),stageName: stage["stageName"].to_s,description: stage["description"].to_s,startTime: params_to_i(stage["startTime"]),endTime: params_to_i(stage["endTime"]),boost: boost,pos: pos,quests: quests)
        stages << s
        save_to_redis("X_STAGE_"+stage["stageInfoId"].to_s,s.encode)
      end
      city_pos = Position.new(x: params_to_i(city["x"]),y: params_to_i(city["y"]))
      city_info =  CityInfo.new(version: params_to_i(city["version"]),id: params_to_i(city["id"]),state: params_to_i(city["state"]),cityName: city["cityName"].to_s,description: city["description"],pos: city_pos,stages: stages)
      save_to_redis("X_CITY_"+city["id"],city_info.encode)
      File.open(Rails.root.join("public/city/X_CITY_"+city["id"]+".bytes"), "wb") { | file|  file.write(city_info.encode) } 
    rescue Exception => e
      p e.to_s
    end
  end
  
  
  def self.create_config(quest_config)
    $redis.select(3)
    boss = []
    enemys = []
    colors =  []
    floors = []
    quest_config["boss"].each do |item|
      e = EnemyInfo.new(enemyId: params_to_i(item["enemyId"]),unitId: params_to_i(item["unitId"]),type: params_to_i(item["type"]),hp: params_to_i(item["hp"]),attack: params_to_i(item["attack"]),defense: params_to_i(item["defense"]),nextAttack: params_to_i(item["nextAttack"]))
      boss << EnemyInfoConf.new(enemy: e,dropUnitId: params_to_i(item["dropUnitId"]),dropUnitLevel: params_to_i(item["dropUnitLevel"]),dropRate: params_to_i(item["dropRate"]),addRate: params_to_i(item["addRate"]))
    end
    quest_config["enemys"].each do |item|
      e = EnemyInfo.new(enemyId: params_to_i(item["enemyId"]),unitId: params_to_i(item["unitId"]),type: params_to_i(item["type"]),hp: params_to_i(item["hp"]),attack: params_to_i(item["attack"]),defense: params_to_i(item["defense"]),nextAttack: params_to_i(item["nextAttack"]))
      enemys << EnemyInfoConf.new(enemy: e,dropUnitId: params_to_i(item["dropUnitId"]),dropUnitLevel: params_to_i(item["dropUnitLevel"]),dropRate: params_to_i(item["dropRate"]),addRate: params_to_i(item["addRate"]))
    end
    quest_config["colors"].each do |item|
      colors << ColorPercent.new(color: params_to_i(item["color"]),percent: params_to_f(item["percent"]))
    end
      
    quest_config["floors"].each do |item|
      stars = []
      item["stars"].each do |star|
        coin = NumRange.new(min: params_to_i(star["coin_min"]),max: params_to_i(star["coin_max"]))
        enemyNum = NumRange.new(min: params_to_i(star["enemyNum_min"]),max: params_to_i(star["enemyNum_max"]))
        stars << StarConfig.new(repeat: params_to_i(star["repeat"]),star: params_to_i(star["star"]),coin: coin,enemyPool: star["enemyPool"].to_s.split(",").map{|i|i.to_i},enemyNum: enemyNum,trap: star["trap"].to_s.split(",").map{|i|i.to_i})
      end
      floors << QuestFloorConfig.new(version: params_to_i(item["version"]),treasureNum: params_to_i(item["treasureNum"]),trapNum: params_to_i(item["trapNum"]),enemyNum: params_to_i(item["enemyNum"]),keyNum:  params_to_i(item["keyNum"]),stars: stars)
    end
    id = params_to_i(quest_config["questId"])
    questConfig =  QuestConfig.new(questId: id ,boss: boss,enemys: enemys,colors: colors,floors: floors)
    save_to_redis("X_CONFIG_#{id}",questConfig.encode)
  end
  
  def self.update(params)
    $redis.select(3)
    if params[:type] == "city"
      id = params[:id]
      @city = CityInfo.decode($redis.get("X_CITY_" + id ))
      stages = @city.stages
      pos = Position.new(x: params_to_i(params["x"]),y: params_to_i(params["y"]))
      @city = CityInfo.new(version: params_to_i(params["version"]),id: params_to_i(id),state: params_to_i(params["state"]),cityName: params["cityName"].to_s,description: params["description"],pos: pos,stages: stages)
      save_to_redis("X_CITY_#{id}",@city.encode)
    else
      stage_id = params[:type] == "quest" ? params[:stage_id] : params[:id]
      stage_index  =  params[:stage_index].to_i
      city_id = params[:city_id]
      @city = CityInfo.decode($redis.get("X_CITY_" + city_id ))
      @stage = StageInfo.decode($redis.get("X_STAGE_" + stage_id ))
      if params[:type] == "quest"
        @stage.quests[params[:index].to_i] = QuestInfo.new(id: params_to_i(params[:id]),state: params_to_i(params[:state]),no: params_to_i(params[:no]),name: params[:name],story: params[:story],stamina:  params_to_i(params["stamina"]),floor:  params_to_i(params["floor"]),rewardExp: params_to_i(params["rewardExp"]),rewardMoney: params_to_i(params["rewardMoney"]),bossId:  JSON.parse(params["bossId"]),enemyId: JSON.parse(params["enemyId"]))
        $redis.set "X_STAGE_#{params[:id]}",@stage.encode
        @city.stages[stage_index] = @stage        
        save_to_redis("X_CITY_#{city_id}",@city.encode)
      elsif params[:type] == "stage"
        boost = QuestBoost.new(type: params_to_i(params["boost_type"]),value: params_to_i(params["value"]))
        pos = Position.new(x: params_to_i(params["x"]),y: params_to_i(params["y"]))
        quests = @stage.quests
        @stage = StageInfo.new(version: params_to_i(params["version"]),cityId: params_to_i(params["cityId"]),id: params_to_i(params["id"]),state: params_to_i(params["state"]),type: params_to_i(params["type"]),stageName: params["stageName"].to_s,description: params["description"].to_s,startTime: params_to_i(params["startTime"]),endTime: params_to_i(params["endTime"]),boost: boost,pos: pos,quests: quests)
        $redis.set "X_STAGE_#{stage_id}",@stage.encode
        @city.stages[stage_index] = @stage
        save_to_redis("X_CITY_#{city_id}",@city.encode)
      end
    end
  end
  
  def self.update_config(params)
    op = params[:operation]
    @type =  params[:form_type]
    @index = params[:index].to_i
    @config_id = params[:config_id].to_i
    @floor_index =  params[:floor_index].to_i if @type == "star"
    $redis.select(3)
    @configs = QuestConfig.decode($redis.get("X_CONFIG_#{@config_id}"))
    case @type
    when "boss"
      enemy =  EnemyInfo.new(enemyId: params_to_i(params["enemyId"]),unitId: params_to_i(params["unitId"]),type: params_to_i(params["type"]),hp: params_to_i(params["hp"]),attack: params_to_i(params["attack"]),defense: params_to_i(params["defense"]),nextAttack: params_to_i(params["nextAttack"]))
      if op == "edit"
        @configs.boss[@index] = EnemyInfoConf.new(enemy: enemy,dropUnitId: params_to_i(params["dropUnitId"]),dropUnitLevel: params_to_i(params["dropUnitLevel"]),dropRate: params_to_i(params["dropRate"]),addRate: params_to_i(params["addRate"]))
      elsif op == "new"
        @configs.boss << EnemyInfoConf.new(enemy: enemy,dropUnitId: params_to_i(params["dropUnitId"]),dropUnitLevel: params_to_i(params["dropUnitLevel"]),dropRate: params_to_i(params["dropRate"]),addRate: params_to_i(params["addRate"]))
      end
    when "enemy"
      enemy =  EnemyInfo.new(enemyId: params_to_i(params["enemyId"]),unitId: params_to_i(params["unitId"]),type: params_to_i(params["type"]),hp: params_to_i(params["hp"]),attack: params_to_i(params["attack"]),defense: params_to_i(params["defense"]),nextAttack: params_to_i(params["nextAttack"]))
      if op == "edit"
        @configs.enemys[@index] = EnemyInfoConf.new(enemy: enemy,dropUnitId: params_to_i(params["dropUnitId"]),dropUnitLevel: params_to_i(params["dropUnitLevel"]),dropRate: params_to_i(params["dropRate"]),addRate: params_to_i(params["addRate"]))
      elsif op == "new"
        @configs.enemys << EnemyInfoConf.new(enemy: enemy,dropUnitId: params_to_i(params["dropUnitId"]),dropUnitLevel: params_to_i(params["dropUnitLevel"]),dropRate: params_to_i(params["dropRate"]),addRate: params_to_i(params["addRate"]))
      end
    when "color"
      if op == "edit"
        @configs.colors[@index] = ColorPercent.new(color: params_to_i(params["color"]),percent: params_to_f(params["percent"]))
      elsif op == "new"
        @configs.colors << ColorPercent.new(color: params_to_i(params["color"]),percent: params_to_f(params["percent"]))
      end
    when "floor"
      @floor = @configs.floors[@index]
      stars = @floor.stars
      if op == "edit"
        @configs.floors[@index] = QuestFloorConfig.new(version: params_to_i(params["version"]),treasureNum: params_to_i(params["treasureNum"]),trapNum: params_to_i(params["trapNum"]),enemyNum: params_to_i(params["enemyNum"]),keyNum:  params_to_i(params["keyNum"]),stars: stars)
      elsif op == "new"
        new_stars = []
        JSON.parse(params["floors_stars"]).each do |item|
          coin = NumRange.new(min: params_to_i(item["coin_min"]),max: params_to_i(item["coin_max"]))
          enemyNum = NumRange.new(min: params_to_i(item["enemyNum_min"]),max: params_to_i(item["enemyNum_max"]))
          new_stars << StarConfig.new(repeat: params_to_i(item["repeat"]),star: params_to_i(item["star"]),coin: coin,enemyPool: JSON.parse(item["enemyPool"]),enemyNum: enemyNum,trap: JSON.parse(item["trap"]))
        end
        new_floor = QuestFloorConfig.new(version: params_to_i(params["version"]),treasureNum: params_to_i(params["treasureNum"]),trapNum: params_to_i(params["trapNum"]),enemyNum: params_to_i(params["enemyNum"]),keyNum:  params_to_i(params["keyNum"]),stars: new_stars)
        @configs.floors << new_floor
      end
    when "star"
      coin = NumRange.new(min: params_to_i(params["coin_min"]),max: params_to_i(params["coin_max"]))
      enemyNum = NumRange.new(min: params_to_i(params["enemyNum_min"]),max: params_to_i(params["enemyNum_max"]))
      @configs.floors[@floor_index].stars[@index] = StarConfig.new(repeat: params_to_i(params["repeat"]),star: params_to_i(params["star"]),coin: coin,enemyPool: JSON.parse(params["enemyPool"]),enemyNum: enemyNum,trap: JSON.parse(params["trap"]))
    end    
    save_to_redis("X_CONFIG_#{@config_id}" ,@configs.encode)
  end
  
  def self.to_zip
    FileUtils.rm_rf(Rails.root.join("public/city/."))
    redis_to_file
    directory = Rails.root.join("public/city")
    zipfile_name = Rails.root.join("public/city/city.zip")
    File.delete(zipfile_name) if File.exist?(zipfile_name)
    
    Zip::File.open(zipfile_name, Zip::File::CREATE) do |zipfile|
      Dir[File.join(directory, '**', '**')].each do |file|
        zipfile.add(File.basename(file),file )
      end
    end
  end
  
  def self.redis_to_file
    $redis.select(3)
    $redis.keys.map{|k|k if k.start_with?("X_CITY_")}.compact.each do |key|
      File.open(Rails.root.join("public/city/#{key.split("_")[2]}.bytes"), "wb") { | file|  file.write($redis.get key) } 
    end
  end
  
  def self.params_to_i(s)
    (s == "") ? nil : s.to_i
  end
  
  def self.params_to_f(s)
    (s == "") ? nil : s.to_f.round(3)
  end
  
  def self.save_to_redis(key,value)
    $redis.select(3)
    $redis.set(key,value)
  end
  
end
