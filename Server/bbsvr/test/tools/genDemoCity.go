package main

import (
	//"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	//"errors"
	_ "html"

	//"math/rand"
	//"time"
)
import (
	"bbproto"
	"common"
	"common/consts"
	"common/log"
	"data"
	//"model/unit"
	//"src/quest"
	//"src/model/user"
	//"src/friend"
	//redis "github.com/garyburd/redigo/redis"
)

const (
	QUEST_STORY  = 0
	QUEST_EVENT  = 1
	QUEST_EVOLVE = 2
)

func DataAddQuest1Config(questId uint32) (bossId uint32, enemyId []uint32, conf *bbproto.QuestConfig, err error) {

	conf = &bbproto.QuestConfig{}
	conf.QuestId = proto.Uint32(questId)

	boss := &bbproto.EnemyInfoConf{}
	enemy := &bbproto.EnemyInfo{}

	bossId = uint32(66)
	enemy.EnemyId = proto.Uint32(uint32(900))
	enemy.UnitId = proto.Uint32(bossId)
	unitType := bbproto.EUnitType_UWATER
	enemy.Type = &unitType
	enemy.Hp = proto.Int32(230)
	enemy.Attack = proto.Int32(30)
	enemy.Defense = proto.Int32(2)
	enemy.NextAttack = proto.Int32(3)
	boss.Enemy = enemy
	boss.DropUnitId = proto.Uint32(bossId)
	boss.DropUnitLevel = proto.Int32(1)
	boss.DropRate = proto.Float32(0.35)
	boss.AddRate = proto.Float32(0.005)
	conf.Boss = append(conf.Boss, boss)
	conf.Enemys = append(conf.Enemys, boss)


	bossId2 := uint32(63)
	benemy2 := &bbproto.EnemyInfo{}
	benemy2.EnemyId = proto.Uint32(uint32(899))
	benemy2.UnitId = proto.Uint32(bossId2)
	unitType = bbproto.EUnitType_UWIND
	benemy2.Type = &unitType
	benemy2.Hp = proto.Int32(280)
	benemy2.Attack = proto.Int32(15)
	benemy2.Defense = proto.Int32(2)
	benemy2.NextAttack = proto.Int32(3)
	boss2 := &bbproto.EnemyInfoConf{}
	boss2.Enemy = benemy2
	boss2.DropUnitId = proto.Uint32(bossId2)
	boss2.DropUnitLevel = proto.Int32(1)
	boss2.DropRate = proto.Float32(0.35)
	boss2.AddRate = proto.Float32(0.005)
	conf.Boss = append(conf.Boss, boss2)
	conf.Enemys = append(conf.Enemys, boss2)


	enemy1 := &bbproto.EnemyInfo{}
	enemy1.EnemyId = proto.Uint32(uint32(901))
	enemy1.UnitId = proto.Uint32(uint32(49))
	unitType = bbproto.EUnitType_UFIRE
	enemy1.Type = &unitType
	enemy1.Hp = proto.Int32(18)
	enemy1.Attack = proto.Int32(6)
	enemy1.Defense = proto.Int32(1)
	enemy1.NextAttack = proto.Int32(int32(3))
	enemyConf1 := &bbproto.EnemyInfoConf{}
	enemyConf1.Enemy = enemy1
	enemyConf1.DropUnitId = proto.Uint32(*enemy1.UnitId)
	enemyConf1.DropUnitLevel = proto.Int32(1)
	enemyConf1.DropRate = proto.Float32(0.5)
	enemyConf1.AddRate = proto.Float32(0.005)

	conf.Enemys = append(conf.Enemys, enemyConf1)
	enemyId = append(enemyId, *enemy1.UnitId) //return enemyId


	enemy2 := &bbproto.EnemyInfo{}
	enemy2.EnemyId = proto.Uint32(uint32(902))
	enemy2.UnitId = proto.Uint32(uint32(51))
	unitType = bbproto.EUnitType_UWATER
	enemy2.Type = &unitType
	enemy2.Hp = proto.Int32(20)
	enemy2.Attack = proto.Int32(6)
	enemy2.Defense = proto.Int32(1)
	enemy2.NextAttack = proto.Int32(int32(3))
	enemyConf2 := &bbproto.EnemyInfoConf{}
	enemyConf2.Enemy = enemy2
	enemyConf2.DropUnitId = proto.Uint32(*enemy2.UnitId)
	enemyConf2.DropUnitLevel = proto.Int32(1)
	enemyConf2.DropRate = proto.Float32(0.5)
	enemyConf2.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf2)
	enemyId = append(enemyId, *enemy2.UnitId) //return enemyId

	enemy3 := &bbproto.EnemyInfo{}
	enemy3.EnemyId = proto.Uint32(uint32(903))
	enemy3.UnitId = proto.Uint32(uint32(53))
	unitType = bbproto.EUnitType_UWIND
	enemy3.Type = &unitType
	enemy3.Hp = proto.Int32(23)
	enemy3.Attack = proto.Int32(7)
	enemy3.Defense = proto.Int32(1)
	enemy3.NextAttack = proto.Int32(int32(3))
	enemyConf3 := &bbproto.EnemyInfoConf{}
	enemyConf3.Enemy = enemy3
	enemyConf3.DropUnitId = proto.Uint32(*enemy3.UnitId)
	enemyConf3.DropUnitLevel = proto.Int32(1)
	enemyConf3.DropRate = proto.Float32(0.5)
	enemyConf3.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf3)
	enemyId = append(enemyId, *enemy3.UnitId) //return enemyId

	//fill block color
	for n := 1; n <= 7; n++ {
		color := &bbproto.ColorPercent{}
		unitType := bbproto.EUnitType(n)
		color.Color = &unitType
		if unitType == bbproto.EUnitType_UHeart { //
			color.Percent = proto.Float32(0.16)
		} else if unitType == bbproto.EUnitType_UWIND { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UFIRE { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UWATER { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_ULIGHT { //
			color.Percent = proto.Float32(0)
		} else {
			color.Percent = proto.Float32(0)
		}
		conf.Colors = append(conf.Colors, color)
	}

	//fill QuestFloorConfig
	floor1 := &bbproto.QuestFloorConfig{}
	floor1.TreasureNum = proto.Int32(12)
	floor1.TrapNum = proto.Int32(2)
	floor1.EnemyNum = proto.Int32(10)

	star1 := bbproto.EGridStar_GS_STAR_1
	star2 := bbproto.EGridStar_GS_STAR_2
	star3 := bbproto.EGridStar_GS_STAR_3
	star4 := bbproto.EGridStar_GS_STAR_4
	star5 := bbproto.EGridStar_GS_STAR_5
	star6 := bbproto.EGridStar_GS_STAR_6
	starKey := bbproto.EGridStar_GS_KEY
	starX := bbproto.EGridStar_GS_EXCLAMATION
//	starQ := bbproto.EGridStar_GS_QUESTION

	star1Conf := &bbproto.StarConfig{}
	star1Conf.Star = &star1
	star1Conf.Repeat = proto.Int32(6)
	star1Conf.Coin = &bbproto.NumRange{proto.Int32(20), proto.Int32(45), nil}
	star1Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(1), nil}
	star1Conf.EnemyPool = []uint32{901, 902}
	star1Conf.Trap = []uint32{1, 2} //trapId


	star2Conf := &bbproto.StarConfig{}
	star2Conf.Star = &star2
	star2Conf.Repeat = proto.Int32(3)
	star2Conf.Coin = &bbproto.NumRange{proto.Int32(50), proto.Int32(75), nil}
	star2Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(2), nil}
	star2Conf.EnemyPool = []uint32{901, 902, 903}
	star2Conf.Trap = []uint32{1, 2} //trapId

	star3Conf := &bbproto.StarConfig{}
	star3Conf.Star = &star3
	star3Conf.Repeat = proto.Int32(4)
	star3Conf.Coin = &bbproto.NumRange{proto.Int32(100), proto.Int32(110), nil}
	star3Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star3Conf.EnemyPool = []uint32{901, 902, 903}
	star3Conf.Trap = []uint32{1, 2} //trapId

	star4Conf := &bbproto.StarConfig{}
	star4Conf.Star = &star4
	star4Conf.Repeat = proto.Int32(3)
	star4Conf.Coin = &bbproto.NumRange{proto.Int32(120), proto.Int32(130), nil}
	star4Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star4Conf.EnemyPool = []uint32{901, 902, 903}

	star5Conf := &bbproto.StarConfig{}
	star5Conf.Star = &star5
	star5Conf.Repeat = proto.Int32(2)
	star5Conf.Coin = &bbproto.NumRange{proto.Int32(160), proto.Int32(170), nil}
	star5Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(4), nil}
	star5Conf.EnemyPool = []uint32{901, 902, 903}

	star6Conf := &bbproto.StarConfig{}
	star6Conf.Star = &star6
	star6Conf.Repeat = proto.Int32(1)
	star6Conf.Coin = &bbproto.NumRange{proto.Int32(220), proto.Int32(230), nil}
	star6Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(5), nil}
	star6Conf.EnemyPool = []uint32{901, 902, 903}


	star0Conf := &bbproto.StarConfig{}
	star0Conf.Star = &starKey
	star0Conf.Repeat = proto.Int32(1)
	star0Conf.Coin = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}
	star0Conf.EnemyNum = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}
