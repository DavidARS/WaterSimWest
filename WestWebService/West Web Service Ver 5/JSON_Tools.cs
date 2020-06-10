using System;
using System.Collections.Generic;

namespace JSTOOLS
{
///-------------------------------------------------------------------------------------------------
/// <summary>   Base Json value class. </summary>
///
///-------------------------------------------------------------------------------------------------

public abstract class JSON_Value
{
    
    protected bool FisError = false;
    protected string FErrMessage = "";

    protected string FOriginalStr = "";

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Default constructor. </summary>
    ///
    /// <remarks>   Ray Quay, 1/9/2014. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Value()
    {

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the serialize JSON string. </summary>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public abstract string Serialize();

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this object is json bool. </summary>
    ///
    /// <value> true if this object is json bool, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool isJSON_Bool
    { get {return (this is JSON_Bool); } }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this object is json number. </summary>
    ///
    /// <value> true if this object is json number, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool isJSON_Number
    { get { return (this is JSON_Number); } }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this object is json string. </summary>
    ///
    /// <value> true if this object is json string, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool isJSON_String
    { get { return (this is JSON_String); } }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this object is json array. </summary>
    ///
    /// <value> true if this object is json array, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool isJSON_Array
    { get { return (this is JSON_Array); } }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this object is json object. </summary>
    ///
    /// <value> true if this object is json object, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool isJSON_Object
    { get { return (this is JSON_Object); } }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether there was an error creating the object. </summary>
    ///
    /// <value> true if parse error, false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool ParseError
    {
        get { return FisError; }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a message describing the error. </summary>
    ///
    /// <value> A message describing the error. </value>
    ///-------------------------------------------------------------------------------------------------

    public string ErrorMessage
    {
        get { return FErrMessage; }
    }

       
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Json string value class </summary>
///-------------------------------------------------------------------------------------------------

public class JSON_String : JSON_Value
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Default Constructor. </summary>
    ///-------------------------------------------------------------------------------------------------

    public JSON_String()
    {
        FOriginalStr = "";
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    /// <param name="value">    The value. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_String(string value) 
    {
        FOriginalStr = value;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the value. </summary>
    ///
    /// <value> The value. </value>
    ///-------------------------------------------------------------------------------------------------

    public string Value
    {
        get { return FOriginalStr; }
        set { FOriginalStr = value; }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the serialize JSON string. </summary>
    ///
    /// <remarks>   Ray Quay, 1/14/2014. </remarks>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public override string Serialize()
    {
        return '\"' + FOriginalStr + '\"'; 
    }

}

///-------------------------------------------------------------------------------------------------
/// <summary>   Json bool. </summary>
///-------------------------------------------------------------------------------------------------

public class JSON_Bool : JSON_Value
{
    bool FValue = false;

    /// <summary>
    /// Default Constructor
    /// </summary>
    public JSON_Bool()
    {
        FValue = false;
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    ///
    /// <param name="value">    true to value. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Bool(bool value)
    {
        FValue = value;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    ///
    /// <param name="JsonString">   The json string. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Bool(string JsonString)
    {
        string temp = JsonString.ToUpper();
        FOriginalStr = JsonString;
        if (temp == "TRUE")
        {
            FValue = true;
        }
        else
            if (temp == "FALSE")
            {
                FValue = false;
            }
            else
            {
                FisError = true;
                FErrMessage = "Invalid Bool Value - " + JsonString;
            }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets a bool value. </summary>
    ///
    /// <value> true if value = true , false if not. </value>
    ///-------------------------------------------------------------------------------------------------

    public bool Value
    {
        get { return FValue; }
        set { FValue = value; }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the serialize JSON string. </summary>
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public override string Serialize()
    {
        return FValue.ToString();
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Json number value class </summary>
///
/// <remarks>   Ray Quay, 1/9/2014. </remarks>
///-------------------------------------------------------------------------------------------------

public class JSON_Number : JSON_Value
{
    double FValue = 0;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Default Constructor. </summary>
    ///-------------------------------------------------------------------------------------------------
    public JSON_Number()
    {
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    ///
    /// <param name="value">    The value. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Number(double value)
    {
        FValue = value;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    ///
    /// <param name="value">    The value. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Number(int value)
    {
        FValue = value;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the value. </summary>
    ///
    /// <value> The value. </value>
    ///-------------------------------------------------------------------------------------------------

    public double Value
    {
        get { return FValue; }
        set { FValue = value; }
    }

    public bool asInt(ref int Value)
    {
        bool isvalid = true;
        try
        {
            int temp = Convert.ToInt32(FValue);
            Value = temp;
        }
        catch 
        {
            isvalid = false;
        }
        return isvalid;
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the serialize JSON string. </summary>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public override string Serialize()
    {
        return FValue.ToString();
    }
}

/// <summary>
/// JsonValue Class  used to hold C# structure for JSON value
/// Json value takes form of "NAME":"VALUE"
/// </summary>
public class JSON_NameValuePair
{
    protected bool FisError = false;
    protected string FErrMessage = "";

    protected string FName = "";
    protected JSON_Value FValue = null;

    //--------------------------------------
    /// <summary>
    /// Default Constructor (non parameters)
    /// </summary>
    public JSON_NameValuePair()
    {
        FName = "";
        FValue = null;
    }
    //--------------------------------------------
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="aName"></param>
    /// <param name="aValue"></param>
    public JSON_NameValuePair(string aName, JSON_Value aValue)
    {
        FName = aName;
        FValue = aValue;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    /// <param name="aName">  string for the Name  . </param>
    /// <param name="aValue"> a JSON value string </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_NameValuePair(string aName, string aValue)
    {
        FName = aName;
        FValue = JSONTool.ToJSON_Value(aValue);
        FisError = FValue.ParseError;
        if (FisError)
        { FErrMessage = FValue.ErrorMessage; }
    }

    /// <summary>
    /// Constructor using a JSON string
    /// </summary>
    /// <param name="JSON_String"></param>
    public JSON_NameValuePair(string JSON_String)
    {
        string errMessage = "";
        JSON_Value aValue = null;
        string aName = "";
        if (JSONTool.parseJSONValuePairSting(JSON_String, ref aName, ref aValue, ref errMessage))
        {
            FName = aName;
            FValue = aValue;
        }
        else
        {
            FisError = true;
            FErrMessage = errMessage;
        }
    }

    /// <summary>
    /// The Name
    /// </summary>
    public string Name
    {
        get { return FName; }
        set { FName = value; }
    }

    /// <summary>
    /// The Value
    /// </summary>
    public JSON_Value Value
    {
        get { return FValue; }
        set { FValue = value; }
    }
    
    public string Serialize()
    {
        string temp = JSON_Util.JSONQUOTE + Name + JSON_Util.JSONQUOTE + JSON_Util.JSONVALUESEPERATOR + Value.Serialize();
        return temp;
    }
    public string Serialize(bool UseCRLF)
    {
        string temp = JSON_Util.JSONQUOTE + Name + JSON_Util.JSONQUOTE + JSON_Util.JSONVALUESEPERATOR;
        if (Value.isJSON_Object)
        {
            temp += (Value as JSON_Object).Serialize(UseCRLF);
        }
        else
        {
            temp += Value.Serialize();
        }
        return temp;
    }


}

public static class JSON_Util
{
    public const char JSONOBJECTOPENTAG = '{';
    public const char JSONObjectCLOSETAG = '}';
    public const char JSONOBJECTPAIRSEPERATOR = ',';
    public const char JSONVALUESEPERATOR = ':';
    public const char JSONVALUEPAIRSEPERATOR = ',';
    public const char JSONQUOTE = '\"';
  

}
/// <summary>   JSON object. </summary>
/// <remarks> Class to contain a JSON object as a list of JSONValues</remarks>
public class JSON_Object :JSON_Value
{  
    //const char JSONOBJECTOPENTAG = '{';
    //const char JSONObjectCLOSETAG = '}';
    //const char JSONOBJECTPAIRSEPERATOR = ',';
    //const char JSONVALUESEPERATOR = ':';
    //const char JSONVALUEPAIRSEPERATOR = ',';

    List<JSON_NameValuePair> FValues = new List<JSON_NameValuePair>();

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Default constructor. </summary>
    ///
    /// <remarks>   Ray Quay, 1/8/2014. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Object()
    {

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    /// <remarks>  Creates a Json Object based on an array of JsonValues</remarks>
    /// <remarks>   Ray Quay, 1/8/2014. </remarks>
    ///
    /// <param name="Values">   The values. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Object(JSON_NameValuePair[] Values)
    {
        foreach(JSON_NameValuePair JSVP in Values) { FValues.Add(JSVP); }
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Constructor. </summary>
    /// <remarks>  creates a JSONObject based on a string (stringified) that describes a JSON</remarks>
    /// <remarks>   Ray Quay, 1/8/2014. </remarks>
    ///
    /// <param name="JSONObjectStr">    The json object string. </param>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Object(string JSONObjectStr)
    {
        // clean the string up
        string temp = JSONTool.RemoveNonJSON(JSONObjectStr).Trim();
        FOriginalStr = temp;
        //OK lets do some basic checks
        // JSON string must... 
        //    Have at least a form of "{A:B}" which is length of 5
        //    Have a Open and Close tag {}
        //    Have at least one ':'
        int JSONOpenTagPos = temp.IndexOf(JSON_Util.JSONOBJECTOPENTAG);
        int JSONCloseTagPos = temp.IndexOf(JSON_Util.JSONObjectCLOSETAG);
        int len = temp.Length;
        int JSONValueSepPos = temp.IndexOf(JSON_Util.JSONVALUESEPERATOR);

        if (!((JSONOpenTagPos>=0)&&(JSONCloseTagPos>3)&&(len>4)&&(JSONValueSepPos>1)))
        {
            // npot good to go  there is an error
            FisError = true;
            FErrMessage = "Malformed JSON string - "+FOriginalStr;
        }
        else
        {
            // OK good to go
            string errMessage = "";
            // strip off open and close tags  leave "A:B" or "A:B,C:D  and trim what is left"
            if (JSONTool.FindTagPair(temp, "{", "}", ref JSONOpenTagPos, ref JSONCloseTagPos, ref errMessage))
            {
                temp = temp.Substring(JSONOpenTagPos + 1, JSONCloseTagPos - (JSONOpenTagPos + 1)).Trim();
                // ok lopp through one or a series of value pairs and add to JSONValue list
                // get the seperator position (if more than one value pair)
                while ((temp.Length > 0) && (!FisError))
                {
                    // check if the name is a quited string, if not this si an error
                    if (temp[0] != '\"')
                    {
                        FisError = true;
                        FErrMessage = "Object name not a quoted string";
                    }
                    else
                    {
                        
                        // ok pop off a name
                        string NameStr = JSONTool.GetQuotedString(temp, '\"', ref FisError);
                        if (FisError)
                        {
                            FErrMessage = "Invalid String for Object Name";
                        }
                        else
                        {
                            // OK now remove name from temp, and trim;
                            temp = temp.Remove(0, NameStr.Length + 2).TrimStart();
                            // ok go to the value seperator and trim it off
                            int JSONVSepPos = temp.IndexOf(JSON_Util.JSONVALUESEPERATOR);
                            if (JSONVSepPos < 0)
                            {
                                FisError = true;
                                FErrMessage = "Value Seperator Missing";
                            }
                            else
                            {
                                temp = temp.Remove(0, JSONVSepPos + 1).TrimStart();
                                // OK process this value
                                string NextTemp = "";
                                JSON_Value JSV = JSONTool.ParseFirstValue(temp, ref FisError, ref FErrMessage, ref NextTemp);
                                // check if JSV is null
                                if (JSV == null)
                                {
                                    FisError = true;
                                    FErrMessage = "Invalid value in list";
                                }
                                else
                                {
                                    // OK create a value pair to add to this object
                                    JSON_NameValuePair JSNVP = new JSON_NameValuePair(NameStr, JSV);
                                    // ok make sure no error occurs but this is unlikely
                                    if (JSV.ParseError)
                                    {
                                        FisError = true;
                                        FErrMessage = JSV.ErrorMessage;
                                    }
                                    // add it
                                    FValues.Add(JSNVP);
                                    // ok now move beyond this value
                                    temp = NextTemp;

                                } //else JSV==null
                            }
                        } // Fiserror
                    } // else Temp[0]!= '/"'
                } // while
            } // FindPaired Tag

      
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the values Pair List </summary>
    ///
    /// <value> The values. </value>
    ///-------------------------------------------------------------------------------------------------

    public List<JSON_NameValuePair> Values
    {
        get { return FValues; }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds Json_Value to Json Object </summary>
    ///
    /// <param name="value">    The JSON_NameValuePair to add. </param>
    ///-------------------------------------------------------------------------------------------------

    public void Add(JSON_NameValuePair value)
    {
        FValues.Add(value);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds Json_Value to Json Object. </summary>
    ///
    /// <param name="Name">         The name. </param>
    /// <param name="StringValue">  The string value. </param>
    ///-------------------------------------------------------------------------------------------------

    public void Add(string Name, string StringValue)
    {
        JSON_NameValuePair JSVP = new JSON_NameValuePair(Name, StringValue);
        Add(JSVP);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds Json_Value to Json Object. </summary>
    ///
    /// <param name="Name">         The name. </param>
    /// <param name="AJSON_Value">  The ajson value. </param>
    ///-------------------------------------------------------------------------------------------------

    public void Add(string Name, JSON_Value AJSON_Value)
    {
        JSON_NameValuePair JSVP = new JSON_NameValuePair(Name, AJSON_Value);
        Add(JSVP);
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Indexer to get items within this collection using array index syntax. </summary>
    ///
    /// <param name="index">    Zero-based index of the. </param>
    ///
    /// <value> . </value>
    ///-------------------------------------------------------------------------------------------------

    public JSON_NameValuePair this[int index]
    {
        get
        {
            if ((index >= 0) && (index < FValues.Count))
            {
                return FValues[index];
            }
            else
                return null;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Searches for the first Json Value Pair whose name match the given string, case sensitive. </summary>
    ///
    /// <param name="Name"> The name. </param>
    ///
    /// <returns>  JSON_Value or null if not found . </returns>
    ///-------------------------------------------------------------------------------------------------

    public JSON_NameValuePair Find(string Name)
    {
        JSON_NameValuePair temp = null;
        
        foreach (JSON_NameValuePair JSVP in FValues)
        {
            if (JSVP.Name == Name)
                temp = JSVP;
        }
        return temp;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Searches for the first Json Value Pair whose name match the given string. Case Insensitive</summary>
    ///
    /// <param name="Name"> The name. </param>
    ///
    /// <returns>  JSON_Value or null if not found . </returns>
    ///-------------------------------------------------------------------------------------------------

    public JSON_NameValuePair Find_CaseInsensitive(string Name)
    {
        JSON_NameValuePair temp = null;
        string Source = Name.ToUpper();
        foreach (JSON_NameValuePair JSVP in FValues)
        {
            if (JSVP.Name.ToUpper() == Name)
                temp = JSVP;
        }
        return temp;
    }

    const char QT = '\"';
    const char CM = ',';

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the serialize JSON string. </summary>
    ///
    /// <remarks>   Ray Quay, 1/14/2014. </remarks>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public override string Serialize()
    {
        string temp = "{";
        int cnt = 0;
        foreach (JSON_NameValuePair JSVP in FValues)
        {
            if (cnt > 0) { temp += JSON_Util.JSONVALUEPAIRSEPERATOR; }
            cnt++;
            temp += JSVP.Serialize();// QT + JSVP.Name + QT + JSON_Util.JSONVALUESEPERATOR + JSVP.Value.Serialize();
        }

        temp += "}";
        return temp;
    }

    public string Serialize(bool useCRLF)
    {
        string temp = "{";
        if (useCRLF)
            temp = Environment.NewLine + temp + Environment.NewLine;
        int cnt = 0;
        foreach (JSON_NameValuePair JSVP in FValues)
        {
            if (cnt > 0) { temp += JSON_Util.JSONVALUEPAIRSEPERATOR; }
            cnt++;

            temp += JSVP.Serialize(useCRLF);
            //temp +=  QT + JSVP.Name + QT + JSON_Util.JSONVALUESEPERATOR;
//            if (JSVP.Value.isJSON_Object)
  //          {
    //            temp += (JSVP.Value as JSON_Object).Serialize(useCRLF);
      //      }
        //    else
          //  {
            //    temp += JSVP.Value.Serialize();
            //}
        }

        temp += "}";
        if (useCRLF)
        {
            temp = Environment.NewLine + temp +Environment.NewLine;
        }
        return temp;
    }

}

//==========================================

public class JSON_Array : JSON_Value
{
    List<JSON_Value> FJsonList = new List<JSON_Value>();

   ///-------------------------------------------------------------------------------------------------
   /// <summary>    Default constructor. </summary>
   ///
   ///-------------------------------------------------------------------------------------------------

   public JSON_Array()
   {
   }

   ///-------------------------------------------------------------------------------------------------
   /// <summary>    Constructor. </summary>
   ///
   /// <remarks>    Ray Quay, 1/21/2014. </remarks>
   ///
   /// <param name="values">    The json objects. </param>
   ///-------------------------------------------------------------------------------------------------

   public JSON_Array(JSON_Value[] values)
   {
       foreach(JSON_Value JSV in values)
           FJsonList.Add(JSV);
   }

   const char Delimeter = ',';

   ///-------------------------------------------------------------------------------------------------
   /// <summary>    Constructor. </summary>
   /// <param name="jsonStr">   The json string. </param>
   ///-------------------------------------------------------------------------------------------------

   public JSON_Array(string jsonStr)
   {
       string ParseStr = jsonStr.Trim();
       FOriginalStr = jsonStr;
       if (ParseStr.StartsWith("[") && ParseStr.EndsWith("]"))
       {
           // remove the array brackets
           ParseStr = ParseStr.Remove(ParseStr.Length - 1).Remove(0, 1);
           FJsonList = JSONTool.ParseJSONValueList(ParseStr, ref FisError, ref FErrMessage);
       }
       else
       {
           FisError = true;
           FErrMessage = "Malformed JSON Array string - " + jsonStr;
       }
   }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Indexer to get or set items within this collection using array index syntax. </summary>
    ///<remarks> If invalid index, get returns null and set is ignored</remarks>
    /// <param name="index">    Zero-based index of the. </param>
    ///
    /// <value> JsonObject </value>
    ///-------------------------------------------------------------------------------------------------

    public JSON_Value this[int index]
    {
        get
        {
            if ((index>=0)&&(index<FJsonList.Count))  // wont provide access if list is empty
            {
                return FJsonList[index];
            }
            else
                return null;
        }
        set
        {
            if ((index >= 0) && (index < FJsonList.Count))  // wont provide access if list is empty
            {
                FJsonList[index] = value;
            }
            // else, could add code to make this work like javascript, and create entries to match index
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the json objects. </summary>
    ///
    /// <value> The json objects. </value>
    ///-------------------------------------------------------------------------------------------------

    public List<JSON_Value> Values
    {
        get { return FJsonList; }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the length of List of JsonObjects </summary>
    ///
    /// <value> The length. </value>
    ///-------------------------------------------------------------------------------------------------

    public int Length
    {
        get {return FJsonList.Count; }
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds JsonObject.. </summary>
    ///
    /// <remarks>   Ray Quay, 1/8/2014. </remarks>
    ///
    /// <param name="aJsonObject">  The JSONObject to add. </param>
    ///-------------------------------------------------------------------------------------------------

    public void Add(JSON_Value value)
    {
        FJsonList.Add(value);
    }

    public override string Serialize()
    {
        string temp = "[";
        int cnt = 0;
        foreach (JSON_Value JSV in FJsonList)
        {
            if (cnt > 0)
                temp += ",";
            cnt++;
            temp += JSV.Serialize();
        }
        temp += "]";
        return temp;
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Json tools </summary>
///
/// <remarks>   Ray Quay, 1/8/2014. </remarks>
///-------------------------------------------------------------------------------------------------

public static class JSONTool
{
    public static string parseQuotedString(string QuotedString)
    {
        string temp = "";
        char Qchar = '"';
        string Qstr = QuotedString.TrimStart(' ');
        Qstr = Qstr.TrimEnd(' ');
        int qpos = Qstr.IndexOf(Qchar);
        if (qpos < 0)
        {
            Qchar = '\'';
            qpos = Qstr.IndexOf(Qchar);
        }
        if (qpos < 0)
        {
            temp = Qstr;
        }
        else
        {
            temp = Qstr.Substring(qpos + 1, Qstr.Length - (qpos + 1));
            qpos = temp.IndexOf(Qchar);
            if (qpos >= 0)
            {
                temp = temp.Substring(0, qpos);
            }
        }
        return temp;
    }

    //-------------------------------------------------------------------------
    const char JSONVALUEDIVIDER = ':';

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Parse json value pair sting. </summary>
    ///
    /// <param name="theJSON">      the json. </param>
    /// <param name="aName">        [in,out] The name. </param>
    /// <param name="aValue">       [in,out] The value. </param>
    /// <param name="errMessage">   [in,out] Message describing the error. </param>
    ///
    /// <returns>   true if it succeeds, false if it fails. </returns>
    ///-------------------------------------------------------------------------------------------------

    public static bool parseJSONValuePairSting(string theJSON, ref string aName, ref JSON_Value aValue, ref string errMessage)
    {
        bool parseResult = false;
        // can not be null string
        if ((theJSON.Length > 0))// && (theJSON[0] == '{') && (theJSON[theJSON.Length - 1] == '}'))
        {
            int ColonPos = theJSON.IndexOf(JSONVALUEDIVIDER);
            // must have a colon with something before and after
            if ((ColonPos > 0) && (ColonPos < (theJSON.Length - 1)))
            {
                // ok grab to parts (either side of colon)
                string part1 = theJSON.Substring(0, ColonPos);
                string part2 = theJSON.Substring(ColonPos + 1, (theJSON.Length - ColonPos) - 1);
                // OK, grabquoted string   
                aName = JSONTool.parseQuotedString(part1);
                string aValueString = JSONTool.parseQuotedString(part2);
                aValue = JSONTool.ToJSON_Value(aValueString);
                parseResult = true;
                errMessage = "";
            }
        }
        else
        {
            errMessage = "Malformed JSON string";
        }
        return parseResult;
    }

    static char[] NONJSONCHARS = { '\a', '\b', '\f', '\n', '\r', '\t', '\v'};

    internal static bool isNonJSONChar(char target)
    {
        foreach(char c in NONJSONCHARS)
        {
            if (c==target)
            {
                return true;
            }
        }
        return false;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Removes the non json chars from the json string value. </summary>
    ///
    ///<remarks> CR and Newlines etc.  This needs some work, extract this from quoted strings, which should be allowed</remarks>
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public static string RemoveNonJSON(string value)
    {
        string temp = "";
        foreach(char c in value)
        {
            if (!isNonJSONChar(c))
            {
                temp += c;
            }
        }
        return temp;
    }

    public static string ConversionErrorMessage = "";
    public const double InvalidNumber = double.NaN;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Json value to double. </summary>
    ///
    /// <param name="value">    The value. </param>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public static double JSONValueToDouble(string value)
    {
        double thevalue = InvalidNumber;
        try
        {
            thevalue = Convert.ToDouble(value);
        }
        catch (Exception ex)
        {
            ConversionErrorMessage = ex.Message;
        }
        return thevalue;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Json value to bool. </summary>
    ///
    /// <param name="value">    The value. </param>
    ///
    /// <returns>   true if it succeeds, false if it fails. </returns>
    ///-------------------------------------------------------------------------------------------------

    public static bool JSONValueToBool(string value)
    {
        bool thevalue = false;
        try
        {
            Convert.ToBoolean(value);
        }
        catch (Exception ex)
        {
            ConversionErrorMessage = ex.Message;
        }
        return thevalue;
    }

    //--------------------------------------------------------------------

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Converts a Json string to C# json value. </summary>
    ///
    /// <remarks>   Ray Quay, 1/14/2014. </remarks>
    ///
    /// <param name="value">    The value. </param>
    ///
    /// <returns>   value as a JSON_Value. </returns>
    ///-------------------------------------------------------------------------------------------------

    public static JSON_Value ToJSON_Value(string value)
    {
        JSON_Value temp = null;
        string jsonStr = value.Trim();
        string test = jsonStr.ToUpper();
        if (jsonStr.Length == 0)
        {
            temp = new JSON_String(value);
        }
        else
            if (test.StartsWith("[")&&test.EndsWith("]"))
            {

                temp = new JSON_Array(jsonStr);
            }
            else
                if ((jsonStr[0] == '{') && (jsonStr[value.Length - 1] == '}'))
                        {
                            // this is an object
                            temp = new JSON_Object(jsonStr);
                        }
                else
                if ((test == "FALSE") || (test == "TRUE"))
                {
                    // this is a boolean
                    temp = new JSON_Bool(JSONTool.JSONValueToBool(test));
                }
                else
                {
                    // test of number
                    double dtest = JSONTool.JSONValueToDouble(jsonStr);
                    if (!double.IsNaN(dtest))
                    {
                        // is a number
                        temp = new JSON_Number(dtest);
                    }
                        else
                        if ((jsonStr[0] == '[') && (jsonStr[value.Length - 1] == ']'))
                        {
                            // this is an array
                            temp = new JSON_Array(jsonStr);
                        }
                        else
                        {
                            // treat like a string
                            string tempstr = parseQuotedString(jsonStr);
                            temp = new JSON_String(tempstr);
                        }
                }
        return temp;
    }

    public static string ToString(JSON_Value value)
    {
        string temp = "";

        return temp;
    }
    public static double ToDouble(JSON_Value value)
    {
        double temp = 0;

        return temp;
    }
    public static bool ToBool(JSON_Value value)
    {
        bool temp = false;

        return temp;
    }
    //public abstract bool Value();
    //public abstract JSON_Value[] Value();
    //public abstract JSON_Object Value();

    public static bool FindTagPair(string Source, string Open, string Close, ref int Openindex, ref int Closeindex, ref string ErrMessage)
    {
        bool found = false;
        bool err = false;
        int oindex = -1;
        int cindex = -1;

        int OpenCnt = 0;
        int CloseCnt = 0;
        int SourceLen = Source.Length;
        int index = 0;
        int OpenTagDelLength = Open.Length-1;
        int CloseTagDelLength = Close.Length -1;
        string temp = Source;
        while ((!err) && (!found)&&(index<SourceLen))
        {
            // find first open tag
            int OpenPos = temp.IndexOf(Open,index);
            int ClosePos = temp.IndexOf(Close,index);
            if ((OpenPos >= 0) && (OpenPos < ClosePos))
            {
                // if first Open then set open index
                if (OpenCnt < 1) oindex = OpenPos;
                // count open
                OpenCnt++;
                // reset index
                index = OpenPos + 1;
            }
            else
            {
                // count close
                CloseCnt++;
                // save this position, the last one will beit
                cindex = ClosePos;
                // reset index
                index = ClosePos + 1;
            }
            // ok test states
            // if Opencnt > 1 and opencnt=closecnt then we have found it.
            // if CLosecnt > Open cnt, then we have an err
            // if OpenPos and CLosePso <0 && not found, then err
            if ((CloseCnt == OpenCnt) && (OpenCnt > 0))
            {
                found = true;
            }
            else
            {
                if (CloseCnt > OpenCnt)
                {
                    err = true;
                }
                else
                    if ((ClosePos<0)&&(OpenPos<0))
                    {
                        err = true;
                    }
            }
            
        } // while
        if (found)
        {
            Openindex = oindex;
            Closeindex = cindex;
        }
        else
        {
            Openindex = -1;
            Closeindex = -1;
        }
        return found;
    }
    public static List<JSON_Value> ParseJSONValueList(string ValueList, ref bool IsError, ref string ErrMessage)
    {
        string temp = ValueList;
        bool iserror = false;
        string erMessage = "";
        List<JSON_Value> TheList = new List<JSON_Value>();

        while ((temp!="")&&(!iserror))
        {
            string NextTemp = "";
            JSON_Value JSV = ParseFirstValue(temp, ref iserror, ref erMessage, ref NextTemp);
            temp = NextTemp;
            TheList.Add(JSV);
        }
        IsError = iserror;
        ErrMessage = erMessage;
        return TheList;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Parse first value. </summary>
    ///
    /// <remarks>   Ray Quay, 1/14/2014. </remarks>
    ///
    /// <param name="Source">       Source for the. </param>
    /// <param name="isError">      [in,out] The is error. </param>
    /// <param name="ErrMessage">   [in,out] Message describing the error. </param>
    /// <param name="RestofValues"> [in,out] The restof values. </param>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

    public static JSON_Value ParseFirstValue(string Source, ref bool isError, ref string ErrMessage, ref string RestofValues)
    {
        string temp = Source.TrimStart();
        char valuechar1 = temp[0];
        isError = false;
        ErrMessage = "";
        // OK process based on this char which identifies vakue type
        JSON_Value JSV = null;
        switch (valuechar1)
        {
            case '[':  // its an array value
                {
                    // OK, let;s find end of array
                    int start = 0;
                    int end = 0;
                    if (JSONTool.FindTagPair(temp, "[", "]", ref start, ref end, ref ErrMessage))
                    {
                        string arraystr = temp.Substring(start, (end - start) + 1);
                        JSON_Array JSA = new JSON_Array(arraystr);
                        JSV = JSA;
                        temp = temp.Remove(0, end + 1).TrimStart();
                    }
                    else
                    {
                        isError = true;
                    }
                    break;
                }
            case '{': // its an object value
                {
                    // OK, let;s find end of array
                    int start = 0;
                    int end = 0;
                    if (JSONTool.FindTagPair(temp, "{", "}", ref start, ref end, ref ErrMessage))
                    {
                        string objectstr = temp.Substring(start, (end - start) + 1);
                        JSON_Object JSO = new JSON_Object(objectstr);
                        JSV = JSO;
                        temp = temp.Remove(0, end + 1).TrimStart();
                    }
                    else
                    {
                        isError = true;
                    }
                    break;
                }
            default:
                {
                    int JSONVPSep = temp.IndexOf(',');
                    string valuestr = "";
                    if (JSONVPSep < 0)
                    {
                        valuestr = temp;
                        temp = "";
                    }
                    else
                    {
                        valuestr = temp.Substring(0, JSONVPSep);
                        temp = temp.Remove(0, JSONVPSep).TrimStart();
                    }
                    JSV = JSONTool.ToJSON_Value(valuestr);
                    break;
                }
        }
        if (temp.IndexOf(',') == 0)
        {
            temp = temp.Remove(0, 1).TrimStart();
        }
        RestofValues = temp;
        return JSV;

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a quoted string. </summary>
    ///
    /// <remarks>   Ray Quay, 1/14/2014. </remarks>
    ///
    /// <param name="source">       Source for the. </param>
    /// <param name="theQuotechar"> the quotechar. </param>
    /// <param name="isError">      The is error. </param>
    ///
    /// <returns>   The quoted string. </returns>
    ///-------------------------------------------------------------------------------------------------

    public static string GetQuotedString(string source, char theQuotechar, ref bool isError)
    {
        string temp = "";
        isError = true;        
        int Qpos = source.IndexOf(theQuotechar);
        if (Qpos >= 0)
        {
            temp = source.Substring(Qpos + 1, source.Length - (Qpos + 1));
            Qpos = temp.IndexOf(theQuotechar);
            if (Qpos >= 0)
            {
                temp = temp.Substring(0, Qpos);
                isError = false;
            }
        }
        return temp;
    }
}
}





