namespace Leaderboard.Data

/// Documentation for my library
///
/// ## Example
///
///     let h = Library.hello 1
///     printfn "%d" h
///
#if INTERACTIVE
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#endif
open FSharp.Data
open System
open System.IO
open System.Net


type LeaderboardInfo = 
  JsonProvider<""" /data/schema.json """>

type LeaderboardRegion =
    Americas
    | Europe
    | China
    | SoutheastAsia
    override this.ToString() =
        match this with
        | Americas -> "americas"
        | Europe -> "europe"
        | China -> "china"
        | SoutheastAsia -> "se_asia"


module Source = 
    /// Returns 42
    ///
    /// ## Parameters
    ///  - `num` - whatever
    let Regions = [Americas; Europe; China; SoutheastAsia; ]
    let baseUrl = """http://www.dota2.com/webapi/ILeaderboard/GetDivisionLeaderboard/v0001?division={0}"""

    let getRegionData (url: string) =
        let request = WebRequest.Create(url)
        use response = request.GetResponse()
        let dataStream = response.GetResponseStream()
        use reader = new StreamReader(dataStream)
        let responseFromServer = reader.ReadToEnd()
        responseFromServer
    
    let getAllData () =
        Regions
        |> List.map (fun i -> 
            let regionName = i.ToString()
            let url = String.Format(baseUrl, regionName)
            regionName, getRegionData url
            )
        |> List.iter (fun (region, data) -> Database.insertData data region |> Async.RunSynchronously)