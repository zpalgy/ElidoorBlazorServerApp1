using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Text.Json.Serialization;

namespace BlazorServerApp1.Data
{
    [DataContract]
    public class DoorConfig
    {
        #region form header
        [DataMember(Order = 1)]
        public int TRSH_DOORCONFIG = 0; // { get; set; }
        [DataMember(Order = 2), Order]
        public string REFERENCE { get; set; } = String.Empty;
        //[Required]
        [DataMember(Order = 3), Order]
        public string FORMDATE { get; set; }

        [DataMember(Order = 4), Order]
        public string ESTSHIPDATE { get; set; }

        [DataMember(Order = 5), Order]
        public string FORMFILLER { get; set; }
        [DataMember(Order = 6)]
        public string CUSTORDNAME { get; set; }

        [DataMember(Order = 7), Order]
        public int CUST { get; set; }
        //[DataMember(Order = 5), Order]
        //public string AGENTCODE { get; set; }
        //[DataMember(Order = 5), Order]
        //public string AGENTNAME { get; set; }


        //[Required]      
        [DataMember(Order = 8), Order]
        public string ENDCUSTDES { get; set; }
        //public int CUST { get; set; }
        //[DataMember(Order = 6), Order]
        //public string CUSTNAME { get; set; }
        //[DataMember(Order = 7), Order]

        [DataMember(Order = 9), Order]
        public string SHIPADDRESS { get; set; }
        [DataMember(Order = 10), Order]
        public string TMPSHIPADDRESS { get; set; }

        //[DataMember(Order = 10), Order]
        //public string ADDRESS3 { get; set; }
        #endregion form header

        #region door header

        //[DataMember(Order = 12), Order]
        //public string TRSH_WINGSNUMCODE { get; set; }  // '1','1.5','2'

        [DataMember(Order = 13), Order]
        public string TRSH_WINGSNUMDES { get; set; }  // כנף, כנף וחצי, דו כנפי
        [DataMember(Order = 14), Order]
        public string OPENMODE { get; set; }  // in,out  D-40   >> moved to Movingwing

		[DataMember(Order = 15), Order]
		public string OPENIN { get; set; }
		[DataMember(Order = 16), Order]
		public string OPENOUT { get; set; }

		[DataMember(Order = 17), Order]
        //public string COMPLIENTDOOR { get; set; }  // Y | N | '' D-920
        public int TRSH_COMPLIENT { get; set; }   //D-920 
                                                  //[DataMember(Order = 23), Order]
                                                  //public int PART { get; set; }  //D-20
                                                  //[Required]
                                                  //*******************************
                                                  // new field TRSH_MODELNAME
                                                  //[TODO]  - check that it works on sendToPriority ***
        [NonSerialized]
        public bool useLOGO;
        [NonSerialized]
        public bool useTurbo;
        [DataMember(Order = 18), Order]
        public string TRSH_MODELNAME { get; set; }    // MLI, 1029, 1040, ...
                                                      //***********************************************
        [DataMember(Order = 19), Order]
        public string PARTNAME { get; set; }  //D-20
                                              //[NonSerialized]
                                              //public string PARTNAME;

        //[DataMember(Order = 24), Order]
        //public string PARTDES { get; set; }

        //[DataMember(Order = 18), Order]
        [NonSerialized]
        public int FAMILY; // { get; set; }  // D-10
                           //[NonSerialized]
                           //public int FAMILY;

        //[DataMember(Order = 22), Order]
        //public string FAMILYDES { get; set; }
        [NonSerialized]
        public string FAMILYNAME; // = string.Empty;   // TRSH_DOORCONFIG.FAMILYNAME does not exist in Priority 

        [DataMember(Order = 21), Order]
        public int TRSH_DOOR_HWCATCODE { get; set; }

		//[DataMember(Order = 22), Order]
		//[NonSerialized] - 23/03/2023 : TRSH_CYLCATEGORY is NonSerialized !
		public int TRSH_CYLCATEGORY { get; set; }

		//[NonSerialized]
		//public int TRSH_DOOR_HWCATCODE;

		[DataMember(Order = 23), Order]
        public string MEASURESDOC { get; set; }  // Y | N |  D-922 assigned by me 

        //[NonSerialized]
        //public string MEASURESDOC;

