using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System.Text;
using DataLayer;

namespace DataLayer
{
    public static class JsonMaker
    {
       
        public static string GetJSONFromList<T>(List<T> ListParam, string Prefix = "")
        {
            try
            {
                return Prefix == "" ? "" : "\"" + Prefix + "\": " + JsonConvert.SerializeObject(ListParam);
            }
            catch (Exception ex)
            {
                return Logger.LogException(ex);
            }
        }
        public static string GetJSON(dynamic obj, Boolean SkipTableName = false, string Prefix = "")
        {
            try
            {


                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                JsonSerializerSettings Jsettings = new JsonSerializerSettings();
                if (!SkipTableName)
                {
                    if (Prefix == "") Prefix = obj.GetTableName(); else Prefix += obj.GetTableName();
                    json = "\"" + Prefix + "\": " + json;
                }
                return json;
            }
            catch (Exception ex)
            {
                return Logger.LogException(ex);
            }
        }

        public static object GetJsonExtract(string Input, string Expression)
        {
            object value;
            try
            {
                string JsonString = Input.Replace("'", @"\'").Replace("\"", "'");  //.Replace(".", "@");
                if (Expression.Contains("@"))
                {
                    string[] SplitArray = Expression.Split('@');
                    Expression += "." + SplitArray[SplitArray.Length - 1];
                }


                JObject JsonObject = JObject.Parse(@JsonString);
                IEnumerable<JToken> Selection = (JToken)null;
                if (!Expression.Contains("*"))
                    Selection = JsonObject.SelectToken(Expression);
                else
                    Selection = JsonObject.SelectTokens(Expression);
                value = Selection == null ? null : Selection;
                if (!(value + "").Contains("{") && !(value + "").Contains("["))
                {
                    string[] TempArray = Expression.Split('.');
                }
            }

