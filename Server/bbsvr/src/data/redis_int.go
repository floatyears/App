package data

import (
	"log"
	"time"
)
import redis "github.com/garyburd/redigo/redis"

const (
	//SERVERADDR = "211.155.86.174:6789"
	SERVERADDR = "127.0.0.1:6379"
)

func TestRedis() error {
	db := new(redis.Conn)
	log.Printf("TestRedis db: %v", db)

	return nil
}

type Data struct {
	conn redis.Conn
}

func (t *Data) Open(table string) error {
	var err error
	t.conn, err = redis.DialTimeout("tcp", SERVERADDR, 3*time.Second, 10*time.Second, 10*time.Second) //timeout=10s

	if err != nil {
		log.Printf("ERR: redis Open(table:%v) ret err:%v t.conn:%v", table, err, t.conn)
		return err
	}
	_, err = t.conn.Do("SELECT", table)
	log.Printf("redis.Select(%v) ret err:%v", table, err)
	return err
}

func (t *Data) Close() (err error) {
	//TODO: when need to FLUSHDB???

	//_, err := t.conn.Do("FLUSHDB")
	//if err != nil {
	//	return err
	//}

	if t.conn != nil {
		log.Printf("t.conn.Close().")
		err = t.conn.Close()
		t.conn = nil
		return err
	} else {
		log.Printf("FATAL ERR: t.conn=nil")
	}
	return err
}

func (t *Data) Select(table string) (err error) {
	_, err = t.conn.Do("SELECT", table)
	log.Printf("redis.Select(%v) ret err:%v", table, err)
	return err
}

//================= String ==================
func (t *Data) Get(key string) (value string, err error) {
	if t.conn != nil {
		value, err := redis.String(t.conn.Do("GET", key))
		//log.Printf("redis.GET(%v) ret err:%v value:%v", key, err, value)
		return value, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return "", err
}

//
func (t *Data) MGet(args []interface{}) (values []interface{}, err error) {
	if t.conn != nil {
		values, err := redis.Values(t.conn.Do("MGET", args...))
		//log.Printf("redis.GET(%v) ret err:%v value:%v", args, err, values)
		return values, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return values, err
}

//return []byte
func (t *Data) Gets(key string) (value []byte, err error) {
	if t.conn != nil {
		value, err := redis.Bytes(t.conn.Do("GET", key))
		return value, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return nil, err
}

func (t *Data) GetInt(key string) (value int, err error) {
	if t.conn != nil {
		value, err := redis.Int(t.conn.Do("GET", key))
		return value, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return 0, err
}

func (t *Data) Set(key string, value []byte) error {
	if t.conn != nil {
		log.Printf("try redis.Set(%v) value:%v", key, value)
		_, err := redis.String(t.conn.Do("SET", key, value))
		log.Printf("after redis.Set(%v) ret err:%v", key, err)
		return err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}
	return nil
}

func (t *Data) SetInt(key string, value int32) error {
	if t.conn != nil {
		_, err := redis.String(t.conn.Do("SET", key, value))
		return err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}
	return nil
}

func (t *Data) SetUInt(key string, value uint32) error {
	if t.conn != nil {
		_, err := redis.String(t.conn.Do("SET", key, value))
		return err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}
	return nil
}

//================= List ==================
func (t *Data) GetList(key string) (values []interface{}, err error) {

	values, err = redis.Values(t.conn.Do("LRANGE", key, 0, -1))
	return values, err
}

func (t *Data) AddToList(key string, value []byte) (err error) {
	_, err = redis.Values(t.conn.Do("LPUSH", key, value))

	return err
}

func (t *Data) Remove(key string, removeValue []byte) (err error) {

	_, err = redis.Values(t.conn.Do("LREM", 0, removeValue))
	return err
}

//================= HASH ==================
func (t *Data) HGetAll(key string) (values []interface{}, err error) {

	values, err = redis.Values(t.conn.Do("HGETALL", key))
	return values, err
}

func (t *Data) HGet(key string, field string) (value []byte, err error) {
	value, err = redis.Bytes(t.conn.Do("HGET", key, field))
	return
}

func (t *Data) HSet(key string, field string, value []byte) (err error) {
	_, err = t.conn.Do("HSET", key, field, value)
	return
}

func (t *Data) HMSet(key string, fields ...[]byte) (err error) {
	_, err = t.conn.Do("HMSET", key, fields)
	return err
}

func (t *Data) HDel(key string, field string) (err error) {
	_, err = t.conn.Do("HDEL", key, field)
	return err
}

//================= ZSET ==================
func (t *Data) ZAdd(key string, member string, score int32) (err error) {
	_, err = t.conn.Do("ZADD", key, member, score)
	return err
}

func (t *Data) ZRem(key string, member string) (err error) {
	_, err = t.conn.Do("ZREM", key, member)
	return err
}

func (t *Data) ZRangeByScore(key string, min int, max int, offset int, count int) (values []interface{}, err error) {
	log.Printf("ZRangeByScore key:%v %v %v limit %v %v", key, min, max, offset, count)
	values, err = redis.Values(t.conn.Do("ZRANGEBYSCORE", key, min, max, "limit", offset, count))
	return values, err
}
