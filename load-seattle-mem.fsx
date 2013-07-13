
#r @"TypeProvider\bin\Debug\FSharp.Data.Datomic.dll"
//#r @"RunTime\bin\Debug\FSharp.Data.Datomic.dll"
#r @"TypeProvider\bin\Debug\FSharp.Data.Datomic.TypeProvider.dll"
#load "Config.fs"

open System
open System.IO
open FSharp.Data
open FSharp.Data.Datomic

let dbName = "seattle"
HttpClient.storages serviceUri
HttpClient.createDatabase serviceUri alias dbName
HttpClient.databaseInfo serviceUri alias dbName "-"

let schema_tx  = Path.Combine(datomicRootFolder, "samples\seattle\seattle-schema.dtm") |> File.ReadAllText
HttpClient.transact serviceUri alias dbName schema_tx |> Async.RunSynchronously

type Test = DatomicDatabase<"http://localhost:9000/","test","seattle","1000">