//	star0Conf.EnemyPool = []uint32{902, 903, 904, 905}
//	star0Conf.Trap = []uint32{1, 2, 4, 5, 6} //trapId

//	starQConf := &bbproto.StarConfig{}
//	starQConf.Star = &starQ
//	starQConf.Repeat = proto.Int32(1)
//	starQConf.Coin = &bbproto.NumRange{proto.Int32(450), proto.Int32(460), nil}
//	starQConf.EnemyNum = &bbproto.NumRange{proto.Int32(5), proto.Int32(5), nil}
//	starQConf.EnemyPool = []uint32{901, 902, 903}
//	starQConf.Trap = []uint32{1, 2} //trapId


	starX_Conf := &bbproto.StarConfig{}
	starX_Conf.Star = &starX
	starX_Conf.Repeat = proto.Int32(1)
	starX_Conf.Coin = &bbproto.NumRange{proto.Int32(450), proto.Int32(460), nil}
	starX_Conf.EnemyNum = &bbproto.NumRange{proto.Int32(2), proto.Int32(2), nil}
	starX_Conf.EnemyPool = []uint32{901, 902, 903}
	starX_Conf.Trap = []uint32{1, 2} //trapId
	//star_Conf.Trap = []uint32{5}

	floor1.Stars = append(floor1.Stars, star1Conf)
	floor1.Stars = append(floor1.Stars, star2Conf)
	floor1.Stars = append(floor1.Stars, star3Conf)
	floor1.Stars = append(floor1.Stars, star4Conf)
	floor1.Stars = append(floor1.Stars, star5Conf)
	floor1.Stars = append(floor1.Stars, star6Conf)
	floor1.Stars = append(floor1.Stars, star0Conf) // key
	floor1.Stars = append(floor1.Stars, starX_Conf) // !
	//	floor1.Stars = append(floor1.Stars, starQConf) // ?

	conf.Floors = append(conf.Floors, floor1)

	log.Printf("QuestConfig: %+v", conf)

	db := &data.Data{}
	err = db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return bossId, enemyId, conf,err
	}
	defer db.Close()

	zData, err := proto.Marshal(conf)
	if err != nil {
		log.Printf("unmarshal error.")
		return bossId, enemyId,conf,err
	}
	if err = db.Set(consts.X_QUEST_CONFIG+common.Utoa(questId), zData); err != nil {
		return bossId, enemyId,conf,err
	}

	return bossId, enemyId,conf,err
}

