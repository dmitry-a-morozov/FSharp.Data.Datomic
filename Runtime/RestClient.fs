namespace Datomic

open System.Net
open System.Net.Http
open System.Net.Http.Headers

type RestClient(uri, storageAlias) = 

    let httpClient = new HttpClient(BaseAddress = uri)
    let storagePath = sprintf "data/%s/" storageAlias
    do 
        httpClient.DefaultRequestHeaders.Accept.Add <| MediaTypeWithQualityHeaderValue "application/edn"

    member __.CreateDatabase dbName = 
        use content = new FormUrlEncodedContent(dict["db-name", dbName])
        use response = httpClient.PostAsync(storagePath, content).Result
        response.StatusCode = HttpStatusCode.Created

    member __.DatabaseInfo(dbName, ?t : int64) = 
        let version = if t.IsSome then string t.Value else "-"
        let requestUri = sprintf "%s%s/%s/" storagePath dbName version
        use response = httpClient.GetAsync(requestUri).Result
        response.Content.ReadAsStringAsync().Result
         
    member __.Transact(dbName, data : string) = 
        async {
            use content = new FormUrlEncodedContent(dict["tx-data", data])
            let requestUri = sprintf "%s%s/" storagePath dbName
            use! response = httpClient.PostAsync(requestUri, content) |> Async.AwaitTask
            return response.StatusCode = HttpStatusCode.Created
        }