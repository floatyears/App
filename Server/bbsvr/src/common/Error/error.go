// error.go
// created by kory 2014-02-10
//

// Package errors implements functions to manipulate errors.
package Error

import (
	"fmt"
)

// New returns an error that formats as the given text.
func Err(errCode int) Error {
	return Error{errCode, ""}
}

func NewError(errCode int, err error) Error {
	if err != nil {
		return Error{-1, err.Error()}
	}
	return Error{0, ""}
}

func New(errCode int, errStr string) Error {
	return Error{errCode, errStr}
}

func Newerror(errCode int, errStr string) error {
	return &Error{errCode, errStr}
}

func OK() Error {
	return Error{0, ""}
}

// errorString is a trivial implementation of error.
type Error struct {
	errCode int
	errStr  string
}

func (e *Error) Error() string {
	if e == nil {
		return ""
	}
	return fmt.Sprintf("[%v]%v", e.errCode, e.errStr)
}

func (e *Error) Code() int {
	if e == nil {
		return 0
	}

	return e.errCode
}

func (e *Error) IsError() bool {
	if e == nil {
		return false
	}

	return e.errCode != 0
}

func (e *Error) Assign(err error) Error {
	if err == nil {
		e.errCode = 0
		e.errStr = ""
	} else {
		e.errCode = -1
		e.errStr = err.Error()
	}
	return *e
}

func (e *Error) SetError(code int, err error) {
	e.errCode = code
	e.errStr = ""
}
