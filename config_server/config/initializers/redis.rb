$redis = Redis.new(:host => 'localhost', :port => 9999)
$redis.select 2