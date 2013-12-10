// Code generated by protoc-gen-go.
// source: unit.proto
// DO NOT EDIT!

package bbproto

import proto "code.google.com/p/goprotobuf/proto"
import json "encoding/json"
import math "math"
import bbproto1 "base.pb"
import bbproto2 "skill.pb"

// Reference proto, json, and math imports to suppress error if they are not otherwise used.
var _ = proto.Marshal
var _ = &json.SyntaxError{}
var _ = math.Inf

// Request from public import base.proto
type Request bbproto1.Request

func (m *Request) Reset()               { (*bbproto1.Request)(m).Reset() }
func (m *Request) String() string       { return (*bbproto1.Request)(m).String() }
func (*Request) ProtoMessage()          {}
func (m *Request) GetApiVer() string    { return (*bbproto1.Request)(m).GetApiVer() }
func (m *Request) GetSessionId() string { return (*bbproto1.Request)(m).GetSessionId() }
func (m *Request) GetPacketId() int32   { return (*bbproto1.Request)(m).GetPacketId() }

// Response from public import base.proto
type Response bbproto1.Response

func (m *Response) Reset()             { (*bbproto1.Response)(m).Reset() }
func (m *Response) String() string     { return (*bbproto1.Response)(m).String() }
func (*Response) ProtoMessage()        {}
func (m *Response) GetApiVer() string  { return (*bbproto1.Response)(m).GetApiVer() }
func (m *Response) GetCode() int32     { return (*bbproto1.Response)(m).GetCode() }
func (m *Response) GetError() string   { return (*bbproto1.Response)(m).GetError() }
func (m *Response) GetPacketId() int32 { return (*bbproto1.Response)(m).GetPacketId() }

// EUnitType from public import base.proto
type EUnitType bbproto1.EUnitType

var EUnitType_name = bbproto1.EUnitType_name
var EUnitType_value = bbproto1.EUnitType_value

func NewEUnitType(x EUnitType) *EUnitType { e := EUnitType(x); return &e }

const EUnitType_UALL = EUnitType(bbproto1.EUnitType_UALL)
const EUnitType_UFIRE = EUnitType(bbproto1.EUnitType_UFIRE)
const EUnitType_UWATER = EUnitType(bbproto1.EUnitType_UWATER)
const EUnitType_UWIND = EUnitType(bbproto1.EUnitType_UWIND)
const EUnitType_ULIGHT = EUnitType(bbproto1.EUnitType_ULIGHT)
const EUnitType_UDARK = EUnitType(bbproto1.EUnitType_UDARK)
const EUnitType_UNONE = EUnitType(bbproto1.EUnitType_UNONE)

// SkillSingleAttack from public import skill.proto
type SkillSingleAttack bbproto2.SkillSingleAttack

func (m *SkillSingleAttack) Reset()         { (*bbproto2.SkillSingleAttack)(m).Reset() }
func (m *SkillSingleAttack) String() string { return (*bbproto2.SkillSingleAttack)(m).String() }
func (*SkillSingleAttack) ProtoMessage()    {}
func (m *SkillSingleAttack) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillSingleAttack)(m).GetType())
}
func (m *SkillSingleAttack) GetValue() float32 { return (*bbproto2.SkillSingleAttack)(m).GetValue() }

// SkillAllAttack from public import skill.proto
type SkillAllAttack bbproto2.SkillAllAttack

func (m *SkillAllAttack) Reset()         { (*bbproto2.SkillAllAttack)(m).Reset() }
func (m *SkillAllAttack) String() string { return (*bbproto2.SkillAllAttack)(m).String() }
func (*SkillAllAttack) ProtoMessage()    {}
func (m *SkillAllAttack) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillAllAttack)(m).GetType())
}
func (m *SkillAllAttack) GetValue() float32 { return (*bbproto2.SkillAllAttack)(m).GetValue() }

// SkillSingleAtkRecoverHP from public import skill.proto
type SkillSingleAtkRecoverHP bbproto2.SkillSingleAtkRecoverHP

func (m *SkillSingleAtkRecoverHP) Reset() { (*bbproto2.SkillSingleAtkRecoverHP)(m).Reset() }
func (m *SkillSingleAtkRecoverHP) String() string {
	return (*bbproto2.SkillSingleAtkRecoverHP)(m).String()
}
func (*SkillSingleAtkRecoverHP) ProtoMessage() {}
func (m *SkillSingleAtkRecoverHP) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillSingleAtkRecoverHP)(m).GetType())
}
func (m *SkillSingleAtkRecoverHP) GetValue() float32 {
	return (*bbproto2.SkillSingleAtkRecoverHP)(m).GetValue()
}

// SkillSuicideAttack from public import skill.proto
type SkillSuicideAttack bbproto2.SkillSuicideAttack

