package quest

import (
	"container/list"
)

import (
	"bbproto"
	proto "code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
)

type QuestDataMaker struct {
}

func (qm *QuestDataMaker) makeColors(colorPercent []*bbproto.ColorPercent, count int) (colorPack []byte, e Error.Error) {
	log.T("makeColors[ %v ] ...", count)
	colors := make([]byte, count)

	totalPercent := int32(0)
	colorConf := list.New()
	for _, p := range colorPercent {
		totalPercent += int32(*p.Percent * 100)
		colorConf.PushBack(bbproto.ColorPercent{p.Color, proto.Float32(float32(totalPercent)), nil})
	}

	for i := 0; i < count; i++ {
		randNum := common.Randn(totalPercent)
		for e := colorConf.Front(); e != nil; e = e.Next() {
			cp := e.Value.(bbproto.ColorPercent)
			//log.T("\t ===>  loop list: (randNum:%v) {%+v %+v}", randNum, *cp.Color, *cp.Percent)
			if float32(randNum) < *cp.Percent {
//				log.T("\t \t >>>  found match: randNum:%v < %v, color:%v", randNum, *cp.Percent, *cp.Color)
				colors[i] = byte(*cp.Color)
				break
			}
		}
	}

	//for trace log
	line:=""
	log.T("origin colors:   \tcount=(%v)", len(colors))
	for k, c := range colors {
		if k % 40 == 0 && k>0 {
			log.T("color[%v]: %v", k/40+1, line)
			line = ""
		}else {
			line += common.Utoa(uint32(c))+" "
		}
	}

	return qm.makePackColors(colors)
}

// use 3 bits to store a color, Convert colors to colorPack
func (qm *QuestDataMaker) makePackColors(colors []byte) (colorPack []byte, e Error.Error) {
	const (
		HEAD_BIT1 = byte(0x4) // 100
		HEAD_BIT2 = byte(0x6) // 110

		TAIL_BIT1 = byte(0x1) // 001
		TAIL_BIT2 = byte(0x3) // 011
	)

	datalen := (len(colors) * 3) / 8
	log.T("makePackColors:: datLen:%v, mod:%v", datalen, len(colors)*8%3)
	if (len(colors)*3)%8 != 0 {
		datalen += 1
	}

	result := make([]byte, datalen)

	count := len(colors)
	k := 0
	for i := 0; i < count; i += 8 {
		//log.T("result[%v]:%v colors[%v]: %v<<5 = %v", k, result[k], i, colors[i], colors[i]<<5)
		result[k] += (colors[i] << 5)

		if i+1 < count {
			//log.T("result[%v]:%v %v<< 2 = %v", k+1, result[k], colors[i+1], colors[i+1]<<2)
			result[k] += (colors[i+1] << 2)
		}
		if i+2 < count {
			//log.T("result[%v]:%v %v&HEAD_BIT2  = %v", k+2, result[k], colors[i+2], colors[i+2]&HEAD_BIT2>>1)
			result[k] += (colors[i+2] & HEAD_BIT2 >> 1)
		}

		if i+2 < count {
			//log.T("result[%v]:%v & TAIL_BIT1<< 7 = %v", k+3, result[k], colors[i+2]&TAIL_BIT1<<7)
			result[k+1] += (colors[i+2] & TAIL_BIT1 << 7)
		}
		if i+3 < count {
			result[k+1] += (colors[i+3] << 4)
		}
		//log.T("result[%v]:%v", k+1, result[k+1])
		if i+4 < count {
			result[k+1] += (colors[i+4] << 1)
		}
		if i+5 < count {
			result[k+1] += (colors[i+5] & HEAD_BIT1 >> 2)
		}
		if i+5 < count {
			result[k+2] += (colors[i+5] & TAIL_BIT2 << 6)
		}
		if i+6 < count {
			result[k+2] += (colors[i+6] << 3)
		}
		if i+7 < count {
			result[k+2] += (colors[i+7])
		}
		k += 3
	}

//	for k, b := range result {
//		log.T("[%v] makePackColors result b=%v ", k, b)
//	}

	return result, Error.OK()
}

//retrieve target item from list
func (qm *QuestDataMaker) getItemFromList(pos int32, list *map[int32]TUsedValue) (result int32) {
	result = -1
	if pos >= int32(len(*list)) {
		return result
	}

	index := int32(0)
	for k, ss := range *list {
		//log.T("LOOP list[%v]: %+v ...  Now index=%v ", k, ss, index)
		if ss.Used {
			//log.T("==> list[%v] is used.", k)
			continue
		}

		if index == pos {
			(*list)[k] = TUsedValue{ss.Value, true}
			result = ss.Value
			log.T("LOOP list[%v]: found ss:%+v list[k]:%+v", k, ss, (*list)[k])
			log.T("\t==> all list: %+v", (*list))
			break
		}
		index++
	}
	return result
}

