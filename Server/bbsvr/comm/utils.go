package utils

import (
	"fmt"
	"os"
)

func WriteData(data []byte, outfile string) error {
	file, err := os.Create(outfile)
	if err != nil {
		fmt.Println("Failed to create the output file: ", outfile)
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
