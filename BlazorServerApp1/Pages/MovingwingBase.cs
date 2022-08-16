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
        protected bool wingWidthIsOk(DoorConfig doorConfig, int wingWidth, ref string errMsg)
        {
            //string query = string.Format("FROM_WIDTH <= {0} AND {0} <= TO_WIDTH", wingWidth);
            //DataRow[] rowsArray = PrApiCalls.dtWingWidth.Select(query);
            //if (rowsArray.Length == 0)
            //{
            //    errMsg = "רוחב כנף חורג מהטולרנס שהוגדר נא למלא רוחב כנף מתאים";
            //    return false;
            //}
            //return true;
            string query = string.Format("TRSH_MODELNAME='{0}'", doorConfig.TRSH_MODELNAME);
            DataRow[] rowsArray = PrApiCalls.dtWindowWidths.Select(query);
            if (rowsArray.Length == 0)
            {
                //return 0;  // A DOOR  without WINDOW is legal 
                errMsg = "דגם זה לא קיים בטבלת מידות רוחב חלון - אנא פנה למנהל המערכת";  // now (21/06/2022) any MODEL should be in  TRSH_WINDOWWIDTH 
                                                                                         // even if it doesn't have a Window !
                myLogger.log.Error(errMsg);
                return false;
            }
            query = string.Format("TRSH_MODELNAME='{0}'  AND TRSH_WINGSNUMDES = '{1}' AND MIN_DOORWIDTH <= {2} AND {2} <= MAX_DOORWIDTH",
                        doorConfig.TRSH_MODELNAME, doorConfig.TRSH_WINGSNUMDES, wingWidth);

            rowsArray = PrApiCalls.dtWindowWidths.Select(query);
            if (rowsArray.Length == 0 && (wingWidth < UiLogic.MIN_MOVINWING_W || wingWidth > UiLogic.MAX_ANYWING_W ))
            {
                errMsg = "רוחב כנף חורג מהטולרנס שהוגדר נא למלא רוחב כנף מתאים";
                myLogger.log.Error(errMsg);
                return false;
            }
            return true;
        }
        
        protected int calcWindowWidth(DoorConfig doorConfig, ref string errMsg)
        {
            try
            {
                string query = string.Format("TRSH_MODELNAME='{0}'", doorConfig.TRSH_MODELNAME);
                DataRow[] rowsArray = PrApiCalls.dtWindowWidths.Select(query);
                if (rowsArray.Length == 0)
                {
                    //return 0;  // A DOOR  without WINDOW is legal 
                    errMsg = "דגם זה לא קיים בטבלת מידות רוחב חלון - אנא פנה למנהל המערכת";  // now (21/06/2022) any MODEL should be in  TRSH_WINDOWWIDTH 
                                                                                                // even if it doesn't have a Window !
                    myLogger.log.Error(errMsg);
                    return -1;
                }
                query = string.Format("TRSH_MODELNAME='{0}'  AND TRSH_WINGSNUMDES = '{1}' AND MIN_DOORWIDTH <= {2} AND {2} <= MAX_DOORWIDTH", 
                        doorConfig.TRSH_MODELNAME, doorConfig.TRSH_WINGSNUMDES,  doorConfig.DOORWIDTH);
                rowsArray = PrApiCalls.dtWindowWidths.Select(query);
                if (rowsArray.Length > 0)
                {
                    //UiLogic.hideErrMsg(lblMsg3);
                    int windowWidth = 0;
                    if (rowsArray[0]["WINDOWWIDTH"] != null && !String.IsNullOrEmpty(rowsArray[0]["WINDOWWIDTH"].ToString()) )
                    {
                        windowWidth = int.Parse(rowsArray[0]["WINDOWWIDTH"].ToString());
                    }
                    return windowWidth;
                }
                else
                {
                    //errMsg = string.Format("שגיאה: לא נמצא רוחב חלון לדלת {0} ברוחב {1}  בטבלת מידות רוחב חלון", doorConfig.PARTNAME, doorConfig.DOORWIDTH);
                    errMsg = "רוחב כנף חורג מהטולרנס שהוגדר נא למלא רוחב כנף מתאים";
                    myLogger.log.Error(errMsg);
                    //UiLogic.displayErrMsg(lblMsg3, errMsg2);
                    return -1;
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
        protected int calcWindowHeight(DoorConfig doorConfig, ref string errMsg)
        {
            try
            {
                //string query = string.Format("PARTNAME='{0}'", doorConfig.TRSH_MODELNAME);
                string query = string.Format("TRSH_MODELNAME='{0}'", doorConfig.TRSH_MODELNAME);  //temporary till we update the TRSH_WINDOWHEIGHT table in priority
                DataRow[] rowsArray = PrApiCalls.dtWindowHeights.Select(query);
                if (rowsArray.Length == 0)
                {
                    return 0;  // A DOOR  without WINDOW is legal 
                }
                query = string.Format("TRSH_MODELNAME='{0}'  AND MINDOORHEIGHT <= {1} AND {1} <= MAXDOORHEIGHT", doorConfig.TRSH_MODELNAME, doorConfig.DOORHEIGHT);
                rowsArray = PrApiCalls.dtWindowHeights.Select(query);
                if (rowsArray.Length > 0)
                    return int.Parse(rowsArray[0]["WINDOWHEIGHT"].ToString());
                else
                {
                    string errMsg2 = string.Format("שגיאה: לא נמצא גובה חלון לדלת {0} בגובה {1}  בטבלת גובה חלון ", doorConfig.PARTNAME, doorConfig.DOORHEIGHT);
                    myLogger.log.Error(errMsg2);
                    //UiLogic.displayErrMsg(lblMsgL1, errMsg2);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string errMsg2 = string.Format("שגיאה : אנא פנה למנהל המערכת : {0} , {1}    י", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg2);
                //UiLogic.displayErrMsg(lblMsgL1, errMsg2);
                return 0;
            }
        }
        protected void setHingesAndWindowsData(DoorConfig doorConfig, ref string errMsg)
        {
            try
            {
                if (doorConfig.TRSH_DOOR_HWCATCODE == 0)
                {
                    errMsg = string.Format(@"שגיאה : קטגוריית הפרזול של הדלת   {0} לא נשמרה - לא ניתן לחשב את גובה הניקוב ללשונית",
                      doorConfig.PARTNAME);
                    return;
                }

                string query = string.Format("TRSH_DOOR_HWCATCODE = {0} AND DOORHEIGHTMIN <= {1} AND {1} <= DOORHEIGHTMAX", doorConfig.TRSH_DOOR_HWCATCODE, doorConfig.DOORHEIGHT);
                DataRow[] rowsArray = PrApiCalls.dtLock_Hinge_Dril_Heights.Select(query);
                if (rowsArray.Length == 0)
                {
                    errMsg = string.Format("לא נמצאה שורת 'מידות צירים וניקוב (תואם אלידור)' מתאימה לקטגורית הפרזול {0} ולגובה הדלת {1} - אנא בדוק את הטבלה הזו",
                                    doorConfig.TRSH_DOOR_HWCATCODE, doorConfig.DOORHEIGHT);
                    //UiLogic.displayErrMsg(lblMsgL1, errMsg);
                    return;
                }
                doorConfig.LOCKDRILHEIGHT = int.Parse(rowsArray[0]["LOCKDRILHEIGHT"].ToString());
                doorConfig.BACKPINHEIGHT = int.Parse(rowsArray[0]["BACKPINHEIGHT"].ToString());
                doorConfig.HINGESNUM = int.Parse(rowsArray[0]["HINGESNUM"].ToString());
                doorConfig.HINGE1HEIGHT = int.Parse(rowsArray[0]["HINGE1HEIGHT"].ToString());
                doorConfig.HINGE2HEIGHT = int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString());
                if (doorConfig.HINGESNUM > 2)
                {
                    doorConfig.HINGE3HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());
                    if (doorConfig.HINGESNUM > 3)
                    {
                        doorConfig.HINGE4HEIGHT = int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString());
                        if (doorConfig.HINGESNUM == 5)
                            doorConfig.HINGE5HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());
                        else
                            doorConfig.HINGE5HEIGHT = 0;
                    }
                    else
                    {
                        doorConfig.HINGE4HEIGHT = 0;
                        doorConfig.HINGE5HEIGHT = 0;
                    }
                }
                else
                {
                    doorConfig.HINGE4HEIGHT = 0;
                    doorConfig.HINGE5HEIGHT = 0;
                    doorConfig.HINGE5HEIGHT = 0;
                }
                //check if txtWindowHeight is visible before calculating WindowHeight and windowWidth
                // if one of them is visible and the other NOT - BUG in Meaged definition.

                if (!UiLogic.hideFld(doorConfig, "WindowHeight"))
                {
                    doorConfig.WINDOWHEIGHT = calcWindowHeight(doorConfig, ref errMsg);
                    //doorConfig.WINDOWWIDTH= calcWindowWidth();
                }
            }
            catch (Exception ex)
            {
                string errMsg2 = string.Format("שגיאה : אנא פנה למנהל המערכת : {0} , {1}    י", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg2);
                //UiLogic.displayErrMsg(lblMsgL1, errMsg2);
                return;
            }
        }
        protected bool Dril4HwIsNotIDS(DoorConfig doorConfig)
        {
            foreach (DRIL4HW_Class c in PrApiCalls.lstDril4Hw)
            {
                if (c.DRIL4HWDES == "IDS" && c.DRIL4HW == doorConfig.DRIL4HW)
                    return false;
            }
            return true;
        }
        protected void clearHingesMeasures(DoorConfig doorConfig)
        {
            doorConfig.HINGESNUM = 0;
            doorConfig.BACKPINHEIGHT = 0;
            doorConfig.HINGE1HEIGHT = 0;
            doorConfig.HINGE2HEIGHT = 0;
            doorConfig.HINGE3HEIGHT = 0;
            doorConfig.HINGE4HEIGHT = 0;
            doorConfig.HINGE5HEIGHT = 0;
        }

        
    }
}
