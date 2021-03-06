﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using WarfaceAuth;

namespace WarfaceWrapper
{
    public class Auth
    {
        Process_Start Start = new Process_Start();
        Debug_mod Debug_mod = new Debug_mod();
        static public string login = "";
        static public string password = "";
        static public string server = "";
        static public string shardid = "";
        static string ChannelId = "35";

        static public CookieContainer authInfo;
        static string Cookie_State;
        static string Cookie_Act;
        static string Cookie_o2csrf;
        static string Cookie_SDCS;
        static string Cookie_mc;
        static string session_key;
        static string RedirectUrl;

        static string csrfmiddlewaretoken_jwt;

        static CookieCollection Cookie_party;
        static string Cookie_party_helper = "";
        public void Get_State_Cookies()
        {
            Console.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://auth-ac.my.games/social/mailru");
            req.Method = "GET";
            req.UserAgent = "Downloader/15740";
            req.ContentType = "application/x-www-form-urlencoded";
            req.AllowAutoRedirect = false;
            authInfo = new CookieContainer();

            req.CookieContainer = authInfo;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Match m1 = Regex.Match(response.Headers.ToString(), "state=([\\s\\S]+?)&");
            if(m1.Success == true)
            {
                Cookie_State = m1.Groups[1].Value;
                Debug_mod.Write_debug("1:Get_State_Cookies > Get_Act_Cookies", response.Headers.ToString());
                Debug_mod.Write_NonDebug("Auth:1/12");
                response.Close();
                Get_Act_Cookies();
            }
            response.Close();
        }
        public void Get_Act_Cookies()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://account.mail.ru");
            req.Method = "GET";
            req.UserAgent = "Downloader/15740";
            req.ContentType = "application/x-www-form-urlencoded";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Match m1 = Regex.Match(response.Headers.ToString(), "act=([\\s\\S]+?);");
            Cookie_Act = m1.Groups[1].Value;
            Debug_mod.Write_debug("2:Get_Act_Cookies > Mailru_oauth", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:2/12");
            response.Close();
            Mailru_oauth();
        }
        public void Mailru_oauth()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://auth.mail.ru/cgi-bin/auth");
                req.Method = "POST";
                req.UserAgent = "Downloader/15740";
                req.Referer = "https://account.mail.ru/login?opener=o2";
                req.Headers.Add("Origin: https://account.mail.ru");
                req.ContentType = "application/x-www-form-urlencoded";
                req.AllowAutoRedirect = true;
                req.CookieContainer = authInfo;
                byte[] SomeBytes = null;
                string FormParams = $"username={login}" +
                                    $"&Login={login}" +
                                    $"&Password={password}" +
                                    $"&password={password}" +
                                    $"&act_token={Cookie_Act}" +
                                    $"&page=https://o2.mail.ru/xlogin?authid=kbjooyiv.dej" +
                                    $"&client_id=bbddb88d19b84a62aedd1ffbc71af201" +
                                    $"&force_us=1" +
                                    $"&from=o2" +
                                    $"&logo_target=_none" +
                                    $"&no_biz=1" +
                                    $"&redirect_uri=https%3A%2F%2Fauth-ac.my.games%2Fsocial%2Fmailru_callback%2F" +
                                    $"&remind_target=_self" +
                                    $"&response_type=code" +
                                    $"&scope=" +
                                    $"&signup_target=_self" +
                                    $"&state={Cookie_State}" +
                                    $"&new_auth_form=1" +
                                    $"&FromAccount=opener=o2" +
                                    $"&twoSteps=1" +
                                    $"&lang=en_US";
                SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Debug_mod.Write_debug("3:Mailru_oauth > Login_in_mygames", response.Headers.ToString());
                Debug_mod.Write_NonDebug("Auth:3/12");

