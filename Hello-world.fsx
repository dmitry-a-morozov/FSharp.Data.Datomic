

#r @"RunTime\bin\Debug\FSharp.Data.Datomic.dll"
//#r @"TypeProvider\bin\Debug\FSharp.Data.Datomic.dll"
#r @"TypeProvider\bin\Debug\FSharp.Data.Datomic.TypeProvider.dll"
#load "Config.fs"

open FSharp.Data
open FSharp.Data.Datomic

let dbName = "hello"
HttpClient.createDatabase serviceUri alias dbName
HttpClient.transact serviceUri alias dbName "[[:db/add #db/id[:db.part/user], :db/doc \"hello world\"]]" |> Async.RunSynchronously
HttpClient.q serviceUri alias dbName "[:find ?e :where [?e :db/doc \"hello world\"]]" |> Async.RunSynchronously
