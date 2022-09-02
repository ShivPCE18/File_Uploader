using File_Uploader.App_Start;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace File_Uploader.Controllers
{
    public class HomeController : Controller
    {
        string baseUrl;
        string user;
        string password;
        string company;
        string branch;
        string entity;
        string endpointVersion;
        string endpointName;
        List<Controls> controls = new List<Controls>();
        string notemsg;
        RestService service = null;
        INIFile objCommandIni = null;
        string strCommFileName = "~/Configurations/Question.ini";

        public HomeController()
        {
            baseUrl = Properties.Settings.Default.baseURL;

        }

        [Route("{OrderNbr}")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.paramKey = Properties.Settings.Default.paramKey;

            //order nbr from url
            var orderNbr = HttpUtility.ParseQueryString(this.Request.Url.Query).Get(ViewBag.paramKey);
            if (orderNbr != "" && orderNbr != null)
                ViewBag.disableButton = Convert.ToString(FetchRequest(orderNbr));
            else
                ViewBag.disableButton = true;


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(string OrderNbr, string Note)
        {
            try
            {
                //ExternalLogin();
                loadRequestConfig();

                if (Request.Files.Count > 0)
                {

                    var file = Request.Files[0];
                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    service = new RestService(baseUrl, user, password, endpointName, endpointVersion, company, branch, this.entity);

                    notemsg = " File is updated to the entity.";

                    //service = new RestService(baseUrl);
                    bool isUploaded = UploadFileWithComment(fileName, file.InputStream, OrderNbr, Note);

                    if (isUploaded)
                        return Json(new { success = true, message = "File attached successfully.", notemsg = notemsg }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { success = true, message = "File uploading failed.", notemsg = notemsg }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { success = false, message = "Please select a file !", notemsg = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                service.Logout();
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        void ExternalLogin()
        {
            var client = new RestClient("http://localhost/AcumaticaDBV%2821.115.0017%291/entity/auth/login");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"name\" : \"admin\",\r\n    \"password\" : \"admin\",\r\n    \"company\" :  \"\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var x = response.Content;

            client = new RestClient("http://localhost/AcumaticaDBv%2821.115.0017%291/entity/custom/18.200.001/Request/validateObjects");
            request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "1921142c-779d-fc94-9777-68953a20a543");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"entity\":{\"RefNbr\":{\"value\":\"0000027\"}},\"parameters\":{\"Usrfilename\":{\"value\":\"f#Post_Request.txt\"},\"Usrimage\":{\"value\":\"eyJlbnRpdHkiOiB7DQogICAgICAgICJSZWZOYnIiOiB7DQogICAgICAgICAgICAidmFsdWUiOiAiMDAwMDA4Ig0KICAgICAgICB9DQogICAgfSwNCiAgICAiUGFyYW1ldGVycyI6IHsNCiAgICAgICAgIlVzckN1c3RvbUZpZWxkIjogew0KICAgICAgICAgICAgInZhbHVlIjogInRlc3QgdmFsdWUiDQogICAgICAgIH0NCiAgICB9DQp9\"}}}", ParameterType.RequestBody);
            response = client.Execute(request);
        }

        [HttpGet]
        public ActionResult CaseFeedback()
        {
            if (Directory.Exists(Server.MapPath("~/Configurations")))
            {
                strCommFileName = Server.MapPath(strCommFileName);
                objCommandIni = new INIFile(strCommFileName);
                entity = objCommandIni.Read("configuration", "endpointName", "Case");
                ViewBag.pageTitle = objCommandIni.Read("page", "Header", "Title");
                ViewBag.caseID = objCommandIni.Read("Configuration", "paramKey", "CaseID");
            }

            return View();
        }


        [HttpPost]
        public ActionResult CaseFeedback(string CaseID, string attributes)
        {
            try
            {
                if (CaseID == "" || CaseID == null || CaseID == "undefined")
                    return Json(new { success = true, message = "Invalid CaseID.", notemsg = notemsg }, JsonRequestBehavior.AllowGet); ;

                var res = "";

                // load access info from ini file
                LoadConfiguration();

                CustomAttribute.CaseAttributes crossVersionAttribute = new CustomAttribute.CaseAttributes();
                crossVersionAttribute.Attributes = new List<CustomAttribute._Attribute>();

                List<version20.Case> toBeUpdatedCase = new List<version20.Case>();

                if (string.IsNullOrEmpty(attributes))
                    return Json(new { success = false, message = "Invalid values!", notemsg = "" }, JsonRequestBehavior.AllowGet);

                List<values> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<values>>(attributes);

                service = new RestService(baseUrl, user, password, endpointName, endpointVersion, company, branch, this.entity);

                //Login 
                var x = service.LoginAsync(baseUrl, user, password, company, branch);

                //fetching case before updating
                var fetchedCase = service.FetchCase(CaseID);

                if (!string.IsNullOrEmpty(fetchedCase))
                {
                    toBeUpdatedCase = Newtonsoft.Json.JsonConvert.DeserializeObject<List<version20.Case>>(fetchedCase);

                    for (int i = 0; i < obj.Count; i++)
                    {
                        for (int j = 0; j < toBeUpdatedCase[0].Attributes.Count; j++)
                        {
                            if (obj[i].attributeID.Trim() != "" && toBeUpdatedCase[0].Attributes[j].AttributeID.value == obj[i].attributeID)
                            {
                                crossVersionAttribute.Attributes.Add(new CustomAttribute._Attribute
                                {
                                    Attribute = new CustomAttribute.Attribute2 { value = toBeUpdatedCase[0].Attributes[j].Attribute != null ? toBeUpdatedCase[0].Attributes[j].Attribute.value : "" },
                                    AttributeID = new CustomAttribute.AttributeID { value = obj[i].attributeID },
                                    Value = new CustomAttribute.Value { value = obj[i].value }
                                });
                            }
                        }
                    }


                    if (objCommandIni != null && objCommandIni.Read("defaults", "RequiredDefault", "false").ToLower() == "true" && objCommandIni.Read("defaults", "DefaultAttributeID", "") != "")
                    {
                        crossVersionAttribute.Attributes.Add(new CustomAttribute._Attribute
                        {
                            Attribute = new CustomAttribute.Attribute2 { value = "" },
                            AttributeID = new CustomAttribute.AttributeID { value = objCommandIni.Read("defaults", "DefaultAttributeID", "") },
                            Value = new CustomAttribute.Value { value = CaseID }
                        });
                    }

                    //updating attributes
                    res = service.UpdateAttributes(crossVersionAttribute, CaseID);


                    if (Request.Files.Count > 0)
                    {

                        var file = Request.Files[0];
                        var fileName = System.IO.Path.GetFileName(file.FileName);

                        bool isUploaded = UploadFile(fileName, file.InputStream, CaseID);

                        if (isUploaded)
                            return Json(new { success = true, message = "File attached successfully.", notemsg = notemsg }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new { success = true, message = "File uploading failed.", notemsg = notemsg }, JsonRequestBehavior.AllowGet);

                    }

                    if (!string.IsNullOrEmpty(res))
                        return Json(new { success = false, message = "Attribute updated.", notemsg = "" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { success = false, message = "Failed to update attribute.", notemsg = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false, message = "Failed to update attribute.", notemsg = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                service.Logout();
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// return json for control configuration to be placed on page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetControls()
        {
            if (Directory.Exists(Server.MapPath("~/Configurations")))
            {
                strCommFileName = Server.MapPath(strCommFileName);
                objCommandIni = new INIFile(strCommFileName);
                var val = objCommandIni.Read("Details", "count", "0") != "" ? Convert.ToInt32(objCommandIni.Read("Details", "count", "0")) : 0;


                if (val != 0)
                {
                    for (int i = 0; i < val; i++)
                    {
                        controls.Add(new Controls
                        {
                            question = objCommandIni.Read("Questions", "Q" + (i + 1), ""),
                            controlType = objCommandIni.Read("Type", "Q" + (i + 1), ""),
                            attributeID = objCommandIni.Read("AttributeIDs", "Q" + (i + 1), ""),
                            isRequired = objCommandIni.Read("Required", "Q" + (i + 1), "")
                        });
                    }
                }
                ViewBag.caseID = objCommandIni.Read("Configuration", "paramKey", "CaseID");

            }
            return Json(controls, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Uploading file to the specified entity 
        /// </summary>
        /// <param name="filename">To be uploaded filename</param>
        /// <param name="file">To be uploaded file</param>
        /// <param name="key">To be uploaded file's record key</param>
        /// <param name="Note">note to be attached to record</param>
        /// <returns></returns>
        private bool UploadFile(string filename, Stream file, string key)
        {

            service = new RestService(baseUrl, user, password, endpointName, endpointVersion, company, branch, this.entity);
            var x = service.LoginAsync(baseUrl, user, password,
               company, branch);
            var uploadContent = service.PutFile(key, filename, file);

            return uploadContent.Length == 0;

        }

        /// <summary>
        /// Uploading file to the specified entity 
        /// </summary>
        /// <param name="filename">To be uploaded filename</param>
        /// <param name="file">To be uploaded file</param>
        /// <param name="key">To be uploaded file's record key</param>
        /// <param name="Note">note to be attached to record</param>
        /// <returns></returns>
        private bool UploadFileWithComment(string filename, Stream file, string key, string note)
        {

            service = new RestService(baseUrl, user, password, endpointName, endpointVersion, company, branch, this.entity);

            var uploadContent = service.PutFileWithComment(key, filename, file, note);

            return uploadContent.Length == 0;
        }

        private void loadRequestConfig()
        {
            entity = Properties.Settings.Default.entityName;
            endpointName = Properties.Settings.Default.endpointName;
            endpointVersion = Properties.Settings.Default.endpointVersion;
            company = Properties.Settings.Default.company;
            branch = Properties.Settings.Default.branch;
            user = Properties.Settings.Default.username;
            password = Properties.Settings.Default.password;
        }

        /// <summary>
        ///  load api configuration to get access 
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                if (Directory.Exists(Server.MapPath("~/Configurations")))
                {
                    strCommFileName = Server.MapPath(strCommFileName);
                    objCommandIni = new INIFile(strCommFileName);
                    user = objCommandIni.Read("Configuration", "user", "");
                    password = objCommandIni.Read("Configuration", "password", "");
                    endpointName = objCommandIni.Read("Configuration", "endpointName", "default");
                    endpointVersion = objCommandIni.Read("Configuration", "endpointVersion", "18.200.001");
                    company = objCommandIni.Read("Configuration", "company", "company");
                    branch = objCommandIni.Read("Configuration", "branch", "branch");
                    entity = objCommandIni.Read("Configuration", "entityName", "branch");
                    baseUrl = objCommandIni.Read("Configuration", "baseUrl", "branch");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Fetch request by ordernbr
        /// </summary>
        /// <param name="ordernbr">record key</param>
        /// <returns></returns>
        public bool FetchRequest(string ordernbr)
        {
            var isDisable = false;
            List<Models.Root> request = new List<Models.Root>();
            try
            {

                loadRequestConfig();
                service = new RestService(baseUrl, user, password, endpointName, endpointVersion, company, branch, this.entity);
                service.Logout();

                var json = service.FetchRequestAsync(ordernbr).Result;

                var fetchedReqResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FetchedReqResponse>(json);

                if (fetchedReqResponse.result && fetchedReqResponse.error == "")
                {
                    request = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Root>>(fetchedReqResponse.json);
                    if (request != null && request.Count > 0)
                    {
                        if (request[0].Details != null)
                        {
                            if (request[0].Details.RequiredDate.value.ToString() == "01-01-0001 00:00:00") {
                                isDisable = false;
                                ViewBag.NotExists = "Required date does not exist.";
                            }
                            else
                            {
                                var dateNotPassed = request[0].Details.RequiredDate.value - DateTime.Now > TimeSpan.FromHours(-24);
                                if (dateNotPassed)
                                {
                                    isDisable = true;
                                }
                                else
                                {
                                    isDisable = false;
                                    ViewBag.NotExists = "Required date is passed.";
                                }
                            }
                        }
                        else
                        {
                            isDisable = false;
                            ViewBag.NotExists = "Details does not exists.";
                        }
                    }
                    else
                    {
                        isDisable = false;
                        ViewBag.NotExists = "Request does not exists with RefNbr" +" "+ordernbr+".";
                    }
                }
                else
                {
                    isDisable = false;
                    ViewBag.NotExists = fetchedReqResponse.error;
                }
            }
            catch (Exception ex)
            {
                service.Logout();
                isDisable = false;
            }

            return isDisable;
        }

    }

    public class values
    {
        public string value { get; set; }
        public string attributeID { get; set; }
    }

    public class Controls
    {
        public string question { get; set; }
        public string controlType { get; set; }
        public string attributeID { get; set; }
        public string isRequired { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Approved
    {
        public bool value { get; set; }
    }

    public class Currency
    {
        public string value { get; set; }
    }

    public class CuryViewState
    {
        public bool value { get; set; }
    }

    public class Date
    {
        public DateTime value { get; set; }
    }

    public class Department
    {
    }

    public class Description
    {
        public string value { get; set; }
    }

    public class EstExtCost
    {
        public double value { get; set; }
    }

    public class Location
    {
        public string value { get; set; }
    }

    public class OpenQty
    {
        public double value { get; set; }
    }

    public class Priority
    {
        public string value { get; set; }
    }

    public class RefNbr
    {
        public string value { get; set; }
    }

    public class RequestClass
    {
        public string value { get; set; }
    }

    public class RequestedBy
    {
        public string value { get; set; }
    }

    public class Status
    {
        public string value { get; set; }
    }

    public class Custom
    {
    }

    public class File
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string href { get; set; }
    }

    public class Entity
    {
        public string id { get; set; }
        public int rowNumber { get; set; }
        public string note { get; set; }
        public Approved Approved { get; set; }
        public Currency Currency { get; set; }
        public CuryViewState CuryViewState { get; set; }
        public Date Date { get; set; }
        public Department Department { get; set; }
        public Description Description { get; set; }
        public EstExtCost EstExtCost { get; set; }
        public Location Location { get; set; }
        public OpenQty OpenQty { get; set; }
        public Priority Priority { get; set; }
        public RefNbr RefNbr { get; set; }
        public RequestClass RequestClass { get; set; }
        public RequestedBy RequestedBy { get; set; }
        public Status Status { get; set; }
        public Custom custom { get; set; }
        public List<File> files { get; set; }
    }
    public class Attributes
    {
        public List<_Attribute> attributes { get; set; }
    }

    public class AttributesV18
    {
        //public List<File_UploaderV1.Attribute_> attributes { get; set; }
    }


}


namespace CustomAttribute
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Attribute2
    {
        public string value { get; set; }
    }

    public class AttributeID
    {
        public string value { get; set; }
    }

    public class Value
    {
        public string value { get; set; }
    }

    public class _Attribute
    {
        public Attribute2 Attribute { get; set; }
        public AttributeID AttributeID { get; set; }
        public Value Value { get; set; }
    }

    public class CaseAttributes
    {
        public List<_Attribute> Attributes { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class RefNbr
    {
        public string value { get; set; }
    }

    public class Entity
    {
        public RefNbr RefNbr { get; set; }
    }

    public class Filename
    {
        public string value { get; set; }
    }
    public class imageByte
    {
        public byte[] value { get; set; }
    }

    public class Image
    {
        public string value { get; set; }
    }

    public class Parameters
    {
        public Filename Usrfilename { get; set; }
        public Image Usrimage { get; set; }
    }

    public class FileWithComment
    {
        public Entity entity { get; set; }
        public Parameters parameters { get; set; }
    }






}