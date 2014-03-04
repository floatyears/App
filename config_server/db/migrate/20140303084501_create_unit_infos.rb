class CreateUnitInfos < ActiveRecord::Migration
  def change
    create_table :unit_infos do |t|

      t.timestamps
    end
  end
end
