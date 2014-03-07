namespace SportsITCalExport

module Csv =

  open CsvHelper
  open CsvHelper.Configuration
  open CsvHelper.TypeConversion
  open System.IO


  type CsvRow(subject:string, location:string, game:Game) =
    let _dateFormat = "MM/dd/yyyy"
    let _timeFormat = "hh:mm:ss tt"

    member x.Subject =
      subject

    member x.StartDate =
      game.StartsAt.ToString(_dateFormat)

    member x.StartTime =
      game.StartsAt.ToString(_timeFormat)

    member x.EndDate =
      game.EndsAt.ToString(_dateFormat)

    member x.EndTime =
      game.EndsAt.ToString(_timeFormat)

    member x.AllDayEvent =
      false

    member x.Description =
      game.Description

    member x.Location =
      location

    member x.Private =
      true

  type CsvRowMap() =
    inherit CsvClassMap<CsvRow>()

    override x.CreateMap() =
      x.Map(fun m -> m.Subject :> obj).Name("Subject") |> ignore
      x.Map(fun m -> m.StartDate :> obj).Name("Start Date") |> ignore
      x.Map(fun m -> m.StartTime :> obj).Name("Start Time") |> ignore
      x.Map(fun m -> m.EndDate :> obj).Name("End Date") |> ignore
      x.Map(fun m -> m.EndTime :> obj).Name("End Time") |> ignore
      x.Map(fun m -> m.AllDayEvent :> obj).Name("All Day Event") |> ignore
      x.Map(fun m -> m.Description :> obj).Name("Description") |> ignore
      x.Map(fun m -> m.Location :> obj).Name("Location") |> ignore
      x.Map(fun m -> m.Private :> obj).Name("Private") |> ignore

  let private mkCsvRow config game =
    new CsvRow(config.subject, config.location, game)

  let private writeCsv config (f:CsvWriter -> unit) =
    use streamWriter = new StreamWriter(config.outfile)
    let writer = new CsvWriter(streamWriter)
    writer.Configuration.RegisterClassMap<CsvRowMap>()
    f writer

  let createCsv config games =
    games 
    |> Seq.map (mkCsvRow config)
    |> fun games -> writeCsv config (fun writer -> Seq.iter writer.WriteRecord games)