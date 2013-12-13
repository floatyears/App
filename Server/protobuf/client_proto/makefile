
PROTOBF = $(shell find ./ -name '*.proto')
#OBJECTS = $(subst proto,pb.go,$(PROTOBF))
OBJECTS = bbproto.pb.go

TARGET_PATH=../../bbsvr/src/bbproto

default: $(OBJECTS)
	@echo -n

%.pb.go: %.proto
	protoc --go_out=$(TARGET_PATH) $<
