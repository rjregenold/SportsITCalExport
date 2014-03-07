open SportsITCalExport


let config = {
  // the absolute url to the sports IT schedule page
    pageUrl    = "https://secure.sports-it.com/mysam/index.php?action=team&customerID=12345&teamid=1234"
  // the value of the mysam_session cookie (use chrome dev tools -> resources tab to get this value)
  ; sessionVal = "[value of session cookie]"
  // the subject of the calendar event (ie: FC Awesome soccer game)
  ; subject    = "FC Awesome soccer game"
  // the location for the calendar event
  ; location   = "Blue Sky Allen" 
  // the name of the output file
  ; outfile    = "out.csv"
  }

[<EntryPoint>]
let main _ = 
  Downloader.downloadDocument config
  |> Parsers.parseDocument
  |> concat
  |> Csv.createCsv config

  printfn "Finished"
  System.Console.ReadLine() |> ignore

  0