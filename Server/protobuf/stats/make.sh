OUT_FILE=./stats_all.proto
rm -f $OUT_FILE
echo 'package stats;' > $OUT_FILE
cat ../*.proto|grep -v "^import.*"|sed 's/^package.*/\/\/=====================================================/g' >> $OUT_FILE
cat ./stats.proto >> $OUT_FILE
ls -lrt

stat_path="../../src/stats"
protoc --go_out=$stat_path stats_all.proto

cd ../client_proto/
protoc --go_out=../../src/bbproto skill.proto bbproto.proto
cd -

#cp files to stats dir
cp $stat_path/stats_all.pb.go ../../../../bbmstats/src/stats/
cp ./stats.proto ../../../../bbmstats/src/stats/

ls -lrt ../../../bbsvr/src/stats/stats_all.pb.go
