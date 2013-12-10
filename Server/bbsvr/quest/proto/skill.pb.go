// Code generated by protoc-gen-go.
// source: skill.proto
// DO NOT EDIT!

package bbproto

import proto "code.google.com/p/goprotobuf/proto"
import json "encoding/json"
import math "math"
import bbproto1 "base.pb"

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

type EValueType int32

const (
	EValueType_FIXED       EValueType = 1
	EValueType_MULTIPLE    EValueType = 2
	EValueType_PERCENT     EValueType = 3
	EValueType_SECOND      EValueType = 4
	EValueType_ROUND       EValueType = 5
	EValueType_COLORTYPE   EValueType = 6
	EValueType_RANDOMCOLOR EValueType = 7
)

var EValueType_name = map[int32]string{
	1: "FIXED",
	2: "MULTIPLE",
	3: "PERCENT",
	4: "SECOND",
	5: "ROUND",
	6: "COLORTYPE",
	7: "RANDOMCOLOR",
}
var EValueType_value = map[string]int32{
	"FIXED":       1,
	"MULTIPLE":    2,
	"PERCENT":     3,
	"SECOND":      4,
	"ROUND":       5,
	"COLORTYPE":   6,
	"RANDOMCOLOR": 7,
}

func (x EValueType) Enum() *EValueType {
	p := new(EValueType)
	*p = x
	return p
}
func (x EValueType) String() string {
	return proto.EnumName(EValueType_name, int32(x))
}
func (x *EValueType) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EValueType_value, data, "EValueType")
	if err != nil {
		return err
	}
	*x = EValueType(value)
	return nil
}

type EAttackType int32

const (
	EAttackType_ATK_SINGLE EAttackType = 0
	EAttackType_ATK_ALL    EAttackType = 1
	EAttackType_RECOVER_HP EAttackType = 2
)

var EAttackType_name = map[int32]string{
	0: "ATK_SINGLE",
	1: "ATK_ALL",
	2: "RECOVER_HP",
}
var EAttackType_value = map[string]int32{
	"ATK_SINGLE": 0,
	"ATK_ALL":    1,
	"RECOVER_HP": 2,
}

func (x EAttackType) Enum() *EAttackType {
	p := new(EAttackType)
	*p = x
	return p
}
func (x EAttackType) String() string {
	return proto.EnumName(EAttackType_name, int32(x))
}
func (x *EAttackType) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EAttackType_value, data, "EAttackType")
	if err != nil {
		return err
	}
	*x = EAttackType(value)
	return nil
}

type EBoostType int32

const (
	EBoostType_BOOST_ATTACK EBoostType = 0
	EBoostType_BOOST_HP     EBoostType = 1
)

var EBoostType_name = map[int32]string{
	0: "BOOST_ATTACK",
	1: "BOOST_HP",
}
var EBoostType_value = map[string]int32{
	"BOOST_ATTACK": 0,
	"BOOST_HP":     1,
}

func (x EBoostType) Enum() *EBoostType {
	p := new(EBoostType)
	*p = x
	return p
}
func (x EBoostType) String() string {
	return proto.EnumName(EBoostType_name, int32(x))
}
func (x *EBoostType) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EBoostType_value, data, "EBoostType")
	if err != nil {
		return err
	}
	*x = EBoostType(value)
	return nil
}

type EBoostTarget int32

const (
	EBoostTarget_UNIT_RACE EBoostTarget = 0
	EBoostTarget_UNIT_TYPE EBoostTarget = 1
)

var EBoostTarget_name = map[int32]string{
	0: "UNIT_RACE",
	1: "UNIT_TYPE",
}
var EBoostTarget_value = map[string]int32{
	"UNIT_RACE": 0,
	"UNIT_TYPE": 1,
}

func (x EBoostTarget) Enum() *EBoostTarget {
	p := new(EBoostTarget)
	*p = x
	return p
}
func (x EBoostTarget) String() string {
	return proto.EnumName(EBoostTarget_name, int32(x))
}
func (x *EBoostTarget) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EBoostTarget_value, data, "EBoostTarget")
	if err != nil {
		return err
	}
	*x = EBoostTarget(value)
	return nil
}

type EPeriod int32

