cd ./data/
cityId=$1
cat enemylist_$cityId*.csv|awk -F',' '{print $3}'|grep -v unitId |awk -F'"' '{print $2}'|sort|uniq > ../enemylist_$cityId.txt
cd -
ls -lrt
