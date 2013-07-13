namespace FSharp.Data.Datomic

type Entity(data : (string * obj) list) = 
    
    member this.Item attr = 
        data |> List.tryPick (fun(name, value) -> if name = attr then Some(value) else None) 
    static member (?) (e : Entity, attr) = e.[attr] |> Option.map unbox
        
    

