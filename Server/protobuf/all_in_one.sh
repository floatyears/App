OUT_FILE=./client_proto/bbproto.proto
rm -f $OUT_FILE
echo 'package bbproto;' > $OUT_FILE
cat *.proto|grep -v "^import.*"|sed 's/^package.*/\/\/=====================================================/g' >> $OUT_FILE
ls -lrt
cd client_proto; make
cd -

#cp files to 
cp -R ./ /Users/kory/Documents/Dev/BB002/bb002/Server/protobuf/
cp ../bbsvr/src/bbproto/bbproto.pb.go ../../bbmstats/src/bbproto/
cd stats;./make.sh
