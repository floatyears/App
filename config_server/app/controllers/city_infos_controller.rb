#encoding: utf-8
require "city_info"
class CityInfosController < ApplicationController
  before_filter :set_redis
  
  def new
    if params[:op] == "add"
      @op = params[:op] 
      @city_id =  params[:city_id]
    end 
  end

  def index
    @city_infos =  $redis.keys.map{|k| CityInfo.decode($redis.get(k)) if k.start_with?("X_CITY_")}.compact
    @city_configs =  $redis.keys.map{|k| QuestConfig.decode($redis.get(k)) if k.start_with?("X_CONFIG_")}.compact
  end

  def create
    if CityInfo.create_with_params(params)
      render json: { "status" => "OK"}
    else
      render json: { "status" =>  "FAIL"}
    end
  end

  def update
    if CityInfo.update(params)
      redirect_to city_infos_path
    else
      
    end
  end

  def edit
    @type = params[:type]
    if @type == "city"
      @city = CityInfo.decode($redis.get("X_CITY_" + params[:id]))
    else
      @city_id = params[:city_id]
      @stage_index = params[:stage_index]
      if @type == "quest"
        @stage_id = params[:stage_id]
        @index = params[:id]
        @quest = StageInfo.decode($redis.get("X_STAGE_" + @stage_id).to_s).quests[@index.to_i]
        @city_configs =  QuestConfig.decode($redis.get("X_CONFIG_" + @quest.id.to_s))
      else
        @stage = StageInfo.decode($redis.get("X_STAGE_" + params[:id]).to_s)
      end
    end
  end
  def upload_config
  end
  
  def upload
    name = params[:city_config_file].original_filename
    directory = "public/configs"
    path = Rails.root.join(directory, name)
    File.open(path, "wb") { |f| f.write(params[:city_config_file].read) }
    CityInfo.import_data_from_yaml(path)
    redirect_to city_infos_path
  end
  
  def update_quest
    if CityInfo.update(params)
      redirect_to city_infos_path
    else
      
    end
  end
  
  def update_stage
    if CityInfo.update(params)
      redirect_to city_infos_path
    else
      
    end
  end
  
  def edit_config
    session[:return_to] = request.referer
    @type =  params[:type]
    @op =  params[:operation]
    @index = params[:index].to_i
    @config_id = params[:city_index]
    @floor_index =  params[:floor_index].to_i if @type == "star"
    @configs = QuestConfig.decode($redis.get("X_CONFIG_"+@config_id))
    case @type
    when "boss"
      @boss = @configs.boss[@index]
    when "enemy"
      @enemy = @configs.enemys[@index]
    when "color"
      @color = @configs.colors[@index]
    when "floor"
      @floor = @configs.floors[@index]
    when "star"
      @star = @configs.floors[@floor_index].stars[@index]
    end
  end
  
  def update_config    
    if CityInfo.update_config(params)
      redirect_to session[:return_to] 
    else
      
    end
    
  end
  
  def download
    CityInfo.to_zip
    file_path = "#{Rails.root}/public/city/city.zip"
    send_file file_path, :filename => "city.zip", :disposition => 'attachment'
  end
  
  def addconfig
    if CityInfo.create_config(params)
      render json: { "status" => "OK" }
    else
      render json: { "status" =>  "FAIL" }
    end
  end
  
  def config_index
    @city_configs =  $redis.keys.map{|k| QuestConfig.decode($redis.get(k)) if k.start_with?("X_CONFIG_")}.compact
  end
  
  def questconfig
    @quest_id = params[:quest_id]
  end
  
  private 
  
  def set_redis
    $redis.select(2)
    @units_id =  $redis.keys.map{|k|k.split("_")[2].to_i if k.start_with?("X_UNIT_")}.compact.sort
    $redis.select 3
  end
  
  
end
