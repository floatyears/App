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
	t.conn, err = redis.DialTimeout("tcp", SERVERADDR, 3*time.Second, 10*time.Second, 10*time.Second)
	log.Printf("redis Open(db:%v) ret {err:%v t.conn:%v} time.Second=%v", db, err, t.conn, time.Second)
	if err != nil {
		return err
	}
	_, err = t.conn.Do("SELECT", db)
	log.Printf("t.conn.Select(%v) ret err:%v", db, err)
	return err
}

func (t *Data) Close() error {
	//TODO: when need to FLUSHDB???

	//_, err := t.conn.Do("FLUSHDB")
	//if err != nil {
	//	return err
	//}
	return t.conn.Close()
}

func (t *Data) Get(key string) (value string, err error) {
	log.Printf("try redis.GET(%v) ...", key)
	if t.conn != nil {
		value, err := redis.String(t.conn.Do("GET", key))
		log.Printf("redis.GET(%v) ret err:%v value:%v", key, err, value)
		return value, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return "", err
}

func (t *Data) Gets(key string) (value []byte, err error) {
	log.Printf("try redis.GET(%v) ...", key)
	if t.conn != nil {
		value, err := redis.Bytes(t.conn.Do("GET", key))
		log.Printf("redis.GET(%v) ret err:%v value:%v", key, err, value)
		return value, err
	} else {
		log.Fatal("invalid redis conn:%v", t.conn)
	}

	return nil, err
}

func (t *Data) Set(key string, value string) error {
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
