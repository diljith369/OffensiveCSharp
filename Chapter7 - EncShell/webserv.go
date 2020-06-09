package main

import (
	"fmt"
	"net/http"
	"time"

	"github.com/gorilla/mux"
)

func welcome(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "welcome !!")
}

func main() {
	router := mux.NewRouter()
	router.HandleFunc("/", welcome)
	router.PathPrefix("/OutFiles/").Handler(http.StripPrefix("/OutFiles/", http.FileServer(http.Dir("OutFiles/"))))

	srv := &http.Server{
		Handler: router,
		Addr:    "0.0.0.0:8080",
		// Good practice: enforce timeouts for servers you create!
		WriteTimeout: 180 * time.Second,
		ReadTimeout:  180 * time.Second,
	}
	srv.ListenAndServe()

}
