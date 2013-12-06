
PROTOBF = $(shell find ./ -name '*.proto')
OBJECTS = $(subst proto,pb.go,$(PROTOBF))

TARGET_PATH=../bbsvr/quest/proto

default: $(OBJECTS)
	@echo -n

%.pb.go:	%.proto
	protoc --go_out=$(TARGET_PATH) $<
