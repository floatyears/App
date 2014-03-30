class SkillsController < ApplicationController
  def new
    @type = params[:skillType].downcase
    @id = params[:maxid].to_i + 1
  end

  def create
    if AllSkillConfig.save(params)
      redirect_to skills_path
    else
      
    end
  end

  def index
    skills = $redis.get("X_SKILL_CONF")
    if skills.nil?
    else
      @all_skills =  AllSkillConfig.decode(skills)
    end    
  end
  
  def skill_type
    
  end
  
  def type
    skill_type = "skillType" + params[:skillTag]
    case params[:skillTag].to_i
    when 0
      maxid = $redis.keys.map{|k|k.split("_")[3].to_i if k.start_with?("X_SKILL_CONF_4")}.compact.sort.last
    when 1
      maxid = $redis.keys.map{|k|k.split("_")[3].to_i if k.start_with?("X_SKILL_CONF_1")}.compact.sort.last
    when 2
      maxid = $redis.keys.map{|k|k.split("_")[3].to_i if k.start_with?("X_SKILL_CONF_2")}.compact.sort.last
    when 3
      maxid = $redis.keys.map{|k|k.split("_")[3].to_i if k.start_with?("X_SKILL_CONF_3")}.compact.sort.last
    end
    redirect_to new_skill_path(skillType: params[skill_type],maxid: maxid)
  end
  
  def edit
    @all_skills =  AllSkillConfig.decode($redis.get("X_SKILL_CONF"))
    @type = params[:skillType].downcase
    @index = params[:id].to_i
    @method = params[:method]
    @skill_type = params[:skillType]
  end
  
  def update
    if AllSkillConfig.save(params,true)
        redirect_to skills_path
    else
    end
  end
  
  def download
    AllSkillConfig.to_zip
    file_path = "#{Rails.root}/public/skills/skills.zip"
    send_file file_path, :filename => "skills.zip", :disposition => 'attachment'
  end
  
end
