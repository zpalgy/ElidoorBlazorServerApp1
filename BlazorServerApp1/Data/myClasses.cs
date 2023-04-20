using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Reflection.Metadata;

namespace BlazorServerApp1.Data
{
    //public class myClasses
    //{
    //}

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order_;
        public OrderAttribute([System.Runtime.CompilerServices.CallerLineNumber]int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }

    [DataContract]
    public class AGENT_Class
    {
        [DataMember(Order = 1)]
        public int AGENT { get; set; }
        [DataMember(Order = 2)]
        public string AGENTCODE { get; set; }
        [DataMember(Order = 2)]
        public string AGENTNAME { get; set; }
    }
    public class ValuesAGENT_Class
    {
        public List<AGENT_Class> value { get; set; }
    }

    [DataContract,Serializable]
    public class CUSTOMER_Class
    {
        [DataMember(Order = 1)]
        public string CUST { get; set; }
        [DataMember(Order = 2)]
        public string CUSTNAME { get; set; }
        [DataMember(Order = 3)]
        public string CUSTDES { get; set; }
        [DataMember(Order = 4)]
        public string ADDRESS { get; set; }
        [DataMember(Order = 5)]
        public string ADDRESS2 { get; set; }
        [DataMember(Order = 6)]
        public string ADDRESS3 { get; set; }
        [DataMember(Order = 7)]
        public string TRSH_NOTECOMPLIENT { get; set; }   // bool Y | ''
                                                         //public string TRSH_LOGO { get; set; }
        [DataMember(Order = 7)]
        public string TRSH_TURBO { get; set; }   // bool Y | ''
        [DataMember(Order = 8)]
        public string TRSH_SALESMAN { get; set; }  // bool Y | ''
    }
    public class ValuesCUSTOMER_Class
    {
        public List<CUSTOMER_Class> value { get; set; }
    }