func (m *SkillSuicideAttack) Reset()         { (*bbproto2.SkillSuicideAttack)(m).Reset() }
func (m *SkillSuicideAttack) String() string { return (*bbproto2.SkillSuicideAttack)(m).String() }
func (*SkillSuicideAttack) ProtoMessage()    {}
func (m *SkillSuicideAttack) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillSuicideAttack)(m).GetType())
}
func (m *SkillSuicideAttack) GetValue() float32 { return (*bbproto2.SkillSuicideAttack)(m).GetValue() }

// SkillTargetTypeAttack from public import skill.proto
type SkillTargetTypeAttack bbproto2.SkillTargetTypeAttack

func (m *SkillTargetTypeAttack) Reset()         { (*bbproto2.SkillTargetTypeAttack)(m).Reset() }
func (m *SkillTargetTypeAttack) String() string { return (*bbproto2.SkillTargetTypeAttack)(m).String() }
func (*SkillTargetTypeAttack) ProtoMessage()    {}
func (m *SkillTargetTypeAttack) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillTargetTypeAttack)(m).GetType())
}
func (m *SkillTargetTypeAttack) GetValue() float32 {
	return (*bbproto2.SkillTargetTypeAttack)(m).GetValue()
}
func (m *SkillTargetTypeAttack) GetPeriod() int32 {
	return (*bbproto2.SkillTargetTypeAttack)(m).GetPeriod()
}

// SkillKillHP from public import skill.proto
type SkillKillHP bbproto2.SkillKillHP

func (m *SkillKillHP) Reset()              { (*bbproto2.SkillKillHP)(m).Reset() }
func (m *SkillKillHP) String() string      { return (*bbproto2.SkillKillHP)(m).String() }
func (*SkillKillHP) ProtoMessage()         {}
func (m *SkillKillHP) GetType() EValueType { return (EValueType)((*bbproto2.SkillKillHP)(m).GetType()) }
func (m *SkillKillHP) GetValue() float32   { return (*bbproto2.SkillKillHP)(m).GetValue() }

// SkillRecoverHP from public import skill.proto
type SkillRecoverHP bbproto2.SkillRecoverHP

func (m *SkillRecoverHP) Reset()         { (*bbproto2.SkillRecoverHP)(m).Reset() }
func (m *SkillRecoverHP) String() string { return (*bbproto2.SkillRecoverHP)(m).String() }
func (*SkillRecoverHP) ProtoMessage()    {}
func (m *SkillRecoverHP) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillRecoverHP)(m).GetType())
}
func (m *SkillRecoverHP) GetValue() float32 { return (*bbproto2.SkillRecoverHP)(m).GetValue() }
func (m *SkillRecoverHP) GetPeriod() EPeriod {
	return (EPeriod)((*bbproto2.SkillRecoverHP)(m).GetPeriod())
}

// SkillRecoverSP from public import skill.proto
type SkillRecoverSP bbproto2.SkillRecoverSP

func (m *SkillRecoverSP) Reset()         { (*bbproto2.SkillRecoverSP)(m).Reset() }
func (m *SkillRecoverSP) String() string { return (*bbproto2.SkillRecoverSP)(m).String() }
func (*SkillRecoverSP) ProtoMessage()    {}
func (m *SkillRecoverSP) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillRecoverSP)(m).GetType())
}
func (m *SkillRecoverSP) GetValue() float32 { return (*bbproto2.SkillRecoverSP)(m).GetValue() }

// SkillReduceHurt from public import skill.proto
type SkillReduceHurt bbproto2.SkillReduceHurt

func (m *SkillReduceHurt) Reset()         { (*bbproto2.SkillReduceHurt)(m).Reset() }
func (m *SkillReduceHurt) String() string { return (*bbproto2.SkillReduceHurt)(m).String() }
func (*SkillReduceHurt) ProtoMessage()    {}
func (m *SkillReduceHurt) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillReduceHurt)(m).GetType())
}
func (m *SkillReduceHurt) GetValue() float32 { return (*bbproto2.SkillReduceHurt)(m).GetValue() }
func (m *SkillReduceHurt) GetPeriod() int32  { return (*bbproto2.SkillReduceHurt)(m).GetPeriod() }

// SkillReduceDefence from public import skill.proto
type SkillReduceDefence bbproto2.SkillReduceDefence

func (m *SkillReduceDefence) Reset()         { (*bbproto2.SkillReduceDefence)(m).Reset() }
func (m *SkillReduceDefence) String() string { return (*bbproto2.SkillReduceDefence)(m).String() }
func (*SkillReduceDefence) ProtoMessage()    {}
func (m *SkillReduceDefence) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillReduceDefence)(m).GetType())
}
func (m *SkillReduceDefence) GetValue() float32 { return (*bbproto2.SkillReduceDefence)(m).GetValue() }
func (m *SkillReduceDefence) GetPeriod() int32  { return (*bbproto2.SkillReduceDefence)(m).GetPeriod() }

