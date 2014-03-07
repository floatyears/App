TARGET=bb_svr

default: $(TARGET)
	@echo -n

%: %.go
	go build -o ../bin/$(TARGET) $(TARGET).go
	ls -lrt ../bin/
	cd ../bin/;./restart.sh;ps -ef|grep -w bb_svr