OUT_FILE=./client_proto/bbproto.proto
rm -f $OUT_FILE
cat *.proto|grep -v "^import.*"|sed 's/^package.*/\/\/=====================================================/g' >> $OUT_FILE
ls -lrt
cd client_proto; make
