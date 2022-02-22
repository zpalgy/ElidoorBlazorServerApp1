﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Configuration;
using RestSharp;
using System.Diagnostics;

namespace BlazorServerApp1.Data
{
    public static class UiLogic
    {
        #region local arrays and tables
        public static string[] tabNames = new string[] { "movingwing", "extdecor", "intdecor", "staticwing", "hinges", "handle", "accessories" };
        public static string[] Fields2Keep = new string[] { "txtWindowWidth", "txtWindowHeight", "txtOpenDirection",  "txtLockDrilHeight",
                                                            "txtBackPinHeight", "txtHingesNum",
                                                            "txtHinge1Height","txtHinge2Height","txtHinge3Height","txtHinge4Height","txtHinge5Height"};
        public static string[] prodButtonIDs; //= new Button[] { new Button()};
        public static string currentMeaged = string.Empty;
        public static string decoreSideCode = string.Empty;
        #endregion local arrays and tables

        public static bool setHideFld(string fldName)
        {
            bool showByMeaged = true;
            if (string.IsNullOrEmpty(currentMeaged))
                showByMeaged = true;
            else if (meagedContains(currentMeaged, fldName, PrApiCalls.dtMeagedFields))
                showByMeaged = true;
            else
                showByMeaged = false;  //hide

            if (!showByMeaged)
                return true;  // hide
            else if (HiddenDecorSideFldsContains(fldName, PrApiCalls.dtDecorSideFlds, Helper.DecorFormat2Code(PrApiCalls.doorConfig.DECORFORMAT)))
                return true;  //hide
            else
                return false; // show

        }
        public static bool setHideFld(int i)
        {
            return true;
        }
        public static void initTabNames()
        {
            DataView view = new DataView(PrApiCalls.dtConfFields);
            DataTable dtTabs = view.ToTable(true, "CONFIG_SUBFORM");
            tabNames = new string[] { };
            for (int i = 0; i < dtTabs.Rows.Count; i++)
            {
                tabNames[i] = dtTabs.Rows[i]["CONFIG_SUBFORM"].ToString();
            }
        }

        public static bool hideStaticWing(string wingsNum)
        {
            return (wingsNum == "כנף");
        }

        //public static void toggleColor(Button btn)
        //{
        //    if (btn.BackColor == Color.Blue)
        //    {
        //        btn.BackColor = Color.White;
        //        btn.ForeColor = Color.Blue;
        //    }
        //    else
        //    {
        //        btn.BackColor = Color.Blue;
        //        btn.ForeColor = Color.Yellow;
        //    }
        //}
        //public static void enableBtn(Button btn)
        //{
        //    btn.Enabled = true;
        //    btn.BackColor = Color.White;
        //    btn.ForeColor = Color.Blue;
        //}
        //public static void disableBtn(Button btn)
        //{
        //    btn.Enabled = false;
        //    btn.BackColor = Color.Gray;
        //}





        #region Meageds
        //public static void applyMeaged(string PARTNAME, DataTable MeagedFields, HtmlGenericControl dvTab, ref string errMsg)
        //{
        //    try
        //    {
        //        //get MeagedName - of PART
        //        //mark the Meaged fields Visible in  
        //        //DataTable MeagedFields = (DataTable)ViewState["MeagedFields"];
        //        string meagedName = PrApiCalls.getMeagedOfPart(PARTNAME, ref errMsg);
        //        if (string.IsNullOrEmpty(errMsg) && !string.IsNullOrEmpty(meagedName))
        //        {
        //            ShowMeaged(dvTab, meagedName, MeagedFields, PARTNAME);
        //        }
        //        else
        //        {
        //            string errMsg2 = string.Format("שגיאה  {0}  י", errMsg);
        //            myLogger.log.Error(errMsg2);
        //        }

        //        //for (int r = 0; r < MeagedFields.Rows.Count; r++)
        //        //{
        //        //    DataRow row = MeagedFields.Rows[r];
        //        //    if (row["MEAGEDNAME"].ToString() == meagedName)
        //        //    {
        //        //        row["Visible"] = true;
        //        //        //use dvTab.findControl - a recursive method !

        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}


