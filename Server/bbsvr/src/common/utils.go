package common

import (
	"fmt"
	"os"
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
