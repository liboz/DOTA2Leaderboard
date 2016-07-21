namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Leaderboard.Data")>]
[<assembly: AssemblyProductAttribute("DOTA2Leaderboard")>]
[<assembly: AssemblyDescriptionAttribute("Grabs and displays DOTA 2 Leaderboard data")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
    let [<Literal>] InformationalVersion = "1.0"
