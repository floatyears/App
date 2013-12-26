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

func (t *Data) Open(db string) error {
	var err error
	t.conn, err = redis.DialTimeout("tcp", SERVERADDR, 3*time.Second, 10*time.Second, 10*time.Second) //timeout=10s

	if err != nil {
		log.Printf("ERR: redis Open(db:%v) ret err:%v t.conn:%v", db, err, t.conn)
		return err
	}
	_, err = t.conn.Do("SELECT", db)
	log.Printf("redis.Select(%v) ret err:%v", db, err)
	return err
}

func (t *Data) Close() error {
	//TODO: when need to FLUSHDB???

	//_, err := t.conn.Do("FLUSHDB")
	//if err != nil {
	//	return err
	//}

	if t.conn != nil {
		log.Printf("t.conn.Close()...")
		return t.conn.Close()
	} else {
		log.Printf("FATAL ERR: t.conn=nil")
	}
	return nil
}

//return string
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
