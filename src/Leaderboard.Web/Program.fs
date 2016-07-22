#if INTERACTIVE
#r "../../packages/Suave/lib/net40/Suave.dll"
#endif
open Suave
open Leaderboard.Data
open MongoDB.Bson
open MongoDB.Driver

type CmdArgs = { IP: System.Net.IPAddress; Port: Sockets.Port }

[<EntryPoint>]
let main argv = 
    Source.getAllData() |> ignore
    Database.getData "americas" ""
    Database.getData "europe" ""
    // parse arguments
    let args =
        let parse f str = match f str with (true, i) -> Some i | _ -> None

        let (|Port|_|) = parse System.UInt16.TryParse
        let (|IPAddress|_|) = parse System.Net.IPAddress.TryParse

        //default bind to 127.0.0.1:8083
        let defaultArgs = { IP = System.Net.IPAddress.Loopback; Port = 8083us }

        let rec parseArgs b args =
            match args with
            | [] -> b
            | "--ip" :: IPAddress ip :: xs -> parseArgs { b with IP = ip } xs
            | "--port" :: Port p :: xs -> parseArgs { b with Port = p } xs
            | invalidArgs ->
                printfn "error: invalid arguments %A" invalidArgs
                printfn "Usage:"
                printfn "    --ip ADDRESS   ip address (Default: %O)" defaultArgs.IP
                printfn "    --port PORT    port (Default: %i)" defaultArgs.Port
                exit 1

        argv |> List.ofArray |> parseArgs defaultArgs

    // start suave
    startWebServer
        { defaultConfig with
            bindings = [ HttpBinding.mk HTTP args.IP args.Port ] }
        (Successful.OK "Hello World!")

    0