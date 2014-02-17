package common

import (
	//"log"
	"time"
)

type Cost struct {
	t1 int64
	t2 int64
}

func (c *Cost) Begin() {
	//c.t1 = time.Now().Nanosecond()/
	c.t1 = time.Now().UnixNano() / 1000000
}

func (c *Cost) Cost() uint32 {
	c.t2 = time.Now().UnixNano() / 1000000
	return uint32(c.t2 - c.t1)
}
