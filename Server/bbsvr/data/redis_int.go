package data

import (
	//	"fmt"
	//"log"
	"time"
)
import redis "github.com/garyburd/redigo/redis"

const (
	//SERVERADDR = "211.155.86.174:6789"
	SERVERADDR = "127.0.0.1:6397"
)

type Data struct {
	redis.Conn
}

func (t *Data) Open(db *string) error {
	c, err := redis.DialTimeout("tcp", SERVERADDR, 0, 1*time.Second, 1*time.Second)
	if err != nil {
		return err
	}
	t = &Data{c}
	_, err = t.Conn.Do("SELECT", db)
	return err
}

func (t *Data) Close() error {
	_, err := t.Conn.Do("FLUSHDB")
	if err != nil {
		return err
	}
	return t.Conn.Close()
}

func (t *Data) Get(key *string) (value *string, err error) {
	*value, err = redis.String(t.Do("GET", key))
	return value, err
}

func (t *Data) Set(key *string, value *string) error {
	_, err := redis.String(t.Do("SET", key, value))
	return err
}
