package common

import (
	_ "fmt"
	"net/http"
)

func SendResponse(rsp http.ResponseWriter, data []byte) (size int, err error) {
	if data == nil {
		return 0, nil
	}

	size, err = rsp.Write(data)
	return size, err
}
