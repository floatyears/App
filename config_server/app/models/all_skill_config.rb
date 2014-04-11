#encoding: utf-8
require "beefcake"
require 'zip'

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

module ETrapType
  Move = 0
  StateException = 1
  ChangeEnvir = 2
  Injured = 3
end

module EValueType
  FIXED = 1
  MULTIPLE = 2
  PERCENT = 3
  SECOND = 4
  ROUND = 5
  COLORTYPE = 6
  RANDOMCOLOR = 7
end

module EAttackType
  ATK_SINGLE = 0
  ATK_ALL = 1
  RECOVER_HP = 2
end

module EBoostType
  BOOST_ATTACK = 0
  BOOST_HP = 1
end

module EBoostTarget
  UNIT_RACE = 0
  UNIT_TYPE = 1
end

module EPeriod
  EP_RIGHT_NOW = 0
  EP_EVERY_ROUND = 1
  EP_EVERY_STEP = 2
end

class SkillBase
  include Beefcake::Message
end

class SkillSingleAttack
  include Beefcake::Message
end

class SkillAttackRecoverHP
  include Beefcake::Message
end

class SkillSuicideAttack
  include Beefcake::Message
end

class SkillTargetTypeAttack
  include Beefcake::Message
end

class SkillStrengthenAttack
  include Beefcake::Message
end

class SkillKillHP
  include Beefcake::Message
end

class SkillRecoverHP
  include Beefcake::Message
end

class SkillRecoverSP
  include Beefcake::Message
end

class SkillReduceHurt
  include Beefcake::Message
end

class SkillReduceDefence
  include Beefcake::Message
end

class SkillDeferAttackRound
  include Beefcake::Message
end

class SkillPoison
  include Beefcake::Message
end

class SkillDelayTime
  include Beefcake::Message
end

class SkillConvertUnitType
  include Beefcake::Message
end

class SkillDodgeTrap
  include Beefcake::Message
end

class SkillAntiAttack
  include Beefcake::Message
end

class NormalSkill
  include Beefcake::Message
end

class SkillBoost
  include Beefcake::Message
end

class SkillExtraAttack
  include Beefcake::Message
end

class SkillMultipleAttack
  include Beefcake::Message
end

class AllSkillConfig
  include Beefcake::Message
end

class SkillBase
  optional :id, :int32, 1
  optional :name, :string, 2
  optional :description, :string, 3
  optional :skillCooling, :int32, 4
end

class SkillSingleAttack
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :unitType, EUnitType, 4
  optional :attackRange, EAttackType, 5
  optional :ignoreDefense, :bool, 6
end

class SkillAttackRecoverHP
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :unitType, EUnitType, 4
  optional :attackType, EAttackType, 5
end

class SkillSuicideAttack
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :unitType, EUnitType, 4
  optional :attackType, EAttackType, 5
end

class SkillTargetTypeAttack
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :targetUnitType, EUnitType, 5
  optional :hurtUnitType, EUnitType, 6
end

class SkillStrengthenAttack
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :periodValue, :int32, 4
  optional :targetType, EUnitType, 5
  optional :targetRace, EUnitRace, 6
end

class SkillKillHP
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
end

class SkillRecoverHP
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :period, EPeriod, 4
end

class SkillRecoverSP
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
end

class SkillReduceHurt
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :period, EPeriod, 4
  optional :periodValue, :int32, 5
  optional :unitType, EUnitType, 6
end

class SkillReduceDefence
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :period, :int32, 4
end

class SkillDeferAttackRound
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
end

class SkillPoison
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :roundValue, :int32, 4
end

class SkillDelayTime
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :value, :float, 3
  optional :periodValue, :int32, 4
end

class SkillConvertUnitType
  optional :baseInfo, SkillBase, 1
  optional :type, EValueType, 2
  optional :unitType1, EUnitType, 3
  optional :unitType2, EUnitType, 4
end

class SkillDodgeTrap
  optional :baseInfo, SkillBase, 1
  optional :trapLevel, :int32, 2
  optional :trapType, ETrapType, 3
end

class SkillAntiAttack
  optional :baseInfo, SkillBase, 1
  optional :probability, :float, 2
  optional :attackSource, EUnitType, 3
  optional :antiAttack, EUnitType, 4
  optional :antiAtkRatio, :float, 5
end

class NormalSkill
  optional :baseInfo, SkillBase, 1
  optional :attackType, EAttackType, 2
  repeated :activeBlocks, :uint32, 3
  optional :attackValue, :float, 4
  optional :attackUnitType, EUnitType, 5
