namespace SportsITCalExport

[<AutoOpen>]
module Types =

  open System


  type Config = 
    { pageUrl:string
    ; sessionVal:string
    ; subject:string
    ; location:string 
    ; outfile:string
    }

  type GameMonthDay = 
    { monthAbbr:string
    ; day:int16; 
    }

  type Game(startsAt:DateTime, durationMin:int16, description:string) =
    let endsAt = startsAt.AddMinutes(float durationMin)

    member x.StartsAt =
      startsAt

    member x.EndsAt =
      endsAt

    member x.Description =
      description