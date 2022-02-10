using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerApp1.Data;

namespace BlazorServerApp1.Pages
{
    public class Configurator3Base : ComponentBase
    {
        public static List<string>? getCustomers()
        {
            try
            {
                string errMsg = string.Empty;
                List<CUSTOMER_Class> values = PrApiCalls.getCustomers(ref errMsg);
                List<string> lstCustDes = new List<string>();
                lstCustDes.Add(" ");
                lstCustDes.Add("aaaaaaaaaaaaaa");
                lstCustDes.Add("bbb");
                return lstCustDes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