func DataAddQuest2Config(questId uint32) (bossId uint32, enemyId []uint32, conf *bbproto.QuestConfig, err error) {

	conf = &bbproto.QuestConfig{}
	conf.QuestId = proto.Uint32(questId)

	boss := &bbproto.EnemyInfoConf{}
	enemy := &bbproto.EnemyInfo{}

	bossId = uint32(90)
	enemy.EnemyId = proto.Uint32(uint32(900))
	enemy.UnitId = proto.Uint32(bossId)
	unitType := bbproto.EUnitType_UWATER
	enemy.Type = &unitType
	enemy.Hp = proto.Int32(350)
	enemy.Attack = proto.Int32(12)
	enemy.Defense = proto.Int32(2)
	enemy.NextAttack = proto.Int32(3)
	boss.Enemy = enemy
	boss.DropUnitId = proto.Uint32(bossId)
	boss.DropUnitLevel = proto.Int32(1)
	boss.DropRate = proto.Float32(0.15)
	boss.AddRate = proto.Float32(0.005)
	conf.Boss = append(conf.Boss, boss)
	conf.Enemys = append(conf.Enemys, boss)


	bossId2 := uint32(75)
	benemy2 := &bbproto.EnemyInfo{}
	benemy2.EnemyId = proto.Uint32(uint32(899))
	benemy2.UnitId = proto.Uint32(bossId2)
	unitType = bbproto.EUnitType_UWIND
	benemy2.Type = &unitType
	benemy2.Hp = proto.Int32(400)
	benemy2.Attack = proto.Int32(14)
	benemy2.Defense = proto.Int32(2)
	benemy2.NextAttack = proto.Int32(3)
	boss2 := &bbproto.EnemyInfoConf{}
	boss2.Enemy = benemy2
	boss2.DropUnitId = proto.Uint32(bossId2)
	boss2.DropUnitLevel = proto.Int32(1)
	boss2.DropRate = proto.Float32(0.15)
	boss2.AddRate = proto.Float32(0.005)
	conf.Boss = append(conf.Boss, boss2)
	conf.Enemys = append(conf.Enemys, boss2)


	enemy1 := &bbproto.EnemyInfo{}
	enemy1.EnemyId = proto.Uint32(uint32(901))
	enemy1.UnitId = proto.Uint32(uint32(49))
	unitType = bbproto.EUnitType_UFIRE
	enemy1.Type = &unitType
	enemy1.Hp = proto.Int32(18)
	enemy1.Attack = proto.Int32(6)
	enemy1.Defense = proto.Int32(1)
	enemy1.NextAttack = proto.Int32(int32(3))
	enemyConf1 := &bbproto.EnemyInfoConf{}
	enemyConf1.Enemy = enemy1
	enemyConf1.DropUnitId = proto.Uint32(*enemy1.UnitId)
	enemyConf1.DropUnitLevel = proto.Int32(1)
	enemyConf1.DropRate = proto.Float32(0.5)
	enemyConf1.AddRate = proto.Float32(0.005)

	conf.Enemys = append(conf.Enemys, enemyConf1)
	enemyId = append(enemyId, *enemy1.UnitId) //return enemyId


	enemy2 := &bbproto.EnemyInfo{}
	enemy2.EnemyId = proto.Uint32(uint32(902))
	enemy2.UnitId = proto.Uint32(uint32(51))
	unitType = bbproto.EUnitType_UWATER
	enemy2.Type = &unitType
	enemy2.Hp = proto.Int32(20)
	enemy2.Attack = proto.Int32(6)
	enemy2.Defense = proto.Int32(1)
	enemy2.NextAttack = proto.Int32(int32(3))
	enemyConf2 := &bbproto.EnemyInfoConf{}
	enemyConf2.Enemy = enemy2
	enemyConf2.DropUnitId = proto.Uint32(*enemy2.UnitId)
	enemyConf2.DropUnitLevel = proto.Int32(1)
	enemyConf2.DropRate = proto.Float32(0.5)
	enemyConf2.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf2)
	enemyId = append(enemyId, *enemy2.UnitId) //return enemyId

	enemy3 := &bbproto.EnemyInfo{}
	enemy3.EnemyId = proto.Uint32(uint32(903))
	enemy3.UnitId = proto.Uint32(uint32(53))
	unitType = bbproto.EUnitType_UWIND
	enemy3.Type = &unitType
	enemy3.Hp = proto.Int32(23)
	enemy3.Attack = proto.Int32(7)
	enemy3.Defense = proto.Int32(1)
	enemy3.NextAttack = proto.Int32(int32(3))
	enemyConf3 := &bbproto.EnemyInfoConf{}
	enemyConf3.Enemy = enemy3
	enemyConf3.DropUnitId = proto.Uint32(*enemy3.UnitId)
	enemyConf3.DropUnitLevel = proto.Int32(1)
	enemyConf3.DropRate = proto.Float32(0.5)
	enemyConf3.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf3)
	enemyId = append(enemyId, *enemy3.UnitId) //return enemyId

	//fill block color
	for n := 1; n <= 7; n++ {
		color := &bbproto.ColorPercent{}
		unitType := bbproto.EUnitType(n)
		color.Color = &unitType
		if unitType == bbproto.EUnitType_UHeart { //
			color.Percent = proto.Float32(0.16)
		} else if unitType == bbproto.EUnitType_UWIND { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UFIRE { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UWATER { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_ULIGHT { //
			color.Percent = proto.Float32(0)
		} else {
			color.Percent = proto.Float32(0)
		}
		conf.Colors = append(conf.Colors, color)
	}

	//fill QuestFloorConfig
	floor1 := &bbproto.QuestFloorConfig{}
	floor1.TreasureNum = proto.Int32(11)
	floor1.TrapNum = proto.Int32(3)
	floor1.EnemyNum = proto.Int32(10)

	star1 := bbproto.EGridStar_GS_STAR_1
	star2 := bbproto.EGridStar_GS_STAR_2
	star3 := bbproto.EGridStar_GS_STAR_3
	star4 := bbproto.EGridStar_GS_STAR_4
	star5 := bbproto.EGridStar_GS_STAR_5
	star6 := bbproto.EGridStar_GS_STAR_6
	starKey := bbproto.EGridStar_GS_KEY
	starX := bbproto.EGridStar_GS_EXCLAMATION
	//	starQ := bbproto.EGridStar_GS_QUESTION

	star1Conf := &bbproto.StarConfig{}
	star1Conf.Star = &star1
	star1Conf.Repeat = proto.Int32(5)
	star1Conf.Coin = &bbproto.NumRange{proto.Int32(20), proto.Int32(45), nil}
	star1Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(1), nil}
	star1Conf.EnemyPool = []uint32{901, 902}
	star1Conf.Trap = []uint32{1, 2} //trapId


	star2Conf := &bbproto.StarConfig{}
	star2Conf.Star = &star2
	star2Conf.Repeat = proto.Int32(3)
	star2Conf.Coin = &bbproto.NumRange{proto.Int32(50), proto.Int32(75), nil}
	star2Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(2), nil}
	star2Conf.EnemyPool = []uint32{901, 902, 903}
	star2Conf.Trap = []uint32{1, 2} //trapId

	star3Conf := &bbproto.StarConfig{}
	star3Conf.Star = &star3
	star3Conf.Repeat = proto.Int32(4)
	star3Conf.Coin = &bbproto.NumRange{proto.Int32(100), proto.Int32(110), nil}
	star3Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star3Conf.EnemyPool = []uint32{901, 902, 903}
	star3Conf.Trap = []uint32{1, 2} //trapId

	star4Conf := &bbproto.StarConfig{}
	star4Conf.Star = &star4
	star4Conf.Repeat = proto.Int32(4)
	star4Conf.Coin = &bbproto.NumRange{proto.Int32(120), proto.Int32(130), nil}
	star4Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star4Conf.EnemyPool = []uint32{901, 902, 903}

	star5Conf := &bbproto.StarConfig{}
	star5Conf.Star = &star5
	star5Conf.Repeat = proto.Int32(2)
	star5Conf.Coin = &bbproto.NumRange{proto.Int32(160), proto.Int32(170), nil}
	star5Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(4), nil}
	star5Conf.EnemyPool = []uint32{901, 902, 903}

	star6Conf := &bbproto.StarConfig{}
	star6Conf.Star = &star6
	star6Conf.Repeat = proto.Int32(1)
	star6Conf.Coin = &bbproto.NumRange{proto.Int32(220), proto.Int32(230), nil}
	star6Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(5), nil}
	star6Conf.EnemyPool = []uint32{901, 902, 903}


	star0Conf := &bbproto.StarConfig{}
	star0Conf.Star = &starKey
	star0Conf.Repeat = proto.Int32(1)
	star0Conf.Coin = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}
	star0Conf.EnemyNum = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}

	starX_Conf := &bbproto.StarConfig{}
	starX_Conf.Star = &starX
	starX_Conf.Repeat = proto.Int32(1)
	starX_Conf.Coin = &bbproto.NumRange{proto.Int32(450), proto.Int32(460), nil}
	starX_Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(5), nil}
	starX_Conf.EnemyPool = []uint32{901, 902, 903}
	starX_Conf.Trap = []uint32{1, 2} //trapId
	//star_Conf.Trap = []uint32{5}

	floor1.Stars = append(floor1.Stars, star1Conf)
	floor1.Stars = append(floor1.Stars, star2Conf)
	floor1.Stars = append(floor1.Stars, star3Conf)
	floor1.Stars = append(floor1.Stars, star4Conf)
	floor1.Stars = append(floor1.Stars, star5Conf)
	floor1.Stars = append(floor1.Stars, star6Conf)
	floor1.Stars = append(floor1.Stars, star0Conf) // key
	floor1.Stars = append(floor1.Stars, starX_Conf) // !
	//	floor1.Stars = append(floor1.Stars, starQConf) // ?

	conf.Floors = append(conf.Floors, floor1)

	log.Printf("QuestConfig: %+v", conf)

	db := &data.Data{}
	err = db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return bossId, enemyId, conf,err
	}
	defer db.Close()

	zData, err := proto.Marshal(conf)
	if err != nil {
		log.Printf("unmarshal error.")
		return bossId, enemyId,conf,err
	}
	if err = db.Set(consts.X_QUEST_CONFIG+common.Utoa(questId), zData); err != nil {
		return bossId, enemyId,conf,err
	}

	return bossId, enemyId,conf,err
}

