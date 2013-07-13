namespace FSharp.Data.Datomic

open System
open System.Diagnostics
open System.Threading
open Xunit
open Swensen.Unquote

type HttpClientTests() = 

    let datomicRootFolder, port, connection = @"c:\datomic", 9000, "datomic:mem://"
    let serverUri, storage, database = Uri "http://mitekm-pc2:9000", "test", "tryDatomic"

//    //Start Datomic server + shell
//    let datomic = 
//        ProcessStartInfo("bin\shell", WorkingDirectory = datomicRootFolder) 
//        |> Process.Start
//    do Thread.Sleep(5000)
//    //Start local Datomic HTTP gateway
//    let restGateway = 
//        ProcessStartInfo(@"bin\rest", Arguments = sprintf "-p %i %s %s" port storage connection, WorkingDirectory = datomicRootFolder) 
//        |> Process.Start
//    do Thread.Sleep(10000)
//    do assert (HttpClient.storages serverUri  = [])

    [<Fact>]
    let createDatabase() = 
        Assert.True(HttpClient.createDatabase serverUri storage database)
        
    [<Fact>]
    let databaseInfo() = 
        let actual = HttpClient.databaseInfo serverUri storage database "-"
        let expected = "{:db/alias \"test/tryDatomic\", :basis-t 62}";
        Assert.Equal<string>(expected, actual) 
        //test <@ actual = expected @>
        
//    interface IDisposable with 
//        member __.Dispose() = 
//            restGateway.CloseMainWindow() |> ignore
//            datomic.CloseMainWindow() |> ignore



