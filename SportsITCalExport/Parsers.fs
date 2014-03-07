namespace SportsITCalExport

module Parsers =

  open HtmlAgilityPack
  open System
  open System.Collections.Generic
  open System.Linq
  open System.Text
  open System.Text.RegularExpressions
  open System.Xml


  let private parseGameMonthDay (text:string) =
    let ps = text.Split(' ')
    match List.ofSeq ps with
    | _ :: monthAbbr :: dayWithOrdinal :: _ -> 
      let dayStr = dayWithOrdinal.Remove(dayWithOrdinal.Length - 2)
      let couldParse, parsedDay = System.Int16.TryParse(dayStr)
      if couldParse 
      then Some({ monthAbbr = monthAbbr; day = parsedDay })
      else None
    | _ -> None

  let private parseTime (time:string) (gameMonthDate:GameMonthDay) =
    let dateStr = sprintf "%s %i %i %s" gameMonthDate.monthAbbr gameMonthDate.day DateTime.Now.Year time
    let formats = ["MMM d yyyy h:mmtt"]
    let couldParse, parsedDate = DateTime.TryParseExact(dateStr, formats.ToArray(), null, Globalization.DateTimeStyles.None)
    if couldParse
    then Some(parsedDate)
    else None

  let private parseDuration (duration:string) =
    let duration' = duration.Remove(0, "Game".Length) |> (fun x -> x.Remove(x.Length - 1))
    let couldParse, parsedDuration = System.Int16.TryParse(duration')
    if couldParse
    then Some(parsedDuration)
    else None

  let private parseDescription (description:String) =
    Regex.Replace(description, @"\s+", " ")

  let private mkGame startsAt durationMin description =
    new Game(startsAt, durationMin, description)

  let private parseGameNode (text:String) =
    let parts = text.Trim().Split('\n') |> Seq.map (fun x -> x.Trim()) |> Seq.filter (fun x -> not(String.IsNullOrEmpty(x)))
    match List.ofSeq parts with
    | gameMonthDay :: time :: durationStr :: description :: _ ->
      let mStartsAt = parseGameMonthDay gameMonthDay |> Option.bind (parseTime time)
      let mDuration = parseDuration durationStr
      let mDescription = parseDescription description |> Some
      liftA3 mkGame mStartsAt mDuration mDescription
    | _ -> None

  let private findGameNodes (node:HtmlNode) =
    let nodes = node.SelectNodes("//td[@class='dateTime']")
    if nodes = null
    then Seq.empty
    else Seq.ofArray(nodes.ToArray())

  let private gameNodeText (node:HtmlNode) =
    node.ParentNode.InnerText

  let parseDocument (doc:HtmlDocument) =
    findGameNodes(doc.DocumentNode)
    |> Seq.map gameNodeText
    |> Seq.map parseGameNode