        #endregion door header

        #region movingwing
        [DataMember(Order = 27), Order]
        public int LOCKDRILHEIGHT { get; set; }        //D-650
        [NonSerialized]
        public string LockDrilHMeasure;

        [DataMember(Order = 29), Order]
        public string OPENSIDE { get; set; }  //R, L    D-50

		[DataMember(Order = 30), Order]
		public string OPENSIDE_RIGHT { get; set; }
		[DataMember(Order = 31), Order]
		public string OPENSIDE_LEFT { get; set; }

		[DataMember(Order = 40), Order]
        public string DECORFORMAT { get; set; }      // D-60
        [DataMember(Order = 41), Order]
        public string COLORSNUM { get; set; }  // was D-70  string TwoColors  (boolean)   Y | N | '' D-72,  now:  ללא ,1,2
        [DataMember(Order = 42), Order]
        public int DOORWIDTH { get; set; }           // D-80
        [DataMember(Order = 43), Order]
        public int DOORCOLORID { get; set; }      // D-110
                                                  //[DataMember(Order = 44), Order]
                                                  //public string DOORCOLORDES { get; set; }
        [DataMember(Order = 45), Order]
        public int DOORHEIGHT { get; set; }         // D-570
        [DataMember(Order = 46), Order]
        public string TURBOAPPARATUS { get; set; } //Y, N, ''  //D-660
        [DataMember(Order = 47), Order]
        public string LOGO { get; set; }              // D-680
        [DataMember(Order = 48), Order]
        public int TRSH_HARDWARE { get; set; }  // Pirzul  D-870
                                                //[DataMember(Order = 49), Order]
                                                //public string HARDWAREDES { get; set; }
        [DataMember(Order = 49), Order]
        public int HWACCESSORYID { get; set; }   // new נילווים לפירזול

        [DataMember(Order = 50), Order]
        public int HWCOLORID { get; set; }      // D-880
                                                //[DataMember(Order = 60), Order]
                                                //public string HWCOLORDES { get; set; }
        [DataMember(Order = 61), Order]
        public int DRIL4HW { get; set; }  //D-890 table 300 DRIL4HW (DRIL4HW, DRIL4HWDES)
                                          //[DataMember(Order = 62), Order]
                                          //public string DRIL4HWDES { get; set; }
        [DataMember(Order = 63), Order]
        public int TRSH_CYLINDER { get; set; }  //D-900  , table 310
                                                  //[DataMember(Order = 64), Order]
                                                  //public string PARTDES { get; set; }
        [DataMember(Order = 65), Order]
        public string ELECTRICAPPARATUS { get; set; } // Y, N, ''  D-910

        [DataMember(Order = 66), Order]
        public string HASLOCK { get; set; }   // D-932  Y | N | "" - boolean

        //[NonSerialized]
        //public string HASLOCK;


        [DataMember(Order = 67), Order]
        public string LOCKNAME { get; set; }   // D-930

        //[NonSerialized]
        //public string LOCKNAME;

        //[DataMember(Order = 67), Order]
        //public string LOCKDES { get; set; }

        [DataMember(Order = 69), Order]
        public string DRIL4TWOSIDESIDS { get; set; }   //D-980  Y, N ,'' - // previous טבלת ניקוב לפירזול  300  Dril 4 two sides IDS      ניקוב שני צדדים IDS

        //[NonSerialized]
        //public string DRIL4TWOSIDESIDS;

        [DataMember(Order = 70), Order]
        public string RAFAFAONMOVINGWING { get; set; }  // D-1010

        //[DataMember(Order = 71), Order]
        //public string LCDEYE { get; set; }  // Y, N, ''  //D-1030   moved to accessories tabPage 

        //[NonSerialized]
        //public string LCDEYE;

        [DataMember(Order = 72), Order]
        public string VENTS { get; set; }

        [DataMember(Order = 73), Order]
        public string CATDOOR { get; set; }

        //[NonSerialized]
        //public string CATDOOR;

        #endregion movingwing

        #region Ext Decoration (extdecor )
        #region extDecorL1
        [DataMember(Order = 80), Order]
        public string LINESFORMAT { get; set; }  //D-90  Shkuim Boltin שקועים \ בולטים 
                                                 //[NonSerialized]
                                                 //public string LINESFORMAT;

