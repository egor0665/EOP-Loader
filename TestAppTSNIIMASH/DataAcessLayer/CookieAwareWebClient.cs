using System;
using System.Net;

public class CookieAwareWebClient : WebClient
{
    public string Method;
    public CookieContainer CookieContainer { get; set; }
    public Uri Uri { get; set; }

    public CookieAwareWebClient()
        : this(new CookieContainer())
    {}

    public CookieAwareWebClient(CookieContainer cookies)
    {
        this.CookieContainer = cookies;
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
        WebRequest request = base.GetWebRequest(address);
        if (request is HttpWebRequest)
        {
            (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
            (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36";
            (request as HttpWebRequest).Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            (request as HttpWebRequest).Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            (request as HttpWebRequest).Referer = "https://urs.earthdata.nasa.gov/home";
            (request as HttpWebRequest).KeepAlive = true;
            (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.Deflate |
                                                                 DecompressionMethods.GZip;
            if (Method == "POST")
            {
                (request as HttpWebRequest).ContentType = "application/x-www-form-urlencoded";
            }
        }
        HttpWebRequest httpRequest = (HttpWebRequest)request;
        httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        return httpRequest;
    }

    protected override WebResponse GetWebResponse(WebRequest request)
    {
        String setCookieHeader = "";
        WebResponse response;
        try
        {
            response = base.GetWebResponse(request);
            setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];
        }
        catch (WebException e)
        {
            if (e.Message.Contains("302"))
            {
                Uri a = new Uri(e.Response.Headers[HttpResponseHeader.Location]);
                setCookieHeader = e.Response.Headers[HttpResponseHeader.SetCookie];
            }
            response = e.Response;
        }

        if (setCookieHeader != null)
        {
            try
            {
                if (setCookieHeader != null)
                {
                    Cookie cookie = new Cookie();
                    this.CookieContainer.Add(cookie);
                }
            }
            catch (Exception)
            {
             
            }
        }
        return response;
    }
}