func DataAddQuest3Config(questId uint32) (bossId uint32, enemyId []uint32, conf *bbproto.QuestConfig, err error) {

	conf = &bbproto.QuestConfig{}
	conf.QuestId = proto.Uint32(questId)

	boss := &bbproto.EnemyInfoConf{}
	enemy := &bbproto.EnemyInfo{}

	bossId = uint32(9)
	enemy.EnemyId = proto.Uint32(uint32(900))
	enemy.UnitId = proto.Uint32(bossId)
	unitType := bbproto.EUnitType_UWATER
	enemy.Type = &unitType
	enemy.Hp = proto.Int32(1200)
	enemy.Attack = proto.Int32(62)
	enemy.Defense = proto.Int32(9)
	enemy.NextAttack = proto.Int32(3)
	boss.Enemy = enemy
	boss.DropUnitId = proto.Uint32(bossId)
	boss.DropUnitLevel = proto.Int32(1)
	boss.DropRate = proto.Float32(0.10)
	boss.AddRate = proto.Float32(0.005)
	conf.Boss = append(conf.Boss, boss)
	conf.Enemys = append(conf.Enemys, boss)


	enemy1 := &bbproto.EnemyInfo{}
	enemy1.EnemyId = proto.Uint32(uint32(901))
	enemy1.UnitId = proto.Uint32(uint32(49))
	unitType = bbproto.EUnitType_UFIRE
	enemy1.Type = &unitType
	enemy1.Hp = proto.Int32(18)
	enemy1.Attack = proto.Int32(6)
	enemy1.Defense = proto.Int32(1)
	enemy1.NextAttack = proto.Int32(int32(3))
	enemyConf1 := &bbproto.EnemyInfoConf{}
	enemyConf1.Enemy = enemy1
	enemyConf1.DropUnitId = proto.Uint32(*enemy1.UnitId)
	enemyConf1.DropUnitLevel = proto.Int32(1)
	enemyConf1.DropRate = proto.Float32(0.5)
	enemyConf1.AddRate = proto.Float32(0.005)

	conf.Enemys = append(conf.Enemys, enemyConf1)
	enemyId = append(enemyId, *enemy1.UnitId) //return enemyId


	enemy2 := &bbproto.EnemyInfo{}
	enemy2.EnemyId = proto.Uint32(uint32(902))
	enemy2.UnitId = proto.Uint32(uint32(51))
	unitType = bbproto.EUnitType_UWATER
	enemy2.Type = &unitType
	enemy2.Hp = proto.Int32(20)
	enemy2.Attack = proto.Int32(6)
	enemy2.Defense = proto.Int32(1)
	enemy2.NextAttack = proto.Int32(int32(3))
	enemyConf2 := &bbproto.EnemyInfoConf{}
	enemyConf2.Enemy = enemy2
	enemyConf2.DropUnitId = proto.Uint32(*enemy2.UnitId)
	enemyConf2.DropUnitLevel = proto.Int32(1)
	enemyConf2.DropRate = proto.Float32(0.5)
	enemyConf2.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf2)
	enemyId = append(enemyId, *enemy2.UnitId) //return enemyId

	enemy3 := &bbproto.EnemyInfo{}
	enemy3.EnemyId = proto.Uint32(uint32(903))
	enemy3.UnitId = proto.Uint32(uint32(53))
	unitType = bbproto.EUnitType_UWIND
	enemy3.Type = &unitType
	enemy3.Hp = proto.Int32(23)
	enemy3.Attack = proto.Int32(7)
	enemy3.Defense = proto.Int32(1)
	enemy3.NextAttack = proto.Int32(int32(3))
	enemyConf3 := &bbproto.EnemyInfoConf{}
	enemyConf3.Enemy = enemy3
	enemyConf3.DropUnitId = proto.Uint32(*enemy3.UnitId)
	enemyConf3.DropUnitLevel = proto.Int32(1)
	enemyConf3.DropRate = proto.Float32(0.5)
	enemyConf3.AddRate = proto.Float32(0.005)
	conf.Enemys = append(conf.Enemys, enemyConf3)
	enemyId = append(enemyId, *enemy3.UnitId) //return enemyId

	//fill block color
	for n := 1; n <= 7; n++ {
		color := &bbproto.ColorPercent{}
		unitType := bbproto.EUnitType(n)
		color.Color = &unitType
		if unitType == bbproto.EUnitType_UHeart { //
			color.Percent = proto.Float32(0.16)
		} else if unitType == bbproto.EUnitType_UWIND { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UFIRE { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_UWATER { //
			color.Percent = proto.Float32(0.28)
		} else if unitType == bbproto.EUnitType_ULIGHT { //
			color.Percent = proto.Float32(0)
		} else {
			color.Percent = proto.Float32(0)
		}
		conf.Colors = append(conf.Colors, color)
	}

	//fill QuestFloorConfig
	floor1 := &bbproto.QuestFloorConfig{}
	floor1.TreasureNum = proto.Int32(11)
	floor1.TrapNum = proto.Int32(3)
	floor1.EnemyNum = proto.Int32(10)

	star1 := bbproto.EGridStar_GS_STAR_1
	star2 := bbproto.EGridStar_GS_STAR_2
	star3 := bbproto.EGridStar_GS_STAR_3
	star4 := bbproto.EGridStar_GS_STAR_4
	star5 := bbproto.EGridStar_GS_STAR_5
	star6 := bbproto.EGridStar_GS_STAR_6
	starKey := bbproto.EGridStar_GS_KEY
	starX := bbproto.EGridStar_GS_EXCLAMATION
	//	starQ := bbproto.EGridStar_GS_QUESTION

	star1Conf := &bbproto.StarConfig{}
	star1Conf.Star = &star1
	star1Conf.Repeat = proto.Int32(5)
	star1Conf.Coin = &bbproto.NumRange{proto.Int32(30), proto.Int32(55), nil}
	star1Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(1), nil}
	star1Conf.EnemyPool = []uint32{901, 902}
	star1Conf.Trap = []uint32{1, 2} //trapId


	star2Conf := &bbproto.StarConfig{}
	star2Conf.Star = &star2
	star2Conf.Repeat = proto.Int32(5)
	star2Conf.Coin = &bbproto.NumRange{proto.Int32(75), proto.Int32(85), nil}
	star2Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(2), nil}
	star2Conf.EnemyPool = []uint32{901, 902, 903}
	star2Conf.Trap = []uint32{1, 2} //trapId

	star3Conf := &bbproto.StarConfig{}
	star3Conf.Star = &star3
	star3Conf.Repeat = proto.Int32(4)
	star3Conf.Coin = &bbproto.NumRange{proto.Int32(140), proto.Int32(150), nil}
	star3Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star3Conf.EnemyPool = []uint32{901, 902, 903}
	star3Conf.Trap = []uint32{1, 2} //trapId

	star4Conf := &bbproto.StarConfig{}
	star4Conf.Star = &star4
	star4Conf.Repeat = proto.Int32(4)
	star4Conf.Coin = &bbproto.NumRange{proto.Int32(180), proto.Int32(190), nil}
	star4Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(3), nil}
	star4Conf.EnemyPool = []uint32{901, 902, 903}

	star5Conf := &bbproto.StarConfig{}
	star5Conf.Star = &star5
	star5Conf.Repeat = proto.Int32(2)
	star5Conf.Coin = &bbproto.NumRange{proto.Int32(220), proto.Int32(230), nil}
	star5Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(5), nil}
	star5Conf.EnemyPool = []uint32{901, 902, 903}

	star6Conf := &bbproto.StarConfig{}
	star6Conf.Star = &star6
	star6Conf.Repeat = proto.Int32(2)
	star6Conf.Coin = &bbproto.NumRange{proto.Int32(600), proto.Int32(610), nil}
	star6Conf.EnemyNum = &bbproto.NumRange{proto.Int32(4), proto.Int32(4), nil}
	star6Conf.EnemyPool = []uint32{901, 902, 903}


	star0Conf := &bbproto.StarConfig{}
	star0Conf.Star = &starKey
	star0Conf.Repeat = proto.Int32(1)
	star0Conf.Coin = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}
	star0Conf.EnemyNum = &bbproto.NumRange{proto.Int32(0), proto.Int32(0), nil}

	starX_Conf := &bbproto.StarConfig{}
	starX_Conf.Star = &starX
	starX_Conf.Repeat = proto.Int32(1)
	starX_Conf.Coin = &bbproto.NumRange{proto.Int32(450), proto.Int32(460), nil}
	starX_Conf.EnemyNum = &bbproto.NumRange{proto.Int32(2), proto.Int32(2), nil}
	starX_Conf.EnemyPool = []uint32{901, 902, 903}
	starX_Conf.Trap = []uint32{1, 2} //trapId
	//star_Conf.Trap = []uint32{5}

	floor1.Stars = append(floor1.Stars, star1Conf)
	floor1.Stars = append(floor1.Stars, star2Conf)
	floor1.Stars = append(floor1.Stars, star3Conf)
	floor1.Stars = append(floor1.Stars, star4Conf)
	floor1.Stars = append(floor1.Stars, star5Conf)
	floor1.Stars = append(floor1.Stars, star6Conf)
	floor1.Stars = append(floor1.Stars, star0Conf) // key
	floor1.Stars = append(floor1.Stars, starX_Conf) // !
	//	floor1.Stars = append(floor1.Stars, starQConf) // ?

	conf.Floors = append(conf.Floors, floor1)

	log.Printf("QuestConfig: %+v", conf)

	db := &data.Data{}
	err = db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return bossId, enemyId, conf,err
	}
	defer db.Close()

	zData, err := proto.Marshal(conf)
	if err != nil {
		log.Printf("unmarshal error.")
		return bossId, enemyId,conf,err
	}
	if err = db.Set(consts.X_QUEST_CONFIG+common.Utoa(questId), zData); err != nil {
		return bossId, enemyId,conf,err
	}

	return bossId, enemyId,conf,err
}