        [DataMember(Order = 81), Order]
        public string TILENAME { get; set; }      // D-100
                                                  //[NonSerialized]
                                                  //public string PARTNAME;

        //[DataMember(Order = 82), Order]
        //public string PARTDES { get; set; }

        [DataMember(Order = 83), Order]
        public int EXTCOLORID { get; set; }   //D-120

        //[DataMember(Order = 84), Order]
        //public string EXTCOLORDES { get; set; }
        [DataMember(Order = 85), Order]
        public int GRIDCOLORID { get; set; }   // D-140
                                               //[DataMember(Order = 86), Order]
                                               //public string GRIDCOLORDES { get; set; }
        [DataMember(Order = 87), Order]
        public string VITRAGECLRBYCTLG { get; set; }  // Y, N , ''  D-150
        [DataMember(Order = 88), Order]
        public string EXTSEPLINES { get; set; }

        [DataMember(Order = 89), Order]
        public int EXTSEPLINESCLRID { get; set; }  //D-160
                                                   //[DataMember(Order = 89), Order]
                                                   //public string EXTSEPLINESCLRDES { get; set; }  
        [DataMember(Order = 90), Order]
        public int EXTCPLATEHTDMNDCLRID { get; set; }   //D-170
                                                        //[DataMember(Order = 100), Order]
                                                        //public string EXTCPLATEHTDMNDCLDES { get; set; }
        [DataMember(Order = 101), Order]
        public int EXTPERIFPROFILECLRID { get; set; }  // D-180
                                                       //[DataMember(Order = 102), Order]
                                                       //public string EXTPERIFPROFILECLDES { get; set; }
        [DataMember(Order = 103), Order]
        public int EXTMODERNCPLATECLRID { get; set; }  //D-250  
                                                       //[DataMember(Order = 104), Order]
                                                       //public string EXTMODERNCPLATECLDES { get; set; }
        [DataMember(Order = 105), Order]
        public int EXTCGTIDCLRID { get; set; }    // D-260
                                                  //[DataMember(Order = 106), Order]
                                                  //public string EXTCGTIDCLRDES { get; set; }
        #endregion extDecorL1
        #region extDecorL2
        [DataMember(Order = 107), Order]
        public int EXTHTPLATESCLRID { get; set; }  // D-270
                                                   //[DataMember(Order = 108), Order]
                                                   //public string EXTHTPLATESCLRDES { get; set; }
        [DataMember(Order = 109), Order]
        public int ATTACHEDPLATECLRID { get; set; }  // D-280
                                                     //[DataMember(Order = 110), Order]
                                                     //public string ATTACHEDPLATECLRDES { get; set; }

        [DataMember(Order = 120), Order]
        public int EXTGLASSPLATECLRID { get; set; }  // D-310
                                                     //[DataMember(Order = 121), Order]
                                                     //public string EXTGLASSPLATECLRDES { get; set; }
        [DataMember(Order = 122), Order]
        public int EXTMODERNPLATECLRID { get; set; }  // D-330
                                                      //[DataMember(Order = 123), Order]
                                                      //public string EXTMODERNPLATECLRDES { get; set; }