// SkillDeferAttackRound from public import skill.proto
type SkillDeferAttackRound bbproto2.SkillDeferAttackRound

func (m *SkillDeferAttackRound) Reset()         { (*bbproto2.SkillDeferAttackRound)(m).Reset() }
func (m *SkillDeferAttackRound) String() string { return (*bbproto2.SkillDeferAttackRound)(m).String() }
func (*SkillDeferAttackRound) ProtoMessage()    {}
func (m *SkillDeferAttackRound) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillDeferAttackRound)(m).GetType())
}
func (m *SkillDeferAttackRound) GetValue() float32 {
	return (*bbproto2.SkillDeferAttackRound)(m).GetValue()
}

// SkillPoison from public import skill.proto
type SkillPoison bbproto2.SkillPoison

func (m *SkillPoison) Reset()              { (*bbproto2.SkillPoison)(m).Reset() }
func (m *SkillPoison) String() string      { return (*bbproto2.SkillPoison)(m).String() }
func (*SkillPoison) ProtoMessage()         {}
func (m *SkillPoison) GetType() EValueType { return (EValueType)((*bbproto2.SkillPoison)(m).GetType()) }
func (m *SkillPoison) GetValue() float32   { return (*bbproto2.SkillPoison)(m).GetValue() }

// SkillDelayTime from public import skill.proto
type SkillDelayTime bbproto2.SkillDelayTime

func (m *SkillDelayTime) Reset()         { (*bbproto2.SkillDelayTime)(m).Reset() }
func (m *SkillDelayTime) String() string { return (*bbproto2.SkillDelayTime)(m).String() }
func (*SkillDelayTime) ProtoMessage()    {}
func (m *SkillDelayTime) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillDelayTime)(m).GetType())
}
func (m *SkillDelayTime) GetValue() float32 { return (*bbproto2.SkillDelayTime)(m).GetValue() }

// SkillConvertUnitType from public import skill.proto
type SkillConvertUnitType bbproto2.SkillConvertUnitType

func (m *SkillConvertUnitType) Reset()         { (*bbproto2.SkillConvertUnitType)(m).Reset() }
func (m *SkillConvertUnitType) String() string { return (*bbproto2.SkillConvertUnitType)(m).String() }
func (*SkillConvertUnitType) ProtoMessage()    {}
func (m *SkillConvertUnitType) GetType() EValueType {
	return (EValueType)((*bbproto2.SkillConvertUnitType)(m).GetType())
}

// ActiveSkill from public import skill.proto
type ActiveSkill bbproto2.ActiveSkill

func (m *ActiveSkill) Reset()          { (*bbproto2.ActiveSkill)(m).Reset() }
func (m *ActiveSkill) String() string  { return (*bbproto2.ActiveSkill)(m).String() }
func (*ActiveSkill) ProtoMessage()     {}
func (m *ActiveSkill) GetNo() int32    { return (*bbproto2.ActiveSkill)(m).GetNo() }
func (m *ActiveSkill) GetName() string { return (*bbproto2.ActiveSkill)(m).GetName() }
func (m *ActiveSkill) GetDesc() string { return (*bbproto2.ActiveSkill)(m).GetDesc() }
func (m *ActiveSkill) GetSingleAttack() *SkillSingleAttack {
	return (*SkillSingleAttack)((*bbproto2.ActiveSkill)(m).GetSingleAttack())
}
func (m *ActiveSkill) GetAllAttack() *SkillAllAttack {
	return (*SkillAllAttack)((*bbproto2.ActiveSkill)(m).GetAllAttack())
}
func (m *ActiveSkill) GetSingleAtkRecoverHP() *SkillSingleAtkRecoverHP {
	return (*SkillSingleAtkRecoverHP)((*bbproto2.ActiveSkill)(m).GetSingleAtkRecoverHP())
}
func (m *ActiveSkill) GetSuicideAttack() *SkillSuicideAttack {
	return (*SkillSuicideAttack)((*bbproto2.ActiveSkill)(m).GetSuicideAttack())
}
func (m *ActiveSkill) GetTargetTypeAttack() *SkillTargetTypeAttack {
	return (*SkillTargetTypeAttack)((*bbproto2.ActiveSkill)(m).GetTargetTypeAttack())
}
func (m *ActiveSkill) GetKillHP() *SkillKillHP {
	return (*SkillKillHP)((*bbproto2.ActiveSkill)(m).GetKillHP())
}
func (m *ActiveSkill) GetRecoverHP() *SkillRecoverHP {
	return (*SkillRecoverHP)((*bbproto2.ActiveSkill)(m).GetRecoverHP())
}
func (m *ActiveSkill) GetRecoverSP() *SkillRecoverSP {
	return (*SkillRecoverSP)((*bbproto2.ActiveSkill)(m).GetRecoverSP())
}
func (m *ActiveSkill) GetReduceHurt() *SkillReduceHurt {
	return (*SkillReduceHurt)((*bbproto2.ActiveSkill)(m).GetReduceHurt())
}
func (m *ActiveSkill) GetReduceDefence() *SkillReduceDefence {
	return (*SkillReduceDefence)((*bbproto2.ActiveSkill)(m).GetReduceDefence())
}
func (m *ActiveSkill) GetDeferAttackRound() *SkillDeferAttackRound {
	return (*SkillDeferAttackRound)((*bbproto2.ActiveSkill)(m).GetDeferAttackRound())
}
func (m *ActiveSkill) GetPoison() *SkillPoison {
	return (*SkillPoison)((*bbproto2.ActiveSkill)(m).GetPoison())
}
func (m *ActiveSkill) GetDelayTime() *SkillDelayTime {
	return (*SkillDelayTime)((*bbproto2.ActiveSkill)(m).GetDelayTime())
}
func (m *ActiveSkill) GetConvertUnitType() *SkillConvertUnitType {
	return (*SkillConvertUnitType)((*bbproto2.ActiveSkill)(m).GetConvertUnitType())
}