            catch (Exception ex) { Logger.LogException(ex); return null; }
            return value;
           

        }
        public static bool UpdateJsonValue(string Path, object Value, ref JToken JsonOut)
        {

            object JsonData = JsonOut; //Pcr.OutgoingJson;
            try
            {
                string OrigPath = Path;
                string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                if (Path.Contains("@"))
                {
                    string[] SplitArray = Path.Split('@');
                    Path += "." + SplitArray[SplitArray.Length - 1];
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                }
                else if (!Path.Contains("['"))
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                else
                    Path = "$~" + Path.Replace("$.", "").Replace("].[", "]~[");
                if (Path.Contains(" '"))
                    Path = Path.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                //  JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                JToken JsonToken = JObject.Parse(@JsonString);
                JToken ModifyToken = JsonToken;
                string RemainingPath = Path;
                if (Value.ToString() == "DELETE!")
                {
                    if (ModifyToken.SelectToken(OrigPath) != null)
                        JsonToken.SelectToken(OrigPath).Parent.Remove();
                }
                else
                {
                    foreach (string item in Path.Split('~'))
                    {
                        // string work_item = item.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                        JToken Selection = (JToken)ModifyToken.SelectToken(item.Replace(" ", "_"));
                        if (Selection != null)
                            ModifyToken = Selection;
                        else if (item != "$")
                        {
                            string last_loop_item = "";
                            JToken PrevJsonToken = ModifyToken;
                            string[] RemainingPathArray = RemainingPath.Split('~');
                            foreach (string loopitem in RemainingPathArray)
                            {
                                dynamic JsonObject;
                                if (Value.ToString() != "[]" || loopitem != RemainingPathArray[RemainingPathArray.Length - 1])
                                    JsonObject = new JObject();
                                else
                                    JsonObject = new JArray();
                                ModifyToken[loopitem.Replace("['", "").Replace("']", "")] = JsonObject;
                                PrevJsonToken = ModifyToken;
                                ModifyToken = JsonObject;
                                last_loop_item = loopitem;
                            }
                            //if (last_loop_item != "") PrevJsonToken[last_loop_item] = Value;
                            //JsonData = PrevJsonToken.Root;
                            if (Value.ToString() == "[]" && ModifyToken.ToString().Contains("[")) break;

                            //JsonObject.AddAfterSelf(@"{" + RemainingPath.Replace(".", "}:{") + "="+ Value+ "}");
                        }
                        RemainingPath = string.Join("~", RemainingPath.Split('~').Skip(1).ToArray());
                    }
                    Path = Path.Replace("~", ".");
                    if (Value.ToString() != "[]") JsonToken.SelectToken(Path).Replace(JToken.Parse(JsonConvert.SerializeObject(Value, Formatting.Indented)));

                }

                JsonData = (JObject)JsonToken;
                JsonOut = (JToken)JsonData; ;
                //StringBuilder message = new StringBuilder();
                //message.Append("UpdateJsonValue: " + System.Environment.NewLine);
                //message.Append("Path: " + Path + System.Environment.NewLine);
                //message.Append("Value: " + Value + System.Environment.NewLine);
                ////Logger.LogJsonUpdates(message.ToString());
                //message = new StringBuilder();
                //if (Value.ToString() != "[]")
                //{
                //    message.Append("Path: " + Path + System.Environment.NewLine);
                //    message.Append("Value: " + Value + System.Environment.NewLine);
                //    Logger.LogJsonUpdates(message.ToString(), "JsonFields");
                //}
                return true;
                //JsonObject[Path] = Value;


            }
            catch (Exception ex) { Logger.LogException(ex); return false; }

        }
        public static bool UpdateJsonValue(string Path, string Value, ref JToken JsonOut)
        {

            object JsonData = JsonOut; //Pcr.OutgoingJson;
            try
            {
                string OrigPath = Path;
                string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                if (Path.Contains("@"))
                {
                    string[] SplitArray = Path.Split('@');
                    Path += "." + SplitArray[SplitArray.Length - 1];
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                }
                else if (!Path.Contains("['"))
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                else
                    Path = "$~" + Path.Replace("$.", "").Replace("].[", "]~[");
                if (Path.Contains(" '"))
                    Path = Path.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                //  JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                JToken JsonToken = JObject.Parse(@JsonString);
                JToken ModifyToken = JsonToken;
                string RemainingPath = Path;
                if (Value.ToString() == "DELETE!")
                {
                    if (ModifyToken.SelectToken(OrigPath) != null)
                        JsonToken.SelectToken(OrigPath).Parent.Remove();
                }
                else
                {
                    foreach (string item in Path.Split('~'))
                    {
                        // string work_item = item.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                        JToken Selection = (JToken)ModifyToken.SelectToken(item.Replace(" ", "_"));
                        if (Selection != null)
                            ModifyToken = Selection;
                        else if (item != "$")
                        {
                            string last_loop_item = "";
                            JToken PrevJsonToken = ModifyToken;
                            string[] RemainingPathArray = RemainingPath.Split('~');
                            foreach (string loopitem in RemainingPathArray)
                            {
                                dynamic JsonObject;
                                if (Value != "[]" || loopitem != RemainingPathArray[RemainingPathArray.Length - 1])
                                    JsonObject = new JObject();
                                else
                                    JsonObject = new JArray();
                                ModifyToken[loopitem.Replace("['", "").Replace("']", "")] = JsonObject;
                                PrevJsonToken = ModifyToken;
                                ModifyToken = JsonObject;
                                last_loop_item = loopitem;
                            }
                            //if (last_loop_item != "") PrevJsonToken[last_loop_item] = Value;
                            //JsonData = PrevJsonToken.Root;
                            if (Value == "[]" && ModifyToken.ToString().Contains("[")) break;

                            //JsonObject.AddAfterSelf(@"{" + RemainingPath.Replace(".", "}:{") + "="+ Value+ "}");
                        }
                        RemainingPath = string.Join("~", RemainingPath.Split('~').Skip(1).ToArray());
                    }
                    Path = Path.Replace("~", ".");
                    if (Value != "[]") JsonToken.SelectToken(Path).Replace(Value + "");
                }

                JsonData = (JObject)JsonToken;
                JsonOut = (JToken)JsonData; ;
                //StringBuilder message = new StringBuilder();
                //message.Append("UpdateJsonValue: " + System.Environment.NewLine);
                //message.Append("Path: " + Path + System.Environment.NewLine);
                //message.Append("Value: " + Value + System.Environment.NewLine);
                //Logger.LogJsonUpdates(message.ToString());
                //message = new StringBuilder();
                //if (Value != "[]")
                //{
                //    message.Append("Path: " + Path + System.Environment.NewLine);
                //    message.Append("Value: " + Value + System.Environment.NewLine);
                //    Logger.LogJsonUpdates(message.ToString(), "JsonFields");
                //}
                return true;
                //JsonObject[Path] = Value;


            }
            catch (Exception ex) { Logger.LogException(ex); return false; }

        }
        public static bool UpdateJsonValue(string Path, string Value, ref string JsonOut)
        {

            object JsonData = JsonOut; //Pcr.OutgoingJson;
            try
            {
                string OrigPath = Path;
                string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                if (Path.Contains("@"))
                {
                    string[] SplitArray = Path.Split('@');
                    Path += "." + SplitArray[SplitArray.Length - 1];
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                }
                else if (!Path.Contains("['"))
                    Path = "$~" + "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                else
                    Path = "$~" + Path.Replace("$.", "").Replace("].[", "]~[");
                if (Path.Contains(" '"))
                    Path = Path.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                //  JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                JToken JsonToken = JObject.Parse(@JsonString);
                JToken ModifyToken = JsonToken;
                string RemainingPath = Path;
                if (Value != "DELETE!")
                {
                    foreach (string item in Path.Split('~'))
                    {
                        // string work_item = item.Replace("['", "[[").Replace("']", "]]").Replace("'", "").Replace("[[", "['").Replace("]]", "']");
                        JToken Selection = (JToken)ModifyToken.SelectToken(item.Replace(" ", "_"));
                        if (Selection != null)
                            ModifyToken = Selection;
                        else if (item != "$")
                        {
                            string last_loop_item = "";
                            JToken PrevJsonToken = ModifyToken;
                            string[] RemainingPathArray = RemainingPath.Split('~');
                            foreach (string loopitem in RemainingPathArray)
                            {
                                dynamic JsonObject;
                                if (Value != "[]" || loopitem != RemainingPathArray[RemainingPathArray.Length - 1])
                                    JsonObject = new JObject();
                                else
                                    JsonObject = new JArray();
                                ModifyToken[loopitem.Replace("['", "").Replace("']", "")] = JsonObject;
                                PrevJsonToken = ModifyToken;
                                ModifyToken = JsonObject;
                                last_loop_item = loopitem;
                            }
                            //if (last_loop_item != "") PrevJsonToken[last_loop_item] = Value;
                            //JsonData = PrevJsonToken.Root;
                            if (Value == "[]" && ModifyToken.ToString().Contains("[")) break;

                            //JsonObject.AddAfterSelf(@"{" + RemainingPath.Replace(".", "}:{") + "="+ Value+ "}");
                        }
                        RemainingPath = string.Join("~", RemainingPath.Split('~').Skip(1).ToArray());
                    }
                    Path = Path.Replace("~", ".");
                    if (Value != "[]") JsonToken.SelectToken(Path).Replace(Value + "");
                }
                else if (ModifyToken.SelectToken(OrigPath) != null)
                    JsonToken.SelectToken(OrigPath).Parent.Remove();
                JsonData = (JObject)JsonToken;
                JsonOut = JsonData.ToString(); ;
                //StringBuilder message = new StringBuilder();
                //message.Append("UpdateJsonValue: " + System.Environment.NewLine);
                //message.Append("Path: " + Path + System.Environment.NewLine);
                //message.Append("Value: " + Value + System.Environment.NewLine);
                //Logger.LogJsonUpdates(message.ToString());
                //message = new StringBuilder();
                //if (Value != "[]")
                //{
                //    message.Append("Path: " + Path + System.Environment.NewLine);
                //    message.Append("Value: " + Value + System.Environment.NewLine);
                //    Logger.LogJsonUpdates(message.ToString(), "JsonFields");
                //}
                return true;
                //JsonObject[Path] = Value;


            }
            catch (Exception ex) { Logger.LogException(ex); return false; }

        }
        public static bool ModifyArrayItem(ref JToken JsonOut, string Path, string SearchField, string SearchValue, String ModifyField, object ModifyValue, bool MustExist = false)
        {
            try
            {
                object JsonData = JsonOut; // HttpContext.Current.Session["json_out"];
                string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                JToken JsonToken = JObject.Parse(@JsonString);
                JArray jarray = (JArray)JsonToken.SelectToken(Path);

                JToken SearchToken = null;
                if (jarray == null && MustExist) return false;
                if (jarray == null)
                {
                    //JsonMaker.UpdateJsonValue(Path.Replace("buttons", "shmuttons"), "[]");
                    JsonMaker.UpdateJsonValue(Path, "[]", ref JsonToken);
                    JsonData = JsonToken; // HttpContext.Current.Session["json_out"]; ;
                    JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'");
                    JsonToken = JObject.Parse(@JsonString);
                    //jarray = (JArray)JsonToken.SelectToken(Path.Replace("buttons", "shmuttons"));
                    jarray = (JArray)JsonToken.SelectToken(Path);
                }
                //return false;
                else
                {
                    SearchToken = jarray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString() == SearchValue);
                }
                if (SearchToken == null && MustExist) return false;

                if (SearchToken == null)
                {

                    JObject ArrayItem = new JObject();
                    JToken tokenToAdd = JToken.Parse(JsonConvert.SerializeObject(SearchValue, Formatting.Indented));
                    ArrayItem[SearchField] = tokenToAdd;
                    tokenToAdd = JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented));
                    ArrayItem[ModifyField] = tokenToAdd;