        [DataMember(Order = 124), Order]
        public int DECORPLATECLRID { get; set; }  //D-350
                                                  //[DataMember(Order = 125), Order]
                                                  //public string DECORPLATECLRDES { get; set; }
        [DataMember(Order = 126), Order]
        public int NIROSTALINESCLRID { get; set; }  //D-360
                                                    //[DataMember(Order = 127), Order]
                                                    //public string NIROSTALINESCLRDES { get; set; }
        [DataMember(Order = 128), Order]
        public int EXTNIROSTALINESCLRID { get; set; }  //D-380
                                                       //[DataMember(Order = 129), Order]
                                                       //public string EXTNIROSTALINESCDES { get; set; }
        [DataMember(Order = 130), Order]
        public int EXTINSERTCLRID { get; set; }   // D-410
                                                  //[DataMember(Order = 140), Order]
                                                  //public string EXTINSERTCLRDES { get; set; }
        [DataMember(Order = 141), Order]
        public string VITRAGEPATTERNONDOOR { get; set; } // Y, N, ''  D-430
        [DataMember(Order = 142), Order]
        public int WINDOWWIDTH { get; set; }  //D-440
        [DataMember(Order = 143), Order]
        public int WINDOWHEIGHT { get; set; }       // D-450
        #endregion extDecorL2
        #region extDecorL3
        [DataMember(Order = 144), Order]
        public string PROFILE4WINDOWNAME { get; set; }  //D-460
                                                        //[DataMember(Order = 145), Order]
                                                        //public string PROFILE4WINDOWDES { get; set; }
        [DataMember(Order = 146), Order]
        public string DECORFRAME { get; set; }  //D-470  Y, N, ''
        [DataMember(Order = 147), Order]
        public int GLASS4WINDOWID { get; set; }  // D-480
                                                 //[DataMember(Order = 148), Order]
                                                 //public string GLASS4WINDOWDES { get; set; }
        [DataMember(Order = 149), Order]
        public int GRID_ID { get; set; }  //D-490
                                          //[DataMember(Order = 150), Order]
                                          //public string GRIDDES { get; set; }
        [DataMember(Order = 160), Order]
        public string VITRAGENAME { get; set; } //D-500
                                                //[DataMember(Order = 161), Order]
                                                //public string VITRAGEDES { get; set; }
        [DataMember(Order = 162), Order]
        public string JAPANESEWINDOW { get; set; }  // Y, N, ''  //D-550
        [DataMember(Order = 163), Order]
        public string PLATESFOR7504 { get; set; }  //Y, N, ''  //D-690
        [DataMember(Order = 164), Order]
        public string EXTCPLATE4HTDMNDNAME { get; set; } //D-700
        [DataMember(Order = 165), Order]
        public string EXTSIDE_C_PLATENAME { get; set; }   //D-720
        [DataMember(Order = 166), Order]
        public string DECORGRIDPLATEDES { get; set; }  //D-740
        #endregion extDecorL3
        #region extDecorL4
        [DataMember(Order = 167), Order]
        public string GRID4HT1084NAME { get; set; } // hidden D-750  ?? - this is the U-key of the table GRID4HT1084 to be created (14/11/2021 )
                                                    // on the form the name of the field id is dlstDecorPlate  and it's th is 
                                                    // דוגמא לוח דקורטיבי  , i.e. 1084 is not present in the field name !
                                                    //   maybe the Hebrew header/title of the field is wrong.
                                                    //  1084 is mentioned in the data source for this dropDownList control.
                                                    //  but, since this is the only control that uses this table - the field name in the DoorConfig record
                                                    //   will be GRID4HT1084NAME .
                                                    //[DataMember(Order = 168), Order]
                                                    //public string GRID4HT1084DES { get; set; } // D-750 ?? displayed
        [DataMember(Order = 169), Order]
        public string EXTGRIDCPLATEDES { get; set; }  // D-760
        [DataMember(Order = 170), Order]
        public string EXTHTPLATENAME { get; set; }  // D-770
        [DataMember(Order = 180), Order]
        public string VITRAGE4DIAMONDNAME { get; set; }  // D-800
                                                         //[DataMember(Order = 181), Order]
                                                         //public string VITRAGE4DIAMONDDES { get; set; }
        [DataMember(Order = 182), Order]
        public int EXTVITRAGE4DIAMONDQ { get; set; }  // D-810
        [DataMember(Order = 183), Order]
        public int INTERPLATESSPACE { get; set; } // D-830
        [DataMember(Order = 184), Order]
        public string EXTFINMODERNCPLATE { get; set; } //D-850  חלק , מחורץ
        [DataMember(Order = 185), Order]
        public string EXTFINMODERNSEPLINE { get; set; }  // D-970
        [DataMember(Order = 186), Order]
        public string EXTFINMODERNPLATE { get; set; }  // D-1000
        [DataMember(Order = 187), Order]
        public string HANDLE4DIAMONDNAME { get; set; } // D-1160
                                                       //[DataMember(Order = 188), Order]
                                                       //public string HANDLE4DIAMONDDES { get; set; }
        #endregion extDecorL4
        #endregion Ext Decoration (extdecor )