                Match error_parse = Regex.Match(response.Headers.ToString(), "o2csrf=([\\s\\S]+?);");
                if (error_parse.Success == true)
                {
                    Cookie_o2csrf = error_parse.Groups[1].Value;
                    response.Close();
                    Login_in_mygames();
                }
                else
                {
                    response.Close();
                    Console.WriteLine("Bad login or password!");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
            catch
            {
                Debug_mod.Write_debug("3:Mailru_oauth > Get_State_Cookies (RESTART)");
                Get_State_Cookies();
            }
        }
        public void Login_in_mygames()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://o2.mail.ru/login");
                req.Method = "POST";
                req.UserAgent = "Downloader/15740";
                req.Referer = "https://o2.mail.ru/xlogin?client_id=bbddb88d19b84a62aedd1ffbc71af201" +
                              "&response_type=code" +
                              "&scope=" +
                               $"&redirect_uri=https%3A%2F%2Fauth-ac.my.games%2Fsocial%2Fmailru_callback%2F&state={Cookie_State}" +
                              "&no_biz=1" +
                              "&force_us=1" +
                              "&signup_target=_self" +
                              "&remind_target=_self" +
                              "&logo_target=_none";
                req.Headers.Add("Origin: https://o2.mail.ru");
                req.AllowAutoRedirect = true;
                req.CookieContainer = authInfo;
                byte[] SomeBytes = null;
                string FormParams = $"Page=https://o2.mail.ru/login?client_id=bbddb88d19b84a62aedd1ffbc71af201&response_type=code&scope=&redirect_uri=https%3A%2F%2Fauth-ac.my.games%2Fsocial%2Fmailru_callback%2F&state={Cookie_State}&no_biz=1" +
                    $"&FailPage=https://o2.mail.ru/login?client_id=bbddb88d19b84a62aedd1ffbc71af201&response_type=code&scope=&redirect_uri=https%3A%2F%2Fauth-ac.my.games%2Fsocial%2Fmailru_callback%2F&state={Cookie_State}&no_biz=1&fail=1" +
                    $"&login={login}&o2csrf={Cookie_o2csrf}&mode=";
                SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Debug_mod.Write_debug("4:Login_in_mygames > Get_SDCS_Cookie", response.Headers.ToString());
                Debug_mod.Write_NonDebug("Auth:4/12");
                response.Close();
                Get_SDCS_Cookie();
            }
            catch
            {
                Debug_mod.Write_debug("4:Login_in_mygames > Get_session_key (RESTART)");
                Get_session_key();
            }
        }
        public void Get_SDCS_Cookie()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://auth-ac.my.games/sdc?from=https%3A%2F%2Fapi.my.games%2Fsocial%2Fprofile%2Fsession&JSONP_call=callback1522169");
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.Referer = "https://o2.mail.ru/xlogin";
            req.Headers.Add("Origin: https://o2.mail.ru");
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Debug_mod.Write_debug("5:Get_SDCS_Cookie > Login_mygames", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:5/12");
            Match m1 = Regex.Match(response.Headers.ToString(), "sdcs=([\\s\\S]+?);");
            Cookie_SDCS = m1.Groups[1].Value;
            response.Close();
            Login_mygames();
        }
        public void Login_mygames()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://auth-ac.my.games/auth");
                req.Method = "POST";
                req.UserAgent = "Downloader/15740";
                req.Referer = "https://api.my.games/gamecenter/login/?lang=en_US";
                req.Headers.Add("Origin: https://api.my.games");
                req.ContentType = "application/x-www-form-urlencoded";
                req.AllowAutoRedirect = true;
                req.CookieContainer = authInfo;
                byte[] SomeBytes = null;
                string FormParams = $"email={login}" +
                                    $"&password={password}" +
                                    $"&continue=https://auth-ac.my.games/sdc?from=https%3A%2F%2Fapi.my.games%2Fgamecenter%2Flogin_finished%2F&failure=https://api.my.games/gamecenter/login/&nosavelogin=0";
                SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Debug_mod.Write_debug("6:Login_mygames > Get_session_key", response.Headers.ToString());
                Debug_mod.Write_NonDebug("Auth:6/12");
                Cookie_party = GetAllCookies(authInfo);

                foreach (Cookie cookie in Cookie_party)
                {
                    Cookie_party_helper += string.Join(".", "," + cookie.ToString()); //BRUH
                }
                Debug_mod.Write_debug("Debug cookie...", Cookie_party_helper);
                Match regex_cookie_mc = Regex.Match(Cookie_party_helper, "mc=([\\s\\S]+?),");
                Match regex_cookie_sdcs = Regex.Match(Cookie_party_helper, "sdcs=([\\s\\S]+?),");
                Cookie_mc = regex_cookie_mc.Groups[1].Value;
                Cookie_party_helper = "";
                response.Close();
                Get_session_key();
            }
            catch
            {
                Debug_mod.Write_debug("6:Login_mygames > Get_Eula");
                Get_Eula();
            }
        }

        public void Get_Eula()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.my.games/social/profile/session");
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            Stream newStream = req.GetRequestStream();
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Debug_mod.Write_debug("7:Get_Eula", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:7/12");

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string temp_page = reader.ReadToEnd();
            stream.Close();
            reader.Close();

            Match m1 = Regex.Match(temp_page, "\"token\":\"([\\s\\S]+?)\"");
            csrfmiddlewaretoken_jwt = m1.Groups[1].Value;
            response.Close();
            Csrfmiddleware_femboy();
        }
        public void Csrfmiddleware_femboy()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://auth-ac.my.games/auth");
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            byte[] SomeBytes = null;
            string FormParams = $"csrfmiddlewaretoken_jwt={csrfmiddlewaretoken_jwt}&csrfmiddlewaretoken=";
            SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
            req.ContentLength = SomeBytes.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(SomeBytes, 0, SomeBytes.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Debug_mod.Write_debug("8:Csrfmiddleware_femboy > Get_session_key", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:8/12");

            response.Close();
            Get_session_key();
        }
        public void Get_session_key()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://authdl.my.games/gem.php?hint=Auth");
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            byte[] SomeBytes = null;
            string FormParams = $"<?xml version='1.0' encoding='UTF-8'?><Auth mc='{Cookie_mc}' sdcs='{Cookie_SDCS}' ChannelId='{ChannelId}' GcLang='en' UserId='' UserId2='' StatPer='0'/>";
            SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
            req.ContentLength = SomeBytes.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(SomeBytes, 0, SomeBytes.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Debug_mod.Write_debug("9:Get_session_key > Get_RedirectUrl", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:9/12");

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string temp_page = reader.ReadToEnd();
            stream.Close();
            reader.Close();

            Match m1 = Regex.Match(temp_page, "SessionKey=\"([\\s\\S]+?)\"");
            session_key = m1.Groups[1].Value;

            response.Close();
            Get_RedirectUrl();
        }
        public void Get_RedirectUrl()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://authdl.my.games/gem.php?hint=Portal");
                req.Method = "POST";
                req.UserAgent = "Downloader/15740";
                req.AllowAutoRedirect = false;
                byte[] SomeBytes = null;
                string FormParams = $"<?xml version='1.0' encoding='UTF-8'?><Portal SessionKey='{session_key}' Url='http://authdl.my.games/robots.txt'/>";
                SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Debug_mod.Write_debug("10:Get_RedirectUrl > Include_session", response.Headers.ToString());
                Debug_mod.Write_NonDebug("Auth:10/12");

                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string temp_page = reader.ReadToEnd();
                stream.Close();
                reader.Close();

                Match m1 = Regex.Match(temp_page, "RedirectUrl=\"([\\s\\S]+?)&amp");
                RedirectUrl = m1.Groups[1].Value;
                response.Close();
                Include_session();
            }
            catch(Exception ex)
            {
                Debug_mod.Write_debug("10:Get_RedirectUrl > Get_State_Cookies", ex.Message);
                Get_State_Cookies();
            }
        }
        void Include_session()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RedirectUrl);
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            Debug_mod.Write_debug("11:Include_session > Get_user_id_and_token");
            Debug_mod.Write_NonDebug("Auth:11/12");
            Get_user_id_and_token();
        }
        public void Get_user_id_and_token()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://authdl.my.games/gem.php?hint=Login");
            req.Method = "POST";
            req.UserAgent = "Downloader/15740";
            req.AllowAutoRedirect = true;
            req.CookieContainer = authInfo;
            byte[] SomeBytes = null;
            string FormParams = $"<?xml version='1.0' encoding='UTF-8'?><Login SessionKey='{session_key}' ProjectId='1177' ShardId='0'/>";
            SomeBytes = Encoding.GetEncoding(1251).GetBytes(FormParams);
            req.ContentLength = SomeBytes.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(SomeBytes, 0, SomeBytes.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Debug_mod.Write_debug("12:Get_user_id_and_token > Start Process", response.Headers.ToString());
            Debug_mod.Write_NonDebug("Auth:12/12");

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string temp_page = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            Match m1 = Regex.Match(temp_page, "Code=\"([\\s\\S]+?)\".GameAccount=\"([\\s\\S]+?)\".PersId=\"([\\s\\S]+?)\".MRACToken=\"([\\s\\S]+?)\"");
            string token = m1.Groups[1].Value;
            string uid = m1.Groups[2].Value;
            Debug_mod.Write_debug("Responce start params",$"Token:{m1.Groups[1].Value}\nUid:{m1.Groups[2].Value}\nPersId:{m1.Groups[3].Value}\nMRACToken:{m1.Groups[4].Value}");

            response.Close();

            Start.Init_Tools();
            Debug_mod.Write_NonDebug("Starting WarfaceBot");
            Start.BotStart(uid, token, server, Args_watcher.bot_dir);
        }
        public static CookieCollection GetAllCookies(CookieContainer cookieJar)
        {
            CookieCollection cookieCollection = new CookieCollection();

            Hashtable table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            cookieJar,
                                                                            new object[] { });

            foreach (var tableKey in table.Keys)
            {
                String str_tableKey = (string)tableKey;

                if (str_tableKey[0] == '.')
                {
                    str_tableKey = str_tableKey.Substring(1);
                }

                SortedList list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            table[tableKey],
                                                                            new object[] { });

                foreach (var listKey in list.Keys)
                {
                    String url = "https://" + str_tableKey + (string)listKey;
                    cookieCollection.Add(cookieJar.GetCookies(new Uri(url)));
                }
            }
            return cookieCollection;
        }
    }
}