const (
	EPeriod_EP_RIGHT_NOW   EPeriod = 0
	EPeriod_EP_EVERY_ROUND EPeriod = 1
	EPeriod_EP_EVERY_STEP  EPeriod = 2
)

var EPeriod_name = map[int32]string{
	0: "EP_RIGHT_NOW",
	1: "EP_EVERY_ROUND",
	2: "EP_EVERY_STEP",
}
var EPeriod_value = map[string]int32{
	"EP_RIGHT_NOW":   0,
	"EP_EVERY_ROUND": 1,
	"EP_EVERY_STEP":  2,
}

func (x EPeriod) Enum() *EPeriod {
	p := new(EPeriod)
	*p = x
	return p
}
func (x EPeriod) String() string {
	return proto.EnumName(EPeriod_name, int32(x))
}
func (x *EPeriod) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EPeriod_value, data, "EPeriod")
	if err != nil {
		return err
	}
	*x = EPeriod(value)
	return nil
}

type SkillSingleAttack struct {
	Type             *EValueType         `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32            `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	UnitType         *bbproto1.EUnitType `protobuf:"varint,3,opt,name=unitType,enum=bbproto.EUnitType" json:"unitType,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillSingleAttack) Reset()         { *m = SkillSingleAttack{} }
func (m *SkillSingleAttack) String() string { return proto.CompactTextString(m) }
func (*SkillSingleAttack) ProtoMessage()    {}

func (m *SkillSingleAttack) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillSingleAttack) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillSingleAttack) GetUnitType() bbproto1.EUnitType {
	if m != nil && m.UnitType != nil {
		return *m.UnitType
	}
	return bbproto1.EUnitType_UALL
}

type SkillAllAttack struct {
	Type             *EValueType         `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32            `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	UnitType         *bbproto1.EUnitType `protobuf:"varint,3,opt,name=unitType,enum=bbproto.EUnitType" json:"unitType,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillAllAttack) Reset()         { *m = SkillAllAttack{} }
func (m *SkillAllAttack) String() string { return proto.CompactTextString(m) }
func (*SkillAllAttack) ProtoMessage()    {}

func (m *SkillAllAttack) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillAllAttack) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillAllAttack) GetUnitType() bbproto1.EUnitType {
	if m != nil && m.UnitType != nil {
		return *m.UnitType
	}
	return bbproto1.EUnitType_UALL
}

type SkillSingleAtkRecoverHP struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillSingleAtkRecoverHP) Reset()         { *m = SkillSingleAtkRecoverHP{} }
func (m *SkillSingleAtkRecoverHP) String() string { return proto.CompactTextString(m) }
func (*SkillSingleAtkRecoverHP) ProtoMessage()    {}

func (m *SkillSingleAtkRecoverHP) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillSingleAtkRecoverHP) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillSuicideAttack struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillSuicideAttack) Reset()         { *m = SkillSuicideAttack{} }
func (m *SkillSuicideAttack) String() string { return proto.CompactTextString(m) }
func (*SkillSuicideAttack) ProtoMessage()    {}

func (m *SkillSuicideAttack) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillSuicideAttack) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillTargetTypeAttack struct {
	Type             *EValueType         `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32            `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	Period           *int32              `protobuf:"varint,3,opt,name=period" json:"period,omitempty"`
	UnitType         *bbproto1.EUnitType `protobuf:"varint,4,opt,name=unitType,enum=bbproto.EUnitType" json:"unitType,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillTargetTypeAttack) Reset()         { *m = SkillTargetTypeAttack{} }
func (m *SkillTargetTypeAttack) String() string { return proto.CompactTextString(m) }
func (*SkillTargetTypeAttack) ProtoMessage()    {}

func (m *SkillTargetTypeAttack) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillTargetTypeAttack) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillTargetTypeAttack) GetPeriod() int32 {
	if m != nil && m.Period != nil {
		return *m.Period
	}
	return 0
}

func (m *SkillTargetTypeAttack) GetUnitType() bbproto1.EUnitType {
	if m != nil && m.UnitType != nil {
		return *m.UnitType
	}
	return bbproto1.EUnitType_UALL
}

type SkillKillHP struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillKillHP) Reset()         { *m = SkillKillHP{} }
func (m *SkillKillHP) String() string { return proto.CompactTextString(m) }
func (*SkillKillHP) ProtoMessage()    {}

func (m *SkillKillHP) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillKillHP) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillRecoverHP struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	Period           *EPeriod    `protobuf:"varint,3,opt,name=period,enum=bbproto.EPeriod" json:"period,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillRecoverHP) Reset()         { *m = SkillRecoverHP{} }
func (m *SkillRecoverHP) String() string { return proto.CompactTextString(m) }
func (*SkillRecoverHP) ProtoMessage()    {}

func (m *SkillRecoverHP) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillRecoverHP) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillRecoverHP) GetPeriod() EPeriod {
	if m != nil && m.Period != nil {
		return *m.Period
	}
	return EPeriod_EP_RIGHT_NOW
}