// SkillDodgeTrap from public import skill.proto
type SkillDodgeTrap bbproto2.SkillDodgeTrap

func (m *SkillDodgeTrap) Reset()              { (*bbproto2.SkillDodgeTrap)(m).Reset() }
func (m *SkillDodgeTrap) String() string      { return (*bbproto2.SkillDodgeTrap)(m).String() }
func (*SkillDodgeTrap) ProtoMessage()         {}
func (m *SkillDodgeTrap) GetTrapLevel() int32 { return (*bbproto2.SkillDodgeTrap)(m).GetTrapLevel() }

// SkillAntiAttack from public import skill.proto
type SkillAntiAttack bbproto2.SkillAntiAttack

func (m *SkillAntiAttack) Reset()         { (*bbproto2.SkillAntiAttack)(m).Reset() }
func (m *SkillAntiAttack) String() string { return (*bbproto2.SkillAntiAttack)(m).String() }
func (*SkillAntiAttack) ProtoMessage()    {}
func (m *SkillAntiAttack) GetProbability() int32 {
	return (*bbproto2.SkillAntiAttack)(m).GetProbability()
}
func (m *SkillAntiAttack) GetAntiAtkRatio() int32 {
	return (*bbproto2.SkillAntiAttack)(m).GetAntiAtkRatio()
}

// PassiveSkill from public import skill.proto
type PassiveSkill bbproto2.PassiveSkill

func (m *PassiveSkill) Reset()          { (*bbproto2.PassiveSkill)(m).Reset() }
func (m *PassiveSkill) String() string  { return (*bbproto2.PassiveSkill)(m).String() }
func (*PassiveSkill) ProtoMessage()     {}
func (m *PassiveSkill) GetNo() int32    { return (*bbproto2.PassiveSkill)(m).GetNo() }
func (m *PassiveSkill) GetName() string { return (*bbproto2.PassiveSkill)(m).GetName() }
func (m *PassiveSkill) GetDesc() string { return (*bbproto2.PassiveSkill)(m).GetDesc() }
func (m *PassiveSkill) GetAntiAttack() *SkillAntiAttack {
	return (*SkillAntiAttack)((*bbproto2.PassiveSkill)(m).GetAntiAttack())
}
func (m *PassiveSkill) GetDodgeTrap() *SkillDodgeTrap {
	return (*SkillDodgeTrap)((*bbproto2.PassiveSkill)(m).GetDodgeTrap())
}

// NormalSkill from public import skill.proto
type NormalSkill bbproto2.NormalSkill

func (m *NormalSkill) Reset()          { (*bbproto2.NormalSkill)(m).Reset() }
func (m *NormalSkill) String() string  { return (*bbproto2.NormalSkill)(m).String() }
func (*NormalSkill) ProtoMessage()     {}
func (m *NormalSkill) GetNo() int32    { return (*bbproto2.NormalSkill)(m).GetNo() }
func (m *NormalSkill) GetName() string { return (*bbproto2.NormalSkill)(m).GetName() }
func (m *NormalSkill) GetDesc() string { return (*bbproto2.NormalSkill)(m).GetDesc() }
func (m *NormalSkill) GetAttackType() EAttackType {
	return (EAttackType)((*bbproto2.NormalSkill)(m).GetAttackType())
}
func (m *NormalSkill) GetActiveBlocks() []uint32 { return (*bbproto2.NormalSkill)(m).GetActiveBlocks() }
func (m *NormalSkill) GetAttackValue() float32   { return (*bbproto2.NormalSkill)(m).GetAttackValue() }