        #region Internal decoration (intdecor)
        #region Internal decoration L1
        [DataMember(Order = 189), Order]
        public int INTCOLORID { get; set; }   //D-130
                                              //[DataMember(Order = 190), Order]
                                              //public string INTCOLORDES { get; set; }
        [DataMember(Order = 200), Order]
        public int INTCPLATEHTDMNDCLRID { get; set; }  //D-190
                                                       //[DataMember(Order = 201), Order]
                                                       //public string INTCPLATEHTDMNDCLDES { get; set; }
        [DataMember(Order = 202), Order]
        public int INTPERIFPROFILECLRID { get; set; }  //D-200
                                                       //[DataMember(Order = 203), Order]
                                                       //public string INTPERIFPROFILECLDES { get; set; }
        [DataMember(Order = 204), Order]
        public int INTSIDECNTRPLATECLID { get; set; }                  //D-220
                                                                       //[DataMember(Order = 205), Order]
                                                                       //public string INTSIDECNTRPLATECDES { get; set; }
        [DataMember(Order = 205), Order]
        public string INTSEPLINES { get; set; }


        [DataMember(Order = 206), Order]
        public int INTSEPLINESCLRID { get; set; }           //D-230
                                                            //[DataMember(Order = 207), Order]
                                                            //public string INTSEPLINESCLRDES { get; set; }
        [DataMember(Order = 208), Order]
        public int INTMODERNCPLATECLRID { get; set; }      //D-240
                                                           //[DataMember(Order = 209), Order]
                                                           //public string INTMODERNCPLATECLDES { get; set; }
        [DataMember(Order = 210), Order]
        public int INTCGRIDCLRID { get; set; }  //D-290
                                                //[DataMember(Order = 220), Order]
                                                //public string INTCGRIDCLRDES { get; set; }
        #endregion Internal decoration L1

        #region Internal decoration L2
        [DataMember(Order = 221), Order]
        public int INTHTPLATESCLRID { get; set; }  //D-300
                                                   //[DataMember(Order = 222), Order]
                                                   //public string INTHTPLATESCLRDES { get; set; }
        [DataMember(Order = 223), Order]
        public int INTGLASSPLATECLRID { get; set; }  //D-320
                                                     //[DataMember(Order = 224), Order]
                                                     //public string INTGLASSPLATECLRDES { get; set; }
        [DataMember(Order = 225), Order]
        public int INTMODERNPLATECLRID { get; set; }  //D-340
                                                      //[DataMember(Order = 226), Order]
                                                      //public string INTMODERNPLATECLRDES { get; set; }
        [DataMember(Order = 227), Order]
        public int INTNIROSTALINESCLRID { get; set; } //D-370
                                                      //[DataMember(Order = 228), Order]
                                                      //public string INTNIROSTALINESCLDES { get; set; }
        [DataMember(Order = 229), Order]
        public int INTINSERTCLRID { get; set; }   //D-420
                                                  //[DataMember(Order = 230), Order]
                                                  //public string INTINSERTCLRDES { get; set; }
        [DataMember(Order = 240), Order]
        public string INTCPLATE4HTDMNDNAME { get; set; }  //D-710
        [DataMember(Order = 241), Order]
        public string INTSIDE_C_PLATE { get; set; }  //D-730
        #endregion Internal decoration L2

        #region Internal decoration L3
        [DataMember(Order = 242), Order]
        public string INTGRIDCPLATE { get; set; } //D-780
        [DataMember(Order = 243), Order]
        public string INTHTPLATE { get; set; } //D-790
        [DataMember(Order = 244), Order]
        public int INTVITRAGE4DIAMONDQ { get; set; } //D-820
        [DataMember(Order = 245), Order]
        public string INTFINMODERNCPLATE { get; set; } //D-840
        [DataMember(Order = 246), Order]
        public string INTFINMODERNSEPLINE { get; set; } //D-960
        [DataMember(Order = 247), Order]
        public string INTFINMODERNPLATE { get; set; } //D-990

        #endregion Internal decoration L3
        #endregion Internal decoration (intdecor)

        #region staticwing
        
        [DataMember(Order = 255), Order]
        public string SWINGHASLOCK { get; set; }  // "Y" | "" (string.empty) FIELDCODE = "SWINGHASLOCK"  - new 
        