type SkillRecoverSP struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillRecoverSP) Reset()         { *m = SkillRecoverSP{} }
func (m *SkillRecoverSP) String() string { return proto.CompactTextString(m) }
func (*SkillRecoverSP) ProtoMessage()    {}

func (m *SkillRecoverSP) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillRecoverSP) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillReduceHurt struct {
	Type             *EValueType         `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32            `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	Period           *int32              `protobuf:"varint,3,opt,name=period" json:"period,omitempty"`
	UnitType         *bbproto1.EUnitType `protobuf:"varint,4,opt,name=unitType,enum=bbproto.EUnitType" json:"unitType,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillReduceHurt) Reset()         { *m = SkillReduceHurt{} }
func (m *SkillReduceHurt) String() string { return proto.CompactTextString(m) }
func (*SkillReduceHurt) ProtoMessage()    {}

func (m *SkillReduceHurt) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillReduceHurt) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillReduceHurt) GetPeriod() int32 {
	if m != nil && m.Period != nil {
		return *m.Period
	}
	return 0
}

func (m *SkillReduceHurt) GetUnitType() bbproto1.EUnitType {
	if m != nil && m.UnitType != nil {
		return *m.UnitType
	}
	return bbproto1.EUnitType_UALL
}

type SkillReduceDefence struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	Period           *int32      `protobuf:"varint,3,opt,name=period" json:"period,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillReduceDefence) Reset()         { *m = SkillReduceDefence{} }
func (m *SkillReduceDefence) String() string { return proto.CompactTextString(m) }
func (*SkillReduceDefence) ProtoMessage()    {}

func (m *SkillReduceDefence) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillReduceDefence) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

func (m *SkillReduceDefence) GetPeriod() int32 {
	if m != nil && m.Period != nil {
		return *m.Period
	}
	return 0
}

type SkillDeferAttackRound struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillDeferAttackRound) Reset()         { *m = SkillDeferAttackRound{} }
func (m *SkillDeferAttackRound) String() string { return proto.CompactTextString(m) }
func (*SkillDeferAttackRound) ProtoMessage()    {}

func (m *SkillDeferAttackRound) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillDeferAttackRound) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillPoison struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillPoison) Reset()         { *m = SkillPoison{} }
func (m *SkillPoison) String() string { return proto.CompactTextString(m) }
func (*SkillPoison) ProtoMessage()    {}

func (m *SkillPoison) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillPoison) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillDelayTime struct {
	Type             *EValueType `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	Value            *float32    `protobuf:"fixed32,2,opt,name=value" json:"value,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *SkillDelayTime) Reset()         { *m = SkillDelayTime{} }
func (m *SkillDelayTime) String() string { return proto.CompactTextString(m) }
func (*SkillDelayTime) ProtoMessage()    {}

func (m *SkillDelayTime) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillDelayTime) GetValue() float32 {
	if m != nil && m.Value != nil {
		return *m.Value
	}
	return 0
}

