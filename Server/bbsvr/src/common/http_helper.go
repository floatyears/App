package common

import (
	_ "fmt"
	"net/http"
)

func SendResponse(rsp http.ResponseWriter, data []byte, rsperr error) (size int, err error) {
	size, err = rsp.Write(data)

	return size, err

}
