namespace FSharp.Data.Datomic

open System
open System.Net.Http
open System.Net.Http.Headers
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Samples.FSharp.ProvidedTypes

open FSharp.Data

[<TypeProvider>]
type public DatomicTypeProvider(config : TypeProviderConfig) as this = 
    inherit TypeProviderForNamespaces()

    let nameSpace = this.GetType().Namespace
    let assembly = Assembly.GetExecutingAssembly()
    let providerType = ProvidedTypeDefinition(assembly, nameSpace, "DatomicDatabase", Some typeof<obj>, HideObjectMethods = true)

    do 
        providerType.DefineStaticParameters(
            parameters = [ 
                ProvidedStaticParameter("ServiceUri ", typeof<string>) 
                ProvidedStaticParameter("Storage ", typeof<string>) 
                ProvidedStaticParameter("Database ", typeof<string>) 
                ProvidedStaticParameter("AsOf ", typeof<string>, "-") 
            ],             
            instantiationFunction = this.CreateRoot
        )
        this.AddNamespace(nameSpace, [ providerType ])

    member internal this.CreateRoot typeName parameters = 
        let serviceUri, storage, database, asofT = 
            match parameters with
            | [| 
                :? string as p1; 
                :? string as p2; 
                :? string as p3; 
                :? string as p4
                |] -> Uri p1, p2, p3, p4
            | _ -> failwith "invalid params"
    
        let root = ProvidedTypeDefinition(assembly, nameSpace, typeName, Some typeof<obj>, HideObjectMethods = true)
//        root.AddMembersDelayed <| fun() ->
//            match Datomic.HttpClient.storage serviceUri with
//            | Edn.List xs -> 
//                xs |> List.map (fun x -> ProvidedTypeDefinition(className = x.Trim('"'), baseType = Some typeof<obj>))
//            | error -> failwithf "Unexpected storage response %s" error
        root

[<assembly:TypeProviderAssembly>]
do()