        //public static void ShowMeaged(Control rootCtl, string meaged2Show, DataTable MeagedFields, string PARTNAME)
        //{
        //    try
        //    {
        //        foreach (Control c in rootCtl.Controls)
        //        {
        //            if (c is HtmlTableCell && c.ID != null)
        //            {
        //                //debug
        //                if (c.ID.Substring(2) == "DecorativePlateClr")
        //                {
        //                    int x = 17;
        //                }
        //                // end debug
        //                HtmlTableCell tdth = (HtmlTableCell)c;
        //                tdth.Visible = meagedContains(meaged2Show, tdth, MeagedFields);
        //                //apply field default based on PARTNAME
        //                if (tdth.ID.StartsWith("td") && tdth.Visible)
        //                {
        //                    applyFldDefault(tdth, PARTNAME);
        //                }
        //            }
        //            else if (c.HasControls())
        //            {
        //                ShowMeaged(c, meaged2Show, MeagedFields, PARTNAME);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}

        public static bool meagedContains(string meaged, string fldName, DataTable MeagedFields)
        {
            try
            {
                //if (c.ID == null)
                //{
                //    int z = 17;
                //}
                //if (c.ID.Contains("IntModernCPlateClr"))
                //{
                //    //Debugger.Break();
                //    int dbg = 17;
                //}
                if (!fldName.StartsWith("th") && !fldName.StartsWith("td"))
                {
                    fldName = "th" + fldName;
                }
                if (fldName.StartsWith("th"))
                {
                    //string x = "20";
                    //string x1 = "thFinModernSeparatingLines";
                    //string x2 = MeagedFields.Rows[1]["MEAGEDNAME"].ToString();
                    //DataRow[] rows1 = MeagedFields.Select(string.Format("MEAGEDNAME='{0}' AND CONFIG_THNAME = '{1}'", x, x1));

                    DataRow[] rowsTh = MeagedFields.Select(string.Format("MEAGEDNAME='{0}' AND CONFIG_THNAME = '{1}'", meaged, fldName));
                    if (rowsTh != null && rowsTh.Length > 0)
                        return true;
                    else
                    {
                        DataRow[] rowsTh2 = MeagedFields.Select(string.Format("CONFIG_THNAME = '{0}'", fldName));
                        if (rowsTh2 != null && rowsTh2.Length > 0)
                            return false;     //exists but in a different Meaged - hide
                        else
                            return true;      // doesn't exist in MeagedFields - should be always Visible
                    }
                }
                else if (fldName.StartsWith("td"))
                {
                    DataRow[] rowsTd = MeagedFields.Select(string.Format("MEAGEDNAME='{0}' AND CONFIG_TDNAME = '{1}'", meaged, fldName));
                    if (rowsTd != null && rowsTd.Length > 0)
                        return true;
                    else
                    {
                        DataRow[] rowsTd2 = MeagedFields.Select(string.Format("CONFIG_TDNAME = '{0}'", fldName));
                        if (rowsTd2 != null && rowsTd2.Length > 0)
                            return false;  //exists but in a different Meaged - hide
                        else
                            return true;  // it's not a Meaged field - it should always be shown
                    }
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Unexpected error: {0} . MEAGEDNAME='{1}' AND CONFIG_TDNAME = '{2}'  Stacktrace : {3}", ex.Message, meaged, fldName, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return false;
            }
        }
        //public static void applyFldDefault(HtmlTableCell td, string PARTNAME)
        //{
        //    try
        //    {
        //        Control c;
        //        string query = string.Format("PARTNAME = '{0}' AND CONFIG_TDNAME = '{1}'", PARTNAME, td.ID);
        //        DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);
        //        if (rowsDefVal.Length > 0)
        //        {
        //            string config_fieldname = rowsDefVal[0]["CONFIG_FIELDNAME"].ToString();
        //            c = td.FindControl(config_fieldname);
        //            string defval = rowsDefVal[0]["DEFVAL"].ToString();
        //            string val_locked = rowsDefVal[0]["VAL_LOCKED"].ToString();

        //            if (c is DropDownList)
        //            {
        //                DropDownList dlst = (DropDownList)c;
        //                dlst.SelectedIndex = dlst.Items.IndexOf(dlst.Items.FindByText(defval));
        //                dlst.Enabled = (val_locked != "Y" && val_locked != "כן");
        //                for (int i = 0; i < rowsDefVal.Length; i++)
        //                {
        //                    string wrongval = rowsDefVal[i]["WRONGVAL"].ToString();
        //                    int index2Remove = dlst.Items.IndexOf(dlst.Items.FindByText(wrongval));
        //                    if (index2Remove >= 0)
        //                        dlst.Items.RemoveAt(index2Remove);
        //                }

        //            }
        //            else if (c is TextBox)
        //            {
        //                TextBox txtB = (TextBox)c;
        //                txtB.Text = defval;
        //                txtB.Enabled = (val_locked != "Y" && val_locked != "כן");
        //            }
        //            else if (c is CheckBox)
        //            {
        //                CheckBox chkb = (CheckBox)c;
        //                chkb.Checked = (defval == "Y" || defval == "כן");
        //                chkb.Enabled = (val_locked != "Y" && val_locked != "כן");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}

        #endregion Meageds
        #region decorSide 
        //public static void applyDecorSide(string decorSideCode, DataTable dtDecoSideFlds, HtmlGenericControl dvTab, ref string errMsg)
        //{
        //    try
        //    {
        //        HideDecorSideFlds(dvTab, decorSideCode, dtDecoSideFlds);
        //    }
        //    catch (Exception ex)
        //    {

        //        errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}


        //public static void HideDecorSideFlds(Control rootCtl, string decorSideCode, DataTable dtDecorSideFlds)
        //{
        //    try
        //    {
        //        foreach (Control c in rootCtl.Controls)
        //        {
        //            if (c is HtmlTableCell && c.ID != null)
        //            {
        //                HtmlTableCell tdth = (HtmlTableCell)c;
        //                if (HiddenDecorSideFldsContains(tdth, dtDecorSideFlds, decorSideCode))
        //                    tdth.Visible = false;
        //            }
        //            else if (c.HasControls())
        //            {
        //                HideDecorSideFlds(c, decorSideCode, dtDecorSideFlds);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}

        public static bool HiddenDecorSideFldsContains(string  fldName, DataTable dtDecorSideFlds, string decorSideCode)
        {
            try
            {
                //if (c.ID == null)
                //{
                //    int z = 17;
                //}
                //if (c.ID.Contains("IntModernCPlateClr"))
                //{
                //    //Debugger.Break();
                //    int dbg = 17;
                //}
                //DataRow[] rowsTh1 = dtDecorSideFlds.Select("DECORSIDECODE = 'O' AND CONFIG_THNAME = 'thExtColor'");

                if (fldName.StartsWith("th")) // && c.Visible)
                {
                    //string x = "20";
                    //string x1 = "thFinModernSeparatingLines";
                    //string x2 = MeagedFields.Rows[1]["MEAGEDNAME"].ToString();
                    //DataRow[] rows1 = MeagedFields.Select(string.Format("MEAGEDNAME='{0}' AND CONFIG_THNAME = '{1}'", x, x1));
                    //  debug
                    //if (c.ID == "thExtSepLinesClr")
                    //{
                    //    int x = 17;
                    //}

                    // end debug 
                    DataRow[] rowsTh = dtDecorSideFlds.Select(string.Format("DECORSIDECODE = '{0}' AND CONFIG_THNAME = '{1}'", decorSideCode, fldName));
                    if (rowsTh != null && rowsTh.Length > 0 && string.IsNullOrEmpty(rowsTh[0]["SHOW"].ToString()))
                        return true;
                    else
                        return false;
                }
                else if (fldName.StartsWith("td"))
                {
                    DataRow[] rowsTd = dtDecorSideFlds.Select(string.Format("DECORSIDECODE = '{0}' AND CONFIG_TDNAME = '{1}'", decorSideCode, fldName));
                    if (rowsTd != null && rowsTd.Length > 0 && string.IsNullOrEmpty(rowsTd[0]["SHOW"].ToString()))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                string x = ex.StackTrace;
                throw ex;
            }
        }
        #endregion decorSide
        #region display errMsg
        //public static void displayErrMsg(Label lbl, string errMsg, Color textColor, string strFontSize)
        //{
        //    lbl.Text = errMsg;
        //    lbl.ForeColor = textColor;//Color.Red;
        //    lbl.Font.Bold = true;
        //    //FontUnit.Parse("20px")
        //    lbl.Font.Size = FontUnit.Parse(strFontSize); //20;
        //    lbl.Visible = true;
        //}
        //public static void displayErrMsg(Label lbl, string errMsg, Color textColor)
        //{
        //    displayErrMsg(lbl, errMsg, textColor, "18px");
        //}
        //public static void displayErrMsg(Label lbl, string errMsg)
        //{
        //    displayErrMsg(lbl, errMsg, Color.Red);
        //}
        //public static void hideErrMsg(Label lbl)
        //{
        //    lbl.Text = null;
        //    lbl.ForeColor = Color.Green;
        //    lbl.Visible = false;
        //}
        #endregion display errMsg
        #region clear page
        //public static void ClearAllControls(Control rootCtl)
        //{
        //    try
        //    {
        //        foreach (Control c in rootCtl.Controls)
        //        {
        //            if (c is TextBox && c.ID.StartsWith("txt") && c.ID != "txtDate")
        //            {
        //                ((TextBox)c).Text = string.Empty;
        //                ((TextBox)c).Enabled = true;
        //            }
        //            else if (c is DropDownList && c.ID.StartsWith("dlst"))
        //            {
        //                ((DropDownList)c).SelectedIndex = -1;
        //                ((DropDownList)c).Enabled = true;
        //            }
        //            else if (c is CheckBox && c.ID.StartsWith("chkb"))
        //            {
        //                ((CheckBox)c).Checked = false;
        //                ((CheckBox)c).Enabled = true;
        //            }
        //            else if (c is Label && c.ID.StartsWith("lbl") && c.ID != "lblBuildDate")
        //                ((Label)c).Text = string.Empty;
        //            else if (c.HasControls())
        //            {
        //                ClearAllControls(c);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}
        //public static void restoreDecorFormat(DropDownList dlstDecorFormat)
        //{
        //    List<string> lstDecorFormats = new List<string>();
        //    lstDecorFormats.Add("ללא");
        //    lstDecorFormats.Add("חוץ");
        //    lstDecorFormats.Add("פנים");
        //    lstDecorFormats.Add("דו צדדי");
        //    dlstDecorFormat.DataSource = lstDecorFormats;
        //    dlstDecorFormat.DataBind();
        //}

        #endregion clear page
        #region clear tab and conf fields
        //public static void clearFollowingTabs(DoorConfig doorConfig, Control dvTab, string tabid, Label lblMsg)
        //{
        //    try
        //    {
        //        int tabIndex = Array.IndexOf(tabNames, tabid);
        //        for (int i = tabIndex + 1; i < tabNames.Length; i++)
        //        {
        //            if (tabNames[i] != "hinges")
        //                clearTab(doorConfig, dvTab, tabNames[i], lblMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        UiLogic.displayErrMsg(lblMsg, errMsg);
        //        return;
        //    }
        //}
        //public static void clearTab(DoorConfig doorConfig, Control dvTab, string tabid, Label lblMsg)
        //{
        //    try
        //    {
        //        DataRow[] tabConfFields = getTabConfFields(tabid, lblMsg);
        //        if (tabConfFields != null && tabConfFields.Length > 0)
        //        {
        //            for (int i = 0; i < tabConfFields.Length; i++)
        //            {
        //                DataRow row = tabConfFields[i];
        //                string fldCode = row["FIELDCODE"].ToString();
        //                ConfField_Class cFld = getConfField(fldCode, lblMsg);
        //                Control c = UiLogic.getControl(dvTab, cFld.CONFIG_FIELDNAME);
        //                if (c is DropDownList)
        //                {
        //                    DropDownList dlst = (DropDownList)c;
        //                    dlst.SelectedIndex = -1;
        //                }
        //                else if (c is TextBox)
        //                {
        //                    TextBox txt = (TextBox)c;
        //                    if (Array.IndexOf(Fields2Keep, txt.ID) == -1)
        //                        txt.Text = string.Empty;
        //                }
        //                //TODO : set dafault value per the Defaults table
        //                UiLogic.setConfFieldVal(doorConfig, cFld.FIELDNAME, cFld.FIELDDATATYPE, string.Empty, lblMsg);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        UiLogic.displayErrMsg(lblMsg, errMsg);
        //        return;
        //    }
        //}
        //public static DataRow[] getTabConfFields(string tabid, Label lblMsg)
        //{
        //    try
        //    {
        //        string query = string.Format("CONFIG_SUBFORM ='{0}'", tabid);
        //        DataRow[] rowsArray = PrApiCalls.dtConfFields.Select(query);
        //        if (rowsArray.Length > 0)
        //            return rowsArray;
        //        else
        //        {
        //            string errMsg2 = string.Format("שגיאה: לא נמצאו שדות קונפיגורטור ללשונית {0} - אנא פנה למנהל המערכת", tabid);
        //            myLogger.log.Error(errMsg2);
        //            UiLogic.displayErrMsg(lblMsg, errMsg2);
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        UiLogic.displayErrMsg(lblMsg, errMsg);
        //        return null;
        //    }
        //}

        #endregion clear tab and conf fields

        #region conf fields
        //public static ConfField_Class getConfField(string fieldCode, Label lblMsg)
        //{
        //    try
        //    {
        //        DataRow[] fldRows = PrApiCalls.dtConfFields.Select(string.Format("FIELDCODE = '{0}'", fieldCode));
        //        if (fldRows != null && fldRows.Length > 0)
        //        {
        //            DataRow fldRow = fldRows[0];
        //            ConfField_Class fld = new ConfField_Class();
        //            //--
        //            Type objType = fld.GetType();

        //            PropertyInfo[] props = objType.GetProperties();
        //            string[] propNames = props.Select(i => i.Name).ToArray();

        //            for (int p = 0; p < propNames.Length; p++)
        //            {
        //                try
        //                {
        //                    props[p].SetValue(fld, fldRow[props[p].Name]);
        //                }
        //                catch (Exception ex)
        //                {
        //                    string errmsg = string.Format("Error : {0} , fld={1}", ex.Message, fld.FIELDCODE);
        //                    throw ex;
        //                }
        //            }
        //            return fld;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        displayErrMsg(lblMsg, errMsg);

        //        return null;
        //    }
        //}
        //public static ConfField_Class getConfField(Control ctl, Label lblMsg)
        //{
        //    try
        //    {
        //        DataRow[] fldRows = PrApiCalls.dtConfFields.Select(string.Format("CONFIG_FIELDNAME = '{0}'", ctl.ID));
        //        if (fldRows != null && fldRows.Length > 0)
        //        {
        //            DataRow fldRow = fldRows[0];
        //            ConfField_Class fld = new ConfField_Class();
        //            //--
        //            Type objType = fld.GetType();

        //            PropertyInfo[] props = objType.GetProperties();
        //            string[] propNames = props.Select(i => i.Name).ToArray();

        //            for (int p = 0; p < propNames.Length; p++)
        //            {
        //                try
        //                {
        //                    if (fldRow[props[p].Name] != DBNull.Value)
        //                        props[p].SetValue(fld, fldRow[props[p].Name]);
        //                }
        //                catch (Exception ex)
        //                {
        //                    string errmsg = string.Format("Error : {0} , fld={1}", ex.Message, fld.FIELDCODE);
        //                    myLogger.log.Error(errmsg);
        //                    //displayErrMsg(lblMsg, errmsg);
        //                }
        //            }
        //            return fld;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} . ctl.ID = {1}  Stacktrace : {2}", ex.Message, ctl.ID,  ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        displayErrMsg(lblMsg, errMsg);

        //        return null;
        //    }
        //}
        //public static Control getControl(Control dvTab, string id)
        //{
        //    try
        //    {
        //        //Control ctd = dvTab.FindControl("tdDecorGridPlate");
        //        Control c = dvTab.FindControl(id);
        //        return c;
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return null;
        //    }
        //}
        //public static void setConfFieldVal(DoorConfig doorConfig, string fldName, string dataType, object val, Label lblMsg)
        //{
        //    string sval;
        //    int ival;

        //    Type objType = doorConfig.GetType();
        //    PropertyInfo[] props = objType.GetProperties();
        //    string[] propNames = props.Select(i => i.Name).ToArray();
        //    try
        //    {
        //        for (int p = 0; p < propNames.Length; p++)
        //        {
        //            if (propNames[p] == fldName)
        //            {
        //                switch (dataType)
        //                {
        //                    case "CHAR":
        //                    case "RCHAR":
        //                        if (val == null || string.IsNullOrEmpty(val.ToString()))
        //                            sval = string.Empty;
        //                        else
        //                            sval = val.ToString();  //(string)val;
        //                        try
        //                        {
        //                            props[p].SetValue(doorConfig, sval);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            string errMsg = string.Format("props[{0}}].SetValue(doorConfig, sval = {1}); FAILED ! \n error: {2} ",
        //                                                   p, sval, ex.Message);
        //                            myLogger.log.Error(errMsg);
        //                            displayErrMsg(lblMsg, errMsg);
        //                        }
        //                        return;
        //                    case "INT":
        //                        if (val == null || string.IsNullOrEmpty(val.ToString()))
        //                            ival = 0;
        //                        else
        //                            ival = int.Parse(val.ToString());

        //                        try
        //                        {
        //                            props[p].SetValue(doorConfig, ival);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            string errMsg = string.Format("props[{0}}].SetValue(doorConfig, ival = {1}); FAILED ! \n error: {2} ",
        //                                                   p, ival, ex.Message);
        //                            myLogger.log.Error(errMsg);
        //                            displayErrMsg(lblMsg, errMsg);
        //                        }
        //                        return;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        displayErrMsg(lblMsg, errMsg);
        //    }
        //}

        //static List<string> lstThNames = new List<string>();
        //static List<string> lstTdNames = new List<string>();
        //public static void getThTdNames(Control rootCtl, ref List<string> lstThNames, ref List<string> lstTdNames)
        //{
        //    try
        //    {
        //        lstTdNames.Clear();
        //        lstThNames.Clear();
        //        foreach (Control c in rootCtl.Controls)
        //        {
        //            if (c is HtmlTableCell && c.ID != null)
        //            {
        //                if (c.ID.StartsWith("th"))
        //                    lstThNames.Add(c.ID);
        //                else if (c.ID.StartsWith("td"))
        //                    lstTdNames.Add(c.ID);

        //                //if (c.ID.Substring(2) == "ExtColor")
        //                //{
        //                //    int x = 17;  //debug
        //                //}
        //            }
        //            else if (c.HasControls())
        //            {
        //                getThTdNames(c, ref lstThNames, ref lstTdNames);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return;
        //    }
        //}
        //public static bool check_thtd_names(Control rootCtl)
        //{
        //    string[] arrThNames;
        //    string[] arrTdNames;
        //    int mismatches = 0;

        //    try
        //    {
        //        myLogger.log.Info(">>>>>> Starting UiLogic.check_thtd_names  >>>>>>>");
        //        getThTdNames(rootCtl, ref lstThNames, ref lstTdNames);
        //        arrThNames = lstThNames.ToArray();
        //        arrTdNames = lstTdNames.ToArray();
        //        if (arrThNames.Length != arrTdNames.Length)
        //        {
        //            myLogger.log.Error(string.Format("num of thNames = {0}  , num of tdNames = {1}", arrThNames.Length, arrTdNames.Length));

        //        }
        //        Array.Sort(arrThNames);
        //        Array.Sort(arrTdNames);
        //        for (int i = 0; i < arrTdNames.Length; i++)
        //        {
        //            //if (arrTdNames[i].Substring(2) == "ExtColor")
        //            //{
        //            //    int x = 17;   //debug
        //            //}

        //            if (i < arrThNames.Length)
        //            {
        //                Debug.Print(string.Format("{0}  {1}  {2}", i, arrThNames[i], arrTdNames[i]));
        //            }
        //            else
        //            {
        //                //if (arrThNames[i].Substring(2) == "ExtColor")
        //                //{
        //                //    int x = 17;  // debug 
        //                //}
        //                Debug.Print(string.Format("{0}   -  {1}", i, arrTdNames[i]));
        //            }
        //        }

        //        for (int h = 0; h < arrThNames.Length; h++)
        //        {
        //            string expectedTd = "td" + arrThNames[h].Substring(2);
        //            if (Array.IndexOf(arrTdNames, expectedTd) < 0)
        //            {
        //                myLogger.log.Error(string.Format("missing {0} in td names array", expectedTd));
        //                mismatches++;
        //            }
        //        }

        //        for (int d = 0; d < arrTdNames.Length; d++)
        //        {
        //            string expectedTh = "th" + arrTdNames[d].Substring(2);
        //            if (Array.IndexOf(arrThNames, expectedTh) < 0)
        //            {
        //                myLogger.log.Error(string.Format("missing {0} in th names array", expectedTh));
        //                mismatches++;
        //            }
        //        }

        //        myLogger.log.Info(string.Format(">>>>>> Finished UiLogic.check_thtd_names  , mismatches = {0}  >>>>>>>", mismatches));
        //        return (mismatches == 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        return false;
        //    }
        //}
        #endregion conf fields
        #region sync doorConfig with Form
        //public static void syncTabsWithDoorConfig(DoorConfig doorConfig, Control dvTab, Label lblMsg)
        //{
        //    try
        //    {
        //        for (int i = 0; i < tabNames.Length; i++)
        //        {
        //            tabValues2DoorConfig(doorConfig, dvTab, tabNames[i], lblMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        UiLogic.displayErrMsg(lblMsg, errMsg);
        //        return;
        //    }
        //}
        //public static void tabValues2DoorConfig(DoorConfig doorConfig, Control dvTab, string tabid, Label lblMsg)
        //{
        //    try
        //    {
        //        DataRow[] tabConfFields = getTabConfFields(tabid, lblMsg);
        //        string sval = string.Empty;

        //        myLogger.log.Info(string.Format("Starting TAB {0}", tabid));

        //        if (tabConfFields != null && tabConfFields.Length > 0)
        //        {
        //            for (int i = 0; i < tabConfFields.Length; i++)
        //            {
        //                DataRow row = tabConfFields[i];
        //                string fldCode = row["FIELDCODE"].ToString();
        //                // debug
        //                //if (fldCode == "D-660")
        //                //{
        //                //    int x = 17;
        //                //}
        //                // end debug
        //                ConfField_Class cFld = getConfField(fldCode, lblMsg);
        //                Control c = UiLogic.getControl(dvTab, cFld.CONFIG_FIELDNAME);
        //                myLogger.log.Info(string.Format("processing: cFld.FIELDCODE = {0}, cFld.CONFIG_FIELDNAME={1}, cFld.FIELDNAME={2}, cFld.CONFIG_SUBFORM={3}",
        //                    cFld.FIELDCODE, cFld.CONFIG_FIELDNAME, cFld.FIELDNAME, cFld.CONFIG_SUBFORM));
        //                try
        //                {
        //                    if (c.Visible)
        //                    {
        //                        if (c is DropDownList)
        //                        {
        //                            DropDownList dlst = (DropDownList)c;
        //                            sval = dlst.SelectedValue;
        //                        }
        //                        else if (c is TextBox)
        //                        {
        //                            TextBox txt = (TextBox)c;
        //                            sval = txt.Text;
        //                        }
        //                        else if (c is CheckBox)
        //                        {
        //                            CheckBox chkb = (CheckBox)c;
        //                            sval = (chkb.Checked ? "Y" : string.Empty);
        //                        }
        //                        UiLogic.setConfFieldVal(doorConfig, cFld.FIELDNAME, cFld.FIELDDATATYPE, sval, lblMsg);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    string errMsg = string.Format("error: {0},  stackTrace : {1}", ex.Message, ex.StackTrace);
        //                    myLogger.log.Error(errMsg);
        //                    displayErrMsg(lblMsg, errMsg);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
        //        myLogger.log.Error(errMsg);
        //        UiLogic.displayErrMsg(lblMsg, errMsg);
        //        return;
        //    }
        //}
        #endregion sync doorConfig with Form
    }
}