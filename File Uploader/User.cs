using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

public class RestService : IDisposable
{
    private readonly HttpClient _httpClient;

    private readonly string _acumaticaBaseUrl;

    public RestService(
      string acumaticaBaseUrl, string userName, string password,
      string company, string branch)
    {
        _acumaticaBaseUrl = acumaticaBaseUrl;
        _httpClient = new HttpClient(
            new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            })
        {
            BaseAddress = new Uri(acumaticaBaseUrl +
              "/entity/Request/18.200.001/"),
            DefaultRequestHeaders =
            {
                Accept = {MediaTypeWithQualityHeaderValue.Parse("text/json")}
            }
        };
        _httpClient.PostAsJsonAsync(
          acumaticaBaseUrl + "/entity/auth/login", new
          {
              name = userName,
              password = password,
              company = company,
              branch = branch
          }).Result
            .EnsureSuccessStatusCode();
    }

    void IDisposable.Dispose()
    {
        _httpClient.PostAsync(_acumaticaBaseUrl + "/entity/auth/logout",
          new ByteArrayContent(new byte[0])).Wait();
        _httpClient.Dispose();
    }

    public string Get(string entityName, string parameters)
    {
        var res = _httpClient.GetAsync(
  _acumaticaBaseUrl + "/entity/Default/6.00.001/"
  + entityName + "?" + parameters).Result
    .EnsureSuccessStatusCode();

        return res.Content.ReadAsStringAsync().Result;
    }
}