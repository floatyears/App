package common

import (
	"fmt"
	"os"
	"strconv"
)

func WriteFile(data []byte, filename string) error {
	file, err := os.Create(filename)
	if err != nil {
		fmt.Println("Failed to create the output file: ", filename)
		return err
	}
	defer file.Close()
	_, err = file.Write(data)

	//for _, value := range values {
	//	str := strconv.Itoa(value)
	//	file.WriteString(str)
	//}
	return err
}

func ReadFile(filename string) (data []byte, err error) {
	file, err := os.Open(filename)
	if err != nil {
		fmt.Println("Failed to open file: ", filename)
		return nil, err
	}
	defer file.Close()
	_, err = file.Read(data)

	return data, err
}

func Itoa(n int32) string {
	return strconv.FormatInt(int64(n), 10)
}

func Utoa(n uint32) string {
	return strconv.FormatUint(uint64(n), 10)
}

func Atou(s string) uint32 {
	u64, _ := strconv.ParseUint(s, 10, 0)
	return uint32(u64)
}

func Atoi(s string) int32 {
	i, _ := strconv.Atoi(s)
	return int32(i)
}
