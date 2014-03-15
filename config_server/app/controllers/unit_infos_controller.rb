class UnitInfosController < ApplicationController
  before_action :set_unit_info, only: [:show, :edit, :update, :destroy,:update_redis]

  # GET /unit_infos
  # GET /unit_infos.json
  def index
    @units =  $redis.keys.map{|k|k if k.start_with?("X_UNIT_")}.compact.sort.map{|key| UnitInfo.decode($redis.get key)}
  end

  # GET /unit_infos/1
  # GET /unit_infos/1.json
  def show
  end

  # GET /unit_infos/new
  def new
    unit_keys =  $redis.keys.map{|k|k if k.start_with?("X_UNIT_")}.compact
    @units = {"请选择卡牌信息" => "请选择卡牌信息" }.merge unit_keys.inject({}){|hsh,key| hsh[key] = key.split("_")[2].to_i;hsh}
  end

  # GET /unit_infos/1/edit
  def edit
    unit_keys =  $redis.keys.map{|k|k if k.start_with?("X_UNIT_")}.compact
    @units = {"请选择卡牌信息" => "请选择卡牌信息" }.merge unit_keys.inject({}){|hsh,key| hsh[key] = key.split("_")[2].to_i;hsh}
  end

  # POST /unit_infos
  # POST /unit_infos.json
  def create
    unit_info = UnitInfo.create_with_params(params)
    if unit_info.save_to_file && unit_info.save_to_redis
      redirect_to unit_infos_path
    else
      
    end
  end

  # PATCH/PUT /unit_infos/1
  # PATCH/PUT /unit_infos/1.json
  def update
    respond_to do |format|
      if @unit_info.update(unit_info_params)
        format.html { redirect_to @unit_info, notice: 'Unit info was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: 'edit' }
        format.json { render json: @unit_info.errors, status: :unprocessable_entity }
      end
    end
  end
  
  def download
    file_path = "#{Rails.root}/public/unit/X_UNIT_#{params[:id]}"
    send_file file_path, :filename => "X_UNIT_"+params[:id], :disposition => 'attachment'
  end
  
  def update_redis
    unit_info = UnitInfo.create_with_params(params)
    if unit_info.save_to_file && unit_info.save_to_redis
      redirect_to unit_infos_path
    else
      
    end
  end

  # DELETE /unit_infos/1
  # DELETE /unit_infos/1.json
  def destroy
    @unit_info.destroy
    respond_to do |format|
      format.html { redirect_to unit_infos_url }
      format.json { head :no_content }
    end
  end

  private
  # Use callbacks to share common setup or constraints between actions.
  def set_unit_info
    @unit_info = UnitInfo.decode($redis.get("X_UNIT_" + params[:id]))#UnitInfo.find(params[:id])
  end

  # Never trust parameters from the scary internet, only allow the white list through.
  def unit_info_params
    params[:unit_info]
  end
end
