OUT_FILE=./stats_all.proto
rm -f $OUT_FILE
echo 'package stats;' > $OUT_FILE
cat ../*.proto|grep -v "^import.*"|sed 's/^package.*/\/\/=====================================================/g' >> $OUT_FILE
cat ./stats.proto >> $OUT_FILE
ls -lrt

protoc --go_out=../../bbsvr/src/stats stats_all.proto

#cp files to stats dir
cp ../../bbsvr/src/stats/stats_all.pb.go ../../../bbmstats/src/stats/
cp ./stats.proto ../../../bbmstats/src/stats/

ls -lrt ../../bbsvr/src/stats/stats_all.pb.go