func (qm *QuestDataMaker) getStarConf(iStar int32, starsConf []*bbproto.StarConfig) (starConf *bbproto.StarConfig, e Error.Error) {
	for i, conf := range starsConf {
		log.T("== getStarConf : loop[%v]:%+v", i, conf)
		if conf.Star != nil && int32(*conf.Star) == iStar {
			starConf = conf
			log.T("found matched starConf[%+v]", iStar)
			return starConf, Error.OK()
		}
	}

	return nil, Error.New(EC.DATA_NOT_EXISTS, "Not found starconf.")
}

func (qm *QuestDataMaker) fillGridsList(typeList map[int32]TUsedValue, starList map[int32]TUsedValue, floorConf *bbproto.QuestFloorConfig) Error.Error {
	currNum := int32(0)
	for n := currNum; n < *floorConf.TreasureNum; n++ {
		typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_TREATURE), false}
	}
	currNum += *floorConf.TreasureNum

	for n := currNum; n < currNum+(*floorConf.TrapNum); n++ {
		typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_TRAP), false}
	}
	currNum += *floorConf.TrapNum

	for n := currNum; n < currNum+(*floorConf.EnemyNum); n++ {
		typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_ENEMY), false}
	}
	currNum += *floorConf.EnemyNum

	for n := currNum; n < consts.N_DUNGEON_GRID_COUNT-1; n++ {
		typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_NONE), true}
	}
	log.T("typeList: len(%v | %v): %v", currNum, len(typeList), typeList)

	starNum := int32(0)
	for i, star := range floorConf.Stars {
		log.T(" floorConf.Stars[%v]: %+v", i, star)
		for n := starNum; n < starNum+(*star.Repeat); n++ {
			starList[n] = TUsedValue{int32(*star.Star), false}
		}
		starNum += *star.Repeat
	}
	for n := starNum; n < consts.N_DUNGEON_GRID_COUNT-1; n++ {
		starList[n] = TUsedValue{int32(bbproto.EGridStar_GS_EMPTY), true}
	}

	log.T("starList (len:%v | %v): %v", starNum, len(starList), starList)

	return Error.OK()
}

func (qm *QuestDataMaker) getEnemyConf(enemyId uint32, confs []*bbproto.EnemyInfoConf) (enemyConf *bbproto.EnemyInfoConf, e Error.Error) {
	for _, enemyConf := range confs {
		if enemyId == *enemyConf.Enemy.EnemyId {
			return enemyConf, Error.OK()
		}
	}
	return nil, Error.New(EC.DATA_NOT_EXISTS)
}

