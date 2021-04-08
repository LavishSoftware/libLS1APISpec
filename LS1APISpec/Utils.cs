using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace libLS1APISpec
{
    public static class Utils
    {

        public static string GetValueString(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null)
                return null;
            return token.ToString();
        }

        public static DateTime GetValueDateTime(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return DateTime.MinValue;

            DateTime result;
            if (!DateTime.TryParse(token.ToString(), out result))
                return DateTime.MinValue;
            return result;
        }

        public static bool GetValueBool(this JObject jo, string valueName, bool defaultValue = false)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null)
                return defaultValue;
            if (token.ToString().ToLowerInvariant() == "true")
                return true;
            return false;
        }

        public static bool? GetValueNBool(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null)
                return null;
            if (token.Type == JTokenType.Boolean)
            {
                if (token.ToString().ToLowerInvariant() == "true")
                    return true;
                return false;
            }
            return null;
        }

        public static float GetValueFloat(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null)
                return 0;
            return token.ToObject<float>();
        }

        public static JArray ToJArray(this ObservableCollection<JObject> list)
        {
            JArray ja = new JArray();
            foreach (JObject jo in list)
            {
                ja.Add(jo);
            }
            return ja;
        }


        public static T GetValue<T>(this JObject jo, string valueName, T defaultValue = default(T))
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<T>();
        }

        public static int GetValueInt(this JObject jo, string valueName, int defaultValue = 0)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<int>();
        }

        public static int? TryGetValueInt(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return null;
            return token.ToObject<int>();
        }

        public static uint GetValueUInt(this JObject jo, string valueName, uint defaultValue = 0)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<uint>();
        }

        public static long GetValueLong(this JObject jo, string valueName, long defaultValue = 0)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<long>();
        }

        public static ulong GetValueULong(this JObject jo, string valueName, ulong defaultValue = 0)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<ulong>();
        }


        public static JObject JObjectFromFile(string filename)
        {
            JObject jo = null;
            try
            {
                string text = System.IO.File.ReadAllText(filename);
                jo = JObject.Parse(text);
            }
            catch
            {

            }
            return jo;
        }


        public static T GetValue<T>(this KeyValuePair<string, JToken> kvp, T defaultValue = default(T))
        {
            JToken token = kvp.Value;
            if (token == null || token.Type == JTokenType.Null)
                return defaultValue;
            return token.ToObject<T>();
        }

        public static string GetValueString(this KeyValuePair<string, JToken> kvp)
        {
            JToken token = kvp.Value;
            if (token == null || token.Type == JTokenType.Null)
                return null;

            return token.ToString();
        }

        public static JArray GetValueJArray(this KeyValuePair<string, JToken> kvp)
        {
            JToken token = kvp.Value;
            if (token == null || token.Type == JTokenType.Null)
                return null;

            if (token.Type == JTokenType.Array)
                return (JArray)token;
            return null;
        }
        public static IEnumerable<JToken> GetValueJArrayTokens(this KeyValuePair<string, JToken> kvp)
        {
            JToken token = kvp.Value;
            if (token == null || token.Type == JTokenType.Null)
                return null;

            if (token.Type != JTokenType.Array)
                return null;

            JArray jaToken = (JArray)token;

            return jaToken;
        }

        public static JObject GetValueJObject(this KeyValuePair<string, JToken> kvp)
        {
            JToken token = kvp.Value;
            if (token == null || token.Type == JTokenType.Null)
                return null;

            if (token.Type == JTokenType.Object)
                return (JObject)token;
            return null;
        }


        public static JArray GetValueJArray(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);
            if (token == null || token.Type == JTokenType.Null)
                return null;
            try
            {
                if (token.Type == JTokenType.Array)
                    return (JArray)token;
                return null;
            }
            catch
            {

            }
            return null;
        }

        public static JObject GetValueJObject(this JObject jo, string valueName)
        {
            JToken token = jo.GetValue(valueName, StringComparison.InvariantCultureIgnoreCase);

            if (token == null || token.Type == JTokenType.Null)
                return null;
            try
            {
                if (token.Type == JTokenType.Object)
                    return (JObject)token;
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse value=" + token.ToString(), ex);
            }
            return null;
        }

        public static JObject GetAt(this JArray ja, int index)
        {
            JToken token = ja[index];
            if (token == null || token.Type == JTokenType.Null)
                return null;
            try
            {
                if (token.Type == JTokenType.Object)
                    return (JObject)token;
                return null;
            }
            catch
            {

            }
            return null;
        }

        public static string GetStringAt(this JArray ja, int index)
        {
            JToken token = ja[index];
            if (token == null || token.Type == JTokenType.Null)
                return null;

            return token.ToString();
        }


        public static void WriteAllTextSafe(string filename, string text)
        {
            string tempPath = System.IO.Path.GetTempPath();
            string tempFile = System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());
            string backupFile = System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());

            System.IO.File.WriteAllText(tempFile, text);

            bool fileExists = System.IO.File.Exists(filename);
            if (fileExists)
            {
                try
                {
                    System.IO.File.Move(filename, backupFile);
                }
                catch
                {

                }
            }

            try
            {
                System.IO.File.Copy(tempFile, filename, true);

                System.IO.File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                if (fileExists)
                {
                    try
                    {
                        System.IO.File.Move(backupFile, filename);
                    }
                    catch
                    {

                    }
                }
                throw;
            }
        }

        public static void WriteAllTextSafe(string filename, string text, Encoding encoding)
        {
            string tempPath = System.IO.Path.GetTempPath();
            string tempFile = System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());
            string backupFile = System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());

            System.IO.File.WriteAllText(tempFile, text, encoding);

            bool fileExists = System.IO.File.Exists(filename);
            if (fileExists)
            {
                try
                {
                    System.IO.File.Move(filename, backupFile);
                }
                catch
                {

                }
            }

            try
            {
                System.IO.File.Copy(tempFile, filename, true);

                System.IO.File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                if (fileExists)
                {
                    try
                    {
                        System.IO.File.Move(backupFile, filename);
                    }
                    catch
                    {

                    }
                }
                throw;
            }
        }

        public static bool StringEquals(string a, string b, StringComparison comparison, bool null_equals_empty)
        {
            if (null_equals_empty)
            {
                if (a == null)
                    a = string.Empty;
                if (b == null)
                    b = string.Empty;
            }
            return string.Equals(a, b, comparison);
        }

        public static bool ContainsIgnoreCase(this string paragraph, string word)
        {
            if (paragraph == null)
                return false;
            return Contains(paragraph, word, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
        }

        public static bool Contains(this string paragraph, string word, CultureInfo culture, CompareOptions options)
        {
            if (paragraph == null)
                return false;
            return culture.CompareInfo.IndexOf(paragraph, word, options) >= 0;
        }

        public static void FromJObject<T>(this ObservableCollection<T> list, JObject ja) where T : LS1APISpecObject, new()
        {
            list.Clear();
            if (ja == null)
                return;

            foreach(JProperty jp in ja.Properties())
            {
                T val = LS1APISpecObject.FromJObject<T>(ja, jp.Name);
                val.Name = jp.Name;

                list.Add(val);
            }
        }
        public static void FromJObject<T>(this ObservableCollection<T> list, JObject parentObject, string name) where T : LS1APISpecObject, new()
        {
            JObject ja = parentObject.GetValueJObject(name);
            FromJObject<T>(list, ja);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this JObject ja) where T : LS1APISpecObject, new()
        {
            if (ja == null)
                return null;
            ObservableCollection<T> list = new ObservableCollection<T>();
            list.FromJObject(ja);
            return list;
        }



        public static void FromJArray<T>(this ObservableCollection<T> list, JArray ja) where T : LS1APISpecObject, new()
        {
            list.Clear();
            if (ja == null)
                return;
            for (int i = 0; i < ja.Count; i++)
            {
                list.Add(LS1APISpecObject.FromJObject<T>(ja.GetAt(i)));
            }
        }
        public static void FromJArray<T>(this ObservableCollection<T> list, JObject parentObject, string name) where T : LS1APISpecObject, new()
        {
            JArray ja = parentObject.GetValueJArray(name);
            FromJArray<T>(list, ja);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this JArray ja) where T : LS1APISpecObject, new()
        {
            if (ja == null)
                return null;
            ObservableCollection<T> list = new ObservableCollection<T>();
            list.FromJArray(ja);
            return list;
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this JObject parentObject, string name) where T : LS1APISpecObject, new()
        {
            if (parentObject == null)
                return null;

            JArray ja = parentObject.GetValueJArray(name);
            if (ja == null)
            {

                JObject jo = parentObject.GetValueJObject(name);
                if (jo!=null)
                {
                    return jo.ToObservableCollection<T>();
                }
                return null;
            }
            return ja.ToObservableCollection<T>();
        }

        public static JObject ToJObject<T>(this IEnumerable<T> list) where T : LS1APISpecObject, new()
        {
            JObject jo = new JObject();
            if (list != null)
            {
                foreach(var t in list)
                {                    
                    JObject joNew = t.GetJObject();
                    if (joNew.ContainsKey("name"))
                        joNew.Remove("name");

                    jo.Add(t.Name, joNew);
                }
            }
            return jo;
        }

        public static void AddObject<T>(this JObject jo, string name, IEnumerable<T> list) where T : LS1APISpecObject
        {
            if (list == null)
                return;


            JObject ja = new JObject();
            if (list != null)
            {
                foreach (var t in list)
                {
                    JObject joNew = t.GetJObject();
                    if (joNew.ContainsKey("name"))
                        joNew.Remove("name");

                    ja.Add(t.Name, joNew);
                }
            }

            jo.Add(name, ja);
        }

        public static JArray ToJArray<T>(this IEnumerable<T> list) where T : LS1APISpecObject, new()
        {
            JArray ja = new JArray();
            if (list != null)
            {
                foreach (var t in list)
                {
                    ja.Add(t.GetJObject());
                }
            }
            return ja;
        }

        public static void ToJArray<T>(this IEnumerable<T> list, JObject parentObject, string name) where T : LS1APISpecObject, new()
        {
            if (list == null)
                return;

            JArray ja = new JArray();
            if (list != null)
            {
                foreach (var t in list)
                {
                    ja.Add(t.GetJObject());
                }
            }

            parentObject[name] = ja;
        }

        public static void Add(this JObject jo, string name, LS1APISpecObject obj)
        {
            if (obj == null)
                jo.Add(name, null);
            else
                jo.Add(name, obj.GetJObject());
        }
        
        public static void Add<T>(this JObject jo, string name, IEnumerable<T> list) where T : LS1APISpecObject
        {
            if (list == null)
                return;

            JArray ja = new JArray();
            foreach (T o in list)
            {
                ja.Add(o.GetJObject());
            }

            jo.Add(name, ja);
        }

      
    }
}
