require 'test_helper'

class UnitInfosControllerTest < ActionController::TestCase
  setup do
    @unit_info = unit_infos(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:unit_infos)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create unit_info" do
    assert_difference('UnitInfo.count') do
      post :create, unit_info: {  }
    end

    assert_redirected_to unit_info_path(assigns(:unit_info))
  end

  test "should show unit_info" do
    get :show, id: @unit_info
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @unit_info
    assert_response :success
  end

  test "should update unit_info" do
    patch :update, id: @unit_info, unit_info: {  }
    assert_redirected_to unit_info_path(assigns(:unit_info))
  end

  test "should destroy unit_info" do
    assert_difference('UnitInfo.count', -1) do
      delete :destroy, id: @unit_info
    end

    assert_redirected_to unit_infos_path
  end
end
