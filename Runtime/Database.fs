namespace FSharp.Data.Datomic

type Database(serviceUri, storage, dbName, ?asOf : string) = 
    
    member this.Transact data = 
        HttpClient.transact data