end

class SkillBoost
  optional :baseInfo, SkillBase, 1
  optional :boostType, EBoostType, 2
  optional :boostValue, :float, 3
  optional :targetType, EBoostTarget, 4
  optional :targetValue, :int32, 5
end

class SkillExtraAttack
  optional :baseInfo, SkillBase, 1
  optional :unitType, EUnitType, 2
  optional :attackValue, :float, 3
end

class SkillMultipleAttack
  optional :baseInfo, SkillBase, 1
  optional :unitTypeCount, :int32, 2
  optional :value, :float, 3
end

class AllSkillConfig
  
  EUNIT_TYPE    = { "UALL" => 0 , "UFIRE" => 1, "UWATER" => 2, "UWIND" => 3, "ULIGHT" => 4 ,"UDARK" => 5,"UNONE" => 6,"UHeart" => 7 }
  ETRAP_TYPE    = { "Move" => 0 , "StateException" => 1 , "ChangeEnvir" => 2 , "Injured" => 3 }
  EVALUE_TYPE   = { "FIXED" => 1 , "MULTIPLE" => 2 , "PERCENT" => 3 , "SECOND" => 4 , "ROUND" => 5 , "COLORTYPE" => 6 , "RANDOMCOLOR" => 7 }
  EATTACK_TYPE  = { "ATK_SINGLE" => 0 , "ATK_ALL" => 1 , "RECOVER_HP" => 2 }
  EBOOST_TYPE   = { "BOOST_ATTACK" => 0 , "BOOST_HP" => 1 }
  EBOOST_TARGET = { "UNIT_RACE" => 0 , "UNIT_TYPE" => 1 }
  EPERIOD       = { "EP_RIGHT_NOW" => 0 , "EP_EVERY_ROUND" => 1 , "EP_EVERY_STEP" => 2 }
  EUNIT_RACE    = { "ALL" => 0 , "HUMAN" => 1, "UNDEAD" =>2 , "MYTHIC" => 3 , "BEAST" => 4, "MONSTER" => 5 ,"LEGEND" => 6, "SCREAMCHEESE" => 7 }
  ALL_SKILL = %w( Normal SingleAttack SingleAtkRecoverHP SuicideAttack TargetTypeAttack  StrengthenAttack KillHP RecoverHP RecoverSP ReduceHurt ReduceDefence DeferAttackRound Poison DelayTime ConvertUnitType DodgeTrap AntiAttack Boost Extraattack MultipleAttack )
  NORMAL_SKILL =  %w( Normal )
  ACTIVE_SKILL =  %w( SingleAttack SingleAtkRecoverHP SuicideAttack TargetTypeAttack  StrengthenAttack KillHP RecoverHP RecoverSP ReduceHurt ReduceDefence DeferAttackRound Poison DelayTime ConvertUnitType )
  PASSIVE_SKILL = %w( DodgeTrap AntiAttack ) 
  LEADER_SKILL =  %w( Boost Extraattack MultipleAttack RecoverHP ReduceHurt DelayTime ConvertUnitType)
  CLASS_HASH = 
          {
            "Normal" => "NormalSkill",
            "SingleAttack" => "SkillSingleAttack",
            "SingleAtkRecoverHP" => "SkillAttackRecoverHP",
            "SuicideAttack"=>"SkillSuicideAttack",
            "TargetTypeAttack" => "SkillTargetTypeAttack",
            "StrengthenAttack" => "SkillStrengthenAttack",
            "KillHP" =>"SkillKillHP",
            "RecoverHP" =>"SkillRecoverHP",
            "RecoverSP" => "SkillRecoverSP",
            "ReduceHurt"=>"SkillReduceHurt",
            "ReduceDefence" => "SkillReduceDefence",
            "DeferAttackRound" => "SkillDeferAttackRound",
            "Poison" => "SkillPoison",
            "DelayTime" => "SkillDelayTime",
            "ConvertUnitType" => "SkillConvertUnitType",
            "DodgeTrap" => "SkillDodgeTrap",
            "AntiAttack" => "SkillAntiAttack",
            "Boost" => "SkillBoost",
            "Extraattack" => "SkillExtraAttack",
            "MultipleAttack"=> "SkillMultipleAttack"
          }
  
  repeated :Normal, NormalSkill, 1
  repeated :SingleAttack, SkillSingleAttack, 2
  repeated :SingleAtkRecoverHP, SkillAttackRecoverHP, 3
  repeated :SuicideAttack, SkillSuicideAttack, 4
  repeated :TargetTypeAttack, SkillTargetTypeAttack, 5
  repeated :StrengthenAttack, SkillStrengthenAttack, 6
  repeated :KillHP, SkillKillHP, 7
  repeated :RecoverHP, SkillRecoverHP, 8
  repeated :RecoverSP, SkillRecoverSP, 9
  repeated :ReduceHurt, SkillReduceHurt, 10
  repeated :ReduceDefence, SkillReduceDefence, 11
  repeated :DeferAttackRound, SkillDeferAttackRound, 12
  repeated :Poison, SkillPoison, 13
  repeated :DelayTime, SkillDelayTime, 14
  repeated :ConvertUnitType, SkillConvertUnitType, 15
  repeated :DodgeTrap, SkillDodgeTrap, 16
  repeated :AntiAttack, SkillAntiAttack, 17
  repeated :Boost, SkillBoost, 18
  repeated :Extraattack, SkillExtraAttack, 19
  repeated :MultipleAttack, SkillMultipleAttack, 20
  
  def self.save(params,update = nil)
    base_info = SkillBase.new(id: params[:skill_id].to_i,name: params[:name],description: params[:description],skillCooling: params[:skillCooling].to_i)
    id = params[:id].to_i
    case params[:skill_type]
    when "normal"
      active_blocks = build_active_blocks(params[:activeBlocks1],params[:activeBlocks2],params[:activeBlocks3],params[:activeBlocks4],params[:activeBlocks5])
      normalSkill = NormalSkill.new(baseInfo: base_info,attackType: params_to_i(params[:attackType]),activeBlocks: active_blocks,attackValue: params_to_f(params[:attackValue]),attackUnitType: params_to_i(params[:attackUnitType]))
      if update.nil?
        save_to_redis("Normal",normalSkill)
      else
        update_to_redis("Normal",normalSkill,id)
      end
    when "singleattack"
      singleAttack  = SkillSingleAttack.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),unitType: params_to_i(params[:unitType]),attackRange: params_to_i(params[:attackRange]),ignoreDefense: params_to_bool(params[:ignoreDefense]))
      if update.nil?
        save_to_redis("SingleAttack",singleAttack)
      else
        update_to_redis("SingleAttack",singleAttack,id)
      end
    when "singleatkrecoverhp"
      skillSingleAtkRecoverHP =  SkillAttackRecoverHP.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),unitType: params_to_i(params[:unitType]),attackType: params_to_i(params[:attackType]))
      if update.nil?
        save_to_redis("SingleAtkRecoverHP",skillSingleAtkRecoverHP)
      else
        update_to_redis("SingleAtkRecoverHP",skillSingleAtkRecoverHP,id)
      end
    when "suicideattack"
      skillSuicideAttack =  SkillSuicideAttack.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),unitType: params_to_i(params[:unitType]),attackType: params_to_i(params[:attackType]))
      if update.nil?
        save_to_redis("SuicideAttack",skillSuicideAttack)
      else
        update_to_redis("SuicideAttack",skillSuicideAttack,id)
      end
    when "targettypeattack"
      skillTargetTypeAttack =  SkillTargetTypeAttack.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),targetUnitType: params_to_i(params[:targetUnitType]),hurtUnitType: params_to_i(params[:hurtUnitType]))
      if update.nil?
        save_to_redis("TargetTypeAttack",skillTargetTypeAttack)
      else
        update_to_redis("TargetTypeAttack",skillTargetTypeAttack,id)
      end
    when "strengthenattack"
      skillStrengthenAttack = SkillStrengthenAttack.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),periodValue: params_to_i(params[:periodValue]),targetType: params_to_i(params[:targetType]),targetRace: params_to_i(params[:targetRace]))
      if update.nil?
        save_to_redis("StrengthenAttack",skillStrengthenAttack)
      else
        update_to_redis("StrengthenAttack",skillStrengthenAttack,id)
      end
    when "killhp"
      skillKillHP = SkillKillHP.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]))
      if update.nil?
        save_to_redis("KillHP",skillKillHP)
      else
        update_to_redis("KillHP",skillKillHP,id)
      end
    when "recoverhp"
      skillRecoverHP = SkillRecoverHP.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),period: params_to_i(params[:period]))
      if update.nil?
        save_to_redis("RecoverHP",skillRecoverHP)
      else
        update_to_redis("RecoverHP",skillRecoverHP,id)
      end
    when "recoversp"
      skillRecoverSP = SkillRecoverSP.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]))
      if update.nil?
        save_to_redis("RecoverSP",skillRecoverSP)
      else
        update_to_redis("RecoverSP",skillRecoverSP,id)
      end
    when "reducehurt"
      skillReduceHurt = SkillReduceHurt.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),period: params_to_i(params[:period]),periodValue: params_to_i(params[:periodValue]),unitType: params_to_i(params[:unitType]))
      if update.nil?
        save_to_redis("ReduceHurt",skillReduceHurt)
      else
        update_to_redis("ReduceHurt",skillReduceHurt,id)
      end
    when "reducedefence"
      skillReduceDefence = SkillReduceDefence.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),period: params_to_i(params[:period]))
      if update.nil?
        save_to_redis("ReduceDefence",skillReduceDefence)
      else
        update_to_redis("ReduceDefence",skillReduceDefence,id)
      end
    when "deferattackround"
      skillDeferAttackRound = SkillDeferAttackRound.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]))
      if update.nil?
        save_to_redis("DeferAttackRound",skillDeferAttackRound)
      else
        update_to_redis("DeferAttackRound",skillDeferAttackRound,id)
      end
    when "poison"
      skillPoison = SkillPoison.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),roundValue: params_to_i(params[:roundValue]))
      if update.nil?
        save_to_redis("Poison",skillPoison)   
      else
        update_to_redis("Poison",skillPoison,id)
      end
    when "delaytime"
      skillDelayTime = SkillDelayTime.new(baseInfo: base_info,type: params_to_i(params[:type]),value: params_to_f(params[:value]),periodValue: params_to_i(params[:periodValue]))
      if update.nil?
        save_to_redis("DelayTime",skillDelayTime)
      else
        update_to_redis("DelayTime",skillDelayTime,id)
      end
    when "convertunittype"
      skillConvertUnitType = SkillConvertUnitType.new(baseInfo: base_info,type: params_to_i(params[:type]),unitType1: params_to_i(params[:unitType1]),unitType2: params_to_i(params[:unitType2]))
      if update.nil?
        save_to_redis("ConvertUnitType",skillConvertUnitType)
      else
        update_to_redis("ConvertUnitType",skillConvertUnitType,id)
      end 
    when "dodgetrap"
      skillDodgeTrap = SkillDodgeTrap.new(baseInfo: base_info,trapLevel: params_to_i(params[:trapLevel]),trapType: params_to_i(params[:trapType]))
      if update.nil?
        save_to_redis("DodgeTrap",skillDodgeTrap)
      else
        update_to_redis("DodgeTrap",skillDodgeTrap,id)
      end
    when "antiattack"
      skillAntiAttack  = SkillAntiAttack.new(baseInfo: base_info,probability: params_to_f(params[:probability]),attackSource: params_to_i(params[:attackSource]),antiAttack: params_to_i(params[:antiAttack]),antiAtkRatio: params_to_f(params[:antiAtkRatio]))
      if update.nil?
        save_to_redis("AntiAttack",skillAntiAttack)
      else
        update_to_redis("AntiAttack",skillAntiAttack,id)
      end
    when "boost"
      targetValue =  (params_to_i(params[:targetType]) == 0) ? params_to_i(params[:targetValue]) : params_to_i(params[:targetValue1])
      skillBoost = SkillBoost.new(baseInfo: base_info,boostType: params_to_i(params[:boostType]),boostValue: params_to_f(params[:boostValue]),targetType: params_to_i(params[:targetType]),targetValue: targetValue)
      if update.nil?
        save_to_redis("Boost",skillBoost)
      else
        update_to_redis("Boost",skillBoost,id)
      end
    when "extraattack"
      skillExtraAttack = SkillExtraAttack.new(baseInfo: base_info,unitType: params_to_i(params[:unitType]),attackValue: params_to_f(params[:attackValue]))
      if update.nil?
        save_to_redis("Extraattack",skillExtraAttack)
      else
        update_to_redis("Extraattack",skillExtraAttack,id)
      end
    when "multipleattack"
      skillMultipleAttack = SkillMultipleAttack.new(baseInfo: base_info,unitTypeCount: params_to_i(params[:unitTypeCount]),value: params_to_f(params[:value]))
      if update.nil?
        save_to_redis("MultipleAttack",skillMultipleAttack)  
      else
        update_to_redis("MultipleAttack",skillMultipleAttack,id)
      end 
    else
      raise "input error"
    end
  end
  
  def self.build_active_blocks(p1,p2,p3,p4,p5)
    active_blocks = []
    active_blocks << p1.to_i if p1 != "请选择"
    active_blocks << p2.to_i if p2 != "请选择"
    active_blocks << p3.to_i if p3 != "请选择"
    active_blocks << p4.to_i if p4 != "请选择"
    active_blocks << p5.to_i if p5 != "请选择"
    active_blocks
  end
  
  def self.params_to_i(s)
    (s == "") ? nil : s.to_i
  end
  
  def self.params_to_f(s)
    (s == "") ? nil : s.to_f.round(3)
  end
  
  def self.params_to_bool(s)
    return true   if s == "true"   || self =~ (/(true|t|yes|y|1)$/i)
    return false  if s == "false"  || self.blank? || self =~ (/(false|f|no|n|0)$/i)
    raise ArgumentError.new("invalid value for Boolean: \"#{self}\"")
  end
  
  def self.save_to_redis(type,skill)
    if $redis.exists "X_SKILL_CONF"
      allskill = decode($redis.get("X_SKILL_CONF"))
      allskill[type].nil? ? (allskill[type] = skill) : (allskill[type] << skill)
      $redis.set "X_SKILL_CONF",allskill.encode
    else
      allskill = AllSkillConfig.new
      allskill[type] = skill
      $redis.set "X_SKILL_CONF",allskill.encode
    end
    export_to_file_and_redis
  end
  
  def self.update_to_redis(type,skill,id)
    if $redis.exists "X_SKILL_CONF"
      allskill = decode($redis.get("X_SKILL_CONF"))
      allskill[type][id] = skill
      $redis.set "X_SKILL_CONF",allskill.encode
    end
    export_to_file_and_redis
  end
  
  def self.save_to_file
    File.open(Rails.root.join("public/skills/X_SKILL_CONF"), "wb") { | file|  file.write($redis.get("X_SKILL_CONF")) } 
  end
  
  def self.export_to_file_and_redis
    @allskills = AllSkillConfig.decode($redis.get("X_SKILL_CONF"))
    ALL_SKILL.each do |key|
      if @allskills[key].present?
          @allskills[key].each do |skill|
            $redis.set "X_SKILL_CONF_#{skill.try(:baseInfo).try(:id)}",skill.encode
            File.open(Rails.root.join("public/skills/#{skill.try(:baseInfo).try(:id)}.bytes"), "wb") { | file|  file.write(skill.encode) } 
          end
      end
    end
  end
  
  def self.to_zip
    FileUtils.rm_rf(Rails.root.join("public/skills/."))
    redis_to_file
    directory = Rails.root.join("public/skills")
    zipfile_name = Rails.root.join("public/skills/skills.zip")
    File.delete(zipfile_name) if File.exist?(zipfile_name)
    
    Zip::File.open(zipfile_name, Zip::File::CREATE) do |zipfile|
      Dir[File.join(directory, '**', '**')].each do |file|
        zipfile.add(File.basename(file),file )
      end
    end
  end
  
  def self.redis_to_file
    $redis.keys.map{|k|k if k.start_with?("X_SKILL_CONF")}.compact.each do |key|
      File.open(Rails.root.join("public/skills/#{key.split("_")[3]}.bytes"), "wb") { | file|  file.write($redis.get key) } 
    end
    allskills = AllSkillConfig.decode($redis.get("X_SKILL_CONF"))
    skills_json = {}
    ALL_SKILL.each do |key|
      if allskills[key].present?
          allskills[key].each do |skill|
            skills_json[skill.try(:baseInfo).try(:id)] = CLASS_HASH[key]
          end
      end
    end
    File.open(Rails.root.join("public/skills/skills.json"), "w") { | file|  file.write(skills_json.to_json) } 
  end
  
  def self.file_to_redis(path)
    all_skill_config =  self.decode(File.read(path))
    
    $redis.set("X_SKILL_CONF",File.read(path))
     
    all_skill_config["Normal"].each{|skill|  $redis.set("X_SKILL_CONF_#{skill.try(:baseInfo).try(:id)}",skill.encode) }
    all_skill_config["SingleAttack"].each{|skill|  $redis.set("X_SKILL_CONF_#{skill.try(:baseInfo).try(:id)}",skill.encode) }
    all_skill_config["Boost"].each{|skill|  $redis.set("X_SKILL_CONF_#{skill.try(:baseInfo).try(:id)}",skill.encode) }
    
  end
end