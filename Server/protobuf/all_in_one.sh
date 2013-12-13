OUT_FILE=bbproto.proto
rm -f $OUT_FILE
cat *.proto|grep -v "^import.*"|sed 's/^package.*/\/\/=====================================================/g' >> $OUT_FILE
ls -lrt
make
