[<RequireQualifiedAccess>]
module FSharp.Data.Edn

open System

let (|List|_|) (s : string) = 
    let s = s.Trim()
    if s.StartsWith("[") && s.EndsWith("]")
    then 
        s.TrimStart('[').TrimEnd(']').Split(',') |> Array.map (fun s -> s.Trim('"')) |> Array.toList |> Some
    else
        None