        //[NonSerialized]                         //TODO - check this : in case of half wing only staticwing is filled so SWING_OPENSIDE should also be saved in Priority !
        public string SWING_OPENSIDE { get; set; }  // at present - these three fields are not SERIALIZED !
        public string SWING_OPENSIDE_RIGHT { get; set; }
		public string SWING_OPENSIDE_LEFT { get; set; }

		[DataMember(Order = 260), Order]
        
        public int CENTRALCOLCLRID { get; set; }         //D-390
        [DataMember(Order = 261), Order]
        public string CENTRALCOLCLRDES { get; set; }
        [DataMember(Order = 262), Order]
        public int SHALVANIACLRID { get; set; }          //D-400
        [DataMember(Order = 263), Order]
        public string SHALVANIACLRDES { get; set; }
        [DataMember(Order = 264), Order]
        public int TRSH_SWING_CYLINDER { get; set; }    //D-510

        [DataMember(Order = 265), Order]
        public string SWING_LOCKNAME { get; set; }    // new
        [DataMember(Order = 266), Order]
        public int HW4EXTRAWING { get; set; }         //D-520  was logic (== bool)  now - 26/06/2022 -  it's int - it's a foreign key for  TRSH_HARDWARE   
                                                      //  i.e. it points at a TRSH_HARDWARE record . Note TRSH_HARDWARE is a table, it's primary key is TRSH_HARDWARE.TRSH_HARDWARE
        [DataMember(Order = 267), Order]
        public int SWING_HWACCESSORYID { get; set; }   // new 12/07/2022 

        [DataMember(Order = 268), Order]
        public int SWING_HWCOLORID { get; set; }  // new 
        [DataMember(Order = 269), Order]
        public int SWING_DRIL4HW { get; set; }   // new 

        [DataMember(Order = 270), Order]
        public string DESIGNEDEXTRAWING { get; set; }    //D-530  Y, N , ''
        [DataMember(Order = 271), Order]
        public string DESIGNEDWINDOWEDWING { get; set; } //D-540   Y, N , ''
        [DataMember(Order = 272), Order]
        public int EXTRAWINGWIDTH { get; set; }          //D-560
        [DataMember(Order = 273), Order]
        public int CENTRALCOLWIDTH { get; set; }         //D-860
                                                         //TODO
        [DataMember(Order = 274), Order]
        public string SWING_TURBO { get; set; }   // new Y, N, ''  TURBOAPPARATUS for staticwing .

        [DataMember(Order = 275), Order]
        public string SWING_HANDLENAME { get; set; }   // new
        [DataMember(Order = 276), Order]
        public int SWING_HANDLECOLORID { get; set; }   // new
        [DataMember(Order = 277), Order]
        public string SWING_VENTS { get; set; }   //  new 
                                                  //end TODO
        [DataMember(Order = 278), Order]
        public string RAFAFAONSTATICWING { get; set; }   //D-1150  Y, N , ''
        [DataMember(Order = 279), Order]
        public string SWING_CATDOOR { get; set; }   //new 

        //ref: https://stackoverflow.com/questions/7693391/nonserialized-on-property
        //[XmlIgnore]
        //[ScriptIgnore]
        //[DataMember(Order = 277), Order]
        
        // fields added on 17/10/2022 for handling meaged 903 - of Model A-GLVN  
        public int SWINGCOLORID { get; set; } // just for css class disabled and hidden actually gets the value of movingwing DOORCOLORID (D-510)
        public int SWINGEXTCOLORID { get; set; } //  just for css class disabled and hidden actually gets the value of movingwing EXTCOLORID
        public int SWINGINTTCOLORID { get; set; } //  just for css class disabled and hidden actually gets the value of movingwing INTCOLORID
        //  end of 17/10/2022 fields 
        public int SWING_LOCKDRILHEIGHT { get; set; } // just for css class and disabled