func (qm *QuestDataMaker) MakeData(config *bbproto.QuestConfig) (questData bbproto.QuestDungeonData, e Error.Error) {


	questData.QuestId = config.QuestId
	questData.QuestId = config.QuestId
	questData.Colors, e = qm.makeColors(config.Colors, consts.N_QUEST_COLOR_BLOCK_NUM)

	for _, bossConf := range config.Boss {
		questData.Boss = append(questData.Boss, bossConf.Enemy)
	}
	for _, enemyConf := range config.Enemys {
		questData.Enemys = append(questData.Enemys, enemyConf.Enemy)
	}

	//generate floor's grid data
	starList := make(map[int32]TUsedValue, consts.N_DUNGEON_GRID_COUNT-1)
	typeList := make(map[int32]TUsedValue, consts.N_DUNGEON_GRID_COUNT-1)

	for k, floorConf := range config.Floors {
		questFloor := &bbproto.QuestFloor{}

		log.T("--pre typeList:%+v", typeList)

		qm.fillGridsList(typeList, starList, floorConf)
		log.T("--after typeList:%+v", typeList)

		gridCount := int32(consts.N_DUNGEON_GRID_COUNT - 1)
		for i := 0; i < consts.N_DUNGEON_GRID_COUNT-1 && gridCount > 0; i++ {
			grid := &bbproto.QuestGrid{}
			grid.Position = proto.Int32(int32(i))

			randPos := common.Randn(gridCount)
			gridCount -= 1

			//retrieve target item from list
			iType := qm.getItemFromList(randPos, &typeList)
			iStar := qm.getItemFromList(randPos, &starList)

			log.T("--[GRID-%v] randPos:%v iType:%v, iStar:%v,\n floorConf.Stars:%+v",
				i, randPos, iType, iStar, floorConf.Stars)
			if iType < 0 || iStar < 0 {
				//it should be empty grid
				questFloor.GridInfo = append(questFloor.GridInfo, grid)
				continue
			}

			starConf, e := qm.getStarConf(iStar, floorConf.Stars)
			if e.IsError() {
				log.Error("getStarConf[%v] ret err:%v ", iStar, e.Error())
				return questData, e //unreachable here
			}
			log.T("After found starConf[%v] ret err:%v,  starConf:%+v ", iStar, e.Error(), starConf)

			grid.Star = starConf.Star
			grid.Color = proto.Int32(common.Randn(3)) //0:red,1:yellow,2:blue

			if iType == int32(bbproto.EQuestGridType_Q_TREATURE) {
				tmpType := bbproto.EQuestGridType_Q_TREATURE
				grid.Type = &tmpType
				grid.Coins = proto.Int32(common.Rand(*starConf.Coin.Min, *starConf.Coin.Max))
				log.T("random ret treasure, grid: %+v", grid)
			} else if iType == int32(bbproto.EQuestGridType_Q_TRAP) {
				tmpType := bbproto.EQuestGridType_Q_TRAP
				grid.Type = &tmpType

				trapNum := len(starConf.Trap)
				if starConf.Trap == nil || trapNum == 0 {
//					e = Error.New("invalid quest data config: starConf.Trap is empty.")
					log.Error("invalid quest data config: starConf.Trap is empty.")

					tmpType = bbproto.EQuestGridType_Q_NONE
					grid.Type = &tmpType
					questFloor.GridInfo = append(questFloor.GridInfo, grid)
					continue
				}

				randn := common.Randn(int32(trapNum))
				log.T("star:%v, randn=%v trapNum:%v starConf.Trap:%v", starConf.Star, randn, trapNum, starConf.Trap)

				grid.TrapId = proto.Uint32(starConf.Trap[randn])
				log.T(" randn:%v -> trapId: %v", randn, starConf.Trap[randn])

			} else if iType == int32(bbproto.EQuestGridType_Q_ENEMY) {
				tmpType := bbproto.EQuestGridType_Q_ENEMY
				if *starConf.EnemyNum.Max == 0 {
					tmpType = bbproto.EQuestGridType_Q_NONE
				}
				grid.Type = &tmpType
				log.T("EnemyNum: [min:%v max:%v]", *starConf.EnemyNum.Min, *starConf.EnemyNum.Max)

				enemyNum := common.Rand(*starConf.EnemyNum.Min, *starConf.EnemyNum.Max)
				log.T("randn:%v enemys: ", enemyNum)

				for x := int32(0); x < enemyNum; x++ {
					nn := int32(len(starConf.EnemyPool))
					nn = common.Randn(nn) % (nn - 1)
					enemyId := starConf.EnemyPool[nn]
					grid.EnemyId = append(grid.EnemyId, enemyId)
					log.T("\t--randnn:%v enemyId:%v ", nn, starConf.EnemyPool[nn])

					//fill drop unit by dropRate
					if grid.DropId == nil { //only drop one unit
						enemyConf, e := qm.getEnemyConf(enemyId, config.Enemys)
						if !e.IsError() && enemyConf != nil {
							if common.HitRandomRate(*enemyConf.DropRate) {
								dropUnit := &bbproto.DropUnit{}
								dropUnit.UnitId = enemyConf.DropUnitId
								dropUnit.Level = enemyConf.DropUnitLevel
								if enemyConf.AddRate != nil && common.HitRandomRate(*enemyConf.AddRate) {
									iRandAdd := common.Randn(2)
									if iRandAdd == 0 {
										dropUnit.AddHp = proto.Int32(1)	
									}else {
										dropUnit.AddAttack = proto.Int32(1)
									}
								}
								
								log.T("\t -- append dropUnit:%+v", dropUnit)

								dropUnit.DropId = proto.Uint32(uint32(len(questData.Drop) + 1))

								grid.DropId = proto.Uint32(*dropUnit.DropId)
								grid.DropPos = proto.Int32(x)
								questData.Drop = append(questData.Drop, dropUnit)
							}
						}
					}
				}

			} else {
				log.Warn("Random ret grid type is Empty. (randPos: %v iType:%v)", randPos, iType)
			}

			questFloor.GridInfo = append(questFloor.GridInfo, grid)
			log.T("===[GRID-%v]: %+v", i, grid)
		}

		log.T("starList: %+v", starList)
		log.T("typeList: %+v", typeList)

		log.T("floor[%v]: questFloor.GridInfo:%+v floorConf:%+v", k, questFloor.GridInfo, floorConf)

		questData.Floors = append(questData.Floors, questFloor)
	}

	return questData, Error.OK()
}