// SkillBoost from public import skill.proto
type SkillBoost bbproto2.SkillBoost

func (m *SkillBoost) Reset()         { (*bbproto2.SkillBoost)(m).Reset() }
func (m *SkillBoost) String() string { return (*bbproto2.SkillBoost)(m).String() }
func (*SkillBoost) ProtoMessage()    {}
func (m *SkillBoost) GetBoostType() EBoostType {
	return (EBoostType)((*bbproto2.SkillBoost)(m).GetBoostType())
}
func (m *SkillBoost) GetBoostValue() float32 { return (*bbproto2.SkillBoost)(m).GetBoostValue() }
func (m *SkillBoost) GetTargetType() EBoostTarget {
	return (EBoostTarget)((*bbproto2.SkillBoost)(m).GetTargetType())
}
func (m *SkillBoost) GetTargetValue() int32 { return (*bbproto2.SkillBoost)(m).GetTargetValue() }

// SkillExtraAttack from public import skill.proto
type SkillExtraAttack bbproto2.SkillExtraAttack

func (m *SkillExtraAttack) Reset()         { (*bbproto2.SkillExtraAttack)(m).Reset() }
func (m *SkillExtraAttack) String() string { return (*bbproto2.SkillExtraAttack)(m).String() }
func (*SkillExtraAttack) ProtoMessage()    {}
func (m *SkillExtraAttack) GetAttackValue() float32 {
	return (*bbproto2.SkillExtraAttack)(m).GetAttackValue()
}

// LeaderSkill from public import skill.proto
type LeaderSkill bbproto2.LeaderSkill

func (m *LeaderSkill) Reset()          { (*bbproto2.LeaderSkill)(m).Reset() }
func (m *LeaderSkill) String() string  { return (*bbproto2.LeaderSkill)(m).String() }
func (*LeaderSkill) ProtoMessage()     {}
func (m *LeaderSkill) GetNo() int32    { return (*bbproto2.LeaderSkill)(m).GetNo() }
func (m *LeaderSkill) GetName() string { return (*bbproto2.LeaderSkill)(m).GetName() }
func (m *LeaderSkill) GetDesc() string { return (*bbproto2.LeaderSkill)(m).GetDesc() }
func (m *LeaderSkill) GetRaceBoost() *SkillBoost {
	return (*SkillBoost)((*bbproto2.LeaderSkill)(m).GetRaceBoost())
}
func (m *LeaderSkill) GetRecoverHP() *SkillRecoverHP {
	return (*SkillRecoverHP)((*bbproto2.LeaderSkill)(m).GetRecoverHP())
}
func (m *LeaderSkill) GetReduceHurt() *SkillReduceHurt {
	return (*SkillReduceHurt)((*bbproto2.LeaderSkill)(m).GetReduceHurt())
}
func (m *LeaderSkill) GetDelayTime() *SkillDelayTime {
	return (*SkillDelayTime)((*bbproto2.LeaderSkill)(m).GetDelayTime())
}
func (m *LeaderSkill) GetConvertUnitType() *SkillConvertUnitType {
	return (*SkillConvertUnitType)((*bbproto2.LeaderSkill)(m).GetConvertUnitType())
}
func (m *LeaderSkill) GetExtraAttack() *SkillExtraAttack {
	return (*SkillExtraAttack)((*bbproto2.LeaderSkill)(m).GetExtraAttack())
}

// EValueType from public import skill.proto
type EValueType bbproto2.EValueType

var EValueType_name = bbproto2.EValueType_name
var EValueType_value = bbproto2.EValueType_value

func NewEValueType(x EValueType) *EValueType { e := EValueType(x); return &e }

const EValueType_FIXED = EValueType(bbproto2.EValueType_FIXED)
const EValueType_MULTIPLE = EValueType(bbproto2.EValueType_MULTIPLE)
const EValueType_PERCENT = EValueType(bbproto2.EValueType_PERCENT)
const EValueType_SECOND = EValueType(bbproto2.EValueType_SECOND)
const EValueType_ROUND = EValueType(bbproto2.EValueType_ROUND)
const EValueType_COLORTYPE = EValueType(bbproto2.EValueType_COLORTYPE)
const EValueType_RANDOMCOLOR = EValueType(bbproto2.EValueType_RANDOMCOLOR)

// EAttackType from public import skill.proto
type EAttackType bbproto2.EAttackType

var EAttackType_name = bbproto2.EAttackType_name
var EAttackType_value = bbproto2.EAttackType_value

func NewEAttackType(x EAttackType) *EAttackType { e := EAttackType(x); return &e }

