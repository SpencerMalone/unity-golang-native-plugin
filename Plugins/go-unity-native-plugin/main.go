package main

import "C"

import (
	"fmt"
	"sync"
)

var count int
var mtx sync.Mutex

//export Add
func Add(a int, b int) int {
	return a + b
}

//export Squares
func Squares(vals []int) *C.char {
	resultChan := make(chan string)

	for _, val := range vals {
		go func (myval int) {
			res := myval * myval
	        resultChan <- fmt.Sprintf("%d squared is %d\n", myval, res)
	    } (val);
	}

	tarLen := len(vals)

	results := ""
	for i := 0; i < tarLen; i++ {
		resultsDat := <- resultChan
		results = results + resultsDat
	}

	cres := C.CString(results)
	// defer C.free(unsafe.Pointer(cres))
	return cres
}

//export SlowSquares
func SlowSquares(vals []int) *C.char {
	results := ""
	for _, val := range vals {
		res := val * val
        results = results + fmt.Sprintf("%d squared is %d\n", val, res)
	}

	cres := C.CString(results)
	// defer C.free(unsafe.Pointer(cres))
	return cres
}

//export Log
func Log(msg string) int {
	mtx.Lock()
	defer mtx.Unlock()
	fmt.Println(msg)
	count++
	return count
}

func main() {
	fmt.Println(Squares([]int{1, 3}))
	fmt.Println(SlowSquares([]int{1, 3}))
}