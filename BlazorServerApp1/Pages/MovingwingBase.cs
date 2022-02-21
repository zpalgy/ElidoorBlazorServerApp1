using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using BlazorServerApp1.Data;


namespace BlazorServerApp1.Pages
{
    public class MovingwingBase : ComponentBase
    {
        protected int calcWindowWidth(DoorConfig doorConfig, ref string errMsg)
        {
            try
            {
                string query = string.Format("PARTNAME='{0}'", doorConfig.PARTNAME);
                DataRow[] rowsArray = PrApiCalls.dtWindowWidths.Select(query);
                if (rowsArray.Length == 0)
                {
                    return 0;  // A DOOR  without WINDOW is legal 
                }
                query = string.Format("PARTNAME='{0}'  AND MINDOORWIDTH <= {1} AND {1} <= MAXDOORWIDTH", doorConfig.PARTNAME, doorConfig.DOORWIDTH);
                rowsArray = PrApiCalls.dtWindowWidths.Select(query);
                if (rowsArray.Length > 0)
                {
                    //UiLogic.hideErrMsg(lblMsg3);
                    return (rowsArray[0]["WINDOWWIDTH"] != null ? int.Parse(rowsArray[0]["WINDOWWIDTH"].ToString()) : 0);
                }
                else
                {
                    errMsg = string.Format("שגיאה: לא נמצא רוחב חלון לדלת {0} ברוחב {1}  בטבלת מידות רוחב חלון", doorConfig.PARTNAME, doorConfig.DOORWIDTH);
                    myLogger.log.Error(errMsg);
                    //UiLogic.displayErrMsg(lblMsg3, errMsg2);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("שגיאה : אנא פנה למנהל המערכת : {0} , {1}    י", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                //UiLogic.displayErrMsg(lblMsgL1, errMsg2);
                return 0;
            }
        }
    }
}