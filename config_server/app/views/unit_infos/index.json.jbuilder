json.array!(@unit_infos) do |unit_info|
  json.extract! unit_info, :id
  json.url unit_info_url(unit_info, format: :json)
end