                    jarray.Add(ArrayItem);


                }
                else
                {
                    // SearchToken.SelectToken(ModifyField).Replace(ModifyValue);
                    SearchToken[ModifyField] = JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented));
                }

                JsonOut = JsonToken;

                //StringBuilder message = new StringBuilder();

                return true;
            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }


        public static string GetJsonExtract(string Expression, object JsonData, bool LogExtracts = false)
        {

            string value;
            try
            {
                string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                if (Expression.Contains("@"))
                {
                    string[] SplitArray = Expression.Split('@');
                    Expression += "." + SplitArray[SplitArray.Length - 1];
                    Expression = ("$~" + "['" + Expression.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']").Replace("~", ".");
                }


                JObject JsonObject = JObject.Parse(@JsonString);
                JToken Selection = JsonObject.SelectToken(Expression);
                value = Selection == null || Selection.Type == JTokenType.Null ? null : Selection.ToString();

                if (!(value + "").Contains("{") && !(value + "").Contains("["))
                {
                    string[] TempArray;
                    if (Expression.Contains("['"))
                    {
                        Expression = Expression.Replace("'].['", "~");
                        Expression = Expression.Replace("['", "").Replace("']", "");
                        TempArray = Expression.Split('~');
                    }
                    else
                        TempArray = Expression.Split('.');
                    //Logger.LogJsonUpdates(TempArray[TempArray.Length - 1], "JsonExtracts");
                }

            }
            catch (Exception ex) { Logger.LogException(ex); return null; }
            return value;

        }



    }
}