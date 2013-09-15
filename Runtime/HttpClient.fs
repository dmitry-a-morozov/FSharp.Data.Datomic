[<RequireQualifiedAccess>]
module FSharp.Data.Datomic.HttpClient

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Web

open FSharp.Data

let private httpClient = 
    let x = new HttpClient()
    x.DefaultRequestHeaders.Accept.Add <| MediaTypeWithQualityHeaderValue "application/edn"
    x

let private uriDataBuilder baseUri pathItems = 
    let path =  "data" :: pathItems |> String.concat "/" |> sprintf "/%s/"
    Uri(baseUri, path)
            
let storages serverUri = 
    let requestUri = uriDataBuilder serverUri [] 
    use response = httpClient.GetAsync(requestUri).Result
    match response.Content.ReadAsStringAsync().Result with
    | Edn.List xs -> xs 
    | response -> failwithf "Unexpected response %s" response
        
let createDatabase serverUri storage dbName = 
    let requestUri = uriDataBuilder serverUri [storage]
    use content = new FormUrlEncodedContent(dict["db-name", dbName])
    use response = httpClient.PostAsync(requestUri, content).Result.EnsureSuccessStatusCode()
    response.StatusCode = HttpStatusCode.Created

let databaseInfo serverUri storage dbName t = 
    //let version = match t with | Some t -> string t | None -> "-"
    let requestUri = uriDataBuilder serverUri [storage; dbName; t ]
    use response = httpClient.GetAsync(requestUri).Result.EnsureSuccessStatusCode()
    response.Content.ReadAsStringAsync().Result
         
let transact serverUri storage dbName data =
    async {
        let requestUri = uriDataBuilder serverUri [storage; dbName ] 
        use content = new FormUrlEncodedContent(dict["tx-data", data])
        use! response = httpClient.PostAsync(requestUri, content) |> Async.AwaitTask
        return response.EnsureSuccessStatusCode().StatusCode = HttpStatusCode.Created
        //return response
    }

let q serverUri storage dbName query  =
    async {
        let args = sprintf "[{:db/alias \"%s/%s\"}]" storage dbName
        let queryString = 
            [ "q", query; "args", args] 
            |> List.map (fun(name, value) -> sprintf "%s=%s" name (Uri.EscapeDataString value))
            |> String.concat "&" 
            |> (+) "api/query?"
        let requestUri = Uri(serverUri,  queryString)
        use! response = httpClient.GetAsync(requestUri.AbsoluteUri) |> Async.AwaitTask
        return! response.EnsureSuccessStatusCode().Content.ReadAsStringAsync() |> Async.AwaitTask
    }
    