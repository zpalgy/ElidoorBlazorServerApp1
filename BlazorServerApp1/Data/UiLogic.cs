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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlazorServerApp1.Data
{
	public static class UiLogic
	{
		#region local arrays and tables
		// 07/07/2022 : removed "handle" ,  "ידית אומנותית"   tabPage . it's empty 
		public static string[] tabNames = new string[] { "divHeader", "divDoorTitle", "movingwing", "decor", "staticwing", "hinges", "accessories" }; //"extdecor", "intdecor"
		public static string[] tabTexts = new string[] { "כותרת שאלון", "כותרת דלת", "כנף נעה", "עיצוב", "כנף קבועה", "פרטי צירים", "נילווים" };  // "פרטי דקורציה חוץ", "פרטי דקורציה פנים",

		public static string[] prodButtonIDs; //= new Button[] { new Button()};
		public static string[] propNames;
		public static int[] propIndex;


		// halfwing tabPage is actually staticwing with some changes in the list of Mandatory and optional fields .
		//  so I keep the halfwing mandatory fields in a special in-memory table.
		public static List<string> lstHalfwingMfields = new List<string> { "EXTRAWINGWIDTH", "DOORHEIGHT", "CENTRALCOLWIDTH", "SWING_HANDLENAME",
										  "OPENMODE", "OPENSIDE", "COLORSNUM"};

		public static List<string> lstColorsNum1 = new List<string>();
		public static int MAX_ANYWING_W = 1400;
		public static int MIN_MOVINWING_W = 300;
		//public static int MAX_MOVINWING = 1400;

		public static int MIN_SWING_WITHLOCK_W = 300;
		public static int MAX_SMOOTH_SWING_WIDTH = 680;  //new 26/07/2023
		//public static int MAX_SWING_WITHLOCK = 1400;

		public static int MIN_SWING_NOLOCK_W = 160;
		//public static int MAX_SWING_NOLOCK = 1400;

		//public static string currentMeaged = string.Empty;
		//public static string decoreSideCode = string.Empty;

		//public static System.Web.UI.InputLanguage HebInputLang;
		//public static InputLanguage EngInputLang;
		#endregion local arrays and tables

		public static void initPropNames(DoorConfig doorConfig)
		{
			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			propNames = props.Select(i => i.Name).ToArray();
			//List<int> lstPropIndices = new List<int>();
			//for (int i = 0; i < propNames.Length; i++)
			//    lstPropIndices.Add(i);
			int dbg = Array.IndexOf(propNames, "SWINGHASLOCK");
		}
		public static int getIndOfProp(string prop)
		{
			return Array.IndexOf(propNames, prop);
		}
		public static bool hideFld(DoorConfig doorConfig, string fldName)
		{
			if (fldName.ToUpper() == "PARTNAME" || fldName.ToUpper() == "FAMILYNAME")
				return true;  // hardcoded -these fields are temporarily not visible 22/05/2022

			//debug
			if (fldName == "ELECTRICAPPARATUS")
			{
				int dbg = 17;
			}
			// end debug
			if (doorConfig != null)
			{
				bool showByMeaged = true;
				if (string.IsNullOrEmpty(doorConfig.meaged))
					showByMeaged = true;
				else if (meagedContains(doorConfig.meaged, fldName, PrApiCalls.dtMeagedFields))
					showByMeaged = true;
				else
					return true;   //showByMeaged = false;  //hide
								   // if we're here showByMeaged == true
				if (HiddenDecorSideFldsContains(fldName, PrApiCalls.dtDecorSideFlds, Helper.DecorFormat2Code(doorConfig.DECORFORMAT)))
					return true;  //hide
				else
					return false; // show
			}
			else
			{
				return false;
			}
		}
		public static bool disableFld(DoorConfig doorConfig, string fldName)
		{
			if (doorConfig != null)
			{
				string query = string.Format("TRSH_MODELNAME = '{0}' AND FIELDNAME = '{1}'", doorConfig.TRSH_MODELNAME, fldName);
				DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);
				if (rowsDefVal.Length > 0)
				{
					//string defval = rowsDefVal[0]["DEFVAL"].ToString();
					string val_locked = rowsDefVal[0]["VAL_LOCKED"].ToString();
					doorConfig.disabledFlds[fldName] = (val_locked == "Y");
					return (val_locked == "Y");
				}
				return false;
			}
			else
				return false;
		}

		public static bool disableOption(DoorConfig doorConfig, string configFldName, string optionVal)
		{
			//configFldName = configFldName.ToUpper();
			if (doorConfig != null)
			{
				if (string.IsNullOrEmpty(doorConfig.PARTNAME))
					return false;

				string query = string.Format("TRSH_MODELNAME = '{0}' AND CONFIG_FIELDNAME = '{1}'", doorConfig.TRSH_MODELNAME, configFldName);
				DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);

				for (int r = 0; r < rowsDefVal.Length; r++)
				{
					string wrongval = rowsDefVal[r]["WRONGVAL"].ToString();
					if (optionVal == wrongval)
						return true;  //disable option
				}
				return false;
			}
			else
				return false;
		}
		public static bool disableOption2(DoorConfig doorConfig, string fldName, string optionVal)
		{
			//configFldName = configFldName.ToUpper();
			if (doorConfig != null)
			{
				if (string.IsNullOrEmpty(doorConfig.PARTNAME))
					return false;

				if (("101" + "מחסנים " + "ניקוב חצי צילינדר").Contains(optionVal))
				{
					int dbg = 17;
				}

				string query = string.Format("TRSH_MODELNAME = '{0}' AND FIELDNAME = '{1}'", doorConfig.TRSH_MODELNAME, fldName);
				DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);

				for (int r = 0; r < rowsDefVal.Length; r++)
				{
					string wrongval = rowsDefVal[r]["WRONGVAL"].ToString();
					if (optionVal == wrongval)
						return true;  //disable option
				}
				return false;
			}
			else
				return false;
		}
		public static bool disableOption3(DoorConfig doorConfig, string fldName, string optionVal)
		{
			//configFldName = configFldName.ToUpper();
			if (doorConfig != null)
			{
				if (string.IsNullOrEmpty(doorConfig.PARTNAME))
					return false;

				#region toDEL
				// string query = string.Format("TRSH_MODELNAME = '{0}' AND FIELDNAME = '{1}'", doorConfig.TRSH_MODELNAME, fldName);
				//DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);

				//for (int r = 0; r < rowsDefVal.Length; r++)
				//{
				//	string wrongval = rowsDefVal[r]["WRONGVAL"].ToString();
				//	if (optionVal == wrongval)
				//		return true;  //disable option
				//}
				//return false;
				#endregion toDel
				Defaults_Class default_rec = PrApiCalls.lstDefaults.Find(x => x.FAMILYNAME == doorConfig.FAMILYNAME
													&& x.FIELDNAME == fldName && x.WRONGVAL == optionVal);
				return (default_rec != null);
			}
			else
				return false;
		}
		public static bool disableOptionNoPart(DoorConfig doorConfig, string fldName, string optionVal)
		{
			if (doorConfig != null)
			{
				Defaults_Class default_rec = PrApiCalls.lstDefaults.Find(x => string.IsNullOrEmpty(x.FAMILYNAME) && x.FIELDNAME == fldName && x.WRONGVAL == optionVal);
				return (default_rec != null);
			}
			else
				return false;
		}
		#region saveRestore DoorConfig in SessionStore
		public static async void saveDoorConfig(DoorConfig doorConfig, ProtectedSessionStorage ProtectedSessionStore)
		{
			string doorConfigJson = PrApiCalls.JsonSerializer<DoorConfig>(doorConfig);
			await ProtectedSessionStore.SetAsync("doorConfigJson", doorConfigJson);
			string doorConfigJson2 = ProtectedSessionStore.GetAsync<string>("doorConfigJson").ToString();
		}
		public static async void restoreDoorConfig(ProtectedSessionStorage ProtectedSessionStore, DoorConfig doorConfig)
		{
			var doorConfigJsonV = await ProtectedSessionStore.GetAsync<string>("doorConfigJson");
			if (doorConfigJsonV.Success)
			{
				string doorConfigJson2 = doorConfigJsonV.Value;
				if (!string.IsNullOrEmpty(doorConfigJson2))
					doorConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<DoorConfig>(doorConfigJson2);
			}
		}
		#endregion saveRestore DoorConfig in SessionStore

		public static bool tabPageIsNotEmpty(string tabName, DoorConfig doorConfig)
		{
			string query = string.Format("CONFIG_SUBFORM = '{0}'", tabName.ToLower());
			DataRow[] tabFields = PrApiCalls.dtConfFields.Select(query);
			int fieldsNum = tabFields.Length;

			for (int r = 0; r < fieldsNum; r++)
			{
				string fldName = tabFields[r]["FIELDNAME"].ToString();
				string fldDataType = tabFields[r]["FIELDDATATYPE"].ToString();
				string controlName = tabFields[r]["CONFIG_FIELDNAME"].ToString();
				string controlThName = tabFields[r]["CONFIG_THNAME"].ToString();
				string fldDes = tabFields[r]["FIELDDES"].ToString();
				//borderColor = string.Empty;

				if (!hideFld(doorConfig, controlThName)
					&& !controlName.StartsWith("chkb")
					&& doorFldIsNotEmpty(doorConfig, fldName, fldDataType))
				{
					return true;
				}
			}
			return false;  // if we're here no field was non-empty 
		}
		public static bool tabPageIsFilled(string tabName, DoorConfig doorConfig)
		{
			//if (tabName == "movingwing")
			//{
			//    string nextTabName = getNextTabName(doorConfig, tabName);

			//    if (   nextTabName == "staticwing" 
			//        && doorConfig.TRSH_MODELNAME == "MLI"
			//        && doorConfig.TRSH_WINGSNUMDES == "כנף וחצי" )
			//        return true;     //11/07/2022 - for MLI + 1.5 wings allow moving from movingwing to staticwing even if movingwing is not filled .
			//}
			if (doorConfig.TRSH_WINGSNUMDES == HebNouns.HalfWing && tabName == "staticwing")
			{
				return halfWingIsFilled(doorConfig);
			}

			string query = string.Format("CONFIG_SUBFORM = '{0}'", tabName.ToLower());
			DataRow[] tabFields = PrApiCalls.dtConfFields.Select(query);
			int fieldsNum = tabFields.Length;
			bool isFilled = true;
			bool isFilled2 = true;

			if (tabName == "decor")
			{
				int dbg = 17; //debug
			}

			for (int r = 0; r < fieldsNum; r++)
			{
				string fldName = tabFields[r]["FIELDNAME"].ToString();
				string fldDataType = tabFields[r]["FIELDDATATYPE"].ToString();
				string controlName = tabFields[r]["CONFIG_FIELDNAME"].ToString();
				string controlThName = tabFields[r]["CONFIG_THNAME"].ToString();
				string fldDes = tabFields[r]["FIELDDES"].ToString();
				bool fldIsMandatory = (tabFields[r]["MANDATORY"].ToString() == "M");
				//borderColor = string.Empty;

				if (fldName == "LOCKDRILHEIGHT")
				{
					int dbg = 18;
				}

				if (!hideFld(doorConfig, controlThName)
					&& !controlName.StartsWith("chkb")
					//&& fldIsMandatory
					&& !doorFldIsFilled(doorConfig, fldName, fldDataType))
				//&& thTdFIELDNAME.ToUpper() != "REFERENCE"
				//&& thTdFIELDNAME.ToUpper() != "FORMDATE"
				//&& thTdFIELDNAME.ToUpper() != "TMPSHIPADDRESS"
				//&& thTdFIELDNAME.ToUpper() != "FAMILYNAME")
				{
					if (fldName == "HWACCESSORYID")
					{
						if (doorConfig.TRSH_HARDWARE != 0)    // do not change the border color of HWACCESSORYID to red
															  // if TRSH_HARDWARE is empty.
							doorConfig.borderColors[fldName] = "redBorder";
					}
					//else if (fldName == "LOCKDRILHEIGHT" && "D-CLR A-GLVN".Contains(doorConfig.TRSH_MODELNAME))
					//{
					//    doorConfig.borderColors["LOCKDRILHEIGHT"] = (doorConfig.LOCKDRILHEIGHT >= 0 ? "blueBorder" : "redBorder");
					//}
					else if (doorConfig.COLORSNUM == "2")
					{
						doorConfig.borderColors["EXTCOLORID"] = (doorConfig.EXTCOLORID != 0 ? "blueBorder" : "redBorder");   //hardcoded this, because for unknow reason EXCOLORID 
						doorConfig.borderColors["INTCOLORID"] = (doorConfig.INTCOLORID != 0 ? "blueBorder" : "redBorder");   // does not have redBorder when COLORNUM is 2 while
																															 // INTCOLORID is !
					}
					else
					{
						//borderColor = "redBorder";
						doorConfig.borderColors[fldName] = "redBorder";
					}


					isFilled = false;
					//return false;
				}
				else
				{
					if (fldName == "INTCOLORID")
					{
						int dbg = 17;
					}
					doorConfig.borderColors[fldName] = "blueBorder";  //new 05/11/2022
				}

			}

			if (tabName == "movingwing" && isFilled && doorConfig.DECORFORMAT != HebNouns.None)
				isFilled2 = tabPageIsFilled("decor", doorConfig);

			return isFilled && isFilled2;
		}
		public static bool halfWingIsFilled(DoorConfig doorConfig)
		{
			bool isFilled = true;
			foreach (string fldName in lstHalfwingMfields)
			{
				string fldDataType = getFldDataType(fldName);
				if (!doorFldIsFilled(doorConfig, fldName, fldDataType))
				{
					//borderColor = "redBorder";
					doorConfig.borderColors[fldName] = "redBorder";
					isFilled = false;
				}
			}
			//Hardcode for Halfwing SWING_HANDLENAME and SWING_HANDLECOLORID 
			if (PrApiCalls.isHandleColored(doorConfig.SWING_HANDLENAME) && doorConfig.SWING_HANDLECOLORID == 0)
				isFilled = false;
			return isFilled;
		}

		public static bool try2UpdateBtnClass(DoorConfig doorConfig, string tabName) //  , string tabBtn, string nextTabBtn )
		{
			string tabBtnCssName = getTabBtnCssName(doorConfig, tabName);
			string nextTabBtnCssName = getNextTabBtnCssName(doorConfig, tabBtnCssName);
			if (UiLogic.tabPageIsFilled(tabName, doorConfig))
			{
				doorConfig.btnClasses[tabBtnCssName] = "buttonFilled";
				doorConfig.btnClasses[nextTabBtnCssName] = "buttonActive";
				// 4/11/2022 based on Tzvi Okun's mail on 31/10/2022 : hardcode the case of staticwing filled skips hinges and jumps to accesspries . 
				// make also hinges button acive !
				if (tabName == "staticwing" && nextTabBtnCssName == "accessories")
					doorConfig.btnClasses[getTabBtnCssName(doorConfig, "hinges")] = "buttonActive";
				return true;
			}
			else
			{
				doorConfig.btnClasses[tabBtnCssName] = "buttonActive";
				if (doorConfig.btnClasses[nextTabBtnCssName] == "buttonActive")  // it was already set to ACTIVE - we rollback this
				{
					doorConfig.btnClasses[nextTabBtnCssName] = "button";
					return true;  // rollback button colors
				}
				else
					return false;
			}
		}
		public static bool deactivateBtnClass(DoorConfig doorConfig, string tabName)
		{
			string tabBtnCssName = getTabBtnCssName(doorConfig, tabName);
			string nextTabBtnCssName = getNextTabBtnCssName(doorConfig, tabBtnCssName);
			if (!UiLogic.tabPageIsFilled(tabName, doorConfig))
			{
				doorConfig.btnClasses[tabBtnCssName] = "buttonActive";  //"buttonFilled";
				doorConfig.btnClasses[nextTabBtnCssName] = "button";    //"buttonActive";
				return true;
			}
			else
				return false;
		}
		public static string getNextTabName(DoorConfig doorConfig, string tabName)
		{
			string nextTab = string.Empty;

			if (tabName == "staticwing")
			{
				return "accessories";      //24/10/2022
			}

			int t = Array.IndexOf(tabNames, tabName);
			if (t > -1 && t < tabNames.Length - 1)
			{
				nextTab = tabNames[t + 1];
				//if (   (nextTab == "extdecor" && doorConfig.DECORFORMAT == "פנים")
				//    || (nextTab == "intdecor" && doorConfig.DECORFORMAT == "חוץ")
				//|| (nextTab == "hinges") - commented on 14/09/2022
				//    )
				//{
				//    int t1 = Array.IndexOf(tabNames, nextTab);
				//    nextTab = tabNames[t1 + 1];   // staticwing TAB is diabled - skip it and skip hinges tab
				//}

				if (nextTab == "decor")  //&& doorConfig.DECORFORMAT == "ללא")  // == UiLogic.None 
				{
					int t2 = Array.IndexOf(tabNames, nextTab);
					nextTab = tabNames[t2 + 1];  // new 
				}
				if (nextTab == "movingwing" && doorConfig.TRSH_WINGSNUMDES == HebNouns.HalfWing)  //"חצי כנף")
				{
					nextTab = "staticwing";   //13/07/2022 
				}
				if (nextTab == "staticwing" && doorConfig.TRSH_WINGSNUMDES == HebNouns.Wing)     //"כנף")
				{
					int t3 = Array.IndexOf(tabNames, nextTab);
					nextTab = tabNames[t3 + 1];
				}
			}
			return nextTab;
		}
		public static string getTabBtnCssName(DoorConfig doorConfig, string tabName)
		{
			switch (tabName)
			{
				case "divHeader":
					return "general";
				case "divProducts":
					return "selectprod";
				case "divDoorTitle":
					return "proddes";
				default:
					return tabName;   // movingwing, extdecor ....
			}
		}
		public static string getNextTabBtnCssName(DoorConfig doorConfig, string tabBtnCssName)
		{
			switch (tabBtnCssName)
			{
				case "general":
					return "selectprod";
				case "selectprod":
					return "proddes";
				case "proddes":
					if (doorConfig.TRSH_WINGSNUMDES == HebNouns.HalfWing)  //"חצי כנף")
						return "staticwing";
					else
						return "movingwing";
				default:
					return getNextTabName(doorConfig, tabBtnCssName);  // movingwing, decor ....
			}
		}

		public static void disableTabFlds(DoorConfig doorConfig, string tabName)
		{
			string query = string.Format("CONFIG_SUBFORM = '{0}'", tabName.ToLower());
			DataRow[] tabFields = PrApiCalls.dtConfFields.Select(query);
			int fieldsNum = tabFields.Length;
			if (tabName.ToLower().StartsWith("movin"))
			{
				int dbg = 17;
			}

			for (int r = 0; r < fieldsNum; r++)
			{
				string fldName = tabFields[r]["FIELDNAME"].ToString();
				string fldDataType = tabFields[r]["FIELDDATATYPE"].ToString();
				string controlName = tabFields[r]["CONFIG_FIELDNAME"].ToString();
				string controlThName = tabFields[r]["CONFIG_THNAME"].ToString();
				string fldDes = tabFields[r]["FIELDDES"].ToString();

				doorConfig.disabledFlds[fldName] = true;
				if (fldName == "HANDLENAME")
				{
					string dbg = doorConfig.HANDLENAME;
				}
			}
		}
		public static bool doorFldIsNotEmpty(DoorConfig doorConfig, string fldName, string fldDataType)
		{
			string errMsg = string.Empty;
			string sval;
			int ival;

			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			//string[] propNames = props.Select(i => i.Name).ToArray();
			try
			{
				//  debug            ELECTRICAPPARATUS
				if (fldName == "ELECTRICAPPARATUS")
				{
					int x = 17;
				}
				//

				int p = Array.IndexOf(propNames, fldName);
				if (p >= 0)
				{
					var val = props[p].GetValue(doorConfig);
					if (val == null)
						return false;
					else
					{
						switch (fldDataType)
						{
							case "CHAR":
							case "RCHAR":
								sval = val.ToString();
								// commented on 03/05/2022 ללא is not the empty value because it is in the defaults table
								//                  note לא that is also in the defaults table appears also in pair with חוץ 
								//                     and in that case I use חוץ  as the default value.  (e.g. doors : 1082, 2002 etc. )
								//                     note also that the value פנים does not appear in the defaults table for field D-60 DECORFORMAT
								//                    so I updated the code accordingly.
								//                    i.e. DECORFORMAT is not filled when it's value is empty as all the other fields.
								//
								//if ( (string.IsNullOrEmpty(sval.Trim()) || sval.Trim() == " ")
								//    || (thTdFIELDNAME == "DECORFORMAT" && sval == "ללא" ))    //special for DECORFORMAT !
								//-- 
								if (string.IsNullOrEmpty(sval.Trim()) || sval.Trim() == " ")
									return false;
								else
									return true;

							case "INT":
								ival = int.Parse(val.ToString());
								return (ival != 0);
							default:
								return false;
						}
					}
				}
				else
				{
					errMsg = string.Format("Error: field: {0}, dataType {1}  Not found in DoorConfig class !", fldName, fldDataType);
					myLogger.log.Error(errMsg);
					throw new Exception(errMsg);
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
				//displayErrMsg(lblMsg, errMsg);
			}
		}
		public static bool doorFldIsFilled(DoorConfig doorConfig, string fldName, string fldDataType)
		{
			string errMsg = string.Empty;
			string sval;
			int ival;

			if (fldName == "INTSEPLINES")
			{
				int dbg = 18;
			}

			if (!fldIsMandatory(doorConfig, fldName))
			{
				//bool dbg2 = fldIsMandatory(fldName);
				return true;
			}
			//if (optionalFields.Contains(fldName))
			//    return true;
			if (doorConfig.disabledFlds.ContainsKey(fldName) && doorConfig.disabledFlds[fldName])
				return true;
			if (fldName == "HWCOLORID")
				return isHWCOLORID_filled(doorConfig);
			if (fldName == "DRIL4HW")
				return isDRIL4HW_filled(doorConfig);
			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			//string[] propNames = props.Select(i => i.Name).ToArray();
			try
			{
				// special logic for Hinges 
				if (fldName == "HINGE5HEIGHT" && doorConfig.HINGESNUM < 5)
					return true;

				else if (fldName == "HINGE4HEIGHT")  //new UiLogic for the hidden hinge hight fields  HINGE4HEIGHT, HINGE2HEIGHT
				{
					if (doorConfig.HINGESNUM < 4)
						return true;
					else if (doorConfig.HINGESNUM == 4 && doorConfig.HINGE4HEIGHT == 0 && HeightRange(doorConfig.DOORHEIGHT, doorConfig) == 4)
						return true;
				}
				else if (fldName == "HINGE3HEIGHT" && doorConfig.HINGESNUM < 3)
					return true;

				else if (fldName == "HINGE2HEIGHT"
										 && doorConfig.HINGESNUM == 2 && doorConfig.HINGE2HEIGHT == 0 && HeightRange(doorConfig.DOORHEIGHT, doorConfig) == 2)
					return true;

				else if (fldName == "DOORCOLORID" && doorConfig.COLORSNUM != "1")
					return true;
				else if (fldName == "EXTCOLORID" && doorConfig.COLORSNUM != "2")
					return true;
				else if (fldName == "INTCOLORID" && doorConfig.COLORSNUM != "2")
					return true;
				else if (fldName == "LOCKDRILHEIGHT" && "D-CLR A-GLVN".Contains(doorConfig.TRSH_MODELNAME) && doorConfig.LOCKDRILHEIGHT >= 0)
					// 13/02/2023 : change requested by Eli 
					return true;


				int p = Array.IndexOf(propNames, fldName);
				if (p >= 0)
				{
					var val = props[p].GetValue(doorConfig);
					if (val == null)
					{
						//errMsg = string.Format("Unexpected error: fldname = {0} , val == null", fldName);
						//myLogger.log.Error(errMsg);
						return false;
					}
					else
					{
						if (fldDataType == null)
						{
							errMsg = string.Format("Unexpected error: fldname = {0} , fldDataType == null", fldName);
							myLogger.log.Error(errMsg);
						}
						switch (fldDataType)
						{
							case "CHAR":
							case "RCHAR":
								sval = val.ToString();
								// commented on 03/05/2022 ללא is not the empty value because it is in the defaults table
								//                  note לא that is also in the defaults table appears also in pair with חוץ 
								//                     and in that case I use חוץ  as the default value.  (e.g. doors : 1082, 2002 etc. )
								//                     note also that the value פנים does not appear in the defaults table for field D-60 DECORFORMAT
								//                    so I updated the code accordingly.
								//                    i.e. DECORFORMAT is not filled when it's value is empty as all the other fields.
								//
								//if ( (string.IsNullOrEmpty(sval.Trim()) || sval.Trim() == " ")
								//    || (thTdFIELDNAME == "DECORFORMAT" && sval == "ללא" ))    //special for DECORFORMAT !
								//-- 
								if (string.IsNullOrEmpty(sval.Trim()) || sval.Trim() == " ")
									return false;
								else
									return true;

							case "INT":
								ival = int.Parse(val.ToString());
								return (ival != 0);
							default:
								return false;
						}
					}
				}
				else
				{
					errMsg = string.Format("Error: field: {0}, dataType {1}  Not found in DoorConfig class !", fldName, fldDataType);
					myLogger.log.Error(errMsg);
					throw new Exception(errMsg);
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
				//displayErrMsg(lblMsg, errMsg);
			}
		}
		public static bool isHWCOLORID_filled(DoorConfig doorConfig)
		{
			string errMsg = string.Empty;
			try
			{
				doorConfig.disabledFlds["HWCOLORID"] = !PrApiCalls.isHWColored(doorConfig.TRSH_HARDWARE);
				bool isFilled = (doorConfig.disabledFlds["HWCOLORID"] ? true : (doorConfig.HWCOLORID != 0));
				return isFilled;
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
			}
		}
		public static bool isDRIL4HW_filled(DoorConfig doorConfig)
		{
			// 26/03/2023 I don't remember the reason for changing doorConfig.disabledFlds["DRIL4HW"] here, in a method that just checks
			//   whether DRIL4HW field is filled , so in case of DECORATED door I do not modify this property.
			try
			{
				if (!UiLogic.isDecorated(doorConfig))
					doorConfig.disabledFlds["DRIL4HW"] = (doorConfig.TRSH_HARDWARE == HebNouns.IdOfNone);

				bool isFilled = (doorConfig.disabledFlds["DRIL4HW"] ? true : (doorConfig.DRIL4HW != 0));
				return isFilled;
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
			}
		}
		public static void calcCentralColClrID(DoorConfig doorConfig)
		{
			switch (doorConfig.COLORSNUM)
			{
				case "מגולוון":
				case "1":
					doorConfig.CENTRALCOLCLRID = doorConfig.DOORCOLORID;
					break;
				case "2":
					doorConfig.CENTRALCOLCLRID = (doorConfig.OPENMODE == "פנימה" ? doorConfig.INTCOLORID : doorConfig.EXTCOLORID);
					break;
			}
		}
		public static void applySwingHasLock(DoorConfig doorConfig)
		{
			//Staticwing   setHiddens() logic
			bool hideCentralColClr = UiLogic.hideFld(doorConfig, "CentralColClrID");
			bool hideSwingClr = UiLogic.hideFld(doorConfig, "SwingColorID");
			bool hideSwingExtClr = UiLogic.hideFld(doorConfig, "SwingExtColorID");
			bool hideSwingIntClr = UiLogic.hideFld(doorConfig, "SwingIntColorID");
			bool hideSwingCyl = UiLogic.hideFld(doorConfig, "Cylinder4HalfWing");
			bool hideHW4ExtraWing = UiLogic.hideFld(doorConfig, "thHW4ExtraWing");
			bool hideSwingHwAcc = UiLogic.hideFld(doorConfig, "SWING_HWACCESSORYID");
			bool hideSwingHwClr = UiLogic.hideFld(doorConfig, "SWING_HWCOLORID");
			bool hideSwingHandle = UiLogic.hideFld(doorConfig, "SWING_HANDLENAME");
			bool hideSwingHandleClr = UiLogic.hideFld(doorConfig, "SWING_HANDLECOLORID");


			//bool _SwingHasLock = (doorConfig.SWINGHASLOCK == "Y");

			if (doorConfig.SWINGHASLOCK != "Y")
			{
				doorConfig.HW4EXTRAWING = 0;
				doorConfig.SWING_HWACCESSORYID = 0;
				doorConfig.SWING_HWCOLORID = 0;
				doorConfig.SWING_DRIL4HW = 0;
				doorConfig.TRSH_SWING_CYLINDER = 0;
				doorConfig.SWING_LOCKNAME = string.Empty;

				//doorConfig.TURBOAPPARATUS = string.Empty;  //06/07/2022  TURBOAPPARATUS is N when StaticWing Doesn't have a LOCK !
				//doorConfig.LOCKDRILHEIGHT = 0;
				//doorConfig.SWING_TURBO = string.Empty;   // 11/07/2022 TURBO is affected ONLY by  Cust.TRSH_NOTECOMPLIENT
				//  so we don't change it here inside applySwingHasLock() !

				doorConfig.thClasses["LOCKDRILHEIGHT"] =
				doorConfig.thClasses["TRSH_SWING_CYLINDER"] =
				doorConfig.thClasses["HW4EXTRAWING"] =
				doorConfig.thClasses["SWING_HWACCESSORYID"] =
				doorConfig.thClasses["SWING_HWCOLORID"] =
				doorConfig.thClasses["SWING_DRIL4HW"] =
				doorConfig.thClasses["SWING_LOCKNAME"] =
				doorConfig.thClasses["SWING_TURBO"] = "thGray";      //TODO - check this !
			}
			else  // withLOCK 
			{
				doorConfig.thClasses["LOCKDRILHEIGHT"] = "thBlue";
				doorConfig.thClasses["TRSH_SWING_CYLINDER"] = (hideSwingCyl ? "thGray" : "thBlue");
				doorConfig.thClasses["HW4EXTRAWING"] = (hideHW4ExtraWing ? "thGray" : "thBlue");
				doorConfig.thClasses["SWING_HWACCESSORYID"] = (hideSwingHwAcc ? "thGray" : "thBlue");
				doorConfig.thClasses["SWING_HWCOLORID"] = (hideSwingHwClr ? "thGray" : "thBlue");
				doorConfig.thClasses["SWING_DRIL4HW"] = "thBlue";
				doorConfig.thClasses["SWING_TURBO"] = "thGray";
				doorConfig.thClasses["SWING_LOCKNAME"] = "thBlue";

				if (!hideSwingCyl)
					doorConfig.borderColors["TRSH_SWING_CYLINDER"] = (doorConfig.TRSH_SWING_CYLINDER != 0 ? "blueBorder" : "redBorder");  // special for this field
																																		  // that was changed from disabled to enabled+mandatory when SWINGHASLOCK was changed !

				// we do the following two statements in Staticwing.razor : applySwingHasLock(...) (a local method in Staticwing.razor). It is called also by OnInitializedAsync.
				// we can't do that here in UiLogic.cs, because lstCylinders1 can't be populated in Halfwing OnInitializedAsync, bacause OPENMODE was not SET yet
				//  at that moment.
				//
				//CYLINDER_Class noCyl = lstCylinders1.Find(x => x.PARTNAME == UiLogic.NameOfNone);
				//lstCylinders1.Remove(noCyl);

			}
			//if (doorConfig.TRSH_WINGSNUMDES != "חצי כנף")
			//{
			doorConfig.disabledFlds["LOCKDRILHEIGHT"] = (doorConfig.SWINGHASLOCK != "Y");
			doorConfig.disabledFlds["TRSH_SWING_CYLINDER"] = hideSwingCyl || (doorConfig.SWINGHASLOCK != "Y");

			// 06/07/2022 HW4EXTRAWING is disabled by default !
			doorConfig.disabledFlds["HW4EXTRAWING"] = hideHW4ExtraWing || (doorConfig.SWINGHASLOCK != "Y");
			// SWING_HWACCESSORYID and SWING_HWCOLORID are enabled only after HW4EXTRAWING was set - here they're disabled.
			doorConfig.disabledFlds["SWING_HWACCESSORYID"] = true; //hideSwingHwAcc || (doorConfig.SWINGHASLOCK != "Y");
			doorConfig.disabledFlds["SWING_HWCOLORID"] = true;     //hideSwingHwClr || (doorConfig.SWINGHASLOCK != "Y");
			doorConfig.disabledFlds["SWING_DRIL4HW"] = (doorConfig.SWINGHASLOCK != "Y");
			doorConfig.disabledFlds["SWING_TURBO"] = (doorConfig.SWINGHASLOCK != "Y");    //TODO - check this ?
			doorConfig.disabledFlds["SWING_LOCKNAME"] = (doorConfig.SWINGHASLOCK != "Y");
			//}
		}
		public static void clearFollowingTabFields(DoorConfig doorConfig, string tabName)
		{
			string errMsg = string.Empty;
			int t = Array.IndexOf(tabNames, tabName);
			if (t > -1 && t < tabNames.Length - 1)
			{
				//string nextTab = tabNames[t + 1];
				//if (nextTab == "staticwing" && doorConfig.TRSH_WINGSNUMDES == "כנף")
				//{
				//    nextTab = tabNames[t + 3];  // staticwing TAB is diabled - skip it and skip hinges tab
				//}
				//if (nextTab == "hinges")
				//{
				//    nextTab = tabNames[t + 2];
				//}
				string nextTab = getNextTabName(doorConfig, tabName);
				if (nextTab == "hinges")
				{
					t = Array.IndexOf(tabNames, "hinges");
					nextTab = tabNames[t + 1];
				}
				string query = string.Format("CONFIG_SUBFORM = '{0}'", nextTab.ToLower());
				DataRow[] tabFields = PrApiCalls.dtConfFields.Select(query);
				for (int r = 0; r < tabFields.Length; r++)
				{
					string fldName = tabFields[r]["FIELDNAME"].ToString();
					//string fldDataType = tabFields[r]["FIELDDATATYPE"].ToString();
					//clearConfField(doorConfig, fldName, fldDataType, ref errMsg);
					clearConfField(doorConfig, fldName, ref errMsg);
					applyFldDefault(doorConfig, fldName);
				}
			}
		}
		//setHingesAndWindowsData(...) is called ONLY after DOORHEIGHT is set in Movingwing.razor !
		public static void setHingesAndWindowsData(DoorConfig doorConfig, ref string errMsg)
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
				doorConfig.BACKPINHEIGHT = (doorConfig.LOCKDRILHEIGHT > 0 ? doorConfig.LOCKDRILHEIGHT + 55 : 0);  //Eli 26/12/2022
				doorConfig.HINGESNUM = int.Parse(rowsArray[0]["HINGESNUM"].ToString());
				doorConfig.HINGE1HEIGHT = int.Parse(rowsArray[0]["HINGE1HEIGHT"].ToString());

				// doorConfig.HINGE2HEIGHT = int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString());

				doorConfig.HINGE2HEIGHT = (UiLogic.showHinge2(doorConfig) ? int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString()) : 0); // created on 05/11/2022 


				// new 05/11/2022 - set doorConfig.optionalHingeHeight
				doorConfig.optionalHingeHeight = 0;
				if (doorConfig.HINGESNUM == 2 && int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString()) > 0)
					doorConfig.optionalHingeHeight = int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString());
				else if (doorConfig.HINGESNUM == 4 && int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString()) > 0)
					doorConfig.optionalHingeHeight = int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString());

				// new 05/11/2022
				doorConfig.HINGE3HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());

				//doorConfig.HINGE4HEIGHT = int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString());
				doorConfig.HINGE4HEIGHT = (UiLogic.showHinge4(doorConfig) ? int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString()) : 0); // created on 05/11/2022

				doorConfig.HINGE5HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());

				//21/12/2022
				if (HeightRange(doorConfig.DOORHEIGHT, doorConfig) == 2 && !UiLogic.showHinge2(doorConfig))
				{
					doorConfig.HINGE2HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());
					doorConfig.HINGE3HEIGHT = 0;
				}
				if (HeightRange(doorConfig.DOORHEIGHT, doorConfig) == 4 && !UiLogic.showHinge4(doorConfig))
				{
					doorConfig.HINGE4HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());
					doorConfig.HINGE5HEIGHT = 0;
				}
				//  end of new logic 21/12/2022
				// old logic before 05/11/2022 
				if (false)
				{
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
		//
		public static void setHingesHeights(DoorConfig doorConfig, ref string errMsg)
		{
			try
			{
				string query = string.Format("TRSH_DOOR_HWCATCODE = {0} AND DOORHEIGHTMIN <= {1} AND {1} <= DOORHEIGHTMAX", doorConfig.TRSH_DOOR_HWCATCODE, doorConfig.DOORHEIGHT);
				DataRow[] rowsArray = PrApiCalls.dtLock_Hinge_Dril_Heights.Select(query);
				if (rowsArray.Length == 0)
				{
					errMsg = string.Format("לא נמצאה שורת 'מידות צירים וניקוב (תואם אלידור)' מתאימה לקטגורית הפרזול {0} ולגובה הדלת {1} - אנא בדוק את הטבלה הזו",
									doorConfig.TRSH_DOOR_HWCATCODE, doorConfig.DOORHEIGHT);
					//UiLogic.displayErrMsg(lblMsgL1, errMsg);
					return;
				}

				doorConfig.HINGE1HEIGHT = int.Parse(rowsArray[0]["HINGE1HEIGHT"].ToString());

				// 21/12/2022 : per Eli's request commented the following two  lines and replaced it by the other lines.
				//doorConfig.HINGE2HEIGHT = (showHinge2(doorConfig) ? int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString()) : 0);
				//doorConfig.HINGE3HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());
				if (showHinge2(doorConfig))
				{
					doorConfig.HINGE2HEIGHT = int.Parse(rowsArray[0]["HINGE2HEIGHT"].ToString());
					doorConfig.HINGE3HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());
				}
				else
				{
					doorConfig.HINGE2HEIGHT = int.Parse(rowsArray[0]["HINGE3HEIGHT"].ToString());
					doorConfig.HINGE3HEIGHT = 0;
				}

				// doorConfig.HINGE4HEIGHT = (showHinge4(doorConfig) ? int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString()) : 0);
				// doorConfig.HINGE5HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());
				if (showHinge4(doorConfig))
				{
					doorConfig.HINGE4HEIGHT = int.Parse(rowsArray[0]["HINGE4HEIGHT"].ToString());
					doorConfig.HINGE5HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());
				}
				else
				{
					doorConfig.HINGE4HEIGHT = int.Parse(rowsArray[0]["HINGE5HEIGHT"].ToString());
					doorConfig.HINGE5HEIGHT = 0;
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
		public static int HeightRange(int DOORHEIGHT, DoorConfig doorConfig)
		{
			//         int res = 0;
			//         int[] hRanges = new int[] { 1050, 1450, 1890, 2431 };

			//         for (int i = 0; i < 4; i++)
			//         {
			//             if (DOORHEIGHT < hRanges[i])
			//             {
			//                 //return i;
			//                 res = i;
			//                 break;
			//             }
			//         }
			//         if (DOORHEIGHT >= hRanges[3])
			//         {
			//             //return 4;
			//             res = 4;
			//         }
			//		//return 0;  // error !
			//		//return res;

			string query = string.Format("TRSH_DOOR_HWCATCODE = {0} AND DOORHEIGHTMIN <= {1} AND {1} <= DOORHEIGHTMAX", doorConfig.TRSH_DOOR_HWCATCODE, doorConfig.DOORHEIGHT);
			DataRow[] rowsArray = PrApiCalls.dtLock_Hinge_Dril_Heights.Select(query);
			int hingesNum = int.Parse(rowsArray[0]["HINGESNUM"].ToString());
			int res2 = (hingesNum == 0 ? 1 : hingesNum);
			//if (res != res2)
			//{
			//    int dbg = 17;
			//}
			return res2;
		}
		public static bool showHinge2(DoorConfig doorConfig)
		{
			switch (HeightRange(doorConfig.DOORHEIGHT, doorConfig))
			{
				case 2:
					if (doorConfig.HINGESNUM == 2)
						return false;
					else if (doorConfig.HINGESNUM == 3)
						return true;
					break;
				default:
					return true;
			}
			return true;   // default !
		}
		public static bool showHinge4(DoorConfig doorConfig)
		{
			switch (HeightRange(doorConfig.DOORHEIGHT, doorConfig))
			{
				case 4:
					if (doorConfig.HINGESNUM == 4)
						return false;
					else if (doorConfig.HINGESNUM == 5)
						return true;
					break;

				default:
					return true;
			}
			return true;   // default !
		}

		//
		public static int calcWindowHeight(DoorConfig doorConfig, ref string errMsg)
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
		public static void clearDoorConfig(DoorConfig doorConfig, bool applyDefaults = true)
		{
			string errMsg = string.Empty;
			doorConfig.PARTNAME = string.Empty;
			doorConfig.COMMENTS = string.Empty;

			// clear NonSerialized scalar/non-dictionary/table/List fields
			doorConfig.useLOGO = false;
			doorConfig.useTurbo = false;
			doorConfig.handleName1 = string.Empty;
			doorConfig.optionalHingeHeight = 0;
			doorConfig.staticwingChanged = false;
			doorConfig.showDecor = false;
			doorConfig.LockDrilHChanged = false;
			doorConfig.meaged = string.Empty;
			doorConfig.currPropName = String.Empty;
			doorConfig.currTabName = String.Empty;
			doorConfig.prevTabName = string.Empty;
			doorConfig.LockDrilHMeasure = string.Empty;
			doorConfig.TRSH_CYLCATEGORY = 0;
			doorConfig.decor = null;

			for (int r = 0; r < PrApiCalls.dtConfFields.Rows.Count; r++)
			{
				DataRow row = PrApiCalls.dtConfFields.Rows[r];
				string fldname = row["FIELDNAME"].ToString();
				//debug
				if (fldname == "TRSH_CYLINDER")
				{
					int x = 17;
				}
				//end debug
				string dataType = row["FIELDDATATYPE"].ToString();
				if (dataType != "Date" && fldname != "PARTNAME")
				{
					//clearConfField(doorConfig, fldname, dataType, ref errMsg);
					clearConfField(doorConfig, fldname, ref errMsg);
				}
				if (applyDefaults)
					applyFldDefault(doorConfig, fldname);
			}
			doorConfig.initBorderColors();
			UiLogic.tabPageIsFilled("divHeader", doorConfig);  //set redborder on Required fields in divHeader
			doorConfig.staticwingChanged = false;
		}
		public static void clearDisabledFields(DoorConfig doorConfig)
		{
			string errMsg = string.Empty;
			foreach (ConfField_Class fld in doorConfig.lstTabFlds)
			{
				if (doorConfig.disabledFlds[fld.FIELDNAME])
					clearConfField(doorConfig, fld.FIELDNAME, ref errMsg);
			}
		}
		public static void syncFldHeaderWithBody(DoorConfig doorConfig)
		{
			foreach (ConfField_Class fld in doorConfig.lstTabFlds)
			{
				doorConfig.thClasses[fld.FIELDNAME] = (doorConfig.disabledFlds[fld.FIELDNAME] ? "thGray" : "thBlue");
			}
		}
		//public static void clearConfField(DoorConfig doorConfig, string fldName, string dataType, ref string errMsg)
		public static void clearConfField(DoorConfig doorConfig, string fldName, ref string errMsg)
		{
			string sval;
			int ival;
			string dataType1;

			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			if (fldName == "EXTCOLORID" || fldName == "MEASURESDOC")
			{
				int x = 17;
			}
			try
			{
				int p = Array.IndexOf(propNames, fldName);
				if (p >= 0)
				{
					dataType1 = props[p].PropertyType.ToString();
					string dataType2 = dataType1.Split('.')[1];
					switch (dataType2.ToLower())
					{
						//case "CHAR":
						//case "RCHAR":
						case "string":
							sval = string.Empty;
							try
							{
								props[p].SetValue(doorConfig, sval);
							}
							catch (Exception ex)
							{
								errMsg = string.Format("props[{0}}].SetValue(doorConfig, sval = {1}); FAILED ! \n error: {2} ",
													   p, sval, ex.Message);
								myLogger.log.Error(errMsg);
							}
							return;
						//case "INT":
						case "int32":
							ival = 0;
							try
							{
								props[p].SetValue(doorConfig, ival);
							}
							catch (Exception ex)
							{
								errMsg = string.Format("props[{0}}].SetValue(doorConfig, ival = {1}); FAILED ! \n error: {2} ",
													   p, ival, ex.Message);
								myLogger.log.Error(errMsg);
							}
							return;
						default:
							errMsg = string.Format("unexpected data type of DoorConfig.{0} - {1}", fldName, dataType2);
							myLogger.log.Error(errMsg);
							throw new Exception(errMsg);
					}
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
			}
		}

		#region toDel
		//public static bool hideStaticWing(string wingsNum)
		//{
		//    return (wingsNum == "כנף");
		//}

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
		#endregion toDel
		#region Meageds
		#region toDel
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
		#endregion toDel
		public static bool meagedContains(string meaged, string thTdFIELDNAME, DataTable MeagedFields)
		{
			try
			{
				//we may come here also with FIELDNAME in priority e.g. DOORCOLORID !
				//  before returning false check also that FIELNAME is in teh meaged !

				//TODO - handle that 
				//if (c.ID == null)
				//{
				//    int z = 17;
				//}
				if (thTdFIELDNAME == "DOORCOLORID")
				{
					//Debugger.Break();
					int dbg = 17;
				}
				string thName = string.Empty;
				string tdName = string.Empty;
				string FIELDNAME = string.Empty;

				if (meaged == "900" && thTdFIELDNAME.ToUpper() == "DECORFORMAT")
				{
					int dbg = 19;
				}

				if (thTdFIELDNAME.StartsWith("th"))
					thName = thTdFIELDNAME;
				else if (thTdFIELDNAME.StartsWith("td"))
					tdName = thTdFIELDNAME;
				else
				{
					thName = "th" + thTdFIELDNAME;
					tdName = "td" + thTdFIELDNAME;
					if (thTdFIELDNAME == thTdFIELDNAME.ToUpper())  // FIELDNAME must be UPPERCASE
						FIELDNAME = thTdFIELDNAME;
				}

				string fldCode = string.Empty;
				if (!string.IsNullOrEmpty(thName))
					fldCode = PrApiCalls.getFieldCodebyTh(thName);
				if (string.IsNullOrEmpty(fldCode) && !string.IsNullOrEmpty(tdName))
					fldCode = PrApiCalls.getFieldCodebyTd(tdName);
				if (string.IsNullOrEmpty(fldCode) && !string.IsNullOrEmpty(FIELDNAME))
				{
					fldCode = PrApiCalls.getFieldCodebyFIELDNAME(FIELDNAME);
					//if (string.IsNullOrEmpty(fldCode) && !string.IsNullOrEmpty(thName))
					//    fldCode = PrApiCalls.getFieldCodebyTh(thName);
					//if (string.IsNullOrEmpty(fldCode) && !string.IsNullOrEmpty(tdName))
					//    fldCode = PrApiCalls.getFieldCodebyTh(tdName);
				}
				if (!string.IsNullOrEmpty(fldCode))
				{
					DataRow[] resRows = MeagedFields.Select(string.Format("MEAGEDNAME='{0}' AND FIELDCODE = '{1}'", meaged, fldCode));
					if (resRows != null && resRows.Length > 0)
						return true;
					else
					{
						DataRow[] resRows2 = MeagedFields.Select(string.Format("FIELDCODE = '{0}'", fldCode));
						if (resRows2 != null && resRows2.Length > 0)
							return false;  //exists but in a different Meaged - hide
						else
							return true;  // the field does not exist in any meaged - it's always shown !
					}
				}
				else //bug fldCode is empty !!
				{
					string errMsg = string.Format("Unexpected error didn't find FIEDLCODE of '{0}'", thTdFIELDNAME);
					myLogger.log.Error(errMsg);
					throw new Exception(errMsg);
					//return false;
				}
				return true;    // the default  - show the field !
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} . MEAGEDNAME='{1}' AND CONFIG_TDNAME = '{2}'  Stacktrace : {3}", ex.Message, meaged, thTdFIELDNAME, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
			}
		}

		public static void setHiddens(DoorConfig doorConfig, string tabName)
		{
			try
			{
				string query = string.Format("CONFIG_SUBFORM = '{0}'", tabName.ToLower());
				DataRow[] tabFields = PrApiCalls.dtConfFields.Select(query);
				for (int r = 0; r < tabFields.Length; r++)
				{
					string fldName = tabFields[r]["FIELDNAME"].ToString();
					bool hideFldName = hideFld(doorConfig, fldName);
					doorConfig.thClasses[fldName] = (hideFldName ? "thGray" : "thBlue");
					doorConfig.disabledFlds[fldName] = (doorConfig.disabledFlds[fldName] || hideFldName);
					if (fldName.StartsWith("SWING_HANDLE"))
					{
						int dbg = 17;
					}
				}
				//
				doorConfig.thClasses["HANDLECOLORID"] = doorConfig.thClasses["HANDLENAME"];    //hardcode this
																							   //
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} . tabName ='{1}'  Stacktrace : {2}", ex.Message, tabName, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return;
			}
		}
		public static void applyModelDefaults(DoorConfig doorConfig)
		{
			try
			{
				if (doorConfig != null && !string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
				{
					string query = string.Format("TRSH_MODELNAME = '{0}'", doorConfig.TRSH_MODELNAME);
					DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);
					string errMsg = string.Empty;
					// apply model defaults 
					for (int r = 0; r < rowsDefVal.Length; r++)
					{
						string defval = rowsDefVal[r]["DEFVAL"].ToString();

						// configFldName example : dlstDecorFormat , cfld.FIELDNAME is DECORFORMAT 
						string configFldName = rowsDefVal[r]["CONFIG_FIELDNAME"].ToString();  //dlstDecorFormat 
						if (configFldName == "dlstDecorFormat")
						{
							int dbg = 17;
						}
						ConfField_Class cFld = getConfFieldByFldName(configFldName, ref errMsg);
						if (cFld != null)
						{
							if (cFld.FIELDNAME == "DECORFORMAT")
							{
								int x = 17;
							}
							applyFldDefault(doorConfig, cFld.FIELDNAME);
						}

						//UiLogic.setConfFieldVal(doorConfig, cFld.FIELDNAME, cFld.FIELDDATATYPE, defval, ref errMsg);
						//// apply VAL_LOCKED  to doorConfig.disabled
						//if (rowsDefVal[r]["VAL_LOCKED"].ToString() == "Y")
						//    doorConfig.disabledFlds[configFldName] = true;
					}
					// apply family defaults , in case the default applies to all the models in a family
					//  e.g. COLORSNUM  
					applyFamilyDefaults(doorConfig);

				}
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return;
			}
		}
		public static void applyFamilyDefaults(DoorConfig doorConfig)
		{
			try
			{
				List<Defaults_Class> lstDefaults = PrApiCalls.lstDefaults.FindAll(x => x.FAMILYNAME == doorConfig.FAMILYNAME);
				foreach (Defaults_Class defRec in lstDefaults)
				{
					applyFldDefault(doorConfig, defRec.FIELDNAME);
				}
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return;
			}
		}

		public static void applyFldDefault(DoorConfig doorConfig, string fldName)
		{
			try
			{
				//Control c;
				//configFldName = configFldName.ToUpper();
				//string query = string.Format("PARTNAME = '{0}' AND CONFIG_FIELDNAME = '{1}'", doorConfig.PARTNAME, configFldName);
				if (doorConfig != null)
				{
					if (fldName == "COLORSNUM")
					{
						int x = 17;
					}
					//if (!string.IsNullOrEmpty(doorConfig.PARTNAME))
					if (!string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
					{

						//string query = string.Format("PARTNAME = '{0}'", doorConfig.PARTNAME);
						string query = string.Format("TRSH_MODELNAME = '{0}' AND FIELDNAME = '{1}'", doorConfig.TRSH_MODELNAME, fldName);
						DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);
						string errMsg = string.Empty;
						if (rowsDefVal.Length > 0)
						{
							for (int r = 0; r < rowsDefVal.Length; r++)
							{
								string defval = rowsDefVal[r]["DEFVAL"].ToString();
								string fldDataType = rowsDefVal[r]["FIELDDATATYPE"].ToString();
								//ConfField_Class cFld = getConfFieldByFldName(thTdFIELDNAME, ref errMsg);
								// configFldName example : dlstDecorFormat , cfld.FIELDNAME is DECORFORMAT 
								//UiLogic.setConfFieldVal(doorConfig, cFld.FIELDNAME, cFld.FIELDDATATYPE, defval, ref errMsg);
								if (defval != "noDefVal")
									UiLogic.setConfFieldVal(doorConfig, fldName, fldDataType, defval, ref errMsg);
								if (rowsDefVal[r]["VAL_LOCKED"].ToString() == "Y")
									doorConfig.disabledFlds[fldName] = true;
							}
						}
						else  //no default for  (doorConfig.TRSH_MODELNAME, fldName was found in TRSH_DEFAULTS
							  // try (doorConfig.FAMILY, fldName)
						{
							Defaults_Class defRec = PrApiCalls.lstDefaults.Find(x => x.FAMILYNAME == doorConfig.FAMILYNAME && x.FIELDNAME == fldName);
							if (defRec != null)
							{
								if (defRec.DEFVAL != "noDefVal")
									UiLogic.setConfFieldVal(doorConfig, fldName, defRec.FIELDDATATYPE, defRec.DEFVAL, ref errMsg);
								if (defRec.VAL_LOCKED == "Y")
									doorConfig.disabledFlds[fldName] = true;
							}
						}
					}
					else // field was not found in the DEFAULTs that depend on MODEL (TRSH_MODELNAME),
						 // maybe it depends on FAMILY 
						 // maybe it has a general default that does not depend on MODEL (TRSH_MODELNAME) or FAMILY
					{
						string query = string.Format("(TRSH_MODELNAME = '' OR TRSH_MODELNAME IS NULL) "
							+ "AND (FAMILYNAME = '' OR FAMILYNAME IS NULL) "
							+ "AND FIELDNAME = '{0}'", fldName);  //default that does not depend on MODEL
						DataRow[] rowsDefVal = PrApiCalls.dtDefaults.Select(query);
						string errMsg = string.Empty;
						for (int r = 0; r < rowsDefVal.Length; r++)
						{
							string defval = rowsDefVal[r]["DEFVAL"].ToString();
							string fldDataType = rowsDefVal[r]["FIELDDATATYPE"].ToString();
							//ConfField_Class cFld = getConfFieldByFldName(thTdFIELDNAME, ref errMsg);
							// configFldName example : dlstDecorFormat , cfld.FIELDNAME is DECORFORMAT 
							//UiLogic.setConfFieldVal(doorConfig, cFld.FIELDNAME, cFld.FIELDDATATYPE, defval, ref errMsg);
							if (defval != "noDefVal")
							{
							   if (fldDataType.Contains("CHAR"))  //string 
								UiLogic.setConfFieldVal(doorConfig, fldName, fldDataType, defval, ref errMsg);
							   else if (fldDataType == "INT")
								{
									int iDefval;
									if (int.TryParse(defval, out iDefval))
										UiLogic.setConfFieldVal(doorConfig, fldName, fldDataType, iDefval, ref errMsg);
								}
							}

						}
					}
				}
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return;
			}
		}

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

		public static bool HiddenDecorSideFldsContains(string fldName, DataTable dtDecorSideFlds, string decorSideCode)
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
				if (!fldName.StartsWith("th") && !fldName.StartsWith("td"))
				{
					fldName = "th" + fldName;
				}
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
		public static DataTable getTabFields(string tabName, DoorConfig doorConfig, ref string errMsg)
		{
			try
			{
				if (tabName == "extdecor")
				{
					int dbg = 17;
				}
				string query = string.Format("CONFIG_SUBFORM ='{0}'", tabName);
				DataRow[] rowsArray = PrApiCalls.dtConfFields.Select(query);
				List<ConfField_Class> lstTabFlds = new List<ConfField_Class>();
				if (rowsArray.Length > 0)
				{
					for (int r = 0; r < rowsArray.Length; r++)
					{
						string fldName = rowsArray[r]["FIELDNAME"].ToString();
						if (fldName.StartsWith("DRIL4HW"))
						{
							int dbg = 17;
						}
						if (!hideFld(doorConfig, fldName) && !disableFld(doorConfig, fldName))  //TODO : replace !disableFld(doorConfig, fldName) by
																								//  !doorConfig.disabledFlds[fldName], actually here UiLogic.disableFld() that uses TRSH_DEFAULTS table should set 
																								//   doorConfig.disabledFlds[fldName] which should be in the HTML element of the field.
						{
							ConfField_Class confFld = new ConfField_Class();
							confFld.FIELDNAME = fldName;
							confFld.TABINDEX = int.Parse(rowsArray[r]["TABINDEX"].ToString());
							confFld.MANDATORY = rowsArray[r]["MANDATORY"].ToString();
							lstTabFlds.Add(confFld);
						}
					}
					lstTabFlds.Sort((f, q) => f.TABINDEX.CompareTo(q.TABINDEX));
					return lstTabFlds.ToDataTable<ConfField_Class>();
				}
				else
				{
					errMsg = string.Format("שגיאה: לא נמצאו שדות קונפיגורטור ללשונית {0} - אנא פנה למנהל המערכת", tabName);
					myLogger.log.Error(errMsg);
					return null;
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return null;
			}
		}

		public static List<ConfField_Class> getLstTabFields(string tabName, DoorConfig doorConfig, ref string errMsg)
		{
			List<ConfField_Class> res = PrApiCalls.lstConfFields.FindAll(x => x.CONFIG_SUBFORM == tabName);
			res.Sort((f, q) => f.TABINDEX.CompareTo(q.TABINDEX));
			return res;
		}

		public static string getTabOfField(string fldName)
		{
			string query = string.Format("FIELDNAME='{0}'", fldName);
			DataRow[] rowsArray = PrApiCalls.dtConfFields.Select(query);
			if (rowsArray.Length > 0)
			{
				return rowsArray[0]["CONFIG_SUBFORM"].ToString();
			}
			else
				return string.Empty;
		}

		public static int getTabIndexOfField(string fldName)
		{
			string query = string.Format("FIELDNAME='{0}'", fldName);
			DataRow[] rowsArray = PrApiCalls.dtConfFields.Select(query);
			if (rowsArray.Length > 0)
			{
				return int.Parse(rowsArray[0]["TABINDEX"].ToString());
			}
			else
				return 0;
		}

		public static string getNextTabFld(DoorConfig doorConfig, DataTable dtTabFields, string fldName)
		{
			string query = string.Format("FIELDNAME='{0}'", fldName);
			DataRow[] rowsArray = dtTabFields.Select(query);
			if (fldName == "GRIDCOLORID")
			{
				int x = 17;
			}
			int rowIndex = -1;
			if (rowsArray.Length > 0)
			{
				rowIndex = dtTabFields.Rows.IndexOf(rowsArray[0]);
				if (rowIndex < dtTabFields.Rows.Count - 1)
				{
					string nextFldName = dtTabFields.Rows[rowIndex + 1]["FIELDNAME"].ToString();
					//string nextThName = dtTabFields.Rows[rowIndex + 1]["CONFIG_THNAME"].ToString();
					if (doorConfig.disabledFlds.ContainsKey(nextFldName) && doorConfig.disabledFlds[nextFldName]) //|| hideFld(doorConfig, nextThName))
						return getNextTabFld(doorConfig, dtTabFields, nextFldName);   // recursive call 
					else
						return nextFldName;  //dtTabFields.Rows[rowIndex + 1]["FIELDNAME"].ToString();
				}
				else
					return String.Empty;
			}
			return String.Empty;
		}
		#endregion clear tab and conf fields
		#region disable some tab fields
		public static void disableFollowingTabFields(DoorConfig doorConfig, DataTable dtTabFields, string fldName, bool disable)
		{
			int fldTabIndex = getTabIndexOfField(fldName);
			foreach (DataRow fldRow in dtTabFields.Rows)
			{
				string fldName2 = fldRow["FIELDNAME"].ToString();
				int tabIndex = int.Parse(fldRow["TABINDEX"].ToString());
				string mandatory = fldRow["MANDATORY"].ToString();
				string errMsg = string.Empty;

				if (int.Parse(fldRow["TABINDEX"].ToString()) > fldTabIndex && fldRow["MANDATORY"].ToString() == "M")
				{
					doorConfig.disabledFlds[fldName2] = disable;
					//Eli's req 14/02/2023 - the disabled fields should be emptied !
					if (disable)
						clearConfField(doorConfig, fldName2, ref errMsg);
				}

			}
		}

		#endregion disable some tab fields

		#region conf fields
		public static ConfField_Class getConfFieldByFldName(string configFldName, ref string errMsg)
		{
			//configFldName = configFldName.ToUpper();  e.g. dlstDecorFormat it's not DECORFORMAT
			try
			{
				DataRow[] fldRows = PrApiCalls.dtConfFields.Select(string.Format("CONFIG_FIELDNAME = '{0}'", configFldName));
				if (fldRows != null && fldRows.Length > 0)
				{
					DataRow fldRow = fldRows[0];
					ConfField_Class fld = new ConfField_Class();
					//--
					Type objType = fld.GetType();

					PropertyInfo[] props = objType.GetProperties();
					string[] propNames = props.Select(i => i.Name).ToArray();

					for (int p = 0; p < propNames.Length; p++)
					{
						try
						{
							if (props[p].Name == "CONFIG_SUBFORM" && fldRow[props[p].Name] != null && string.IsNullOrEmpty(fldRow[props[p].Name].ToString()))
							{
								return null;
							}
							if (fldRow[props[p].Name] != null)
								props[p].SetValue(fld, fldRow[props[p].Name]);
						}
						catch (Exception ex)
						{
							string errmsg = string.Format("Error : {0} , fld={1}", ex.Message, fld.FIELDCODE);
							throw ex;
						}
					}
					return fld;
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				//displayErrMsg(lblMsg, errMsg);

				return null;
			}
		}
		public static ConfField_Class getConfFieldByFldCode(string fieldCode, ref string errMsg)
		{
			try
			{
				DataRow[] fldRows = PrApiCalls.dtConfFields.Select(string.Format("FIELDCODE = '{0}'", fieldCode));
				if (fldRows != null && fldRows.Length > 0)
				{
					DataRow fldRow = fldRows[0];
					ConfField_Class fld = new ConfField_Class();
					//--
					Type objType = fld.GetType();

					PropertyInfo[] props = objType.GetProperties();
					string[] propNames = props.Select(i => i.Name).ToArray();

					for (int p = 0; p < propNames.Length; p++)
					{
						try
						{
							props[p].SetValue(fld, fldRow[props[p].Name]);
						}
						catch (Exception ex)
						{
							string errmsg = string.Format("Error : {0} , fld={1}", ex.Message, fld.FIELDCODE);
							throw ex;
						}
					}
					return fld;
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: {0} .  Stacktrace : {1}", ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				//displayErrMsg(lblMsg, errMsg);
				return null;
			}
		}
		#region toDel
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
		#endregion toDel
		public static void AppAllDefaults(DoorConfig doorConfig, ref string errMsg)
		{
			string sval;
			int ival;
			string fldName = string.Empty;

			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			try
			{
				for (int p = 0; p < propNames.Length; p++)
				{
					fldName = propNames[p];
					if (fldName == "ELECTRICAPPARATUS")
					{
						int x = 17;
					}
					applyFldDefault(doorConfig, fldName);
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
			}
		}

		public static void setConfFieldVal(DoorConfig doorConfig, string fldName, string dataType, object val, ref string errMsg)
		{
			string sval;
			int ival;

			Type objType = doorConfig.GetType();
			PropertyInfo[] props = objType.GetProperties();
			//string[] propNames = props.Select(i => i.Name).ToArray();
			try
			{
				//for (int p = 0; p < propNames.Length; p++)
				//{
				int p = Array.IndexOf(propNames, fldName);
				if (p >= 0)
				{
					switch (dataType)
					{
						case "CHAR":
						case "RCHAR":
							if (val == null || string.IsNullOrEmpty(val.ToString()))
								sval = string.Empty;
							else
								sval = val.ToString();  //(string)val;
							try
							{
								props[p].SetValue(doorConfig, sval);
							}
							catch (Exception ex)
							{
								errMsg = string.Format("props[{0}}].SetValue(doorConfig, sval = {1}); FAILED ! \n error: {2} ",
													   p, sval, ex.Message);
								myLogger.log.Error(errMsg);
								//displayErrMsg(lblMsg, errMsg);
							}
							return;
						case "INT":
							if (val == null || string.IsNullOrEmpty(val.ToString()))
								ival = 0;
							// 04/12/2022 - replcaed the following two lines by a try-catch block .
							//else
							// ival = int.Parse(val.ToString());
							else
							{
								try
								{
									if (!int.TryParse(val.ToString(), out ival))  //happens at apply defaults where  defval in priority is
																				  //a string (e.g. color name) and the field is INT (e.g CLRID of COLORID ). 
										ival = 0;
								}
								catch (Exception ex)
								{
									errMsg = string.Format("ival = int.Parse(val.ToString()); val = {0}); FAILED ! \n error: {1} ",
														   val, ex.Message);
									myLogger.log.Error(errMsg);
									ival = 0;
								}
							}
							//
							try
							{
								props[p].SetValue(doorConfig, ival);
							}
							catch (Exception ex)
							{
								errMsg = string.Format("props[{0}}].SetValue(doorConfig, ival = {1}); FAILED ! \n error: {2} ",
													   p, ival, ex.Message);
								myLogger.log.Error(errMsg);
								//displayErrMsg(lblMsg, errMsg);
							}
							return;
					}
				}
			}
			catch (Exception ex)
			{
				errMsg = string.Format("Unexpected error: fldname = {0} , error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				//displayErrMsg(lblMsg, errMsg);
			}
		}

		public static string colorTypeAndDes(TRSH_COLOR_Class c)
		{
			return string.Format("{0}:{1}", c.TRSH_COLORTYPEDES, c.PARTDES);
		}
		#region unused methods
		//public static string colorTypeAndDes(int ColorId)
		//{
		//    TRSH_COLOR_Class color = PrApiCalls.getColorById(ColorId);
		//    if (color != null && !string.IsNullOrEmpty(color.TRSH_COLORTYPEDES))
		//        return string.Format("{0}:{1}", color.TRSH_COLORTYPEDES, color.PARTDES);
		//    else
		//        return string.Empty;
		//}
		//public static string colorName(int ColorId)
		//{
		//    TRSH_COLOR_Class color = PrApiCalls.getColorById(ColorId);
		//    if (color != null)
		//        return color.PARTNAME;
		//    else
		//        return string.Empty;
		//}
		//public static int getColorIdByClrName(string ClrName)
		//{
		//    foreach (TRSH_COLOR_Class color in PrApiCalls.lstColors)
		//    {
		//        if (color.PARTNAME == ClrName)
		//            return color.TRSH_COLORID;
		//    }
		//    return 0;
		//}
		//public static int getColorIdByTypeAndDes(string typeAndDes)
		//{
		//    string[] cTypeAndDes = typeAndDes.Split(':');
		//    foreach (TRSH_COLOR_Class color in PrApiCalls.lstColors)
		//    {
		//        if (color.TRSH_COLORTYPEDES == cTypeAndDes[0] && color.PARTDES == cTypeAndDes[1])
		//            return color.TRSH_COLORID;
		//    }
		//    return 0;
		//}
		#endregion unused methods
		public static string colorDes(int ColorId)
		{
			TRSH_COLOR_Class color = PrApiCalls.getColorById(ColorId);
			if (ColorId > 0)
			{
				int dbg = 17;
			}
			if (color != null)
				return color.PARTDES;
			else
				return string.Empty;
		}
		public static int getColorIdByClrDes(string ClrDes)
		{
			TRSH_COLOR_Class color = PrApiCalls.lstColors.Find(item => item.PARTDES == ClrDes);
			//foreach (TRSH_COLOR_Class color in PrApiCalls.lstColors)
			//{
			//    if (color.PARTDES == ClrDes)
			//        return color.TRSH_COLORID;
			//}
			if (color != null)
				return color.TRSH_COLORID;
			else
				return 0;
		}

		public static void HasFocus(string currFldName, DoorConfig doorConfig)
		{
			doorConfig.currPropName = currFldName;       // System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(hebrew);
			doorConfig.borderColors[currFldName] = "focusBorder";
		}
		// this event handler is used only in Mandatory fields - as we want to set border color to red when the field is empty
		public static string getFldDataType(string fldName)
		{
			string query = string.Format("FIELDNAME='{0}'", fldName);
			DataRow[] fldRows = PrApiCalls.dtConfFields.Select(query);
			if (fldRows.Length > 0)
				return fldRows[0]["FIELDDATATYPE"].ToString();
			else
			{
				string errMsg = string.Format("Unexpected error: FIELDNAME {0} not found in PrApiCalls.dtConfFields", fldName);
				myLogger.log.Error(errMsg);
				throw new Exception(errMsg);    //return string.Empty;
			}
		}
		public static bool fldIsMandatory(DoorConfig doorConfig, string fldName)
		{
			try
			{
				if (doorConfig.TRSH_WINGSNUMDES == HebNouns.HalfWing)  //"חצי כנף")  //new 15/07/2022
				{
					return (lstHalfwingMfields.Contains(fldName));
				}
				if (doorConfig.COLORSNUM == "2")
				{
					if ("EXTCOLORID INTCOLORID".Contains(fldName))
						return true;
				}

				string query = string.Format("FIELDNAME='{0}'", fldName);
				DataRow[] fldRows = PrApiCalls.dtConfFields.Select(query);
				if (fldRows.Length > 0)
					return (fldRows[0]["MANDATORY"].ToString() == "M");
				else
				{
					string errMsg = string.Format("Unexpected error: FIELDNAME {0} not found in PrApiCalls.dtConfFields", fldName);
					myLogger.log.Error(errMsg);
					throw new Exception(errMsg);    //return string.Empty;
				}
			}
			catch (Exception ex)
			{
				string errMsg = string.Format("Unexpected error : fldName = {0} .  error: {1} .  Stacktrace : {2}", fldName, ex.Message, ex.StackTrace);
				myLogger.log.Error(errMsg);
				return false;
			}
		}
		public static void LostFocus(DoorConfig doorConfig)
		{
			//doorConfig.currPropName = currFldName;       // System.Windows.Forms.InputLanguage.CurrentInputLanguage = System.Windows.Forms.InputLanguage.FromCulture(hebrew);
			string fldDataType = getFldDataType(doorConfig.currPropName);
			bool fldIsFilled = doorFldIsFilled(doorConfig, doorConfig.currPropName, fldDataType);
			// do not do set border color to red if field is not Mandatory . e.g COLORSNUM in Halfwing !
			if (fldIsMandatory(doorConfig, doorConfig.currPropName))
				doorConfig.borderColors[doorConfig.currPropName] = (fldIsFilled ? "blueBorder" : "redBorder");
		}

		public static bool wingWidthIsOk(DoorConfig doorConfig, int wingWidth, ref string errMsg)
		{
			int minWingWidth = MIN_SWING_WITHLOCK_W;
			if (doorConfig.FAMILYDES == HebNouns.SmoothDoors && doorConfig.currTabName == "staticwing")
			{
				if (wingWidth > MAX_SMOOTH_SWING_WIDTH)
				{
					errMsg = "לא ניתן להמשיך ! יש לשנות ההגדרה ל'דו כנפית'";
					return false;
				}
			}

			if (doorConfig.currTabName == "staticwing" && doorConfig.SWINGHASLOCK != "Y")
				minWingWidth = MIN_SWING_NOLOCK_W;

			if (wingWidth < minWingWidth || wingWidth > MAX_ANYWING_W)
			{
				errMsg = "רוחב כנף חורג מהטולרנס שהוגדר נא למלא רוחב כנף מתאים";
				return false;
			}

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
			return true;  //[TODO] this is just a placeHolder
		}

		public static bool isDecorated(DoorConfig doorConfig)
		{
			return (doorConfig.DECORFORMAT != null && !string.IsNullOrEmpty(doorConfig.DECORFORMAT) && doorConfig.DECORFORMAT != HebNouns.None);
		}

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