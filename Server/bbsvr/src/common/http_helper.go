package common

import (
	"../const"
	"./Error"
	_ "fmt"
	"net/http"
)

func SendResponse(rsp http.ResponseWriter, data []byte) (size int, e Error.Error) {
	if data == nil {
		return 0, Error.New(cs.INVALID_PARAMS, "")
	}

	size, err := rsp.Write(data)
	return size, Error.New(cs.IOWRITE_ERROR, err.Error())
}
