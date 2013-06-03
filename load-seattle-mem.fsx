
#r @"Runtime\bin\Debug\FSharp.Data.Datomic.Runtime.dll"
#load "Config.fs"

open System
open System.IO
open Datomic

let uri = UriBuilder("http:", "mitekm-pc2", port).Uri
let restClient = RestClient(uri, "test")

let dbName = "seattle"
restClient.CreateDatabase dbName
restClient.DatabaseInfo dbName

let schema_tx  = 
    (datomicRootFolder, "samples\seattle\seattle-schema.dtm")
    |> Path.Combine
    |> File.ReadAllText

restClient.Transact(dbName, schema_tx) |> Async.RunSynchronously
