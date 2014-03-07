namespace SportsITCalExport

module Downloader =

  open HtmlAgilityPack
  open FSharp.Net
  open System.Net
  open System.Xml


  let downloadDocument config =
    let uri = new System.Uri(config.pageUrl)
    let cc = new CookieContainer()
    cc.Add(new Cookie("mysam_company", "bluesky", Domain = uri.Host))
    cc.Add(new Cookie("mysam_session", config.sessionVal, Domain = uri.Host))

    let res = Http.Request(uri.AbsoluteUri, cookieContainer = cc)
    let doc = new HtmlDocument()
    doc.LoadHtml(res)
    doc

  let useLocalDocument =
    let doc = new HtmlDocument()
    doc.Load("out.html")
    doc