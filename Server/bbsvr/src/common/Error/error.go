// error.go
// created by kory 2014-02-10
//

// Package errors implements functions to manipulate errors.
package Error

import (
	"fmt"
)

// New returns an error that formats as the given text.
func New(errCode int, errStr string) error {
	return &Error{errCode, errStr}
}

// errorString is a trivial implementation of error.
type Error struct {
	errCode int
	errStr  string
}

func (e *Error) Error() string {
	return fmt.Sprintf("[%v]%v", e.errCode, e.errStr)
}

func (e *Error) Code() int {
	return e.errCode
}

func (e *Error) IsError() bool {
	return e.errCode != 0
}
