using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerApp1.Data;
namespace BlazorServerApp1.Pages
{
	public class Configurator4Base : ComponentBase
	{
		protected bool hideDoorTitle = true;
		protected bool hideBtnExtDecor = false;
		protected bool hideBtnIntDecor = false;

		public bool ActivatePage(string tabName, DoorConfig doorConfig, ref string errMsg)
		{
			errMsg = string.Empty;
			string prevTab = string.Empty;

			if (doorConfig.CUST != 0 && !string.IsNullOrEmpty(doorConfig.TRSH_WINGSNUMDES) && !string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
			{
				if (doorConfig.TRSH_WINGSNUMDES == "חצי כנף")
				{
					switch (tabName)
					{
						case "staticwing":
							prevTab = "divDoorTitle";
							break;
						case "accessories":
					    	prevTab = "staticwing";
							break;
					}
				}
				else
				{
					int t = Array.IndexOf(UiLogic.tabNames, tabName);
					prevTab = (t > 0 ? UiLogic.tabNames[t - 1] : string.Empty);
					string prevText = (t > 0 ? UiLogic.tabTexts[t - 1] : string.Empty);
					if (prevTab.ToLower() == "staticwing" && doorConfig.TRSH_WINGSNUMDES == "כנף")
					{
						prevTab = UiLogic.tabNames[t - 2];   //intdecor
						prevText = UiLogic.tabTexts[t - 2];
					}
					if (prevTab.ToLower() == "intdecor" && hideBtnIntDecor)
					{
						if (!hideBtnExtDecor)
						{
							prevTab = UiLogic.tabNames[t - 2];  //extdecor
							prevText = UiLogic.tabTexts[t - 2];
						}
						else
						{
							prevTab = UiLogic.tabNames[t - 3];  //movingwing
							prevText = UiLogic.tabTexts[t - 3];
						}
					}
					//if (prevTab.ToLower() == "intdecor" && hideBtnIntDecor && hideBtnExtDecor)
					//{
					//	prevTab = UiLogic.tabNames[t - 3];
					//	prevText = UiLogic.tabTexts[t - 3];
					//}

					if (t == 0)
					{
						//ActivePage = page;
						return true;
					}
				}
				//
				if (UiLogic.tabPageIsFilled(prevTab, doorConfig))
				{
					doorConfig.prevTabName = prevTab;
					doorConfig.currTabName = tabName;
					return true; //ActivePage = page;
				}
				else
				{
					//string prevTabText = UiLogic.tabTexts[t - 1];
					errMsg = UiLogic.requiredFieldsAreEmpty;   //string.Format("יש למלא את כל השדות בלשונית '{0}'  י", prevText);
																// Js.InvokeVoidAsync("alert", errMsg);
																//openMsgBox = true;
					return false;
				}
            }
			else
			{
				if (doorConfig.CUST == null || doorConfig.CUST == 0)
					errMsg += "חובה לציין משווק";
				if (string.IsNullOrEmpty(doorConfig.TRSH_WINGSNUMDES))
					errMsg += " חובה לציין מיפתח , ";
				if (string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
					errMsg += " חובה לציין דגם , ";

				//Js.InvokeVoidAsync("alert", errMsg);
				//openMsgBox = true;
				return false;
			}
		}
		
	}
}