func DataAddStageInfo(db *data.Data, stageId uint32, stageName string, stageType bbproto.QuestType, questNum int) (stageInfo *bbproto.StageInfo, err error) {

	state := bbproto.EQuestState_QS_NEW

	stageInfo = &bbproto.StageInfo{}
	stageInfo.Version = proto.Int(1)
	stageInfo.Id = proto.Uint32(stageId)
	stageInfo.State = &state
	stageInfo.Type = &stageType // story or event
	stageInfo.StageName = proto.String(stageName)
	stageInfo.Description = proto.String("it is :" + stageName)
	stageInfo.StartTime = proto.Uint32(0)
	stageInfo.EndTime = proto.Uint32(0)
	boostType := bbproto.QuestBoostType_QB_BOOST_MONEY
	stageInfo.Boost = new(bbproto.QuestBoost)
	stageInfo.Boost.Type = &boostType //coins , exp , dropRate
	stageInfo.Boost.Value = proto.Int(2)
	//optional Position	pos				= &pos; // stage position of the city

	for i := 1; i <= questNum; i++ {
		questId := (10*stageId + uint32(i) )
		bossId:=uint32(0)
		enemyId:=[]uint32{}
		var conf *bbproto.QuestConfig=nil
		//add quest config
		if i== 1 {
			bossId,enemyId,conf,_ = DataAddQuest1Config(questId)
		}else if i== 2 {
			bossId,enemyId,conf,_ = DataAddQuest2Config(questId)
		}else if i== 3 {
			bossId,enemyId,conf,_ = DataAddQuest3Config(questId)
		}

		qusetInfo := &bbproto.QuestInfo{}
		qusetInfo.Id = proto.Uint32(questId)
		qusetInfo.State = &state
		qusetInfo.No = proto.Int32(int32(i))
		qusetInfo.Name = proto.String("quest " + common.Itoa(i))   // quest name
		qusetInfo.Story = proto.String("it is quest" + common.Itoa(i)) // story description

		qusetInfo.Floor = proto.Int32(int32(len(conf.Floors)))
		if i== 1 {
			qusetInfo.Stamina = proto.Int32(3)                             // cost stamina
			qusetInfo.RewardExp = proto.Int32(5)
			qusetInfo.RewardMoney = proto.Int32(120)
		}else if i== 2 {
			qusetInfo.Stamina = proto.Int32(3)                             // cost stamina
			qusetInfo.RewardExp = proto.Int32(6)
			qusetInfo.RewardMoney = proto.Int32(120)
		}else if i== 3 {
			qusetInfo.Stamina = proto.Int32(5)                             // cost stamina
			qusetInfo.RewardExp = proto.Int32(15)
			qusetInfo.RewardMoney = proto.Int32(160)
		}

		for b := 1; b <= 1; b++ {
			qusetInfo.BossId = append(qusetInfo.BossId, bossId)
		}
		for k := 0; k < len(enemyId); k++ {
			qusetInfo.EnemyId = append(qusetInfo.EnemyId, enemyId[k])
		}

		stageInfo.Quests = append(stageInfo.Quests, qusetInfo)

	}
	log.Printf("====stageInfo: %+vin", stageInfo)
	for k, q := range stageInfo.Quests {
		log.Printf("   --- quest[%v]: %+v", k, q)
	}

	zStageInfo, err := proto.Marshal(stageInfo)
	if err != nil {
		log.Printf("unmarshal error.")
		return nil, err
	}

	err = db.Set(consts.X_QUEST_STAGE+common.Utoa(stageId), zStageInfo)
	if err != nil {
		return nil, err
	}

	return stageInfo, err
}


