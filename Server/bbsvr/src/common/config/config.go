package config

import (
//	"common/log"
//	"time"
)

var TableUnitExpType = make([]int32, 99)
var TableDevourCoin = make([]int32, 99)

func InitConfig() {
	for i := int32(0); i < 99; i++ {
		TableUnitExpType[i] = 100 * i
	}

	for i := int32(0); i < 99; i++ {
		TableDevourCoin[i] = 100 * (i * i / 4)
	}
}
