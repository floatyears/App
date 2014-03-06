## Generated from app/models/normal_skill.proto for 
require "beefcake"


module EAttackType
  ATK_SINGLE = 0
  ATK_ALL = 1
  RECOVER_HP = 2
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

class NormalSkill
  include Beefcake::Message
end

class SkillBase
  include Beefcake::Message
end

class NormalSkill
  EATTACK_TYPE = { "ATK_SINGLE" => 0, "ATK_ALL" => 1, "RECOVER_HP" => 2 } 
  EUNIT_TYPE = { "UALL" => 0 , "UFIRE" => 1, "UWATER" => 2, "UWIND" => 3, "ULIGHT" => 4 ,"UDARK" => 5,"UNONE" => 6,"UHeart" => 7 }
  
  optional :baseInfo, SkillBase, 1
  optional :attackType, EAttackType, 2
  repeated :activeBlocks, :uint32, 3
  optional :attackValue, :float, 4
  optional :attackUnitType, EUnitType, 5
  
  def self.build_active_blocks(p1,p2,p3,p4,p5)
    active_blocks = []
    active_blocks << p1.to_i if p1 != "请选择"
    active_blocks << p2.to_i if p2 != "请选择"
    active_blocks << p3.to_i if p3 != "请选择"
    active_blocks << p4.to_i if p4 != "请选择"
    active_blocks << p5.to_i if p5 != "请选择"
    active_blocks.uniq
  end
  
  def self.create_with_params(params)
    base_info = SkillBase.new(id: params[:id].to_i,name: params[:name],description: params[:description],skillCooling: params[:skillCooling].to_i)
    attack_type = params[:attackType].to_i
    active_blocks = build_active_blocks(params[:activeBlocks1],params[:activeBlocks2],params[:activeBlocks3],params[:activeBlocks4],params[:activeBlocks5])
    attack_value = params[:attackValue].to_f
    attack_unit_type = params[:attackUnitType].to_i
    normal_skill = NormalSkill.new(baseInfo: base_info,attackType: attack_type,activeBlocks: active_blocks,attackValue: attack_value, attackUnitType: attack_unit_type)
  end
  
  def save_to_file
    File.open(Rails.root.join("public/skills/skill_#{self.baseInfo.id}"), "wb") { | file|  file.write(self.encode) } 
  end
  
  def save_to_redis
    $redis.select 20
    $redis.set "skill_"+self.baseInfo.id.to_s,self.encode
  end
  
end

class SkillBase
  optional :id, :int32, 1
  optional :name, :string, 2
  optional :description, :string, 3
  optional :skillCooling, :int32, 4
end