        #endregion staticwing 
        #region hinges
        [DataMember(Order = 290), Order]
        public int HINGESNUM { get; set; }      //D-580
        [DataMember(Order = 291), Order]
        public int BACKPINHEIGHT { get; set; }  //D-590
        [DataMember(Order = 292), Order]
        public int HINGE1HEIGHT { get; set; }   //D-600
        [DataMember(Order = 293), Order]
        public int HINGE2HEIGHT { get; set; }   //D-610
        [DataMember(Order = 294), Order]
        public int HINGE3HEIGHT { get; set; }   //D-620
        [DataMember(Order = 295), Order]
        public int HINGE4HEIGHT { get; set; }   //D-630
        [DataMember(Order = 296), Order]
        public int HINGE5HEIGHT { get; set; }   //D-640
        [NonSerialized]
        public int optionalHingeHeight;
        #endregion hinges
        #region handle
        [DataMember(Order = 300), Order]
        public string HANDLENAME { get; set; }   //D-940
        [NonSerialized]
        public string handleName1;   
        [DataMember(Order = 301), Order]
        public int HANDLECOLORID { get; set; }   //D-950
        [DataMember(Order = 302), Order]
        public string HANDLECOLORDES { get; set; }
        #endregion handle

        #region accessories
        //[DataMember(Order = 310), Order]
        //public string RAFAFALOC { get; set; }
        [DataMember(Order = 310), Order]
        public int WETCOLORBOXID { get; set; }  //D-1050  - renamed and type changed
        [DataMember(Order = 311), Order]        // new 
        public int WETCOLORBOXQ { get; set; }
        [DataMember(Order = 312), Order]
        public int COLORSPRAYID { get; set; }   //D-1060 - renamed and type changed
        [DataMember(Order = 313), Order]
        public int COLORSPRAYQ { get; set; }   //new 
        [DataMember(Order = 314), Order]
        public string CLEANINGSPRAY { get; set; }  //D-1070
        [DataMember(Order = 315), Order]
        public string LCDEYE { get; set; }   //D-1030  // Y, N, ''  moved here from movingwing tabPage  - 

        [DataMember(Order = 316), Order]
        public string SAFELOCK { get; set; }   //D-1040  // Y, N, '' 




        //[DataMember(Order = 315), Order]
        //public string CORE { get; set; }           //D-1080
        //[DataMember(Order = 316), Order]
        //public string MEZUZA { get; set; }         //D-1090