const EAttackType_ATK_SINGLE = EAttackType(bbproto2.EAttackType_ATK_SINGLE)
const EAttackType_ATK_ALL = EAttackType(bbproto2.EAttackType_ATK_ALL)
const EAttackType_RECOVER_HP = EAttackType(bbproto2.EAttackType_RECOVER_HP)

// EBoostType from public import skill.proto
type EBoostType bbproto2.EBoostType

var EBoostType_name = bbproto2.EBoostType_name
var EBoostType_value = bbproto2.EBoostType_value

func NewEBoostType(x EBoostType) *EBoostType { e := EBoostType(x); return &e }

const EBoostType_BOOST_ATTACK = EBoostType(bbproto2.EBoostType_BOOST_ATTACK)
const EBoostType_BOOST_HP = EBoostType(bbproto2.EBoostType_BOOST_HP)

// EBoostTarget from public import skill.proto
type EBoostTarget bbproto2.EBoostTarget

var EBoostTarget_name = bbproto2.EBoostTarget_name
var EBoostTarget_value = bbproto2.EBoostTarget_value

func NewEBoostTarget(x EBoostTarget) *EBoostTarget { e := EBoostTarget(x); return &e }

const EBoostTarget_UNIT_RACE = EBoostTarget(bbproto2.EBoostTarget_UNIT_RACE)
const EBoostTarget_UNIT_TYPE = EBoostTarget(bbproto2.EBoostTarget_UNIT_TYPE)

// EPeriod from public import skill.proto
type EPeriod bbproto2.EPeriod

var EPeriod_name = bbproto2.EPeriod_name
var EPeriod_value = bbproto2.EPeriod_value

func NewEPeriod(x EPeriod) *EPeriod { e := EPeriod(x); return &e }

const EPeriod_EP_RIGHT_NOW = EPeriod(bbproto2.EPeriod_EP_RIGHT_NOW)
const EPeriod_EP_EVERY_ROUND = EPeriod(bbproto2.EPeriod_EP_EVERY_ROUND)
const EPeriod_EP_EVERY_STEP = EPeriod(bbproto2.EPeriod_EP_EVERY_STEP)

