using CustomAttribute;
using File_Uploader.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace File_Uploader
{
    public class RestService : IDisposable
    {
        private HttpClient _httpClient;
        public FetchedReqResponse fetchedReqResponse = new FetchedReqResponse();

        private string _acumaticaBaseUrl;
        private string _user;
        private string _pwd;
        public static bool loggedIn;
        private string _endpointName;
        private string _endpointVersion;
        private string _company;
        private string _branch;
        private string _entity;

        public RestService(string acumaticaBaseUrl, string user, string pwd, string endpointName, string endpointVersion, string company, string branch, string entity)
        {
            _acumaticaBaseUrl = acumaticaBaseUrl;
            _user = user;
            _pwd = pwd;
            _endpointName = endpointName;
            _endpointVersion = endpointVersion;
            _company = company;
            _branch = branch;
            _entity = entity;

        }

        void IDisposable.Dispose()
        {
            _httpClient.PostAsync(_acumaticaBaseUrl + "/entity/auth/logout",
              new ByteArrayContent(new byte[0])).Wait();
            _httpClient.Dispose();
        }


        public void Logout()
        {
            if (_httpClient != null)
            {
                _httpClient.PostAsync(_acumaticaBaseUrl + "/entity/auth/logout",
                 new ByteArrayContent(new byte[0])).Wait();
                _httpClient.Dispose();
            }
        }

        //Attachment of a file to a record
        public string PutFile(string key,
          string fileName, System.IO.Stream file)
        {

            var attachFileUrl = _acumaticaBaseUrl + "/entity/" + this._endpointName + "/" + this._endpointVersion + "/" + this._entity + "/" + key + "/files/" + fileName;

            var res = _httpClient.PutAsync(attachFileUrl, new StreamContent(file)).Result
                .EnsureSuccessStatusCode();

            Logout();
            return res.Content.ReadAsStringAsync().Result;
        }


        //Attachment of a file to a record
        public string PutFileWithComment(string key,
          string fileName, System.IO.Stream file, string comment)
        {
            var x =LoginAsync(_acumaticaBaseUrl, _user, _pwd,
                      _company, _branch);

            using (MemoryStream ms = new MemoryStream())
            {
                var attachFileUrl = _acumaticaBaseUrl + "/entity/" + this._endpointName + "/" + this._endpointVersion + "/" + this._entity + "/validateObjects";

                

                byte[] buffer = new byte[16 * 1024];

                int read;
                while ((read = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }


                var base64Image = Convert.ToBase64String(ms.ToArray());

                FileWithComment fileWithComment = new FileWithComment
                {
                    entity = new CustomAttribute.Entity
                    {
                        RefNbr = new CustomAttribute.RefNbr
                        {
                            value = key
                        }
                    },
                    parameters = new Parameters
                    {
                       Usrfilename = new Filename
                        {
                            value = comment+"#"+fileName,
                        },
                        Usrimage = new Image
                        {
                            value = base64Image
                        }
                    }

                };

                var json = JsonConvert.SerializeObject(fileWithComment, Formatting.None);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                
                var res = _httpClient.PostAsync(attachFileUrl, stringContent).Result.EnsureSuccessStatusCode();

                Logout();
                return res.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// Attach note method
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="key"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> AttachNoteAsync(string entityName, string key, string Note)
        {
            //var x = await LoginAsync(Properties.Settings.Default.baseURL, Properties.Settings.Default.username, Properties.Settings.Default.password,
            //    Properties.Settings.Default.company, Properties.Settings.Default.branch);

            var updateNoteUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity;

            var updateObj = new Root
            {
                RefNbr = new RefNbr { value = key },
                note = Note
            };

            var json = JsonConvert.SerializeObject(updateObj, Formatting.None);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var res = _httpClient.PutAsync(updateNoteUrl, stringContent).Result
                .EnsureSuccessStatusCode();

            Logout();
            return await res.Content.ReadAsStringAsync();
            //rr = await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Fatch case which to be updated
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="CaseID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> FetchRequestAsync(string OrderNbr)
        {
            FetchedReqResponse fetchedReqResponse = new FetchedReqResponse();

            var x = await LoginAsync(_acumaticaBaseUrl, _user, _pwd,
                  _company, _branch);

            if (x == "[]")
            {
                fetchedReqResponse.result = false;
                fetchedReqResponse.error= "Record does not exist.";
                fetchedReqResponse.json = "";

                Logout();
                
            } else if (x.ToLower().Contains("api login limit")) {
                fetchedReqResponse.result = false;
                fetchedReqResponse.error = "Login limit exceeded.";
                fetchedReqResponse.json = "";

                Logout();
            }
            else
            {
                var updateCaseUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity + "/?" + "%24filter=refnbr%20eq%20'" + OrderNbr + "'&%24expand=details";
                var res = _httpClient.GetAsync(updateCaseUrl).Result
                    .EnsureSuccessStatusCode();

                
                fetchedReqResponse.result = true;
                fetchedReqResponse.error = "";
                fetchedReqResponse.json = res.Content.ReadAsStringAsync().Result;

                Logout();
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(fetchedReqResponse);

        }

        /// <summary>
        /// Fatch case which to be updated
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="CaseID"></param>
        /// <returns></returns>
        public string FetchCase(string CaseID)
        {

            var updateCaseUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity + "/?" + "%24filter=CaseID%20eq%20'" + CaseID + "'%20%20&%24expand=attributes";

            var res = _httpClient.GetAsync(updateCaseUrl).Result
                .EnsureSuccessStatusCode();

            return res.Content.ReadAsStringAsync().Result;
            //rr = await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Update case attribute after fetching case and matcing attributeIDs 20.200.001
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="CaseID"></param>
        /// <returns></returns>
        public string UpdateAttributes(Attributes attributes, string CaseID)
        {

            var updateCaseUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity + "/?" + "%24filter=CaseID%20eq%20'" + CaseID + "'%20%20&%24expand=attributes";
            var uri = new Uri(updateCaseUrl);
            var json = JsonConvert.SerializeObject(attributes, Formatting.None);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = _httpClient.PutAsync(uri, stringContent).Result
                .EnsureSuccessStatusCode();
            Logout();
            return res.Content.ReadAsStringAsync().Result;
            //rr = await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Update case attribute after fetching case and matcing attributeIDs v18.200.001
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="CaseID"></param>
        /// <returns></returns>
        public string UpdateAttributes(CustomAttribute.CaseAttributes attributes, string CaseID)
        {

            var updateCaseUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity + "/?" + "%24filter=CaseID%20eq%20'" + CaseID + "'%20%20&%24expand=attributes";
            var uri = new Uri(updateCaseUrl);
            var json = JsonConvert.SerializeObject(attributes, Formatting.None);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = _httpClient.PutAsync(uri, stringContent).Result
                .EnsureSuccessStatusCode();
            Logout();
            return res.Content.ReadAsStringAsync().Result;
            //rr = await response.Content.ReadAsStringAsync();

        }

        public string UpdateCaseAttribute(string param, string CaseID)
        {
            //var x = await LoginAsync(Properties.Settings.Default.baseURL, Properties.Settings.Default.username, Properties.Settings.Default.password,
            //    Properties.Settings.Default.company, Properties.Settings.Default.branch);

            var updateCaseUrl = _acumaticaBaseUrl + "/entity/" + _endpointName + "/" + _endpointVersion + "/" + _entity + "/" + CaseID;

            var res = _httpClient.GetAsync(updateCaseUrl).Result
                .EnsureSuccessStatusCode();

            Logout();
            return res.Content.ReadAsStringAsync().Result;
            //rr = await response.Content.ReadAsStringAsync();

        }

        public async System.Threading.Tasks.Task<string> LoginAsync(string acumaticaBaseUrl, string userName, string password,
          string company, string branch)
        {

            var mainUrl = new Uri(_acumaticaBaseUrl);
            //_acumaticaBaseUrl = acumaticaBaseUrl;

            _httpClient = new HttpClient(
     new HttpClientHandler
     {
         UseCookies = true,
         CookieContainer = new CookieContainer()
     },false)
            {
                BaseAddress = mainUrl,
                DefaultRequestHeaders =
            {
                Accept = {MediaTypeWithQualityHeaderValue.Parse("application/json")}
            }
            };
            var user = new Dictionary<string, string>
              {
                  { "name" , _user },
                  { "password" , _pwd},
                  { "company" ,_company },
                  { "branch" , _branch }
              };
            var json = JsonConvert.SerializeObject(user, Formatting.None);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");




            //_httpClient = new HttpClient(
            //    new HttpClientHandler
            //    {
            //        UseCookies = true,
            //        CookieContainer = new CookieContainer()
            //    })
            //{
            //    BaseAddress = mainUrl,
            //    DefaultRequestHeaders =
            //{
            //    Accept = {MediaTypeWithQualityHeaderValue.Parse("application/json")}
            //}
            //};

            var result = _httpClient.PostAsync(
               acumaticaBaseUrl + "/entity/auth/login", stringContent);


            return await result.Result.Content.ReadAsStringAsync();
            //.EnsureSuccessStatusCode();
        }

    }
    public class RefNbr
    {
        public string value { get; set; }
    }

    public class Root
    {
        public RefNbr RefNbr { get; set; }
        public string note { get; set; }
    }

    public class FileWithNote
    {
        public string comment { get; set; }
        public byte[] file { get; set; }
        public string key { get; set; }
    }



    public class FetchedReqResponse {
        public bool result { get; set; }
        public string json { get; set; }
        public string error { get; set; }
    }


}