        #endregion accessories
        [DataMember(Order = 350), Order]
        public string COMMENTS { get; set; } = string.Empty;
        [NonSerialized]
        public string meaged = string.Empty;
        [NonSerialized]
        public string currPropName = String.Empty;
        [NonSerialized]
        public string currTabName = String.Empty;
        [NonSerialized]
        public string prevTabName = string.Empty;
        [NonSerialized]
        public System.Data.DataTable dtTabFlds = new System.Data.DataTable();
        [NonSerialized]
        public BlazorServerApp1.Pages.Configurator4 config4;
        [NonSerialized]
        public bool RestartClicked = false;
        [NonSerialized]
        public Dictionary<string, string> borderColors; // { get; set; }
        [NonSerialized]
        public Dictionary<string, string> btnClasses; // { get; set; }
        [NonSerialized]
        public Dictionary<string, string> divClasses; // { get; set; }
        [NonSerialized]
        public Dictionary<string, string> thClasses; // { get; set; }
        [NonSerialized]
        public Dictionary<string, bool> disabledFlds; // { get; set; }
        [NonSerialized]
        public Dictionary<string, ElementReference> dicRefs = new Dictionary<string, ElementReference>();
        [NonSerialized]
        public Dictionary<string, bool> hideBtns; // { get; set; }  // not used yet 
        [NonSerialized]
        public bool staticwingChanged = false;
        [NonSerialized]
        public bool showDecor = false;
        [NonSerialized]
        public bool LockDrilHChanged = false;
        public void initBorderColors()
        {
            Type objType = this.GetType();
            PropertyInfo[] props = objType.GetProperties();
            borderColors = new Dictionary<string, string>();
            thClasses = new Dictionary<string, string>();
            disabledFlds = new Dictionary<string, bool>();
            hideBtns = new Dictionary<string, bool>();
            //ElementReference er;
            foreach (PropertyInfo pinfo in props)
            {
                if (pinfo.Name == "SWING_OPENSIDE")
                {
                    int dbg = 17;  
                }
                borderColors.Add(pinfo.Name, "blueBorder"); //string.Empty);
                thClasses.Add(pinfo.Name, "thBlue");
                //if (!disabledFlds.ContainsKey(pinfo.Name))
                //    disabledFlds.Add(pinfo.Name, false);
                //else
                disabledFlds[pinfo.Name] = false;
            }
            initBtnClasses();
            disabledFlds["REFERENCE"] = true;    // 01/06/2022
            disabledFlds["ESTSHIPDATE"] = true;  // it is enabled only before sending the the form to Priority 20/06/2022 
            disabledFlds["btnDoor"] = false;
            disabledFlds["btnLintel"] = disabledFlds["btnCover"] = disabledFlds["btnSideUnit"] = disabledFlds["btnECabinet"] = disabledFlds["btnShelves"] = true;
            disabledFlds["general"] = disabledFlds["selectprod"] = disabledFlds["proddes"] = false;  // on startup I assume these buttons can be Enabled ! 13/06/2022 
                                                                                                     // maybe teh CU will ask to Disable them !
            initDivClasses();
            //initHideBtns();  not used yet 
            RestartClicked = false;
        }
        public void initBtnClasses()
        {
            btnClasses = new Dictionary<string, string>();
            btnClasses["restart"] = "buttonRed";
            btnClasses["general"] =
            btnClasses["selectprod"] =
            btnClasses["btnDoor"] =
            btnClasses["proddes"] = "button";
            btnClasses["btnLintel"] = btnClasses["btnCover"] = btnClasses["btnSideUnit"] = btnClasses["btnECabinet"] = btnClasses["btnShelves"] = "buttonDisabled";

            btnClasses["managerapproval"] = "button";
            btnClasses["save"] = "buttonDisabled";


            foreach (string tabName in UiLogic.tabNames)
            {
                btnClasses.Add(tabName, "button");
            }
        }
        public void initHideBtns()  // not used yet !
        {
            hideBtns["EXTCOLORID"] =
            hideBtns["INTCOLORID"] = false;
        }
        public void initDivClasses()
        {
            divClasses = new Dictionary<string, string>();
            foreach (string tabName in UiLogic.tabNames)
            {
                divClasses[tabName] = "divNotActive";
            }
            divClasses["divProducts"] = "divNotActive";
            divClasses["dynCompStyle"] = "divActive";   // the tabpage is shown only when it's active ! 
        }
        public void markButton(string tabName)
        {
            foreach (string tabName1 in UiLogic.tabNames)
            {
                string btnClass = (tabName1 == tabName ? "buttonActive" : "button");
                btnClasses[tabName1] = btnClass;
                string divClass = (tabName1 == tabName ? "divActive" : "divNotActive");
                divClasses[tabName1] = divClass;
            }
        }
        public async Task KeyPressHandler(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            //string currTabName = "movingwing";
            RestartClicked = false;  // this handler is launched when a key is pressed in a field not btnRestart !
            if (e.Key.ToLowerInvariant() == "enter")
            {
                //RestartClicked = false;
                int j = 0;
                //DataTable dtTabFlds = new DataTable();
                //string currTabName = UiLogic.getTabOfField(doorConfig.currPropName);
                string nextfld = UiLogic.getNextTabFld(this, dtTabFlds, currPropName);
                if (!string.IsNullOrEmpty(nextfld))
                {
                    j = Array.IndexOf(UiLogic.propNames, nextfld);
                    if (j < 0 || j > UiLogic.propNames.Length)
                    {
                        string errFmt = "doorConfig.KeyPressHandler() index out of range! : nextFld = {0} , j=Array.IndexOf(UiLogic.propNames, nextfld) = {1}"
                                         + " pls check in Priority that nextfld is in TRSH_CONF_FIELDS table/form and in DoorConfig class";
                        myLogger.log.Error(string.Format(errFmt, nextfld, j));
                        return;  // indexOutOfRange 
                    }

                    if (dicRefs.ContainsKey(UiLogic.propNames[j]))
                        dicRefs[UiLogic.propNames[j]].FocusAsync();
                    return;
                }
                else
                {
                    string nextTabName = UiLogic.getNextTabName(this, currTabName);
                    if (UiLogic.try2UpdateBtnClass(this, currTabName))
                    {
                        if (config4 != null)
                            config4.RefreshState();
                    }
                }
            }
        }
    }
    public class ValuesDoorConfig
    {
        public List<DoorConfig> value { get; set; }
    }
}
