namespace SportsITCalExport

[<AutoOpen>]
module Common =

  let flip (f: 'a -> 'b -> 'c) (b:'b) =
    fun (x:'a) -> f x b

  let fmap (f:'a -> 'b) (fa:'a option) : 'b option =
    Option.map(f) fa

  let ap (mf:('a -> 'b) option) (ma:'a option) : 'b option =
    Option.bind(fun a -> Option.map(fun f -> f a) mf) ma

  let liftA = fmap

  let liftA2 (f:'a -> 'b -> 'c) (fa:'a option) (fb:'b option) : 'c option =
    liftA f fa |> flip ap fb

  let liftA3 (f:'a -> 'b -> 'c -> 'd) (fa:'a option) (fb:'b option) (fc:'c option) : 'd option =
    liftA2 f fa fb |> flip ap fc

  let concat (xs:('a option) seq) =
    Seq.fold (fun acc (x:'a option) -> if x.IsSome then x.Value :: acc else acc) [] xs