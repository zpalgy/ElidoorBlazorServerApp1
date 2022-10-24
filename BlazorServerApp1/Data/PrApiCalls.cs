using System;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BlazorServerApp1.Data
{
    public static class PrApiCalls
    {

        //public static DataTable MeagedFields = new DataTable();  // it's a table that is common to all users !
        //public static DoorConfig doorConfig = new DoorConfig();

        public static int ELIDOOR_COMPLIENT = 1;
        public static int MEGULVAN_ID;
        public static int IDS_ONESIDE;
        public static int IDS_TWOSIDES;

        public static DataTable dtMeagedFields;
        public static DataTable dtDecorSideFlds;
        public static DataTable dtConfFields;
        public static DataTable dtDefaults;

        public static List<WingsNum_Class> lstWingsNum = new List<WingsNum_Class>();
        public static DataTable dtWingsNum = new DataTable();
        public static List<Model_Class> lstModels = new List<Model_Class>();
        public static DataTable dtModels = new DataTable();
        public static List<Model_Part_Class> lstModel_Parts = new List<Model_Part_Class>();
        public static DataTable dtModel_Parts = new DataTable();
        public static List<Decoration_Class> lstDecorations = new List<Decoration_Class>();
        public static DataTable dtDecorations = new DataTable();
        public static List<Complient_Class> lstComplients = new List<Complient_Class>();
        public static DataTable dtComplients = new DataTable();

        public static List<PART_Class> lstParts = new List<PART_Class>();
        public static DataTable dtParts = new DataTable();

        //public static List<WingWidth_Class> lstWingWidth = new List<WingWidth_Class>();
        //public static DataTable dtWingWidth = new DataTable();

        public static List<TRSH_COLOR_Class> lstColors = new List<TRSH_COLOR_Class>();
        public static List<TRSH_COLOR_Class> lstGlassColors4Diamond = new List<TRSH_COLOR_Class>();
        public static List<TRSH_LOCKHINGE_DRILH_Class> lstLock_Hinge_Dril_Heights = new List<TRSH_LOCKHINGE_DRILH_Class>();  // table 230
        public static DataTable dtLock_Hinge_Dril_Heights = new DataTable();
        public static List<TRSH_HARDWARE_Class> lstHardwares = new List<TRSH_HARDWARE_Class>();
        public static DataTable dtHardwares = new DataTable();
        public static List<HWACCESSORY_Class> lstHwAccessories = new List<HWACCESSORY_Class>();
        public static DataTable dtHwAccessories = new DataTable();
        public static List<DRIL4HW_Class> lstDril4Hw = new List<DRIL4HW_Class>();
        public static DataTable dtDril4Hws = new DataTable();
        public static List<CYLINDER_Class> lstCylinders = new List<CYLINDER_Class>();
        public static DataTable dtCylinders = new DataTable();
        public static List<CYLHW_Class> lstCYLHWs = new List<CYLHW_Class>(); 
        public static List<TRSH_LOCK_Class> lstLocks = new List<TRSH_LOCK_Class>();
        public static DataTable dtLocks = new DataTable();

        public static List<TILETYPE_Class> lstTileTypes = new List<TILETYPE_Class>();
        public static List<RAW4CPLATES_Class> lstRaw4CPlates = new List<RAW4CPLATES_Class>();
        public static List<WINDOWWIDTH_Class> lstWindowWidths = new List<WINDOWWIDTH_Class>();
        public static DataTable dtWindowWidths = new DataTable();
        public static List<WWIDTH_STATIC_Class> lstWWidth_Statics = new List<WWIDTH_STATIC_Class>();
        public static DataTable dtWWidth_Statics = new DataTable();

        public static List<WINDOWHEIGHT_Class> lstWindowHeights = new List<WINDOWHEIGHT_Class>();
        public static DataTable dtWindowHeights = new DataTable();
        public static List<PROFILE4WINDOW_Class> lstProfiles4Windows = new List<PROFILE4WINDOW_Class>();
        public static List<GLASS4WINDOW_Class> lstGlasses4Windows = new List<GLASS4WINDOW_Class>();
        public static List<GRID_Class> lstGrids = new List<GRID_Class>();
        public static List<VITRAGE_Class> lstVitrages = new List<VITRAGE_Class>();
        public static List<VITRAGE4DIAMOND_Class> lstVitrages4Diamond = new List<VITRAGE4DIAMOND_Class>();
        public static List<GRID4HT1084_Class> lstGrid4HT1084 = new List<GRID4HT1084_Class>();
        public static List<HANDLE_Class> lstHandles = new List<HANDLE_Class>();
        public static DataTable dtHandles = new DataTable();
        public static List<HANDLE4DIAMOND_Class> lstHandles4Diamond = new List<HANDLE4DIAMOND_Class>();

        public static string[] D60DataSource = new string[] { "ללא", "חוץ", "פנים", "דו צדדי" };
        public static string[] D60No2DidesDataSource = new string[] { "ללא", "חוץ", "פנים" };

        //public static RestClient restClient = new RestClient();  - this is page instance specific we can't make it application specific
        static string certAlert = "Pls check whether the SSL certificate of the Default Web Site on the web server has expired";

        public static List<string> lstColorsNum = new List<string> { string.Empty, "מגולוון", "1", "2" };

        public static string defLockwithLogo = "880001";                 //11/09/2022 default LOCKNAMEs with and without Logo
        public static string defLockwithOutLogo = "LCK00002";


        public static void initRestClient(RestClient restClient)
        {
            try
            {
                RestApiSettings rest = new RestApiSettings();
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
                myConfigClass myConfig = new myConfigClass(config);
                string host = myConfig.configVal("RestApi:Host");
                string apiPart = myConfig.configVal("RestApi:ApiPart");
                string INI = myConfig.configVal("RestApi:INI");
                string ENV = myConfig.configVal("RestApi:ENV");
                string ApiUser = myConfig.configVal("RestApi:APIUser");
                string APIPWD = myConfig.configVal("RestApi:APIPWD");

                /* Tropical TEST SERVER */
                restClient.BaseUrl = new Uri(string.Format("{0}/{1}/{2}/{3}", host, apiPart, INI, ENV));

                //myLogger1.WriteLog(string.Format("restClient.BaseUrl = {0}", restClient.BaseUrl));
                //restClient.Authenticator = new HttpBasicAuthenticator(System.Configuration.ConfigurationManager.AppSettings["APIUser"],
                //                                 System.Configuration.ConfigurationManager.AppSettings["APIPWD"]);
                restClient.Authenticator = new HttpBasicAuthenticator(ApiUser, APIPWD);
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static void initRestClient(RestClient restClient, string company)
        {
            try
            {
                /* Tropical TEST SERVER */
                restClient.BaseUrl = new Uri(string.Format("{0}/{1}/{2}/{3}", System.Configuration.ConfigurationManager.AppSettings["Host"],
                                    System.Configuration.ConfigurationManager.AppSettings["ApiPart"],
                                    System.Configuration.ConfigurationManager.AppSettings["INI"],
                                             company));
                //myLogger1.WriteLog(string.Format("restClient.BaseUrl = {0}", restClient.BaseUrl));
                restClient.Authenticator = new HttpBasicAuthenticator(System.Configuration.ConfigurationManager.AppSettings["APIUser"],
                                                 System.Configuration.ConfigurationManager.AppSettings["APIPWD"]);
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static string getReference(string reference, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "REFERENCE";
                request.Resource = string.Format("TRSH_DOORCONFIG?$filter=REFERENCE eq '{0}'&$select={1}", reference, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDoorConfig val = JsonConvert.DeserializeObject<ValuesDoorConfig>(response.Content);
                    if (val.value.Count > 0)
                        return val.value[0].REFERENCE;
                    else
                        return string.Empty;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return string.Empty;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<AGENT_Class> getAgents(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "AGENT,AGENTCODE,AGENTNAME";
                request.Resource = string.Format("AGENTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesAGENT_Class val = JsonConvert.DeserializeObject<ValuesAGENT_Class>(response.Content);
                    List<AGENT_Class> val1 = new List<AGENT_Class>();  //val.value;
                    AGENT_Class emptyAgent = new AGENT_Class();
                    emptyAgent.AGENTCODE = " ";
                    emptyAgent.AGENTNAME = " ";
                    val1.Add(emptyAgent);
                    foreach (AGENT_Class ag in val.value)
                    {
                        val1.Add(ag);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<CUSTOMER_Class> getCustomers(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "CUST,CUSTNAME,CUSTDES,TRSH_SALESMAN";  //,ADDRESS,ADDRESS2,ADDRES3";
                request.Resource = string.Format("CUSTOMERS?$filter=TRSH_SALESMAN eq 'Y'&$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCUSTOMER_Class val = JsonConvert.DeserializeObject<ValuesCUSTOMER_Class>(response.Content);
                    List<CUSTOMER_Class> val1 = new List<CUSTOMER_Class>();  //val.value;
                    CUSTOMER_Class emptyCust = new CUSTOMER_Class();
                    emptyCust.CUST = "0";
                    emptyCust.CUSTNAME = " ";
                    emptyCust.CUSTDES = " ";
                    val1.Add(emptyCust);
                    foreach (CUSTOMER_Class cu in val.value)
                    {
                        val1.Add(cu);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static CUSTOMER_Class getCustomer(int CUST, ref string errMsg)
        {
            try
            {
                if (CUST == 0)
                    return null;

                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "CUST,CUSTNAME,CUSTDES,ADDRESS,ADDRESS2,ADDRESS3,TRSH_NOTECOMPLIENT,TRSH_TURBO";
                request.Resource = string.Format("CUSTOMERS?$filter=CUST eq {0}&$select={1}", CUST, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCUSTOMER_Class val = JsonConvert.DeserializeObject<ValuesCUSTOMER_Class>(response.Content);
                    if (val.value != null && val.value.Count > 0)
                    {
                        CUSTOMER_Class val1 = val.value[0];
                        return val1;
                    }
                    else
                        return null;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<WingsNum_Class> getAllWingsNum(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_WINGSNUM,TRSH_WINGSNUMCODE,TRSH_WINGSNUMDES";
                request.Resource = string.Format("TRSH_WINGSNUM?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWingsNum_Class val = JsonConvert.DeserializeObject<ValuesWingsNum_Class>(response.Content);

                    List<WingsNum_Class> val1 = new List<WingsNum_Class>();  //val.value;
                    WingsNum_Class emptyWnum = new WingsNum_Class();
                    emptyWnum.TRSH_WINGSNUM = 0;
                    val1.Add(emptyWnum);
                    foreach (WingsNum_Class wNum in val.value)
                    {
                        val1.Add(wNum);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<Model_Class> getAllModels(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_MODEL,TRSH_MODELNAME,TRSH_MODELDES,TRSH_MEAGEDNAME,TRSH_MEAGEDDES,TRSH_DOOR_HWCATCODE,TRSH_DOOR_HWCAT";
                request.Resource = string.Format("TRSH_MODELS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesModel_Class val = JsonConvert.DeserializeObject<ValuesModel_Class>(response.Content);

                    List<Model_Class> val1 = new List<Model_Class>();  //val.value;
                    Model_Class emptyModel = new Model_Class();
                    emptyModel.TRSH_MODEL = 0;
                    val1.Add(emptyModel);
                    foreach (Model_Class model in val.value)
                    {
                        val1.Add(model);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<Model_Part_Class> getModelParts(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_MODELNAME,TRSH_MODELDES,TRSH_WINGSNUMDES,TRSH_WINGSNUMCODE,PARTNAME,PARTDES,TRSH_MODEL,TRSH_WINGSNUM,PART";
                request.Resource = string.Format("TRSH_MODEL_PARTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesModel_Part_Class val = JsonConvert.DeserializeObject<ValuesModel_Part_Class>(response.Content);

                    List<Model_Part_Class> val1 = new List<Model_Part_Class>();  //val.value;
                    Model_Part_Class emptyModel = new Model_Part_Class();
                    emptyModel.TRSH_MODEL = 0;
                    val1.Add(emptyModel);
                    foreach (Model_Part_Class model in val.value)
                    {
                        val1.Add(model);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
    
        public static Model_Class getModel(string TRSH_MODELNAME, ref string errMsg)
        {
            Model_Class model = new Model_Class();
            string query = string.Format("TRSH_MODELNAME = '{0}'", TRSH_MODELNAME);
            DataRow[] rowsModels = dtModels.Select(query);
            if (rowsModels.Length > 0)
            {
                model.TRSH_MODEL = int.Parse(rowsModels[0]["TRSH_MODEL"].ToString());
                model.TRSH_MODELNAME = TRSH_MODELNAME;
                model.TRSH_MODELDES = rowsModels[0]["TRSH_MODELDES"].ToString();
                model.TRSH_DOOR_HWCAT = rowsModels[0]["TRSH_DOOR_HWCAT"].ToString();
                model.TRSH_DOOR_HWCATCODE = int.Parse(rowsModels[0]["TRSH_DOOR_HWCATCODE"].ToString());
                model.TRSH_MEAGEDNAME = rowsModels[0]["TRSH_MEAGEDNAME"].ToString();
            }
            return model;
        }
        public static string getPARTNAMEbyModelWings(DoorConfig doorConfig, ref string errMsg)
        {
            int TRSH_MODEL = 0;
            int TRSH_WINGSNUM = 0;

            if (doorConfig != null)
            {
                string query = string.Format("TRSH_MODELNAME = '{0}'", doorConfig.TRSH_MODELNAME);
                DataRow[] rowsModels = dtModels.Select(query);
                if (rowsModels.Length > 0)
                {
                    TRSH_MODEL = int.Parse(rowsModels[0]["TRSH_MODEL"].ToString());
                }
                query = string.Format("TRSH_WINGSNUMDES='{0}'", doorConfig.TRSH_WINGSNUMDES);
                DataRow[] rowsWingsNum = dtWingsNum.Select(query);
                if (rowsWingsNum.Length > 0)
                {
                    TRSH_WINGSNUM = int.Parse(rowsWingsNum[0]["TRSH_WINGSNUM"].ToString());
                }
                if (TRSH_MODEL > 0 && TRSH_WINGSNUM > 0)
                {
                    query = String.Format("TRSH_MODEL = {0} AND TRSH_WINGSNUM = {1}", TRSH_MODEL, TRSH_WINGSNUM);
                    DataRow[] rowsParts = dtParts.Select(query);
                    if (rowsParts.Length > 0)
                    {
                        string PARTNAME = rowsParts[0]["PARTNAME"].ToString();
                        return PARTNAME;
                    }
                }
            }
            return string.Empty;  // error !
        }
        public static bool existsPartOfModel(DoorConfig doorConfig, ref string errMsg)
        {
            int TRSH_MODEL = 0;

            if (doorConfig != null)
            {
                string query = string.Format("TRSH_MODELNAME = '{0}'", doorConfig.TRSH_MODELNAME);
                DataRow[] rowsModels = dtModels.Select(query);
                if (rowsModels.Length > 0)
                {
                    TRSH_MODEL = int.Parse(rowsModels[0]["TRSH_MODEL"].ToString());
                }
                if (TRSH_MODEL > 0)
                {
                    query = String.Format("TRSH_MODEL = {0}", TRSH_MODEL);
                    DataRow[] rowsParts = dtParts.Select(query);
                    return (rowsParts.Length > 0);
                }
            }
            return false;  // error !
        }

        //public static List<WingWidth_Class> getAllWingWidths(ref string errMsg)
        //{
        //    try
        //    {
        //        RestClient restClient = new RestClient();
        //        initRestClient(restClient);
        //        RestRequest request = new RestRequest();
        //        string fields = "TRSH_WING_WIDTH,FROM_WIDTH,TO_WIDTH,AUTO_WINDOW_WIDTH";
        //        request.Resource = string.Format("TRSH_WING_WIDTH?$select={0}", fields);
        //        IRestResponse response = restClient.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var settings = new JsonSerializerSettings
        //            {
        //                NullValueHandling = NullValueHandling.Include,
        //                MissingMemberHandling = MissingMemberHandling.Ignore
        //            };
        //            ValuesWingWidth_Class val = JsonConvert.DeserializeObject<ValuesWingWidth_Class>(response.Content);

        //            List<WingWidth_Class> val1 = new List<WingWidth_Class>();  //val.value;
        //            //WingWidth_Class emptyWWidth = new WingWidth_Class();
        //            //emptyWWidth.TRSH_WINGSNUM = 0;
        //            //val1.Add(emptyWnum);
        //            foreach (WingWidth_Class ww in val.value)
        //            {
        //                val1.Add(ww);
        //            }
        //            return val1;
        //        }
        //        else
        //        {
        //            if (response.StatusDescription.ToLower() == "not found")
        //            {
        //                errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
        //                myLogger.log.Error(errMsg);
        //                return null;
        //            }
        //            errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
        //        throw ex;
        //    }
        //}
        public static List<Decoration_Class> getAllDecorations(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_DECORATION,DECORATIONSIDE";
                request.Resource = string.Format("TRSH_DECORATION?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDecoration_Class val = JsonConvert.DeserializeObject<ValuesDecoration_Class>(response.Content);

                    List<Decoration_Class> val1 = new List<Decoration_Class>();  //val.value;
                    Decoration_Class emptyDecor = new Decoration_Class();
                    emptyDecor.TRSH_DECORATION = 0;
                    val1.Add(emptyDecor);
                    foreach (Decoration_Class decor in val.value)
                    {
                        val1.Add(decor);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<PART_Class> getAllParts(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string filter = "TYPE eq 'P' and TRSH_MODEL ne 0 and TRSH_WINGSNUM ne 0";
                //string fields = "PARTNAME,PARTDES,MPARTNAME,MPARTDES,FAMILYDES";
                string fields = "PARTNAME,PARTDES,TRSH_MODEL,TRSH_MODELDES,TRSH_WINGSNUM,TRSH_WINGSNUMDES";
                request.Resource = string.Format("LOGPART?$filter={0}&$select={1}", filter, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    PART_Class emptyPart = new PART_Class();
                    emptyPart.PARTNAME = " ";
                    val1.Add(emptyPart);
                    foreach (PART_Class part in val.value)
                    {
                        val1.Add(part);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static FamilyOfPart_Class getFamilyOfPart(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PARTNAME,FAMILYNAME,FAMILYDES";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFamilyOfPart_Class val = JsonConvert.DeserializeObject<ValuesFamilyOfPart_Class>(response.Content);
                    //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    FamilyOfPart_Class emptyFamily = new FamilyOfPart_Class();
                    emptyFamily.FAMILYNAME = "";
                    emptyFamily.FAMILYDES = "הפריט לא שוייך למשפחה";
                    if (val == null || val.value == null)
                        return emptyFamily;
                    else if (val != null && val.value != null && val.value.Count == 0)
                    {
                        myLogger.log.Error(string.Format("PARTNAME {0} has no FAMILY", PARTNAME));
                        return emptyFamily;
                    }
                    return val.value[0];
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                return null;
            }
        }

        //  API query example: https://prio.mishol-it.com/odata/Priority/tabula.ini/demo/FAMILY_LOG?$select=FAMILYNAME,FAMILYDESC
        public static List<FAMILY_Class> getFamilies(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FAMILY,FAMILYNAME,FAMILYDESC";
                request.Resource = string.Format("FAMILY_LOG?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFAMILY_Class val = JsonConvert.DeserializeObject<ValuesFAMILY_Class>(response.Content);
                    List<FAMILY_Class> val1 = new List<FAMILY_Class>();  //val.value;
                    FAMILY_Class emptyFam = new FAMILY_Class();
                    emptyFam.FAMILYNAME = " ";
                    val1.Add(emptyFam);
                    foreach (FAMILY_Class fam in val.value)
                    {
                        val1.Add(fam);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static int getFAMILY(string FAMILYNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FAMILY,FAMILYNAME,FAMILYDESC";
                request.Resource = string.Format("FAMILY_LOG?$filter=FAMILYNAME eq '{0}'&$select={1}", FAMILYNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFAMILY_Class val = JsonConvert.DeserializeObject<ValuesFAMILY_Class>(response.Content);
                    return val.value[0].FAMILY;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return -1;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }


        public static List<PART_Class> getFamilyParts(int family, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PART,PARTNAME,PARTDES";
                request.Resource = string.Format("FAMILY_LOG?$filter=FAMILY eq {0}&$select=FAMILYNAME&$expand=FAMILY_LOGPART_SUBFORM($select={1};$orderby=PARTNAME)"
                    , family, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    string jsonValue = response.Content;
                    FamilyParts_Class val = JsonConvert.DeserializeObject<FamilyParts_Class>(response.Content);
                    List<PART_Class> result = (List<PART_Class>)val.value[0].FAMILY_LOGPART_SUBFORM;
                    PART_Class emptyPart = new PART_Class();
                    emptyPart.PARTNAME = " ";
                    result.Insert(0, emptyPart);

                    return result;

                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }


        //public static int getDoorHwCatCode(string PARTNAME, ref string errMsg)
        //{
        //    try
        //    {
        //        RestClient restClient = new RestClient();
        //        initRestClient(restClient);
        //        RestRequest request = new RestRequest();
        //        string fields = "PARTNAME,TRSH_DOOR_HWCATCODE";
        //        request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
        //        IRestResponse response = restClient.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var settings = new JsonSerializerSettings
        //            {
        //                NullValueHandling = NullValueHandling.Include,
        //                MissingMemberHandling = MissingMemberHandling.Ignore
        //            };
        //            ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
        //            if (val == null || val.value == null)
        //                return 0;
        //            else if (val != null && val.value != null && val.value.Count == 0)
        //            {
        //                myLogger.log.Error(string.Format("PARTNAME {0} has no TRSH_DOOR_HWCATCODE", PARTNAME));
        //                return 0;
        //            }
        //            return val.value[0].TRSH_DOOR_HWCATCODE;
        //        }
        //        else
        //        {
        //            if (response.StatusDescription.ToLower() == "not found")
        //            {
        //                errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
        //                myLogger.log.Error(errMsg);
        //                return 0;
        //            }
        //            errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
        //        return 0;
        //    }
        //}

        #region get tables data from priority
        public static void InitInMemoryDataSources(ref string errMsg)
        {
            try
            {
                dtMeagedFields = getAllMeagedFields(ref errMsg);
                if (dtMeagedFields == null)
                    return;  // abort 
                dtDecorSideFlds = getDecorSideFlds(ref errMsg);
                if (dtDecorSideFlds == null)
                    return;  //abort
                dtConfFields = getConfFields(ref errMsg);
                if (dtConfFields == null)
                    return;
                dtDefaults = getDefaults(ref errMsg);

                lstWingsNum = getAllWingsNum(ref errMsg);
                dtWingsNum = lstWingsNum.ToDataTable<WingsNum_Class>();
                lstModels = getAllModels(ref errMsg);
                dtModels = lstModels.ToDataTable<Model_Class>();
                lstModel_Parts = getModelParts(ref errMsg);
                dtModel_Parts = lstModel_Parts.ToDataTable<Model_Part_Class>();
                lstDecorations = getAllDecorations(ref errMsg);
                dtDecorations = lstDecorations.ToDataTable<Decoration_Class>();
                dtComplients = getComplients(ref errMsg);
                //lstWingWidth = getAllWingWidths(ref errMsg);
                //dtWingWidth = lstWingWidth.ToDataTable<WingWidth_Class>();

                lstParts = getAllParts(ref errMsg);
                dtParts = lstParts.ToDataTable<PART_Class>();
                lstColors = getColors(ref errMsg);
                lstGlassColors4Diamond = getGlassColors4Diamond(ref errMsg);
                lstLock_Hinge_Dril_Heights = getLockHingeDrilHeights(ref errMsg);
                dtLock_Hinge_Dril_Heights = lstLock_Hinge_Dril_Heights.ToDataTable<TRSH_LOCKHINGE_DRILH_Class>();
                lstHardwares = getHardwares(ref errMsg);
                dtHardwares = lstHardwares.ToDataTable<TRSH_HARDWARE_Class>();
                lstHwAccessories = getHwAccessories(ref errMsg);
                lstDril4Hw = getDril4Hws(ref errMsg);
                dtDril4Hws = lstDril4Hw.ToDataTable<DRIL4HW_Class>();
                lstCylinders = getCylinders(ref errMsg);
                dtCylinders = lstCylinders.ToDataTable<CYLINDER_Class>();
                lstCYLHWs = getCYLHWs(ref errMsg);
                lstLocks = getLocks(ref errMsg);
                dtLocks = lstLocks.ToDataTable<TRSH_LOCK_Class>();

                lstWindowWidths = getWindowWidths(ref errMsg);
                dtWindowWidths = lstWindowWidths.ToDataTable<WINDOWWIDTH_Class>();
                lstWWidth_Statics = getWWidth_Statics(ref errMsg);
                dtWWidth_Statics = lstWWidth_Statics.ToDataTable<WWIDTH_STATIC_Class>();

                lstWindowHeights = getWindowHeights(ref errMsg);
                dtWindowHeights = lstWindowHeights.ToDataTable<WINDOWHEIGHT_Class>();
                lstVitrages4Diamond = getVitrages4Diamond(ref errMsg);
                lstVitrages = getVitrages(ref errMsg);
                lstTileTypes = getTileTypes(ref errMsg);
                lstRaw4CPlates = getRaw4CPlates(ref errMsg);
                lstProfiles4Windows = getProfiles4Windows(ref errMsg);
                lstHandles = getHandles(ref errMsg);
                dtHandles = lstHandles.ToDataTable<HANDLE_Class>();
                lstHandles4Diamond = getHandles4Diamond(ref errMsg);
                lstGrid4HT1084 = getGrid4HT1084(ref errMsg);
                lstGrids = getGrids(ref errMsg);
                lstGlasses4Windows = getGlasses4Windows(ref errMsg);
                //UiLogic.initTabNames();  -> done in TabContro.AddPage() 
            }
            catch (Exception ex)
            {
                string errMsg2 = string.Format("Unexpected error: {0} check Whether all Priority Forms atr Prepared/Ready", ex.Message);
                myLogger.log.Error(errMsg2);
                throw new Exception(errMsg2);
            }
        }

        //public static List<string> getColorsNum()
        //{
        //    return new List<string> { string.Empty, "מגולוון", "1", "2" };
        //}
        public static List<TRSH_COLOR_Class> getColors(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_COLORID,PARTNAME,PARTDES";
                request.Resource = string.Format("TRSH_COLORS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_COLOR_Class val = JsonConvert.DeserializeObject<ValuesTRSH_COLOR_Class>(response.Content);
                    List<TRSH_COLOR_Class> val1 = new List<TRSH_COLOR_Class>();  //val.value;
                    TRSH_COLOR_Class emptyColor = new TRSH_COLOR_Class();
                    emptyColor.PARTNAME = " ";
                    emptyColor.PARTDES = " ";
                    val1.Add(emptyColor);
                    foreach (TRSH_COLOR_Class clr in val.value)
                    {
                        val1.Add(clr);
                        if (clr.PARTDES.Contains("מגולוון"))
                            MEGULVAN_ID = clr.TRSH_COLORID;
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static string getColorDes(int ColorId)
        {
            foreach (TRSH_COLOR_Class c in lstColors)
            {
                if (c.TRSH_COLORID == ColorId)
                    return c.PARTDES;
            }
            return String.Empty;
        }
        public static TRSH_COLOR_Class getColorById (int ColorId )
        {
            foreach (TRSH_COLOR_Class c in lstColors)
            {
                if (c.TRSH_COLORID == ColorId)
                    return c;
            }
            return null;
        }
        public static List<TRSH_COLOR_Class> getGlassColors4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_COLORID,PARTNAME,PARTDES";
                request.Resource = string.Format("TRSH_GLASSCLRS4DMND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_COLOR_Class val = JsonConvert.DeserializeObject<ValuesTRSH_COLOR_Class>(response.Content);
                    List<TRSH_COLOR_Class> val1 = new List<TRSH_COLOR_Class>();  //val.value;
                    TRSH_COLOR_Class emptyColor = new TRSH_COLOR_Class();
                    emptyColor.PARTNAME = " ";
                    emptyColor.PARTDES = " ";
                    val1.Add(emptyColor);
                    foreach (TRSH_COLOR_Class clr in val.value)
                    {
                        val1.Add(clr);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<TRSH_LOCKHINGE_DRILH_Class> getLockHingeDrilHeights(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_LOCKHINGE_DRILH,TRSH_DOOR_HWCATCODE,DOORHEIGHTMIN,DOORHEIGHTMAX,MEASURENAME,LOCKDRILHEIGHT,BACKPINHEIGHT,HINGESNUM,HINGE1HEIGHT,"
                                    + "HINGE2HEIGHT,HINGE3HEIGHT,HINGE4HEIGHT,HINGE5HEIGHT";
                request.Resource = string.Format("TRSH_LOCKHINGE_DRILH?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_LOCKHINGE_DRILH_Class val = JsonConvert.DeserializeObject<ValuesTRSH_LOCKHINGE_DRILH_Class>(response.Content);
                    List<TRSH_LOCKHINGE_DRILH_Class> val1 = new List<TRSH_LOCKHINGE_DRILH_Class>();  //val.value;
                    TRSH_LOCKHINGE_DRILH_Class emptyLockHingeDrilH = new TRSH_LOCKHINGE_DRILH_Class();
                    emptyLockHingeDrilH.TRSH_DOOR_HWCATCODE = 0;
                    val1.Add(emptyLockHingeDrilH);
                    foreach (TRSH_LOCKHINGE_DRILH_Class lhd in val.value)
                    {
                        val1.Add(lhd);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<TRSH_LOCKHINGE_DRILH_Class> getLockHingeDrilHeights1(DoorConfig doorConfig)
        {
            List<TRSH_LOCKHINGE_DRILH_Class> lstLockDH1 = new List<TRSH_LOCKHINGE_DRILH_Class>();
            foreach (TRSH_LOCKHINGE_DRILH_Class LockDH in lstLock_Hinge_Dril_Heights)
            {
                if (LockDH.TRSH_DOOR_HWCATCODE == doorConfig.TRSH_DOOR_HWCATCODE)
                {
                    lstLockDH1.Add(LockDH);
                }
            }
            List<TRSH_LOCKHINGE_DRILH_Class> SortedList = lstLockDH1.OrderBy(o => o.LOCKDRILHEIGHT).ToList();
            return SortedList;
        }

        //getHardwares
        public static List<TRSH_HARDWARE_Class> getHardwares(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_HARDWARE,TRSH_DOOR_HWCATCODE,PARTNAME,PARTDES,OPENSIDE,DRIL4HW,DRIL4HWDES,COLORED,NIKEL,BRONZE,PARTNAME2,OPPOSITESIDE_PART";
                request.Resource = string.Format("TRSH_HARDWARE?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_HARDWARE_Class val = JsonConvert.DeserializeObject<ValuesTRSH_HARDWARE_Class>(response.Content);
                    List<TRSH_HARDWARE_Class> val1 = new List<TRSH_HARDWARE_Class>();  //val.value;
                    TRSH_HARDWARE_Class emptyHw = new TRSH_HARDWARE_Class();
                    emptyHw.TRSH_HARDWARE = 0;
                    emptyHw.PARTNAME = " ";
                    val1.Add(emptyHw);

                    foreach (TRSH_HARDWARE_Class hw in val.value)
                    {
                        val1.Add(hw);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<HWACCESSORY_Class> getHwAccessories(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "HWACCESSORYID,PARTNAME,PARTDES,COLORED,NIKEL,BRONZE"; //"HWACCESSORYID,HWACCESSORYDES,COLORED";
                request.Resource = string.Format("TRSH_HWACCESSORIES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesHWACCESSORY_Class val = JsonConvert.DeserializeObject<ValuesHWACCESSORY_Class>(response.Content);
                    List<HWACCESSORY_Class> val1 = new List<HWACCESSORY_Class>();  //val.value;
                    HWACCESSORY_Class emptyHwa = new HWACCESSORY_Class();
                    emptyHwa.HWACCESSORYID = 0;
                    //emptyHwa.HWACCESSORYDES = " ";
                    emptyHwa.PARTDES = " ";
                    val1.Add(emptyHwa);
                    HWACCESSORY_Class noHwa = new HWACCESSORY_Class();
                    noHwa.HWACCESSORYID = UiLogic.IdOfNone;
                    noHwa.PARTDES = "ללא";
                    val1.Add(noHwa);

                    foreach  (HWACCESSORY_Class hwa in val.value)
                    {
                        val1.Add(hwa);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static HWACCESSORY_Class getHwAcc(int HwAccId)
        {
            HWACCESSORY_Class res = lstHwAccessories.Find(item => item.HWACCESSORYID == HwAccId);
            return res;
        }
        public static string getHwAccDes (int HwAccId)
        {
            HWACCESSORY_Class hwAcc = getHwAcc(HwAccId);
            if (hwAcc != null)
                return hwAcc.PARTDES;
            else
                return String.Empty;
        }
        public static int getHwAccIdbyDes (string HwAccDes)
        {
            HWACCESSORY_Class hwAcc = lstHwAccessories.Find(item => item.PARTDES == HwAccDes);
            if (hwAcc != null)
                return hwAcc.HWACCESSORYID;
            else
                return 0;
        }
        public static bool isHwOpenSideOK(int TRSH_HARDWARE, string OPENSIDE)
        {
            DataRow[] rowsArray;
            string query = string.Format("TRSH_HARDWARE = '{0}'", TRSH_HARDWARE);
            rowsArray = PrApiCalls.dtHardwares.Select(query);
            if (rowsArray.Length > 0)
            {
                string hwOpenSide = rowsArray[0]["OPENSIDE"].ToString();
                return (hwOpenSide == OPENSIDE || string.IsNullOrEmpty(hwOpenSide));
            }
            return false;  // error - no Hw found for this TRSH_HARDWARE 
        }
        public static bool isHWColored(int TRSH_HARDWARE)
        {
            DataRow[] rowsArray;
            string query = string.Format("TRSH_HARDWARE = '{0}'", TRSH_HARDWARE);
            rowsArray = PrApiCalls.dtHardwares.Select(query);
            if (rowsArray.Length > 0)
            {
                string COLORED = rowsArray[0]["COLORED"].ToString();
                if (!string.IsNullOrEmpty(COLORED) && COLORED == "Y")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static bool isHWNikel(int TRSH_HARDWARE)
        {
            DataRow[] rowsArray;
            string query = string.Format("TRSH_HARDWARE = '{0}'", TRSH_HARDWARE);
            rowsArray = PrApiCalls.dtHardwares.Select(query);
            if (rowsArray.Length > 0)
            {
                string NIKEL = rowsArray[0]["NIKEL"].ToString();
                if (!string.IsNullOrEmpty(NIKEL) && NIKEL == "Y")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static bool isHWBronze(int TRSH_HARDWARE)
        {
            DataRow[] rowsArray;
            string query = string.Format("TRSH_HARDWARE = '{0}'", TRSH_HARDWARE);
            rowsArray = PrApiCalls.dtHardwares.Select(query);
            if (rowsArray.Length > 0)
            {
                string BRONZE = rowsArray[0]["BRONZE"].ToString();
                if (!string.IsNullOrEmpty(BRONZE) && BRONZE == "Y")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static TRSH_HARDWARE_Class getHardware1(int TRSH_HARDWARE)
        {
            TRSH_HARDWARE_Class res = lstHardwares.Find(item => item.TRSH_HARDWARE == TRSH_HARDWARE);
            return res;
        }
        public static TRSH_HARDWARE_Class getHardwareByPARTNAME(string PARTNAME)
        {
            TRSH_HARDWARE_Class res = lstHardwares.Find(item => item.PARTNAME == PARTNAME);
            return res;
        }
        public static TRSH_HARDWARE_Class getOpositeSideHw (int TRSH_HARDWARE)
        {
            TRSH_HARDWARE_Class Hw = getHardware1(TRSH_HARDWARE);
            string OpositeHwPARTNAME = Hw.PARTNAME2;
            TRSH_HARDWARE_Class res = getHardwareByPARTNAME(OpositeHwPARTNAME);
            return res;
        }
        public static string getHwDes (int TRSH_HARDWARE)
        {
            TRSH_HARDWARE_Class hw1 = getHardware1(TRSH_HARDWARE);  //lstHardwares.Find(item => item.TRSH_HARDWARE == TRSH_HARDWARE);
            if (hw1 != null)
                return hw1.PARTDES;
            else
                return string.Empty;
        }
        public static int getHwIdByDes(string HwDes)
        {
            TRSH_HARDWARE_Class hw1 = lstHardwares.Find(item => item.PARTDES == HwDes);
            if (hw1 != null)
                return hw1.TRSH_HARDWARE;
            else
                return 0;
        }
        public static int getOpposite_TRSH_HARDWARE(int TRSH_HARDWARE)
        {
            TRSH_HARDWARE_Class hw1 = lstHardwares.Find(item => item.TRSH_HARDWARE == TRSH_HARDWARE);
            if (hw1.PARTNAME == hw1.PARTNAME2)  //it point to itself 
            {
                return TRSH_HARDWARE;
            }
            else
            {
                TRSH_HARDWARE_Class hw2 = lstHardwares.Find(item => item.PARTNAME == hw1.PARTNAME2);
                if (hw2 != null)
                    return hw2.TRSH_HARDWARE;
                else
                    return 0;
            }
        }
        public static List<TRSH_HARDWARE_Class> getDoorHWs(int TRSH_DOOR_HWCATCODE, ref string errMsg)
        {
            try
            {
                DataRow[] rowsArray;
                string query = string.Format("TRSH_DOOR_HWCATCODE = '{0}'", TRSH_DOOR_HWCATCODE);
                rowsArray = PrApiCalls.dtHardwares.Select(query);
                if (rowsArray.Length > 0)
                {
                    List<TRSH_HARDWARE_Class> lstParHWs = new List<TRSH_HARDWARE_Class>();
                    TRSH_HARDWARE_Class emptyHW = new TRSH_HARDWARE_Class();
                    emptyHW.TRSH_HARDWARE = 0;
                    emptyHW.PARTNAME = " ";
                    lstParHWs.Add(emptyHW);

                    TRSH_HARDWARE_Class noHw = new TRSH_HARDWARE_Class();
                    noHw.TRSH_HARDWARE = UiLogic.IdOfNone;
                    noHw.PARTDES = "ללא";
                    lstParHWs.Add(noHw);

                    for (int r = 0; r < rowsArray.Length; r++)
                    {
                        TRSH_HARDWARE_Class Hw1 = new TRSH_HARDWARE_Class();
                        Hw1.TRSH_HARDWARE = int.Parse(rowsArray[r]["TRSH_HARDWARE"].ToString());
                        Hw1.PARTDES = rowsArray[r]["PARTDES"].ToString();  //rowsArray[r]["HARDWAREDES"].ToString();
                        lstParHWs.Add(Hw1);
                    }



                    return lstParHWs;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<DRIL4HW_Class> getDril4Hws(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "DRIL4HW,DRIL4HWDES";
                request.Resource = string.Format("TRSH_DRIL4HW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDRIL4HW_Class val = JsonConvert.DeserializeObject<ValuesDRIL4HW_Class>(response.Content);
                    List<DRIL4HW_Class> val1 = new List<DRIL4HW_Class>();  //val.value;
                    DRIL4HW_Class emptyDril4Hw = new DRIL4HW_Class();
                    emptyDril4Hw.DRIL4HWDES = " ";
                    val1.Add(emptyDril4Hw);
                    foreach (DRIL4HW_Class dril4Hw in val.value)
                    {
                        val1.Add(dril4Hw);
                        if (dril4Hw.DRIL4HWDES.Contains("IDS"))
                        {
                            if (dril4Hw.DRIL4HWDES.Contains("חד צדדי"))
                                IDS_ONESIDE = dril4Hw.DRIL4HW;
                            else
                                IDS_TWOSIDES = dril4Hw.DRIL4HW;
                        }
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<DRIL4HW_Class> getDril4Hw1(DoorConfig doorConfig, ref string errMsg)
        {
            try
            {
                List<DRIL4HW_Class> lstDril4Hw1 = new List<DRIL4HW_Class>();
                DRIL4HW_Class d1 = getDril4HwRec1(doorConfig);
                lstDril4Hw1.Add(d1);

                if (d1.DRIL4HWDES.Contains("IDS"))  //it's either IDS_ONESIDE or ID_TWOSIDES
                {
                    foreach (DRIL4HW_Class d in lstDril4Hw)
                    {
                        if (d.DRIL4HWDES.Contains("IDS") && d.DRIL4HW != d1.DRIL4HW)  // it's teh 2nd IDS drill
                        {
                            DRIL4HW_Class d2 = new DRIL4HW_Class();
                            d2.DRIL4HW = d.DRIL4HW;
                            d2.DRIL4HWDES = d.DRIL4HWDES;
                            lstDril4Hw1.Add(d2);   //now lstDril4Hw1 contains the two IDS drills !
                            break;
                        }
                    }
                }
                return lstDril4Hw1;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<DRIL4HW_Class> getDril4Hw1(int TRSH_HARDWARE, ref string errMsg)
        {
            try
            {
                List<DRIL4HW_Class> lstDril4Hw1 = new List<DRIL4HW_Class>();
                DRIL4HW_Class d1 = getDril4HwRec1(TRSH_HARDWARE);
                lstDril4Hw1.Add(d1);

                if (d1.DRIL4HWDES.Contains("IDS"))
                {
                    foreach (DRIL4HW_Class d in lstDril4Hw)
                    {
                        if (d.DRIL4HWDES.Contains("IDS") && d.DRIL4HW != d1.DRIL4HW)
                        {
                            DRIL4HW_Class d2 = new DRIL4HW_Class();
                            d2.DRIL4HW = d.DRIL4HW;
                            d2.DRIL4HWDES = d.DRIL4HWDES;
                            lstDril4Hw1.Add(d2);
                            break;
                        }
                    }
                }
                return lstDril4Hw1;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static int getDril4HwOfHw(DoorConfig doorConfig)
        {
            // note: per Eli's request: in  thw table TRSH_HARDWARE :  if the TRSH_HARDWARE.DRIL4HW should be IDS it's always IDS_ONESIDE !
            if (doorConfig != null)
            {
                string query = string.Format("TRSH_HARDWARE={0}", doorConfig.TRSH_HARDWARE);
                DataRow[] dril4HwRows = dtHardwares.Select(query);
                if (dril4HwRows.Length > 0)
                    return int.Parse(dril4HwRows[0]["DRIL4HW"].ToString());  //if it should return IDS, it always returns IDS_ONESIDE 
            }
            return 0;
        }
        public static int getDril4HwOfHw(int HwId)
        {
            // note: per Eli's request: in  thw table TRSH_HARDWARE :  if the TRSH_HARDWARE.DRIL4HW should be IDS it's always IDS_ONESIDE !
                string query = string.Format("TRSH_HARDWARE={0}", HwId);
                DataRow[] dril4HwRows = dtHardwares.Select(query);
                if (dril4HwRows.Length > 0)
                    return int.Parse(dril4HwRows[0]["DRIL4HW"].ToString());  //if it should return IDS, it always returns IDS_ONESIDE 
                else
                    return 0;
        }
        public static DRIL4HW_Class getDril4HwRec1(DoorConfig doorConfig)
        {
            if (doorConfig != null)
            {
                DRIL4HW_Class rec1 = new DRIL4HW_Class();
                string query = string.Format("TRSH_HARDWARE={0}", doorConfig.TRSH_HARDWARE);
                DataRow[] dril4HwRows = dtHardwares.Select(query);
                if (dril4HwRows.Length > 0)
                {
                    rec1.DRIL4HW = int.Parse(dril4HwRows[0]["DRIL4HW"].ToString());
                    rec1.DRIL4HWDES = dril4HwRows[0]["DRIL4HWDES"].ToString();
                    return rec1;
                }
            }
            return null;
        }
        public static DRIL4HW_Class getDril4HwRec1(int TRSH_HARDWARE)
        {
            DRIL4HW_Class rec1 = new DRIL4HW_Class();
            string query = string.Format("TRSH_HARDWARE={0}", TRSH_HARDWARE);
            DataRow[] dril4HwRows = dtHardwares.Select(query);
            if (dril4HwRows.Length > 0)
            {
                rec1.DRIL4HW = int.Parse(dril4HwRows[0]["DRIL4HW"].ToString());
                rec1.DRIL4HWDES = dril4HwRows[0]["DRIL4HWDES"].ToString();
                return rec1;
            }

            return null;
        }
        public static DRIL4HW_Class getDril4HalfCylinder()
        {
            foreach (DRIL4HW_Class dril in lstDril4Hw)
            {
                if (dril.DRIL4HWDES.Contains("חצי צילינדר"))
                    return dril;
            }
            return null;
        }
        public static List<CYLINDER_Class> getCylinders(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_CYLINDER,PARTNAME,PARTDES,TRSH_MODELNAME,OPENMODE,ISHALFCYLINDER";
                request.Resource = string.Format("TRSH_CYLINDERS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCYLINDER_Class val = JsonConvert.DeserializeObject<ValuesCYLINDER_Class>(response.Content);
                    List<CYLINDER_Class> val1 = new List<CYLINDER_Class>();  //val.value;
                    CYLINDER_Class emptyCylinder = new CYLINDER_Class();
                    emptyCylinder.PARTNAME = " ";
                    emptyCylinder.PARTDES = " ";
                    val1.Add(emptyCylinder);

                    CYLINDER_Class noCylinder = new CYLINDER_Class();
                    noCylinder.PARTNAME = UiLogic.NameOfNone;//"9999999";
                    noCylinder.PARTDES = "ללא";
                    val1.Add(noCylinder);
                    foreach (CYLINDER_Class cyl in val.value)
                    {
                        if (cyl.OPENMODE == null || string.IsNullOrEmpty(cyl.OPENMODE))
                            cyl.OPENMODE = "2";   // IN and OUT 
                        val1.Add(cyl);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<CYLINDER_Class> getCylindersByModelOpenMode(DoorConfig doorConfig, ref string errMsg, bool incHalfCyl = false)
        {
            try
            {
                //string query = string.Format("TRSH_MODELNAME='{0}' AND (OPENMODE = '2' OR OPENMODE = '{1}')", doorConfig.TRSH_MODELNAME, doorConfig.OPENMODE );
                //DataRow[] cylRows = dtCylinders.Select(query);
                //if (cylRows != null && cylRows.Length > 0)
                //{
                //    //DataTable res = new DataTable();
                //    for (int r = 0; r < cylRows.Length; r++)
                //    {
                //        res.ImportRow(cylRows[r]);
                //    }
                //    lstRes = Helper.ConvertDataTable<CYLINDER_Class>(res);
                List<CYLINDER_Class> lstRes = new List<CYLINDER_Class>();

                CYLINDER_Class emptyCyl = new CYLINDER_Class();
                emptyCyl.PARTNAME = string.Empty;
                emptyCyl.PARTDES = string.Empty;
                lstRes.Add(emptyCyl);
                if (doorConfig.TRSH_MODELNAME != "MLI")  // added on 27/07/2022 per Eli's request 
                {
                    CYLINDER_Class noCylinder = new CYLINDER_Class();
                    noCylinder.PARTNAME = UiLogic.NameOfNone;//"9999999";
                    noCylinder.PARTDES = "ללא";
                    lstRes.Add(noCylinder);
                }
                foreach (CYLINDER_Class cyl in lstCylinders)
                {
                    if (cyl.TRSH_MODELNAME == doorConfig.TRSH_MODELNAME
                        && (cyl.OPENMODE == doorConfig.OPENMODE || cyl.OPENMODE == "2"))
                    {
                        if (incHalfCyl)
                        {
                            lstRes.Add(cyl);
                        }
                        else
                        {
                            if (cyl.ISHALFCYLINDER != "Y")
                                lstRes.Add(cyl);
                        }
                    }
                }
                return lstRes;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static bool cylinderIsHalf(int TRSH_CYLINDER)
        {
            foreach (CYLINDER_Class cyl in lstCylinders)
            {
                if (cyl.TRSH_CYLINDER == TRSH_CYLINDER)
                    return (cyl.ISHALFCYLINDER == "Y");
            }
            return false;   // error - actually this can't happen because the uer selected a Cylinder from lstCylinders !
        }

        public static List<CYLHW_Class> getCYLHWs(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_CYLHW,WING_OPENMODE,PARTDESCYL,OPENSIDE,PARTDESHW,TRSH_CYLINDER,PARTCYL,TRSH_HARDWARE,PARTHW";
                request.Resource = string.Format("TRSH_CYLHW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCYLHW_Class val = JsonConvert.DeserializeObject<ValuesCYLHW_Class>(response.Content);
                    List<CYLHW_Class> val1 = new List<CYLHW_Class>();  //val.value;
                    
                    foreach (CYLHW_Class cyl in val.value)
                    {
                        val1.Add(cyl);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<CYLHW_Class> getCYLHWs1(int TRSH_CYLINDER, string OPENSIDE, string OPENMODE, ref string errMsg)
        {
            try
            {
                if (TRSH_CYLINDER == 0)
                    return null;

                List<CYLHW_Class> res = new List<CYLHW_Class>();
                CYLHW_Class emptyCHW = new CYLHW_Class();
                emptyCHW.TRSH_CYLINDER = 0;
                emptyCHW.TRSH_HARDWARE = 0;
                emptyCHW.PARTDESHW = String.Empty;
                res.Add(emptyCHW);

                CYLHW_Class noCHW = new CYLHW_Class();
                noCHW.TRSH_CYLINDER = 0;
                noCHW.TRSH_HARDWARE = UiLogic.IdOfNone;
                noCHW.PARTDESHW = "ללא";
                res.Add(noCHW);

                if (lstCYLHWs != null)
                {
                    foreach (CYLHW_Class chw in lstCYLHWs)
                    {

                        if (chw.TRSH_CYLINDER == TRSH_CYLINDER && chw.OPENSIDE == OPENSIDE && chw.WING_OPENMODE == OPENMODE)
                            res.Add(chw);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
            //getLocks" 
            public static List<TRSH_LOCK_Class> getLocks(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_LOCK,TRSH_DOOR_HWCATCODE,PARTNAME,PARTDES,TRSH_ELIDOOR_LOGO";
                request.Resource = string.Format("TRSH_LOCKS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_LOCK_Class val = JsonConvert.DeserializeObject<ValuesTRSH_LOCK_Class>(response.Content);
                    List<TRSH_LOCK_Class> val1 = new List<TRSH_LOCK_Class>();  //val.value;
                    TRSH_LOCK_Class emptyLock = new TRSH_LOCK_Class();
                    emptyLock.PARTDES = " ";
                    emptyLock.PARTNAME = " ";
                    val1.Add(emptyLock);
                    foreach (TRSH_LOCK_Class lck in val.value)
                    {
                        val1.Add(lck);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<TRSH_LOCK_Class> getPartLocks(int TRSH_DOOR_HWCATCODE, ref string errMsg)
        {
            try
            {
                DataRow[] rowsArray;
                string query = string.Format("TRSH_DOOR_HWCATCODE = '{0}'", TRSH_DOOR_HWCATCODE);
                if (dtLocks == null)
                {
                    errMsg = "dtLocks is null, maybe caused by page refresh, recreating it";
                    myLogger.log.Error(errMsg);
                    lstLocks = getLocks(ref errMsg);
                    dtLocks = lstLocks.ToDataTable<TRSH_LOCK_Class>();

                }
                rowsArray = PrApiCalls.dtLocks.Select(query);
                if (rowsArray.Length > 0)
                {
                    List<TRSH_LOCK_Class> lstPartLocks = new List<TRSH_LOCK_Class>();
                    TRSH_LOCK_Class emptyLock = new TRSH_LOCK_Class();
                    emptyLock.PARTNAME = " ";
                    emptyLock.PARTDES = " ";
                    lstPartLocks.Add(emptyLock);
                    for (int r = 0; r < rowsArray.Length; r++)
                    {
                        TRSH_LOCK_Class lock1 = new TRSH_LOCK_Class();
                        lock1.TRSH_LOCK = int.Parse(rowsArray[r]["TRSH_LOCK"].ToString());
                        lock1.PARTNAME = rowsArray[r]["PARTNAME"].ToString();
                        lock1.PARTDES = rowsArray[r]["PARTDES"].ToString();
                        lstPartLocks.Add(lock1);
                    }
                    return lstPartLocks;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<TRSH_LOCK_Class> getPartAndLogoLocks(int TRSH_DOOR_HWCATCODE, string UseElidoorLogo, ref string errMsg)
        {
            try
            {
                DataRow[] rowsArray;
                //string LogoCond = (UseElidoorLogo == "Y" ? "=" : "<>");
                string query = string.Format ("TRSH_DOOR_HWCATCODE = '{0}'", TRSH_DOOR_HWCATCODE);
                if (dtLocks == null)
                {
                    errMsg = "dtLocks is null, maybe caused by page refresh, recreating it";
                    myLogger.log.Error(errMsg);
                    lstLocks = getLocks(ref errMsg);
                    dtLocks = lstLocks.ToDataTable<TRSH_LOCK_Class>();

                }
                rowsArray = PrApiCalls.dtLocks.Select(query);
                if (rowsArray.Length > 0)
                {
                    List<TRSH_LOCK_Class> lstPartLocks = new List<TRSH_LOCK_Class>();
                    TRSH_LOCK_Class emptyLock = new TRSH_LOCK_Class();
                    emptyLock.PARTNAME = " ";
                    emptyLock.PARTDES = " ";
                    lstPartLocks.Add(emptyLock);
                    for (int r = 0; r < rowsArray.Length; r++)
                    {
                        if (UseElidoorLogo == "Y")
                        {
                           if (rowsArray[r]["TRSH_ELIDOOR_LOGO"].ToString() == "Y")
                           {
                                TRSH_LOCK_Class lock1 = new TRSH_LOCK_Class();
                                lock1.TRSH_LOCK = int.Parse(rowsArray[r]["TRSH_LOCK"].ToString());
                                lock1.PARTNAME = rowsArray[r]["PARTNAME"].ToString();
                                lock1.PARTDES = rowsArray[r]["PARTDES"].ToString();
                                lstPartLocks.Add(lock1);
                           }
                        }
                        else
                        {
                            if (rowsArray[r]["TRSH_ELIDOOR_LOGO"].ToString() != "Y")
                            {
                                TRSH_LOCK_Class lock1 = new TRSH_LOCK_Class();
                                lock1.TRSH_LOCK = int.Parse(rowsArray[r]["TRSH_LOCK"].ToString());
                                lock1.PARTNAME = rowsArray[r]["PARTNAME"].ToString();
                                lock1.PARTDES = rowsArray[r]["PARTDES"].ToString();
                                lstPartLocks.Add(lock1);
                            }
                        }
                    }
                    return lstPartLocks;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_TILETYPES        -  70
        public static List<TILETYPE_Class> getTileTypes(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PART,PARTNAME,PARTDES";
                request.Resource = string.Format("TRSH_TILETYPES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTILETYPE_Class val = JsonConvert.DeserializeObject<ValuesTILETYPE_Class>(response.Content);
                    List<TILETYPE_Class> val1 = new List<TILETYPE_Class>();  //val.value;
                    TILETYPE_Class emptyTile = new TILETYPE_Class();
                    emptyTile.PARTNAME = " ";
                    emptyTile.PARTDES = " ";
                    val1.Add(emptyTile);
                    foreach (TILETYPE_Class tile in val.value)
                    {
                        val1.Add(tile);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_RAW4CPLATES      - 110  ? (/HT = /HighTech) 
        public static List<RAW4CPLATES_Class> getRaw4CPlates(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "RAW4CPLATES,RAW4CPLATESNAME";
                request.Resource = string.Format("TRSH_RAW4CPLATES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesRAW4CPLATES_Class val = JsonConvert.DeserializeObject<ValuesRAW4CPLATES_Class>(response.Content);
                    List<RAW4CPLATES_Class> val1 = new List<RAW4CPLATES_Class>();  //val.value;
                    RAW4CPLATES_Class emptyRaw4CP = new RAW4CPLATES_Class();
                    emptyRaw4CP.RAW4CPLATESNAME = " ";
                    val1.Add(emptyRaw4CP);
                    foreach (RAW4CPLATES_Class raw4cp in val.value)
                    {
                        val1.Add(raw4cp);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_WINDOWWIDTH      - 150
        public static List<WINDOWWIDTH_Class> getWindowWidths(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "WINDOWWIDTH_ID,TRSH_MODELNAME,TRSH_WINGSNUMDES,MIN_DOORWIDTH,MAX_DOORWIDTH,WINDOWWIDTH";
                request.Resource = string.Format("TRSH_WINDOWWIDTH?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWINDOWWIDTH_Class val = JsonConvert.DeserializeObject<ValuesWINDOWWIDTH_Class>(response.Content);
                    List<WINDOWWIDTH_Class> val1 = new List<WINDOWWIDTH_Class>();  //val.value;
                    WINDOWWIDTH_Class emptyV = new WINDOWWIDTH_Class();
                    //emptyV.TRSH_MODELNAME = " ";
                    //val1.Add(emptyV);
                    foreach (WINDOWWIDTH_Class wwidth in val.value)
                    {
                        val1.Add(wwidth);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<WWIDTH_STATIC_Class> getWWidth_Statics(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "WWIDTH_STATIC_ID,TRSH_MODELNAME,TRSH_WINGSNUMDES,MIN_WINGWIDTH,MAX_WINGWIDTH,WINDOWWIDTH,HASLOCK";
                request.Resource = string.Format("TRSH_WWIDTH_STATIC?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWWIDTH_STATIC_Class val = JsonConvert.DeserializeObject<ValuesWWIDTH_STATIC_Class>(response.Content);
                    List<WWIDTH_STATIC_Class> val1 = new List<WWIDTH_STATIC_Class>();  //val.value;
                    WWIDTH_STATIC_Class emptyV = new WWIDTH_STATIC_Class();
                    //emptyV.TRSH_MODELNAME = " ";
                    //val1.Add(emptyV);
                    foreach (WWIDTH_STATIC_Class wwidth in val.value)
                    {
                        val1.Add(wwidth);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_WINDOWHEIGHT     - 140
        public static List<WINDOWHEIGHT_Class> getWindowHeights(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "WINDOWHEIGHT_ID,TRSH_MODELNAME,MINDOORHEIGHT,MAXDOORHEIGHT,MEASURENAME,WINDOWHEIGHT,HEIGHT4DRIL";
                request.Resource = string.Format("TRSH_WINDOWHEIGHT?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWINDOWHEIGHT_Class val = JsonConvert.DeserializeObject<ValuesWINDOWHEIGHT_Class>(response.Content);
                    List<WINDOWHEIGHT_Class> val1 = new List<WINDOWHEIGHT_Class>();  //val.value;
                                                                                     // WINDOWHEIGHT_Class emptyV = new WINDOWHEIGHT_Class();
                                                                                     // emptyV.WINDOWHEIGHTNAME = " ";
                                                                                     // emptyV.VITRAGEDES = " ";
                                                                                     // val1.Add(emptyV);
                    foreach (WINDOWHEIGHT_Class wh in val.value)
                    {
                        val1.Add(wh);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_PROFILE4WINDOW   - 160
        public static List<PROFILE4WINDOW_Class> getProfiles4Windows(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PROFILE4WINDOWNAME,PROFILE4WINDOWDES";
                request.Resource = string.Format("TRSH_PROFILE4WINDOW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPROFILE4WINDOW_Class val = JsonConvert.DeserializeObject<ValuesPROFILE4WINDOW_Class>(response.Content);
                    List<PROFILE4WINDOW_Class> val1 = new List<PROFILE4WINDOW_Class>();  //val.value;
                    PROFILE4WINDOW_Class emptyP = new PROFILE4WINDOW_Class();
                    emptyP.PROFILE4WINDOWNAME = " ";
                    emptyP.PROFILE4WINDOWDES = " ";
                    val1.Add(emptyP);
                    foreach (PROFILE4WINDOW_Class p4w in val.value)
                    {
                        val1.Add(p4w);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_GLASS4WINDOW     - 170
        public static List<GLASS4WINDOW_Class> getGlasses4Windows(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GLASS4WINDOWID,GLASS4WINDOWDES,GLASS4WINDOWKITNAME";
                request.Resource = string.Format("TRSH_GLASS4WINDOW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGLASS4WINDOW_Class val = JsonConvert.DeserializeObject<ValuesGLASS4WINDOW_Class>(response.Content);
                    List<GLASS4WINDOW_Class> val1 = new List<GLASS4WINDOW_Class>();  //val.value;
                    GLASS4WINDOW_Class emptyG4w = new GLASS4WINDOW_Class();
                    emptyG4w.GLASS4WINDOWDES = " ";
                    emptyG4w.GLASS4WINDOWKITNAME = " ";
                    val1.Add(emptyG4w);
                    foreach (GLASS4WINDOW_Class g4w in val.value)
                    {
                        val1.Add(g4w);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_GRID             - 180
        public static List<GRID_Class> getGrids(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GRID_ID,GRIDNAME,GRIDDES";
                request.Resource = string.Format("TRSH_GRID?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGRID_Class val = JsonConvert.DeserializeObject<ValuesGRID_Class>(response.Content);
                    List<GRID_Class> val1 = new List<GRID_Class>();  //val.value;
                    GRID_Class emptyGrd = new GRID_Class();
                    emptyGrd.GRID_ID = 0;
                    emptyGrd.GRIDNAME = " ";
                    emptyGrd.GRIDDES = " ";
                    val1.Add(emptyGrd);
                    foreach (GRID_Class grd in val.value)
                    {
                        val1.Add(grd);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_VITRAGE          -  90
        public static List<VITRAGE_Class> getVitrages(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "VITRAGENAME,VITRAGEDES";
                request.Resource = string.Format("TRSH_VITRAGE?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesVITRAGE_Class val = JsonConvert.DeserializeObject<ValuesVITRAGE_Class>(response.Content);
                    List<VITRAGE_Class> val1 = new List<VITRAGE_Class>();  //val.value;
                    VITRAGE_Class emptyV = new VITRAGE_Class();
                    emptyV.VITRAGENAME = " ";
                    emptyV.VITRAGEDES = " ";
                    val1.Add(emptyV);
                    foreach (VITRAGE_Class vitr in val.value)
                    {
                        val1.Add(vitr);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_VITRAGE4DIAMOND  - 250
        public static List<VITRAGE4DIAMOND_Class> getVitrages4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "VITRAGE4DIAMONDNAME,VITRAGE4DIAMONDDES";
                request.Resource = string.Format("TRSH_VITRAGE4DIAMOND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesVITRAGE4DIAMOND_Class val = JsonConvert.DeserializeObject<ValuesVITRAGE4DIAMOND_Class>(response.Content);
                    List<VITRAGE4DIAMOND_Class> val1 = new List<VITRAGE4DIAMOND_Class>();  //val.value;
                    VITRAGE4DIAMOND_Class emptyV4D = new VITRAGE4DIAMOND_Class();
                    emptyV4D.VITRAGE4DIAMONDNAME = " ";
                    emptyV4D.VITRAGE4DIAMONDDES = " ";
                    val1.Add(emptyV4D);
                    foreach (VITRAGE4DIAMOND_Class v4d in val.value)
                    {
                        val1.Add(v4d);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_GRID4HT1084      - 370
        public static List<GRID4HT1084_Class> getGrid4HT1084(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GRID4HT1084NAME,GRID4HT1084DES";
                request.Resource = string.Format("TRSH_GRID4HT1084?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGRID4HT1084_Class val = JsonConvert.DeserializeObject<ValuesGRID4HT1084_Class>(response.Content);
                    List<GRID4HT1084_Class> val1 = new List<GRID4HT1084_Class>();  //val.value;
                    GRID4HT1084_Class empty1084 = new GRID4HT1084_Class();
                    empty1084.GRID4HT1084NAME = " ";
                    empty1084.GRID4HT1084DES = " ";
                    val1.Add(empty1084);
                    foreach (GRID4HT1084_Class g1084 in val.value)
                    {
                        val1.Add(g1084);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_HANDLES   - 350
        public static List<HANDLE_Class> getHandles(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PARTNAME,PARTDES,COLORED";
                request.Resource = string.Format("TRSH_HANDLES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesHANDLE_Class val = JsonConvert.DeserializeObject<ValuesHANDLE_Class>(response.Content);
                    List<HANDLE_Class> val1 = new List<HANDLE_Class>();  //val.value;
                    HANDLE_Class emptyHandle = new HANDLE_Class();
                    emptyHandle.PARTNAME = " ";
                    emptyHandle.PARTDES = " ";
                    val1.Add(emptyHandle);

                    HANDLE_Class noHandle = new HANDLE_Class();
                    noHandle.PARTNAME = UiLogic.NameOfNone;
                    noHandle.PARTDES = "ללא";  // new per CU request 23/05/2022
                    val1.Add(noHandle);

                    foreach (HANDLE_Class handle in val.value)
                    {
                        val1.Add(handle);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static bool isHandleColored(string HANDLENAME)
        {

            DataRow[] rowsArray;
            string query = string.Format("PARTNAME = '{0}'", HANDLENAME);
            rowsArray = PrApiCalls.dtHandles.Select(query);
            if (rowsArray.Length > 0)
            {
                string COLORED = rowsArray[0]["COLORED"].ToString();
                if (!string.IsNullOrEmpty(COLORED) && COLORED == "Y")
                    return true;
                else
                    return false;
            }
            else
            { 
                return false;
            }
        }
        public static string getHandleNameByDes (string handleDes)
        {
            HANDLE_Class h = lstHandles.Find(item => item.PARTDES == handleDes);
            return h.PARTNAME;
        }
        public static string getHandleDesByName(string handleName)
        {
            HANDLE_Class h = lstHandles.Find(item => item.PARTNAME == handleName);
            return h.PARTDES;
        }
        //TRSH_HANDLE4DIAMOND   - 360
        public static List<HANDLE4DIAMOND_Class> getHandles4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "HANDLE4DIAMONDNAME,HANDLE4DIAMONDDES";
                request.Resource = string.Format("TRSH_HANDLE4DIAMOND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesHANDLE4DIAMOND_Class val = JsonConvert.DeserializeObject<ValuesHANDLE4DIAMOND_Class>(response.Content);
                    List<HANDLE4DIAMOND_Class> val1 = new List<HANDLE4DIAMOND_Class>();  //val.value;
                    HANDLE4DIAMOND_Class emptyHandle = new HANDLE4DIAMOND_Class();
                    emptyHandle.HANDLE4DIAMONDNAME = " ";
                    emptyHandle.HANDLE4DIAMONDDES = " ";
                    val1.Add(emptyHandle);
                    foreach (HANDLE4DIAMOND_Class handle in val.value)
                    {
                        val1.Add(handle);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }




        #endregion get tables data from priority
        #region form fields
        public static DataTable getAllMeagedFields(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "MEAGEDNAME,CONFIG_THNAME,CONFIG_TDNAME,FIELDCODE,CONFIG_FIELDNAME,FIELDNAME";
                request.Resource = string.Format("TRSH_MEAGEDFIELDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesMeagedFields_Class val = JsonConvert.DeserializeObject<ValuesMeagedFields_Class>(response.Content);
                    List<MeagedFields_Class> val1 = new List<MeagedFields_Class>();  //val.value;
                    foreach (MeagedFields_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<MeagedFields_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    errMsg = "Check whether you are connected to Elidoor VPN";
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg += "\n response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static DataTable getDecorSideFlds(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "DECORSIDECODE,DECORSIDE,CONFIG_THNAME,CONFIG_TDNAME,FIELDCODE,CONFIG_FIELDNAME,SHOW";
                request.Resource = string.Format("TRSH_DECORSIDE_FLDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDecorSideFlds_Class val = JsonConvert.DeserializeObject<ValuesDecorSideFlds_Class>(response.Content);
                    List<DecorSideFlds_Class> val1 = new List<DecorSideFlds_Class>();  //val.value;
                    foreach (DecorSideFlds_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<DecorSideFlds_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }

        public static DataTable getConfFields(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FIELDID,FIELDCODE,CONFIG_THNAME,CONFIG_TDNAME,CONFIG_FIELDNAME,FIELDNAME,FIELDDES,FIELDDATATYPE,CONFIG_SUBFORM,TABINDEX,MANDATORY";
                request.Resource = string.Format("TRSH_CONF_FIELDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesConfField_Class val = JsonConvert.DeserializeObject<ValuesConfField_Class>(response.Content);
                    List<ConfField_Class> val1 = new List<ConfField_Class>();  //val.value;
                    foreach (ConfField_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<ConfField_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static string getFieldCodebyTh(string thName)
        {
            string query = string.Format("CONFIG_THNAME = '{0}'", thName);
            DataRow[] resRows = dtConfFields.Select(query);
            return (resRows.Length > 0 ? resRows[0]["FIELDCODE"].ToString() : string.Empty);
        }
        public static string getFieldCodebyTd(string tdName)
        {
            string query = string.Format("CONFIG_TDNAME = '{0}'", tdName);
            DataRow[] resRows = dtConfFields.Select(query);
            return (resRows.Length > 0 ? resRows[0]["FIELDCODE"].ToString() : string.Empty);
        }
        public static string getFieldCodebyFIELDNAME(string FIELDNAME)
        {
            string query = string.Format("FIELDNAME = '{0}'", FIELDNAME);
            DataRow[] resRows = dtConfFields.Select(query);
            return (resRows.Length > 0 ? resRows[0]["FIELDCODE"].ToString() : string.Empty);
        }
        public static DataTable getDefaults(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "LINENUM,TRSH_NUM,TRSH_MODELNAME,TRSH_MEAGEDNAME,FIELDCODE,CONFIG_FIELDNAME,FIELDNAME,CONFIG_TDNAME,FIELDDATATYPE,DEFVAL,VAL_LOCKED,WRONGVAL";
                request.Resource = string.Format("TRSH_DEFAULTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDefaults_Class val = JsonConvert.DeserializeObject<ValuesDefaults_Class>(response.Content);
                    List<Defaults_Class> val1 = new List<Defaults_Class>();  //val.value;
                    foreach (Defaults_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<Defaults_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    myLogger.log.Error(errMsg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static PART_Class getPart(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PART,PARTNAME,PARTDES,MPARTNAME,FAMILYNAME,FAMILYDES,TRSH_DOOR_HWCATCODE";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    return val.value[0];
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {

                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static DataTable getComplients(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_COMPLIENT,COMPLIENTDES";
                request.Resource = string.Format("TRSH_COMPLIENTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesComplient_Class val = JsonConvert.DeserializeObject<ValuesComplient_Class>(response.Content);
                    List<Complient_Class> val1 = new List<Complient_Class>();  //val.value;
                    foreach (Complient_Class fld in val.value)
                    {
                        val1.Add(fld);
                        if (fld.COMPLIENTDES == "אלידור")
                            ELIDOOR_COMPLIENT = fld.TRSH_COMPLIENT;
                    }
                    lstComplients = val1.ToList();
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<Complient_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    myLogger.log.Error(errMsg);
                    return null;
                }
            }
            catch (Exception ex)
            {

                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        //public static string getMeagedOfPart(string PARTNAME, ref string errMsg)
        //{
        //    try
        //    {
        //        RestClient restClient = new RestClient();
        //        initRestClient(restClient);
        //        RestRequest request = new RestRequest();
        //        string fields = "PARTNAME,MPARTNAME";
        //        request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
        //        IRestResponse response = restClient.Execute(request);
        //        if (response.IsSuccessful)
        //        {
        //            var settings = new JsonSerializerSettings
        //            {
        //                NullValueHandling = NullValueHandling.Include,
        //                MissingMemberHandling = MissingMemberHandling.Ignore
        //            };
        //            ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
        //            //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
        //            return val.value[0].MPARTNAME;
        //        }
        //        else
        //        {
        //            if (response.StatusDescription.ToLower() == "not found")
        //            {
        //                errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
        //                myLogger.log.Error(errMsg);
        //                return string.Empty;
        //            }
        //            errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
        //            return string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return string.Empty;
        //    }
        //}
        #endregion form fields
        #region get last TRSH_DOORCONFIG
        public static string getLastREFERENCE(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_DOORCONFIG,REFERENCE,FORMDATE,FORMFILLER,ENDCUSTDES,PARTNAME";
                request.Resource = string.Format("TRSH_DOORCONFIG?$orderby=TRSH_DOORCONFIG desc&$select={0}&$top=1", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDoorConfig val = JsonConvert.DeserializeObject<ValuesDoorConfig>(response.Content);
                    if (val == null || val.value == null)
                        return string.Empty;
                    else if (val != null && val.value != null && val.value.Count == 0)
                    {
                        myLogger.log.Error(string.Format("TRSH_DOORCONFIG is empty !"));
                        return string.Empty;
                    }
                    return val.value[0].REFERENCE;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return string.Empty;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                return string.Empty;
            }
        }

        #endregion get last TRSH_DOORCONFIG

        public static string JsonSerializer<T>(T t)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, t);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IRestResponse SendToPriority(string form, DoorConfig doorConfig, ref string errMsg)
        //HttpRequest httpRequest)
        {
            try
            {
                myLogger.log.Info("SendToPriority called");

                RestClient restClient = new RestClient();
                initRestClient(restClient);

                RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.Resource = form; //TRSH_DOORCONFIG   name of the form to populate

                //doorConfig.FORMDATE = "2022-02-24";  // just for test 
                //doorConfig.REFERENCE = "XXXX";
                doorConfig.TRSH_DOORCONFIG = 0;

                // doorConfig.HANDLE4DIAMONDNAME = "33001";  debug 
                //debug 
                //UiLogic.clearDoorConfig(doorConfig, false);  //works  without defaults
                //UiLogic.clearDoorConfig(doorConfig, true);  // fails with defaults 
                //doorConfig.PARTNAME = "";     //"224" fails because it does not exist in TRSH_TILETYPES.TRSH_MODELNAME  (the U key of that table ) - should be : 990141
                //doorConfig.VITRAGENAME = "";  //"ויטראז לדגם 5515";  - fails  because it does not exists in TRSH_VITRAGE.VITRAGENAME ( The U key of that table )
                // - should be V5515 

                // end debug  

                //doorConfig.dtTabFlds.TableName = "xxxxx";   // temporary ref: https://www.niteshluharuka.com/cannot-serialize-the-datatable-datatable-name-is-not-set-solution/
                string payload = JsonSerializer<DoorConfig>(doorConfig);

                int xl = payload.Length;    //2373

                //payload = "{ \"REFERENCE\":\"\",\"FORMDATE\":\"2022-05-24\",\"FORMFILLER\":\"x\",\"AGENT\":0,\"CUST\":1,\"TMPSHIPADDRESS\":null}";   // just to DEBUG !
                // this works !
                //                string payload2 = @"{
                //                    ""REFERENCE"":"""",""FORMDATE"":""2022-05-24"",""FORMFILLER"":""רר"",""AGENT"":1,""CUST"":2,""TMPSHIPADDRESS"":null,""SHIPADDRESS"":""תושיה 7  "",
                //""TRSH_WINGSNUMDES"":""כנף""}";   // works
                //--------------------
                string payload21 =
@"{""TRSH_DOORCONFIG"":0,""REFERENCE"":"""",""FORMDATE"":""2022-05-24"",""FORMFILLER"":""רר"",""AGENT"":1,""CUST"":2,""TMPSHIPADDRESS"":null,""SHIPADDRESS"":""תושיה 7  "",
""TRSH_WINGSNUMDES"":""כנף""" +

@",""OPENMODE"":null,""COMPLIENTDOOR"":""Y"",""TRSH_MODELNAME"":""MLI"",""LOCKDRILHEIGHT"":0,""OPENSIDE"":null,""DECORFORMAT"":null,""COLORSNUM"":null," +

@"""DOORWIDTH"":0,""DOORCOLORID"":0,""DOORHEIGHT"":0,""TURBOAPPARATUS"":""Y"",""LOGO"":"""",""TRSH_HARDWARE"":0,""HWCOLORID"":0,""DRIL4HW"":0,""TRSH_MODELNAME"":null," +

@"""ELECTRICAPPARATUS"":""N"",""RAFAFAONMOVINGWING"":null,""VENTS"":null,""CATDOOR"":null,""EXTCOLORID"":0,""GRIDCOLORID"":0,""VITRAGECLRBYCTLG"":null," +

@"""EXTSEPLINESCLRID"":0,""EXTCPLATEHTDMNDCLRID"":0,""EXTPERIFPROFILECLRID"":0,""EXTMODERNCPLATECLRID"":0," +

@"""EXTCGTIDCLRID"":0,""EXTHTPLATESCLRID"":0,""ATTACHEDPLATECLRID"":0,""EXTGLASSPLATECLRID"":0,""EXTMODERNPLATECLRID"":0,""DECORPLATECLRID"":0" +

@",""NIROSTALINESCLRID"":0,""EXTNIROSTALINESCLRID"":0,""EXTINSERTCLRID"":0,""VITRAGEPATTERNONDOOR"":null,""WINDOWWIDTH"":0,""WINDOWHEIGHT"":0" +
@",""PROFILE4WINDOWNAME"":null,""DECORFRAME"":null,""GLASS4WINDOWID"":0,""GRID_ID"":0,""VITRAGENAME"":null,""JAPANESEWINDOW"":null,""PLATESFOR7504"":null" +
@",""EXTCPLATE4HTDMNDNAME"":null,""EXTSIDE_C_PLATENAME"":null,""DECORGRIDPLATEDES"":null,""GRID4HT1084NAME"":null,""EXTGRIDCPLATEDES"":null" +
@",""EXTHTPLATENAME"":null,""VITRAGE4DIAMONDNAME"":null,""EXTVITRAGE4DIAMONDQ"":0,""INTERPLATESSPACE"":0,""EXTFINMODERNCPLATE"":null" +
@",""EXTFINMODERNSEPLINE"":null,""EXTFINMODERNPLATE"":null,""INTCOLORID"":0,""INTSIDE_C_PLATE"":null,""INTGRIDCPLATE"":null,""INTHTPLATE"":null" +
@",""CENTRALCOLCLRID"":0,""SHALVANIACLRID"":0,""INTPERIFPROFILECLRID"":0,""INTSIDECNTRPLATECLID"":0,""INTSEPLINESCLRID"":0" +
@",""INTMODERNCPLATECLRID"":0,""INTCGRIDCLRID"":0,""INTHTPLATESCLRID"":0,""INTGLASSPLATECLRID"":0,""INTMODERNPLATECLRID"":0,""INTNIROSTALINESCLRID"":0" +
@",""INTINSERTCLRID"":0,""INTCPLATE4HTDMNDNAME"":null,""INTCPLATEHTDMNDCLRID"":0,""INTVITRAGE4DIAMONDQ"":0,""INTFINMODERNCPLATE"":null" +
@",""INTFINMODERNSEPLINE"":null,""INTFINMODERNPLATE"":null,""CENTRALCOLCLRDES"":null" +
@",""HANDLE4DIAMONDNAME"":null" +
"}";
                //Works !
                int xl2 = payload21.Length;   // = 1343 

                // problematic fields :
                // ""HANDLE4DIAMONDNAME"":""N""   or ""aaaa""
                // ""HANDLE4DIAMONDNAME"":null works fine 






                //{"REFERENCE":null,"FORMDATE":"24-02-2022",  wrong date format - fails
                //"REFERENCE":null,"FORMDATE":"2022-02-24","FORMFILLER":null,"AGENT":0,"CUST":1, - works


                request.AddParameter("application/json", payload, ParameterType.RequestBody);

                IRestResponse response = restClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    errMsg = response.Content; //response.StatusDescription;
                    myLogger.log.Info(string.Format(" PrApiCalls : Api call failed : {0} , Payload : {1}, errorMessaage : {2} ; {3}",
                                   response.ResponseStatus, payload, response.ErrorMessage, response.ErrorException));
                    myLogger.log.Info(string.Format("REQUEST sent : Method : {0}, Resource: {1}, Parameters[0] :{2}",
                                   request.Method, request.Resource, request.Parameters[0]));

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
