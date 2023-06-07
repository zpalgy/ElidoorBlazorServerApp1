using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerApp1.Data;
namespace BlazorServerApp1.Pages
{
	public class DecorBase : ComponentBase
	{
		protected void applySepLines(DoorConfig doorConfig)
		{
			if (!doorConfig.disabledFlds["EXTSEPLINES"])
			{
				if (doorConfig.EXTSEPLINES == HebNouns.None)
				{
					doorConfig.EXTFINMODERNSEPLINE = string.Empty;
					doorConfig.EXTSEPLINESCLRID = 0;
				}
				doorConfig.disabledFlds["EXTFINMODERNSEPLINE"] = doorConfig.disabledFlds["EXTSEPLINESCLRID"] = (doorConfig.EXTSEPLINES == HebNouns.None);
			}
			else if (!doorConfig.disabledFlds["INTSEPLINES"])
			{
				if (doorConfig.INTSEPLINES == HebNouns.None)
				{
					doorConfig.INTFINMODERNSEPLINE = string.Empty;
					doorConfig.INTSEPLINESCLRID = 0;
				}
				doorConfig.disabledFlds["INTFINMODERNSEPLINE"] = doorConfig.disabledFlds["INTSEPLINESCLRID"] = (doorConfig.INTSEPLINES == HebNouns.None);
			}
		}
		protected void applyCentralSideGrid(DoorConfig doorConfig)
		{
			if (doorConfig.DECORFORMAT == HebNouns.External || doorConfig.DECORFORMAT == HebNouns.BothSides)
			{
				doorConfig.disabledFlds["EXTGRIDCPLATEDES"] =
				doorConfig.disabledFlds["EXTCGRIDBKGDCLRID"] = (doorConfig.EXTFINMODERNCPLATE != HebNouns.Grid);

				if (doorConfig.EXTFINMODERNCPLATE == HebNouns.Grid)
				{
					doorConfig.disabledFlds["EXTSIDEGRID"] =
					doorConfig.disabledFlds["EXTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["EXTSIDEGRIDBKGDCLRID"] = true;

					//doorConfig.EXTSIDEGRID = string.Empty;
					UiLogic.clearDisabledFields(doorConfig);
				}
				else
				{        //EXT*  not grid
					doorConfig.disabledFlds["EXTGRIDCPLATEDES"] = doorConfig.disabledFlds["EXTCGRIDBKGDCLRID"] = true;
					//set *SIDEGRID default ללא
					doorConfig.disabledFlds["EXTSIDEGRID"] = false;
					//doorConfig.thClasses["EXTSIDEGRID"] = "thBlue";

					if (doorConfig.EXTSIDEGRID != null && string.IsNullOrEmpty(doorConfig.EXTSIDEGRID))
						doorConfig.EXTSIDEGRID = HebNouns.None;
				}
				if (doorConfig.EXTSIDEGRID == HebNouns.None)
				{
					doorConfig.disabledFlds["EXTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["EXTSIDEGRIDBKGDCLRID"] = true;
					UiLogic.clearDisabledFields(doorConfig);
				}
				else if (!string.IsNullOrEmpty(doorConfig.EXTSIDEGRID))   //(doorConfig.EXTSIDEGRID == HebNouns.Grid)
				{
					doorConfig.disabledFlds["EXTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["EXTSIDEGRIDBKGDCLRID"] = false;

					//doorConfig.thClasses["EXTSIDEGRIDCLRID"] =
					//doorConfig.thClasses["EXTSIDEGRIDBKGDCLRID"] = "thBlue";
				}
			}
			if (doorConfig.DECORFORMAT == HebNouns.Internal || doorConfig.DECORFORMAT == HebNouns.BothSides)
			{
				doorConfig.disabledFlds["INTGRIDCPLATEDES"] =
							doorConfig.disabledFlds["INTCGRIDBKGDCLRID"] = (doorConfig.INTFINMODERNCPLATE != HebNouns.Grid);
				if (doorConfig.INTFINMODERNCPLATE == HebNouns.Grid)
				{
					doorConfig.disabledFlds["INTSIDEGRID"] =
					doorConfig.disabledFlds["INTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["INTSIDEGRIDBKGDCLRID"] = true;

					//doorConfig.INTSIDEGRID = string.Empty;
					UiLogic.clearDisabledFields(doorConfig);
				}
				else  // INT*    not grid
				{
					doorConfig.disabledFlds["INTGRIDCPLATEDES"] = doorConfig.disabledFlds["INTCGRIDBKGDCLRID"] = true;

					//set *SIDEGRID default ללא
					doorConfig.disabledFlds["INTSIDEGRID"] = false;
					//doorConfig.thClasses["INTSIDEGRID"] = "thBlue";

					if (doorConfig.INTSIDEGRID != null && string.IsNullOrEmpty(doorConfig.INTSIDEGRID))
						doorConfig.INTSIDEGRID = HebNouns.None;
				}

				if (doorConfig.INTSIDEGRID == HebNouns.None)
				{
					doorConfig.disabledFlds["INTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["INTSIDEGRIDBKGDCLRID"] = true;
				}
				else if (!string.IsNullOrEmpty(doorConfig.INTSIDEGRID))   //(doorConfig.INTSIDEGRID == HebNouns.Grid)
				{
					doorConfig.disabledFlds["INTSIDEGRIDCLRID"] =
					doorConfig.disabledFlds["INTSIDEGRIDBKGDCLRID"] = false;
					UiLogic.clearDisabledFields(doorConfig);

					//doorConfig.thClasses["INTSIDEGRIDCLRID"] =
					//doorConfig.thClasses["INTSIDEGRIDBKGDCLRID"] = "thBlue";
				}
			}
			UiLogic.syncFldHeaderWithBody(doorConfig);
		}

		protected void applyDesignersExtSideGrid(DoorConfig doorConfig)
		{
			doorConfig.disabledFlds["EXTSEPLINES"] = true;  // field 10 always disabled for this Family !
			if (!string.IsNullOrEmpty(doorConfig.EXTSIDEGRID) && doorConfig.EXTSIDEGRID != HebNouns.None)
			{
				doorConfig.disabledFlds["EXTSIDEGRIDCLRID"] = doorConfig.disabledFlds["EXTSIDEGRIDBKGDCLRID"] =
				doorConfig.disabledFlds["EXTFINMODERNSEPLINE"] = doorConfig.disabledFlds["EXTSEPLINESCLRID"] = false;
				UiLogic.syncFldHeaderWithBody(doorConfig);
				doorConfig.EXTSEPLINES = HebNouns.With;
				UiLogic.applyFldDefault(doorConfig, "EXTFINMODERNSEPLINE");
				UiLogic.applyFldDefault(doorConfig, "EXTSEPLINESCLRID");
			}
			else if (doorConfig.EXTSIDEGRID == HebNouns.None) //ללא
			{
				doorConfig.disabledFlds["EXTSIDEGRIDCLRID"] = doorConfig.disabledFlds["EXTSIDEGRIDBKGDCLRID"] =
				doorConfig.disabledFlds["EXTFINMODERNSEPLINE"] = doorConfig.disabledFlds["EXTSEPLINESCLRID"] = true;
				UiLogic.syncFldHeaderWithBody(doorConfig);
				doorConfig.EXTSIDEGRIDCLRID = 0;                // Field 8
				doorConfig.EXTSIDEGRIDBKGDCLRID = 0;            // 9
				doorConfig.EXTSEPLINES = string.Empty;          //10
				doorConfig.EXTFINMODERNSEPLINE = string.Empty;  //11
				doorConfig.EXTSEPLINESCLRID = 0;                //12
			}
		}

		protected void applyDesignersIntSideGrid(DoorConfig doorConfig)
		{
			doorConfig.disabledFlds["INTSEPLINES"] = true;  // field 10 always disabled for this Family !
			if (!string.IsNullOrEmpty(doorConfig.INTSIDEGRID) && doorConfig.INTSIDEGRID != HebNouns.None)
			{
				doorConfig.disabledFlds["INTSIDEGRIDCLRID"] = doorConfig.disabledFlds["INTSIDEGRIDBKGDCLRID"] =
				doorConfig.disabledFlds["INTFINMODERNSEPLINE"] = doorConfig.disabledFlds["INTSEPLINESCLRID"] = false;
				UiLogic.syncFldHeaderWithBody(doorConfig);
				doorConfig.INTSEPLINES = HebNouns.With;
				UiLogic.applyFldDefault(doorConfig, "INTFINMODERNSEPLINE");
				UiLogic.applyFldDefault(doorConfig, "INTSEPLINESCLRID");
			}
			else if (doorConfig.INTSIDEGRID == HebNouns.None) //ללא
			{
				//                            8                                                9
				doorConfig.disabledFlds["INTSIDEGRIDCLRID"] = doorConfig.disabledFlds["INTSIDEGRIDBKGDCLRID"] =
				doorConfig.disabledFlds["INTFINMODERNSEPLINE"] = doorConfig.disabledFlds["INTSEPLINESCLRID"] = true;
				//                              11                                             12
				UiLogic.syncFldHeaderWithBody(doorConfig);
				doorConfig.INTSIDEGRIDCLRID = 0;                // Field 8
				doorConfig.INTSIDEGRIDBKGDCLRID = 0;            // 9
				doorConfig.INTSEPLINES = string.Empty;          // 10 
				doorConfig.INTFINMODERNSEPLINE = string.Empty;  // 11
				doorConfig.INTSEPLINESCLRID = 0;                //12
			}
		}
	}
}