type UnitStatus struct {
	Type             *int32 `protobuf:"varint,1,opt,name=type" json:"type,omitempty"`
	Race             *int32 `protobuf:"varint,2,opt,name=race" json:"race,omitempty"`
	Rare             *int32 `protobuf:"varint,3,opt,name=rare" json:"rare,omitempty"`
	Hp               *int32 `protobuf:"varint,4,opt,name=hp" json:"hp,omitempty"`
	Attack           *int32 `protobuf:"varint,5,opt,name=attack" json:"attack,omitempty"`
	Defence          *int32 `protobuf:"varint,6,opt,name=defence" json:"defence,omitempty"`
	Level            *int32 `protobuf:"varint,7,opt,name=level" json:"level,omitempty"`
	NextLvExp        *int32 `protobuf:"varint,8,opt,name=nextLvExp" json:"nextLvExp,omitempty"`
	Cost             *int32 `protobuf:"varint,9,opt,name=cost" json:"cost,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *UnitStatus) Reset()         { *m = UnitStatus{} }
func (m *UnitStatus) String() string { return proto.CompactTextString(m) }
func (*UnitStatus) ProtoMessage()    {}

func (m *UnitStatus) GetType() int32 {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return 0
}

func (m *UnitStatus) GetRace() int32 {
	if m != nil && m.Race != nil {
		return *m.Race
	}
	return 0
}

func (m *UnitStatus) GetRare() int32 {
	if m != nil && m.Rare != nil {
		return *m.Rare
	}
	return 0
}

func (m *UnitStatus) GetHp() int32 {
	if m != nil && m.Hp != nil {
		return *m.Hp
	}
	return 0
}

func (m *UnitStatus) GetAttack() int32 {
	if m != nil && m.Attack != nil {
		return *m.Attack
	}
	return 0
}

func (m *UnitStatus) GetDefence() int32 {
	if m != nil && m.Defence != nil {
		return *m.Defence
	}
	return 0
}

func (m *UnitStatus) GetLevel() int32 {
	if m != nil && m.Level != nil {
		return *m.Level
	}
	return 0
}

func (m *UnitStatus) GetNextLvExp() int32 {
	if m != nil && m.NextLvExp != nil {
		return *m.NextLvExp
	}
	return 0
}

func (m *UnitStatus) GetCost() int32 {
	if m != nil && m.Cost != nil {
		return *m.Cost
	}
	return 0
}

type UnitInfo struct {
	Id               *int32                 `protobuf:"varint,1,req,name=id" json:"id,omitempty"`
	Name             *string                `protobuf:"bytes,2,opt,name=name" json:"name,omitempty"`
	Skill1           *bbproto2.NormalSkill  `protobuf:"bytes,3,opt,name=skill1" json:"skill1,omitempty"`
	Skill2           *bbproto2.NormalSkill  `protobuf:"bytes,4,opt,name=skill2" json:"skill2,omitempty"`
	LeaderSkill      *bbproto2.LeaderSkill  `protobuf:"bytes,5,opt,name=leaderSkill" json:"leaderSkill,omitempty"`
	ActiveSkill      *bbproto2.ActiveSkill  `protobuf:"bytes,6,opt,name=activeSkill" json:"activeSkill,omitempty"`
	PassiveSkill     *bbproto2.PassiveSkill `protobuf:"bytes,7,opt,name=passiveSkill" json:"passiveSkill,omitempty"`
	MaxLevel         *int32                 `protobuf:"varint,8,opt,name=maxLevel" json:"maxLevel,omitempty"`
	ExpType          *int32                 `protobuf:"varint,9,opt,name=expType" json:"expType,omitempty"`
	Profile          *string                `protobuf:"bytes,10,opt,name=profile" json:"profile,omitempty"`
	XXX_unrecognized []byte                 `json:"-"`
}

func (m *UnitInfo) Reset()         { *m = UnitInfo{} }
func (m *UnitInfo) String() string { return proto.CompactTextString(m) }
func (*UnitInfo) ProtoMessage()    {}

func (m *UnitInfo) GetId() int32 {
	if m != nil && m.Id != nil {
		return *m.Id
	}
	return 0
}

func (m *UnitInfo) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

func (m *UnitInfo) GetSkill1() *bbproto2.NormalSkill {
	if m != nil {
		return m.Skill1
	}
	return nil
}

func (m *UnitInfo) GetSkill2() *bbproto2.NormalSkill {
	if m != nil {
		return m.Skill2
	}
	return nil
}

func (m *UnitInfo) GetLeaderSkill() *bbproto2.LeaderSkill {
	if m != nil {
		return m.LeaderSkill
	}
	return nil
}

func (m *UnitInfo) GetActiveSkill() *bbproto2.ActiveSkill {
	if m != nil {
		return m.ActiveSkill
	}
	return nil
}

func (m *UnitInfo) GetPassiveSkill() *bbproto2.PassiveSkill {
	if m != nil {
		return m.PassiveSkill
	}
	return nil
}

func (m *UnitInfo) GetMaxLevel() int32 {
	if m != nil && m.MaxLevel != nil {
		return *m.MaxLevel
	}
	return 0
}

func (m *UnitInfo) GetExpType() int32 {
	if m != nil && m.ExpType != nil {
		return *m.ExpType
	}
	return 0
}

func (m *UnitInfo) GetProfile() string {
	if m != nil && m.Profile != nil {
		return *m.Profile
	}
	return ""
}

type Unit struct {
	UniqueId         *int32 `protobuf:"varint,1,req,name=uniqueId" json:"uniqueId,omitempty"`
	Id               *int32 `protobuf:"varint,2,opt,name=id" json:"id,omitempty"`
	Exp              *int32 `protobuf:"varint,3,opt,name=exp" json:"exp,omitempty"`
	Level            *int32 `protobuf:"varint,4,opt,name=level" json:"level,omitempty"`
	Attack           *int32 `protobuf:"varint,5,opt,name=attack" json:"attack,omitempty"`
	Defense          *int32 `protobuf:"varint,6,opt,name=defense" json:"defense,omitempty"`
	Hp               *int32 `protobuf:"varint,7,opt,name=hp" json:"hp,omitempty"`
	AddAttack        *int32 `protobuf:"varint,8,opt,name=addAttack" json:"addAttack,omitempty"`
	AddDefence       *int32 `protobuf:"varint,9,opt,name=addDefence" json:"addDefence,omitempty"`
	AddHp            *int32 `protobuf:"varint,10,opt,name=addHp" json:"addHp,omitempty"`
	LimitbreakLv     *int32 `protobuf:"varint,11,opt,name=limitbreakLv" json:"limitbreakLv,omitempty"`
	GetTime          *int32 `protobuf:"varint,12,opt,name=getTime" json:"getTime,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *Unit) Reset()         { *m = Unit{} }
func (m *Unit) String() string { return proto.CompactTextString(m) }
func (*Unit) ProtoMessage()    {}

func (m *Unit) GetUniqueId() int32 {
	if m != nil && m.UniqueId != nil {
		return *m.UniqueId
	}
	return 0
}

func (m *Unit) GetId() int32 {
	if m != nil && m.Id != nil {
		return *m.Id
	}
	return 0
}

func (m *Unit) GetExp() int32 {
	if m != nil && m.Exp != nil {
		return *m.Exp
	}
	return 0
}

func (m *Unit) GetLevel() int32 {
	if m != nil && m.Level != nil {
		return *m.Level
	}
	return 0
}

func (m *Unit) GetAttack() int32 {
	if m != nil && m.Attack != nil {
		return *m.Attack
	}
	return 0
}

func (m *Unit) GetDefense() int32 {
	if m != nil && m.Defense != nil {
		return *m.Defense
	}
	return 0
}

func (m *Unit) GetHp() int32 {
	if m != nil && m.Hp != nil {
		return *m.Hp
	}
	return 0
}

func (m *Unit) GetAddAttack() int32 {
	if m != nil && m.AddAttack != nil {
		return *m.AddAttack
	}
	return 0
}

func (m *Unit) GetAddDefence() int32 {
	if m != nil && m.AddDefence != nil {
		return *m.AddDefence
	}
	return 0
}

func (m *Unit) GetAddHp() int32 {
	if m != nil && m.AddHp != nil {
		return *m.AddHp
	}
	return 0
}

func (m *Unit) GetLimitbreakLv() int32 {
	if m != nil && m.LimitbreakLv != nil {
		return *m.LimitbreakLv
	}
	return 0
}

func (m *Unit) GetGetTime() int32 {
	if m != nil && m.GetTime != nil {
		return *m.GetTime
	}
	return 0
}

type ReqGetUnitInfo struct {
	Header           *bbproto1.Request `protobuf:"bytes,1,opt,name=header" json:"header,omitempty"`
	UnitIds          []int32           `protobuf:"varint,2,rep,name=unitIds" json:"unitIds,omitempty"`
	XXX_unrecognized []byte            `json:"-"`
}

func (m *ReqGetUnitInfo) Reset()         { *m = ReqGetUnitInfo{} }
func (m *ReqGetUnitInfo) String() string { return proto.CompactTextString(m) }
func (*ReqGetUnitInfo) ProtoMessage()    {}

func (m *ReqGetUnitInfo) GetHeader() *bbproto1.Request {
	if m != nil {
		return m.Header
	}
	return nil
}

func (m *ReqGetUnitInfo) GetUnitIds() []int32 {
	if m != nil {
		return m.UnitIds
	}
	return nil
}

type RspGetUnitInfo struct {
	Header           *bbproto1.Response `protobuf:"bytes,1,opt,name=header" json:"header,omitempty"`
	Unitinfos        []*UnitInfo        `protobuf:"bytes,2,rep,name=unitinfos" json:"unitinfos,omitempty"`
	XXX_unrecognized []byte             `json:"-"`
}

func (m *RspGetUnitInfo) Reset()         { *m = RspGetUnitInfo{} }
func (m *RspGetUnitInfo) String() string { return proto.CompactTextString(m) }
func (*RspGetUnitInfo) ProtoMessage()    {}

func (m *RspGetUnitInfo) GetHeader() *bbproto1.Response {
	if m != nil {
		return m.Header
	}
	return nil
}

func (m *RspGetUnitInfo) GetUnitinfos() []*UnitInfo {
	if m != nil {
		return m.Unitinfos
	}
	return nil
}

type ReqGetUnit struct {
	Header           *bbproto1.Request `protobuf:"bytes,1,opt,name=header" json:"header,omitempty"`
	UnitIds          []int32           `protobuf:"varint,2,rep,name=unitIds" json:"unitIds,omitempty"`
	XXX_unrecognized []byte            `json:"-"`
}

func (m *ReqGetUnit) Reset()         { *m = ReqGetUnit{} }
func (m *ReqGetUnit) String() string { return proto.CompactTextString(m) }
func (*ReqGetUnit) ProtoMessage()    {}

func (m *ReqGetUnit) GetHeader() *bbproto1.Request {
	if m != nil {
		return m.Header
	}
	return nil
}

func (m *ReqGetUnit) GetUnitIds() []int32 {
	if m != nil {
		return m.UnitIds
	}
	return nil
}

type RspGetUnit struct {
	Header           *bbproto1.Response `protobuf:"bytes,1,opt,name=header" json:"header,omitempty"`
	Units            []*Unit            `protobuf:"bytes,2,rep,name=units" json:"units,omitempty"`
	XXX_unrecognized []byte             `json:"-"`
}

func (m *RspGetUnit) Reset()         { *m = RspGetUnit{} }
func (m *RspGetUnit) String() string { return proto.CompactTextString(m) }
func (*RspGetUnit) ProtoMessage()    {}

func (m *RspGetUnit) GetHeader() *bbproto1.Response {
	if m != nil {
		return m.Header
	}
	return nil
}

func (m *RspGetUnit) GetUnits() []*Unit {
	if m != nil {
		return m.Units
	}
	return nil
}

func init() {
}
