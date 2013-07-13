
#load "Config.fs"

open System.Diagnostics

//Start Datomic server + shell
ProcessStartInfo("bin\shell", WorkingDirectory = datomicRootFolder) |> Process.Start

//Start local Datomic HTTP gateway
ProcessStartInfo(@"bin\rest", Arguments = sprintf "-p %i %s %s" port alias url, WorkingDirectory = datomicRootFolder) |> Process.Start