func DataAddDemoCity() (err error) {
	db := &data.Data{}
	err = db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return err
	}
	defer db.Close()

	CITY_ID := uint32(1)
	city := &bbproto.CityInfo{}
	//city.Pos = proto.Int32(1)
	city.Id = proto.Uint32(CITY_ID)
	city.CityName = proto.String("PrisonCity")

	stageNum := uint32(1)
	questNum := int(3)
	stageNames := []string{"One", "Two", "Three", "Four", "Five", "Six", "Seven"}

	for k := uint32(0); k < stageNum; k++ {
		stageId := CITY_ID*10 + k + 1
		stage, err := DataAddStageInfo(db, stageId, "Prison "+stageNames[k], QUEST_STORY, questNum)
		if err != nil {
			return err
		}
		city.Stages = append(city.Stages, stage)

//		//Add Quest Config
//		for questId := uint32(stageId*10 + 1); questId <= stageId*10+questNum; questId++ {
//			log.T("Add Quest Config for stageId:%v questId:%v", stageId, questId)
//			DataAddQuestConfig(questId)
//		}

	}

	zCityData, err := proto.Marshal(city)
	if err != nil {
		log.Error("marshal error.")
		return err
	}

	//TODO: slim zCityData.stages to only have satgeId
	err = db.Set(consts.X_QUEST_CITY+common.Utoa(*city.Id), zCityData)
	if err != nil {
		log.Error("db.set X_QUEST_CITY error.")
		return err
	}

	err = common.WriteFile(zCityData, "./1.bytes")
	if err != nil {
		log.Error("WriteFile error.")
		return err
	}

	return err
}

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()

//	DataAddEvolveCity()
	DataAddDemoCity()

	log.Fatal("Generate city.bytes finish.")
}