type SkillConvertUnitType struct {
	Type             *EValueType         `protobuf:"varint,1,opt,name=type,enum=bbproto.EValueType" json:"type,omitempty"`
	UnitType1        *bbproto1.EUnitType `protobuf:"varint,2,opt,name=unitType1,enum=bbproto.EUnitType" json:"unitType1,omitempty"`
	UnitType2        *bbproto1.EUnitType `protobuf:"varint,3,opt,name=unitType2,enum=bbproto.EUnitType" json:"unitType2,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillConvertUnitType) Reset()         { *m = SkillConvertUnitType{} }
func (m *SkillConvertUnitType) String() string { return proto.CompactTextString(m) }
func (*SkillConvertUnitType) ProtoMessage()    {}

func (m *SkillConvertUnitType) GetType() EValueType {
	if m != nil && m.Type != nil {
		return *m.Type
	}
	return EValueType_FIXED
}

func (m *SkillConvertUnitType) GetUnitType1() bbproto1.EUnitType {
	if m != nil && m.UnitType1 != nil {
		return *m.UnitType1
	}
	return bbproto1.EUnitType_UALL
}

func (m *SkillConvertUnitType) GetUnitType2() bbproto1.EUnitType {
	if m != nil && m.UnitType2 != nil {
		return *m.UnitType2
	}
	return bbproto1.EUnitType_UALL
}

type ActiveSkill struct {
	No   *int32  `protobuf:"varint,1,opt,name=no" json:"no,omitempty"`
	Name *string `protobuf:"bytes,2,opt,name=name" json:"name,omitempty"`
	Desc *string `protobuf:"bytes,3,opt,name=desc" json:"desc,omitempty"`
	// total 14 active skills
	SingleAttack       *SkillSingleAttack       `protobuf:"bytes,4,opt,name=singleAttack" json:"singleAttack,omitempty"`
	AllAttack          *SkillAllAttack          `protobuf:"bytes,5,opt,name=allAttack" json:"allAttack,omitempty"`
	SingleAtkRecoverHP *SkillSingleAtkRecoverHP `protobuf:"bytes,6,opt,name=singleAtkRecoverHP" json:"singleAtkRecoverHP,omitempty"`
	SuicideAttack      *SkillSuicideAttack      `protobuf:"bytes,7,opt,name=suicideAttack" json:"suicideAttack,omitempty"`
	TargetTypeAttack   *SkillTargetTypeAttack   `protobuf:"bytes,8,opt,name=targetTypeAttack" json:"targetTypeAttack,omitempty"`
	KillHP             *SkillKillHP             `protobuf:"bytes,9,opt,name=killHP" json:"killHP,omitempty"`
	RecoverHP          *SkillRecoverHP          `protobuf:"bytes,10,opt,name=recoverHP" json:"recoverHP,omitempty"`
	RecoverSP          *SkillRecoverSP          `protobuf:"bytes,11,opt,name=recoverSP" json:"recoverSP,omitempty"`
	ReduceHurt         *SkillReduceHurt         `protobuf:"bytes,12,opt,name=reduceHurt" json:"reduceHurt,omitempty"`
	ReduceDefence      *SkillReduceDefence      `protobuf:"bytes,13,opt,name=reduceDefence" json:"reduceDefence,omitempty"`
	DeferAttackRound   *SkillDeferAttackRound   `protobuf:"bytes,14,opt,name=deferAttackRound" json:"deferAttackRound,omitempty"`
	Poison             *SkillPoison             `protobuf:"bytes,15,opt,name=poison" json:"poison,omitempty"`
	DelayTime          *SkillDelayTime          `protobuf:"bytes,16,opt,name=delayTime" json:"delayTime,omitempty"`
	ConvertUnitType    *SkillConvertUnitType    `protobuf:"bytes,17,opt,name=convertUnitType" json:"convertUnitType,omitempty"`
	XXX_unrecognized   []byte                   `json:"-"`
}

func (m *ActiveSkill) Reset()         { *m = ActiveSkill{} }
func (m *ActiveSkill) String() string { return proto.CompactTextString(m) }
func (*ActiveSkill) ProtoMessage()    {}

func (m *ActiveSkill) GetNo() int32 {
	if m != nil && m.No != nil {
		return *m.No
	}
	return 0
}

func (m *ActiveSkill) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

func (m *ActiveSkill) GetDesc() string {
	if m != nil && m.Desc != nil {
		return *m.Desc
	}
	return ""
}

func (m *ActiveSkill) GetSingleAttack() *SkillSingleAttack {
	if m != nil {
		return m.SingleAttack
	}
	return nil
}

func (m *ActiveSkill) GetAllAttack() *SkillAllAttack {
	if m != nil {
		return m.AllAttack
	}
	return nil
}

func (m *ActiveSkill) GetSingleAtkRecoverHP() *SkillSingleAtkRecoverHP {
	if m != nil {
		return m.SingleAtkRecoverHP
	}
	return nil
}

func (m *ActiveSkill) GetSuicideAttack() *SkillSuicideAttack {
	if m != nil {
		return m.SuicideAttack
	}
	return nil
}

func (m *ActiveSkill) GetTargetTypeAttack() *SkillTargetTypeAttack {
	if m != nil {
		return m.TargetTypeAttack
	}
	return nil
}

func (m *ActiveSkill) GetKillHP() *SkillKillHP {
	if m != nil {
		return m.KillHP
	}
	return nil
}

func (m *ActiveSkill) GetRecoverHP() *SkillRecoverHP {
	if m != nil {
		return m.RecoverHP
	}
	return nil
}

func (m *ActiveSkill) GetRecoverSP() *SkillRecoverSP {
	if m != nil {
		return m.RecoverSP
	}
	return nil
}

func (m *ActiveSkill) GetReduceHurt() *SkillReduceHurt {
	if m != nil {
		return m.ReduceHurt
	}
	return nil
}

func (m *ActiveSkill) GetReduceDefence() *SkillReduceDefence {
	if m != nil {
		return m.ReduceDefence
	}
	return nil
}

func (m *ActiveSkill) GetDeferAttackRound() *SkillDeferAttackRound {
	if m != nil {
		return m.DeferAttackRound
	}
	return nil
}

func (m *ActiveSkill) GetPoison() *SkillPoison {
	if m != nil {
		return m.Poison
	}
	return nil
}

func (m *ActiveSkill) GetDelayTime() *SkillDelayTime {
	if m != nil {
		return m.DelayTime
	}
	return nil
}

func (m *ActiveSkill) GetConvertUnitType() *SkillConvertUnitType {
	if m != nil {
		return m.ConvertUnitType
	}
	return nil
}

// ==================PassiveSkill Skills: 2 kinds=====================================================
//
type SkillDodgeTrap struct {
	TrapLevel        *int32 `protobuf:"varint,2,opt,name=trapLevel" json:"trapLevel,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *SkillDodgeTrap) Reset()         { *m = SkillDodgeTrap{} }
func (m *SkillDodgeTrap) String() string { return proto.CompactTextString(m) }
func (*SkillDodgeTrap) ProtoMessage()    {}

func (m *SkillDodgeTrap) GetTrapLevel() int32 {
	if m != nil && m.TrapLevel != nil {
		return *m.TrapLevel
	}
	return 0
}

type SkillAntiAttack struct {
	Probability      *int32 `protobuf:"varint,1,opt,name=probability" json:"probability,omitempty"`
	AntiAtkRatio     *int32 `protobuf:"varint,2,opt,name=antiAtkRatio" json:"antiAtkRatio,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *SkillAntiAttack) Reset()         { *m = SkillAntiAttack{} }
func (m *SkillAntiAttack) String() string { return proto.CompactTextString(m) }
func (*SkillAntiAttack) ProtoMessage()    {}

func (m *SkillAntiAttack) GetProbability() int32 {
	if m != nil && m.Probability != nil {
		return *m.Probability
	}
	return 0
}

func (m *SkillAntiAttack) GetAntiAtkRatio() int32 {
	if m != nil && m.AntiAtkRatio != nil {
		return *m.AntiAtkRatio
	}
	return 0
}

type PassiveSkill struct {
	No               *int32           `protobuf:"varint,1,opt,name=no" json:"no,omitempty"`
	Name             *string          `protobuf:"bytes,2,opt,name=name" json:"name,omitempty"`
	Desc             *string          `protobuf:"bytes,3,opt,name=desc" json:"desc,omitempty"`
	AntiAttack       *SkillAntiAttack `protobuf:"bytes,4,opt,name=antiAttack" json:"antiAttack,omitempty"`
	DodgeTrap        *SkillDodgeTrap  `protobuf:"bytes,5,opt,name=dodgeTrap" json:"dodgeTrap,omitempty"`
	XXX_unrecognized []byte           `json:"-"`
}

func (m *PassiveSkill) Reset()         { *m = PassiveSkill{} }
func (m *PassiveSkill) String() string { return proto.CompactTextString(m) }
func (*PassiveSkill) ProtoMessage()    {}

func (m *PassiveSkill) GetNo() int32 {
	if m != nil && m.No != nil {
		return *m.No
	}
	return 0
}

func (m *PassiveSkill) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

func (m *PassiveSkill) GetDesc() string {
	if m != nil && m.Desc != nil {
		return *m.Desc
	}
	return ""
}

func (m *PassiveSkill) GetAntiAttack() *SkillAntiAttack {
	if m != nil {
		return m.AntiAttack
	}
	return nil
}

func (m *PassiveSkill) GetDodgeTrap() *SkillDodgeTrap {
	if m != nil {
		return m.DodgeTrap
	}
	return nil
}

type NormalSkill struct {
	No               *int32       `protobuf:"varint,1,opt,name=no" json:"no,omitempty"`
	Name             *string      `protobuf:"bytes,2,opt,name=name" json:"name,omitempty"`
	Desc             *string      `protobuf:"bytes,3,opt,name=desc" json:"desc,omitempty"`
	AttackType       *EAttackType `protobuf:"varint,4,opt,name=attackType,enum=bbproto.EAttackType" json:"attackType,omitempty"`
	ActiveBlocks     []uint32     `protobuf:"varint,5,rep,name=activeBlocks" json:"activeBlocks,omitempty"`
	AttackValue      *float32     `protobuf:"fixed32,6,opt,name=attackValue" json:"attackValue,omitempty"`
	XXX_unrecognized []byte       `json:"-"`
}

func (m *NormalSkill) Reset()         { *m = NormalSkill{} }
func (m *NormalSkill) String() string { return proto.CompactTextString(m) }
func (*NormalSkill) ProtoMessage()    {}

func (m *NormalSkill) GetNo() int32 {
	if m != nil && m.No != nil {
		return *m.No
	}
	return 0
}

func (m *NormalSkill) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

func (m *NormalSkill) GetDesc() string {
	if m != nil && m.Desc != nil {
		return *m.Desc
	}
	return ""
}

func (m *NormalSkill) GetAttackType() EAttackType {
	if m != nil && m.AttackType != nil {
		return *m.AttackType
	}
	return EAttackType_ATK_SINGLE
}

func (m *NormalSkill) GetActiveBlocks() []uint32 {
	if m != nil {
		return m.ActiveBlocks
	}
	return nil
}

func (m *NormalSkill) GetAttackValue() float32 {
	if m != nil && m.AttackValue != nil {
		return *m.AttackValue
	}
	return 0
}

type SkillBoost struct {
	BoostType        *EBoostType   `protobuf:"varint,1,opt,name=boostType,enum=bbproto.EBoostType" json:"boostType,omitempty"`
	BoostValue       *float32      `protobuf:"fixed32,2,opt,name=boostValue" json:"boostValue,omitempty"`
	TargetType       *EBoostTarget `protobuf:"varint,3,opt,name=targetType,enum=bbproto.EBoostTarget" json:"targetType,omitempty"`
	TargetValue      *int32        `protobuf:"varint,4,opt,name=targetValue" json:"targetValue,omitempty"`
	XXX_unrecognized []byte        `json:"-"`
}

func (m *SkillBoost) Reset()         { *m = SkillBoost{} }
func (m *SkillBoost) String() string { return proto.CompactTextString(m) }
func (*SkillBoost) ProtoMessage()    {}

func (m *SkillBoost) GetBoostType() EBoostType {
	if m != nil && m.BoostType != nil {
		return *m.BoostType
	}
	return EBoostType_BOOST_ATTACK
}

func (m *SkillBoost) GetBoostValue() float32 {
	if m != nil && m.BoostValue != nil {
		return *m.BoostValue
	}
	return 0
}

func (m *SkillBoost) GetTargetType() EBoostTarget {
	if m != nil && m.TargetType != nil {
		return *m.TargetType
	}
	return EBoostTarget_UNIT_RACE
}

func (m *SkillBoost) GetTargetValue() int32 {
	if m != nil && m.TargetValue != nil {
		return *m.TargetValue
	}
	return 0
}

type SkillExtraAttack struct {
	UnitType         *bbproto1.EUnitType `protobuf:"varint,1,opt,name=unitType,enum=bbproto.EUnitType" json:"unitType,omitempty"`
	AttackValue      *float32            `protobuf:"fixed32,2,opt,name=attackValue" json:"attackValue,omitempty"`
	XXX_unrecognized []byte              `json:"-"`
}

func (m *SkillExtraAttack) Reset()         { *m = SkillExtraAttack{} }
func (m *SkillExtraAttack) String() string { return proto.CompactTextString(m) }
func (*SkillExtraAttack) ProtoMessage()    {}

func (m *SkillExtraAttack) GetUnitType() bbproto1.EUnitType {
	if m != nil && m.UnitType != nil {
		return *m.UnitType
	}
	return bbproto1.EUnitType_UALL
}

func (m *SkillExtraAttack) GetAttackValue() float32 {
	if m != nil && m.AttackValue != nil {
		return *m.AttackValue
	}
	return 0
}

type LeaderSkill struct {
	No               *int32                `protobuf:"varint,1,opt,name=no" json:"no,omitempty"`
	Name             *string               `protobuf:"bytes,2,opt,name=name" json:"name,omitempty"`
	Desc             *string               `protobuf:"bytes,3,opt,name=desc" json:"desc,omitempty"`
	RaceBoost        *SkillBoost           `protobuf:"bytes,4,opt,name=raceBoost" json:"raceBoost,omitempty"`
	RecoverHP        *SkillRecoverHP       `protobuf:"bytes,5,opt,name=recoverHP" json:"recoverHP,omitempty"`
	ReduceHurt       *SkillReduceHurt      `protobuf:"bytes,6,opt,name=reduceHurt" json:"reduceHurt,omitempty"`
	DelayTime        *SkillDelayTime       `protobuf:"bytes,7,opt,name=delayTime" json:"delayTime,omitempty"`
	ConvertUnitType  *SkillConvertUnitType `protobuf:"bytes,8,opt,name=convertUnitType" json:"convertUnitType,omitempty"`
	ExtraAttack      *SkillExtraAttack     `protobuf:"bytes,9,opt,name=extraAttack" json:"extraAttack,omitempty"`
	XXX_unrecognized []byte                `json:"-"`
}

func (m *LeaderSkill) Reset()         { *m = LeaderSkill{} }
func (m *LeaderSkill) String() string { return proto.CompactTextString(m) }
func (*LeaderSkill) ProtoMessage()    {}

func (m *LeaderSkill) GetNo() int32 {
	if m != nil && m.No != nil {
		return *m.No
	}
	return 0
}

func (m *LeaderSkill) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

func (m *LeaderSkill) GetDesc() string {
	if m != nil && m.Desc != nil {
		return *m.Desc
	}
	return ""
}

func (m *LeaderSkill) GetRaceBoost() *SkillBoost {
	if m != nil {
		return m.RaceBoost
	}
	return nil
}

func (m *LeaderSkill) GetRecoverHP() *SkillRecoverHP {
	if m != nil {
		return m.RecoverHP
	}
	return nil
}

func (m *LeaderSkill) GetReduceHurt() *SkillReduceHurt {
	if m != nil {
		return m.ReduceHurt
	}
	return nil
}

func (m *LeaderSkill) GetDelayTime() *SkillDelayTime {
	if m != nil {
		return m.DelayTime
	}
	return nil
}

func (m *LeaderSkill) GetConvertUnitType() *SkillConvertUnitType {
	if m != nil {
		return m.ConvertUnitType
	}
	return nil
}

func (m *LeaderSkill) GetExtraAttack() *SkillExtraAttack {
	if m != nil {
		return m.ExtraAttack
	}
	return nil
}

func init() {
	proto.RegisterEnum("bbproto.EValueType", EValueType_name, EValueType_value)
	proto.RegisterEnum("bbproto.EAttackType", EAttackType_name, EAttackType_value)
	proto.RegisterEnum("bbproto.EBoostType", EBoostType_name, EBoostType_value)
	proto.RegisterEnum("bbproto.EBoostTarget", EBoostTarget_name, EBoostTarget_value)
	proto.RegisterEnum("bbproto.EPeriod", EPeriod_name, EPeriod_value)
}
