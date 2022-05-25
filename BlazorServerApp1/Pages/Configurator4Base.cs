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

			if (doorConfig.CUST != 0 && !string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
			{
				int t = Array.IndexOf(UiLogic.tabNames, tabName);
				string prevTab = (t > 0 ? UiLogic.tabNames[t - 1] : string.Empty);
				string prevText = (t > 0 ? UiLogic.tabTexts[t - 1] : string.Empty);
				if (prevTab.ToLower() == "staticwing" && doorConfig.TRSH_WINGSNUMDES == "כנף")
				{
					prevTab = UiLogic.tabNames[t - 2];
					prevText = UiLogic.tabTexts[t - 2];
				}
				if (prevTab.ToLower() == "intdecor" && hideBtnIntDecor )
                {
					if (hideBtnExtDecor)
                    {
						prevTab = UiLogic.tabNames[t - 3];
						prevText = UiLogic.tabTexts[t - 3];
					}
					else
                    {
						prevTab = UiLogic.tabNames[t - 2];
						prevText = UiLogic.tabTexts[t - 2];
					}
                }
				if (prevTab.ToLower() == "intdecor" && hideBtnExtDecor)
				{
					prevTab = UiLogic.tabNames[t - 2];
					prevText = UiLogic.tabTexts[t - 2];
				}

				if (t == 0)
                {
					//ActivePage = page;
					return true;
                }
                else
                {
					if (UiLogic.tabPageIsFilled(prevTab, doorConfig))
						return true; //ActivePage = page;
					else
					{
						//string prevTabText = UiLogic.tabTexts[t - 1];
						errMsg = UiLogic.requiredFieldsAreEmpty;   //string.Format("יש למלא את כל השדות בלשונית '{0}'  י", prevText);
						// Js.InvokeVoidAsync("alert", errMsg);
						//openMsgBox = true;
						return false;
					}
                }
            }
			else
			{
				if (doorConfig.CUST == null || doorConfig.CUST == 0)
					errMsg += "חובה לציין לקוח";
				if (string.IsNullOrEmpty(doorConfig.TRSH_MODELNAME))
					errMsg += " חובה לציין דגם , ";

				//Js.InvokeVoidAsync("alert", errMsg);
				//openMsgBox = true;
				return false;

			}
		}
	}
}