    [DataContract]
    public class FAMILY_Class
    {
        [DataMember(Order = 1)]
        public int FAMILY { get; set; }
        [DataMember(Order = 2)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 3)]
        public string FAMILYDESC { get; set; }
    }
    public class ValuesFAMILY_Class
    {
        public List<FAMILY_Class> value { get; set; }
    }

    //PARTNAME,FAMILY,FAMILYNAME,FAMILYDES
    public class FamilyOfPart_Class
    {
        [DataMember(Order = 1)]
        public string PARTNAME { get; set; }
        // [DataMember(Order = 2)]
        // public int FAMILY { get; set; }
        [DataMember(Order = 3)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 4)]
        public string FAMILYDES { get; set; }
    }
    public class ValuesFamilyOfPart_Class
    {
        public List<FamilyOfPart_Class> value { get; set; }
    }

    [DataContract]
    public class PART_Class
    {
        [DataMember(Order = 1)]
        public string PART { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
        [DataMember(Order = 4)]
        public int TRSH_MODEL { get; set; }
        [DataMember(Order = 5)]
        public string TRSH_MODELDES { get; set; }
        [DataMember(Order = 6)]
        public int TRSH_WINGSNUM { get; set; }
        [DataMember(Order = 7)]
        public string TRSH_WINGSNUMDES { get; set; }
        //[DataMember(Order = 8)]
        //public string MPARTNAME { get; set; }
        //[DataMember(Order = 9)]
        //public string MPARTDES { get; set; }
        [DataMember(Order = 10)]
        public int FAMILY { get; set; }
        [DataMember(Order = 11)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 12)]
        public string FAMILYDES { get; set; }
       // [DataMember(Order = 13)]
       // public int TRSH_DOOR_HWCATCODE { get; set; }
    }
    public class ValuesFamilyParts
    {
        public List<PART_Class> FAMILY_LOGPART_SUBFORM { get; set; }
    }
    public class FamilyParts_Class
    {
        public List<ValuesFamilyParts> value { get; set; }
    }
    public class ValuesPART_Class
    {
        public List<PART_Class> value { get; set; }
    }


    [DataContract]
    public class TRSH_COLOR_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_COLORID { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_COLORTYPE { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_COLORTYPEDES { get; set; }
        [DataMember(Order = 4)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 5)]
        public string PARTDES { get; set; }
        [DataMember(Order = 6)]
        public string TRSH_TEXTURE { get; set; }

        //[DataMember(Order = 4)]
        //public string PARTNAME { get; set; }
        //[DataMember(Order = 5)]
        //public string PARTDES { get; set; }
    }
    public class ValuesTRSH_COLOR_Class
    {
        public List<TRSH_COLOR_Class> value { get; set; }
    }



    [DataContract]
    //123456789 123456789
    public class TRSH_LOCKHINGE_DRILH_Class   //table 230 for field D-650 in Door title panel (גובה ניקוב \ מרכז לשונית) moved to movinwing tab
                                              // because it depends on FAMILY and DOOHEIGHT  20/11/2021
    {
        [DataMember(Order = 1)]
        public int TRSH_LOCKHINGE_DRILH { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public int DOORHEIGHTMIN { get; set; }
        [DataMember(Order = 4)]
        public int DOORHEIGHTMAX { get; set; }
        [DataMember(Order = 5)]
        public string MEASURENAME { get; set; }
        [DataMember(Order = 6)]
        public int LOCKDRILHEIGHT { get; set; }
        [DataMember(Order = 7)]
        public int BACKPINHEIGHT { get; set; }
        [DataMember(Order = 8)]
        public int HINGESNUM { get; set; }
        [DataMember(Order = 9)]
        public int HINGE1HEIGHT { get; set; }
        [DataMember(Order = 10)]
        public int HINGE2HEIGHT { get; set; }
        [DataMember(Order = 11)]
        public int HINGE3HEIGHT { get; set; }
        [DataMember(Order = 12)]
        public int HINGE4HEIGHT { get; set; }
        [DataMember(Order = 13)]
        public int HINGE5HEIGHT { get; set; }
    }
    public class ValuesTRSH_LOCKHINGE_DRILH_Class
    {
        public List<TRSH_LOCKHINGE_DRILH_Class> value { get; set; }
    }

    [DataContract]
    public class TRSH_HARDWARE_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_HARDWARE { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public string PARTNAME { get; set; }  //HARDWARENAME
        [DataMember(Order = 4)]
        public string PARTDES { get; set; }  //HARDWAREDES
        [DataMember(Order = 5)]
        public string OPENSIDE { get; set; }
		[DataMember(Order = 6)]
		public string OPENSIDE_RIGHT { get; set; }
		[DataMember(Order = 7)]
		public string OPENSIDE_LEFT { get; set; }
		[DataMember(Order = 8)]
		public string OPENSIDE_RIGHTLEFT { get; set; }
		[DataMember(Order = 9)]
        public int DRIL4HW { get; set; }
        [DataMember(Order = 10)]
        public string DRIL4HWDES { get; set; }
        [DataMember(Order = 11)]
        public string COLORED { get; set; }   // boolean Y | '' 
        [DataMember(Order = 12)]
        public string NIKEL { get; set; }   // boolean Y | '' 
        [DataMember(Order = 13)]
        public string BRONZE { get; set; }   // boolean Y | '' 
        [DataMember(Order = 14)]
        public string FORHALFCYL { get; set; }   // boolean Y | '' 
		[DataMember(Order = 15)]
        public string IS4DECOR { get; set; }    //boolean Y | '' 
		[DataMember(Order = 16)]
        public string PARTNAME2 { get; set; }   // dual part right -> left, or left -> right of point to itself.
        [DataMember(Order = 17)]
        public int OPPOSITESIDE_PART { get; set; }    // PART.PART of the opposite Hardware
    }
    public class ValuesTRSH_HARDWARE_Class
    {
        public List<TRSH_HARDWARE_Class> value { get; set; }
    }

    [DataContract]
    public class HWACCESSORY_Class
    {
        [DataMember(Order = 1)]
        public int HWACCESSORYID { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
        //public string HWACCESSORYDES { get; set; }
        [DataMember(Order = 4)]
        public string COLORED { get; set; }   // boolean Y | '' 
        [DataMember(Order = 5)]
        public string NIKEL { get; set; }   // boolean Y | '' 
        [DataMember(Order = 6)]
        public string BRONZE { get; set; }   // boolean Y | '' 
        
    }
    public class ValuesHWACCESSORY_Class
    {
        public List<HWACCESSORY_Class> value { get; set; }
    }
    [DataContract]
    public class HW_ACC_REL_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_HW_ACC_ID { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_HARDWARE { get; set; }
        [DataMember(Order = 3)]
        public string HWPARTDES { get; set; }
        [DataMember(Order = 4)]
        public int HWACCESSORYID { get; set; }
        [DataMember(Order = 5)]
        public string ACCPARTDES { get; set; }
    }

    public class ValuesHW_ACC_REL_Class
    {
        public List<HW_ACC_REL_Class> value { get; set; }
    }

    [DataContract]
    public class DRIL4HW_Class
    {
        [DataMember(Order = 1)]
        public int DRIL4HW { get; set; }
        [DataMember(Order = 2)]
        public string DRIL4HWDES { get; set; }
    }
    public class ValuesDRIL4HW_Class
    {
        public List<DRIL4HW_Class> value { get; set; }
    }

    [DataContract]
    public class CYLINDER_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_CYLINDER { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
        [DataMember(Order =4)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 5)]
        public string OPENMODE { get; set; }
		[DataMember(Order = 6)]
		public string OPENIN { get; set; }
		[DataMember(Order = 7)]
		public string OPENOUT { get; set; }
		[DataMember(Order = 8)]
		public string OPEN_INOUT { get; set; }
		[DataMember(Order = 9)]
        public string ISHALFCYLINDER { get; set; }
        [DataMember(Order = 7)]
        public int SORT { get; set; }
		[DataMember(Order = 8)]
		public int TRSH_CYLCATEGORY { get; set; }
	}
    public class ValuesCYLINDER_Class
    {
        public List<CYLINDER_Class> value { get; set; }
    }

    [DataContract]
    public class CYLHW_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_CYLHW { get; set; }
        [DataMember(Order = 2)]
        public string WING_OPENMODE { get; set; }
        [DataMember(Order = 3)]
        public string PARTDESCYL { get; set; }
        [DataMember(Order = 4)]
        public string OPENSIDE { get; set; }
        [DataMember(Order = 5)]
        public string PARTDESHW { get; set; }
        [DataMember(Order = 6)]
        public int TRSH_CYLINDER { get; set; }
        [DataMember(Order = 7)]
        public int PARTCYL { get; set; }
        [DataMember(Order = 8)]
        public int TRSH_HARDWARE { get; set; }
        [DataMember(Order = 9)]
        public int PARTHW { get; set; }
    }

    public class ValuesCYLHW_Class
    {
        public List<CYLHW_Class> value { get; set; }
    }


    [DataContract]
    public class TRSH_LOCK_Class
    {
        [DataMember (Order = 1)]
        public int TRSH_LOCK { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 4)]
        public string PARTDES { get; set; }
        [DataMember(Order = 5)]
        public string TRSH_ELIDOOR_LOGO { get; set; }
    }
    public class ValuesTRSH_LOCK_Class
    {
        public List<TRSH_LOCK_Class> value { get; set; }
    }



    [DataContract]
    public class WINDOWHEIGHT_Class  // table 140
    {
        [DataMember(Order = 1)]
        public int WINDOWHEIGHT_ID { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 3)]
        public int MINDOORHEIGHT { get; set; }
        [DataMember(Order = 4)]
        public int MAXDOORHEIGHT { get; set; }
        [DataMember(Order = 5)]
        public string MEASURENAME { get; set; }
        [DataMember(Order = 6)]
        public int WINDOWHEIGHT { get; set; }
        [DataMember(Order = 7)]
        public int HEIGHT4DRIL { get; set; }
    }
    public class ValuesWINDOWHEIGHT_Class
    {
        public List<WINDOWHEIGHT_Class> value { get; set; }
    }

    [DataContract]
    public class WINDOWWIDTH_Class  // table 150  - this is only for Movingwing not for Staticwing 22/06/2022
    {
        [DataMember(Order = 1)]
        public int WINDOWWIDTH_ID { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_WINGSNUMDES { get; set; }
        [DataMember(Order = 4)]
        public int MIN_DOORWIDTH { get; set; }
        [DataMember(Order = 5)]
        public int MAX_DOORWIDTH { get; set; }
        [DataMember(Order = 6)]
        public int WINDOWWIDTH { get; set; }
    }
    public class ValuesWINDOWWIDTH_Class
    {
        public List<WINDOWWIDTH_Class> value { get; set; }
    }

    [DataContract]
    public class WWIDTH_STATIC_Class  //  this is only for Staticwing not for Movingwing  22/06/2022
    {
        [DataMember(Order = 1)]
        public int WWIDTH_STATIC_ID { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_WINGSNUMDES { get; set; }
        [DataMember(Order = 4)]
        public int MIN_DOORWIDTH { get; set; }
        [DataMember(Order = 5)]
        public int MAX_DOORWIDTH { get; set; }
        [DataMember(Order = 6)]
        public int WINDOWWIDTH { get; set; }
        [DataMember(Order =7)]
        public string HASLOCK { get; set; }  // Y | '' bool 
    }
    public class ValuesWWIDTH_STATIC_Class
    {
        public List<WWIDTH_STATIC_Class> value { get; set; }
    }


    [DataContract]
    public class PROFILE4WINDOW_Class   //table 160
    {
        [DataMember(Order = 1)]
        //123456789 12345678
        public string PROFILE4WINDOWNAME { get; set; }
        [DataMember(Order = 2)]
        public string PROFILE4WINDOWDES { get; set; }
    }
    public class ValuesPROFILE4WINDOW_Class
    {
        public List<PROFILE4WINDOW_Class> value { get; set; }
    }


    [DataContract]
    public class TILETYPE_Class  // table 70
    {
        [DataMember(Order = 1)]
        public int PART { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
    }
    public class ValuesTILETYPE_Class
    {
        public List<TILETYPE_Class> value { get; set; }
    }

    [DataContract]
    public class GLASS4WINDOW_Class  // table 170
    {
        [DataMember(Order = 1)]
        public int GLASS4WINDOWID { get; set; }
        [DataMember(Order = 2)]
        public string GLASS4WINDOWDES { get; set; }
        [DataMember(Order = 3)]
        public string GLASS4WINDOWKITNAME { get; set; }
    }
    public class ValuesGLASS4WINDOW_Class
    {
        public List<GLASS4WINDOW_Class> value { get; set; }
    }

    [DataContract]
    public class GRID_Class     // table 180
    {
        [DataMember(Order = 1)]
        public int GRID_ID { get; set; }
        [DataMember(Order = 2)]
        public string GRIDNAME { get; set; }
        [DataMember(Order = 2)]
        public string GRIDDES { get; set; }
    }
    public class ValuesGRID_Class
    {
        public List<GRID_Class> value { get; set; }
    }

    [DataContract]
    public class VITRAGE_Class  // table 90
    {
        [DataMember(Order = 1)]
        public string VITRAGENAME { get; set; }
        [DataMember(Order = 2)]
        public string VITRAGEDES { get; set; }
    }
    public class ValuesVITRAGE_Class
    {
        public List<VITRAGE_Class> value { get; set; }
    }

    [DataContract]
    public class RAW4CPLATES_Class  // table 110
    {
        [DataMember(Order = 1)]
        public int RAW4CPLATES { get; set; }
        [DataMember(Order = 2)]
        public string RAW4CPLATESNAME { get; set; }
        // [DataMember(Order = 3)]
        // public string RAW4CPLATEDES { get; set; }
    }
    public class ValuesRAW4CPLATES_Class
    {
        public List<RAW4CPLATES_Class> value { get; set; }
    }
    [DataContract]
    public class GRID4HT1084_Class  // table 370
    {
        [DataMember(Order = 1)]
        public string GRID4HT1084NAME { get; set; }
        [DataMember(Order = 2)]
        public string GRID4HT1084DES { get; set; }
    }
    public class ValuesGRID4HT1084_Class
    {
        public List<GRID4HT1084_Class> value { get; set; }
    }
    [DataContract]
    public class VITRAGE4DIAMOND_Class //table 250
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public string VITRAGE4DIAMONDNAME { get; set; }
        [DataMember(Order = 2)]
        public string VITRAGE4DIAMONDDES { get; set; }
    }
    public class ValuesVITRAGE4DIAMOND_Class
    {
        public List<VITRAGE4DIAMOND_Class> value { get; set; }
    }
    [DataContract]
    public class HANDLE4DIAMOND_Class    // table 360
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public string HANDLE4DIAMONDNAME { get; set; }
        [DataMember(Order = 2)]
        public string HANDLE4DIAMONDDES { get; set; }
        [DataMember(Order = 3)]
        public int LENGTH { get; set; }
    }
    public class ValuesHANDLE4DIAMOND_Class
    {
        public List<HANDLE4DIAMOND_Class> value { get; set; }
    }

    [DataContract]
    public class HANDLE_Class    // table 360
    {
        [DataMember(Order = 1)]
        public int HANDLEID { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
        [DataMember(Order = 4)]
        public int LENGTH { get; set; }
        [DataMember(Order = 5)]
        public string COLORED { get; set; }    // Y | ''
    }
    public class ValuesHANDLE_Class
    {
        public List<HANDLE_Class> value { get; set; }
    }

    [DataContract]
    public class ConfField_Class
    {
        [DataMember(Order = 1)]
        public int FIELDID { get; set; }
        [DataMember(Order = 2)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 4)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 5)]
        public string FIELDDATATYPE { get; set; }
        [DataMember(Order = 6)]
        public string FIELDNAME { get; set; }  // e.g. AGENT, TRSH_MODELNAME - properties of DoorConfig class
        [DataMember(Order = 7)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 8)]
        public string FIELDDES { get; set; }
        [DataMember(Order = 9)]
        public string CONFIG_SUBFORM { get; set; }
        [DataMember(Order = 10)]
        public int TABINDEX { get; set; }
        [DataMember(Order = 11)]
        public string MANDATORY { get; set; }   // M, O (=OPTIONAL), X (not MANDATORY - unknown ) 
    }

    public class ValuesConfField_Class
    {
        public List<ConfField_Class> value { get; set; }
    }

    [DataContract]
    public class MeagedFields_Class
    {
        [DataMember(Order = 1)]
        public string MEAGEDNAME { get; set; }  
        [DataMember(Order = 2)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 4)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 5)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 6)]
        public string FIELDNAME { get; set; }
        [DataMember(Order = 7)]
        public bool Visible { get; set; }
    }
    public class ValuesMeagedFields_Class
    {
        public List<MeagedFields_Class> value { get; set; }
    }

    [DataContract]
    public class DecorSideFlds_Class
    {
        [DataMember(Order = 1)]
        public string DECORSIDECODE { get; set; }
        [DataMember(Order = 2)]
        public string DECORSIDE { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 4)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 5)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 6)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 7)]
        public string SHOW { get; set; }
    }
    public class ValuesDecorSideFlds_Class
    {
        public List<DecorSideFlds_Class> value { get; set; }
    }

    [DataContract]
    public class Defaults_Class
    {
        [DataMember(Order = 1)]
        public int LINENUM { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_NUM { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 4)]
        public string TRSH_MEAGEDNAME { get; set; }
        [DataMember (Order = 5)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 6)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 7)]
        public string CONFIG_FIELDNAME { get; set; }  //e.g. dlstPARTNAME
        [DataMember(Order = 8)]
        public string FIELDNAME { get; set; }  //e.g. TRSH_MODELNAME
        [DataMember(Order = 9)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 9)]
        public string FIELDDATATYPE { get; set; }
        [DataMember(Order = 10)]
        public string DEFVAL { get; set; }
        [DataMember(Order = 10)]
        public string VAL_LOCKED { get; set; }
        [DataMember(Order = 11)]
        public string WRONGVAL { get; set; }
    }
    public class ValuesDefaults_Class
    {
        public List<Defaults_Class> value { get; set; }
    }

    [DataContract]
    public class WingsNum_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_WINGSNUM { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_WINGSNUMCODE { get; set; }
            [DataMember(Order = 3)]
        public string TRSH_WINGSNUMDES { get; set; }
    }
    public class ValuesWingsNum_Class
    {
        public List<WingsNum_Class> value { get; set; }
    }

    [DataContract]
    public class Model_Class
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public int TRSH_MODEL { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_MODELDES { get; set; }
        [DataMember(Order = 4)]
        public string TRSH_MEAGEDNAME { get; set; }
        [DataMember(Order = 5)]
        public string TRSH_MEAGEDDES { get; set; }
        [DataMember(Order = 6)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 7)]
        public string TRSH_DOOR_HWCAT { get; set; }
		[DataMember(Order = 8)]
		public int TRSH_CYLCATEGORY { get; set; }
	}
    public class ValuesModel_Class
    {
        public List<Model_Class> value { get; set; }
    }
    [DataContract]
    public class Model_Part_Class
    {
        [DataMember(Order = 1)]
        public string TRSH_MODELNAME { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_MODELDES { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_WINGSNUMDES { get; set; }
        [DataMember(Order = 4)]
        public string TRSH_WINGSNUMCODE { get; set; }
        [DataMember(Order = 5)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 6)]
        public string PARTDES { get; set; }
        [DataMember(Order = 7)]
        public int TRSH_MODEL { get; set; }
        [DataMember(Order = 8)]
        public int TRSH_WINGSNUM { get; set; }
        [DataMember(Order = 9)]
        public int PART { get; set; }
    }
    public class ValuesModel_Part_Class
    {
        public List<Model_Part_Class> value { get; set; }
    }

    [DataContract]
    public class Decoration_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_DECORATION { get; set; }
                    [DataMember(Order = 2)]
        public string DECORATIONSIDE { get; set; }
    }
    public class ValuesDecoration_Class
    {
        public List<Decoration_Class> value { get; set; }
    }

    //[DataContract]
    //public class WingWidth_Class
    //{
    //    [DataMember(Order = 1)]
    //    public int TRSH_WING_WIDTH { get; set; }
    //    [DataMember(Order = 2)]
    //    public int FROM_WIDTH { get; set; }
    //    [DataMember(Order = 3)]
    //    public int TO_WIDTH { get; set; }
    //    [DataMember(Order = 4)]
    //    public int AUTO_WINDOW_WIDTH { get; set; }
    //}
    //public class ValuesWingWidth_Class
    //{
    //    public List<WingWidth_Class> value { get; set; }
    //}

    [DataContract]
    public class Complient_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_COMPLIENT { get; set; }
        [DataMember(Order = 2)]
        public string COMPLIENTDES { get; set; }
    }
    public class ValuesComplient_Class
    {
        public List<Complient_Class> value { get; set; }
    }

    [DataContract, Serializable]
    
    

    //public class ReadOnlyDictionary<TKey, TValue>
    //{
    //    private Dictionary<TKey, TValue> _dict;
    //    public ReadOnlyDictionary(Dictionary<TKey, TValue> dict)
    //    {
    //        _dict = dict;
    //    }

    //    public TValue this[TKey key] { get { return _dict[key]; } }
    //}
    public class UserData
    {
        public string TabId { get; set; }
    }
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public string compName { get; set; }
    }

    public static class HebNouns
    {
		public static int IdOfNone = 99999;
		public static string NameOfNone = "9999999";
		public static string None = "ללא";
        public static string Wing = "כנף";
		public static string HalfWing = "חצי כנף";
        public static string TwoWings = "דו כנפי";
        public static string Staticwing = "כנף קבועה";
		public static string NameOfSmooth = "Smooth";
		public static string Smooth = "חלק";
		public static string NameOfFluted = "Fluted";
		public static string Fluted = "מחורץ";
		public static string External = "חוץ";
		public static string Outside = "החוצה";
		public static string Internal = "פנים";
		public static string Inside = "פנימה";
		public static string BothSides = "דו צדדי";
		public static string Right = "ימין";
		public static string Left = "שמאל";
		public static string Megulvan = "מגולוון";
		public static string NoColor = "מגולוון";
        public static string Modern = "מודרני";
        public static string With = "עם";
		public static string requiredFieldsAreEmpty = "שדות חובה לא מולאו, על מנת להמשיך נדרש למלא את כל שדות החובה";
	}

    // ref: https://stackoverflow.com/questions/60264657/get-current-user-in-a-blazor-component
    //public class CurrentCorporateUserService : CorporateUserService
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public CurrentCorporateUserService(IHttpContextAccessor httpContextAccessor,
    //        MyDbContext context) : base(context)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public CorporateUser GetCurrentUser()
    //    {
    //        return base.GetUserByUsername(_httpContextAccessor.HttpContext.User.Identity.Name.Substring(8));
    //    }
    //}

    }
