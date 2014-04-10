class QuestsController < ApplicationController
  def new
    
  end

  def index
    @city_infos =  $redis.keys.map{|k| CityInfo.decode($redis.get(k)) if k.start_with?("X_CITY_")}.compact
    @quest_configs =  $redis.keys.map{|k| QuestConfig.decode($redis.get(k)) if k.start_with?("X_CONFIG_")}
  end

  def create
    if CityInfo.create_with_params(params)
      render json: { "status" => "OK"}
    else
      render json: { "status" =>  "FAIL"}
    end
  end

  def update
    
  end

  def edit
    @type = params[:type]
    @city_id = params[:city_id]
    @stage_id = params[:stage_id]
    @index = params[:id]
    @stage_index = params[:stage_index]
    if @type == "quest"
      @quest = StageInfo.decode($redis.get("X_STAGE_" + @stage_id).to_s).quests[@index.to_i]
    else
      @stage = StageInfo.decode($redis.get("X_STAGE_" + params[:id]).to_s)
    end
  end
  
  def update_quest
    if QuestInfo.update(params)
      
    else
      
    end
  end
  
  def update_stage
    
  end
  
  def addconfig
    if CityInfo.create_config(params)
      render json: { "status" => "OK" }
    else
      render json: { "status" =>  "FAIL" }
    end
  end
  
  def questconfig
    @stages =  $redis.keys.map{|k|k.split("_")[2].to_i if k.start_with?("X_STAGE_")}.compact.sort
  end
  
end
