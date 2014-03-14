class SkillsController < ApplicationController
  def new
    @unit_type = {"请选择" => "请选择" }.merge NormalSkill::EUNIT_TYPE
  end

  def create
    normal_skill = NormalSkill.create_with_params(params)
    if normal_skill.save_to_file && normal_skill.save_to_redis
      redirect_to new_skill_path
    else
    end
  end

  def index
    @skills =  $redis.keys.map{|k|k if k.start_with?("X_SKILL_")}.compact.sort.map{|key| NormalSkill.decode($redis.get key)}
  end
end
