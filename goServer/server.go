package main

import (
  "fmt"
  "log"
  "net/http"
)

func Log(handler http.Handler) http.Handler {
  return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
    log.Printf("%s %s %s", r.RemoteAddr, r.Method, r.URL)
    handler.ServeHTTP(w, r)
  })
}

func handler(w http.ResponseWriter, r *http.Request) {
  fmt.Fprintf(w, "Hello %s!", r.URL.Path)
}

func main() {
  http.HandleFunc("/", handler)
  http.ListenAndServe(":8000", Log(http.DefaultServeMux))
}