using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    static class Sync
    {
        static string push_url = "http://127.0.0.1/push_data.php";
        static string get_url = "http://127.0.0.1/get_data.php";
        static string check_version_url = "http://127.0.0.1/check_version.php";

        public static void PushToSyncQueue(int evenid, int type)
        {
            var check = Model.DataProvider.Ins.DB.SYNCQUEUES.Where(p => p.status == false && evenid == p.evenid).Count();
            if (check > 0) return;

            Model.SYNCQUEUE syncItem = new Model.SYNCQUEUE();
            syncItem.evenid = evenid;
            syncItem.status = false;
            syncItem.type = type;
            Model.DataProvider.Ins.DB.SYNCQUEUES.Add(syncItem);
            Model.DataProvider.Ins.DB.SaveChanges();
        }

        public static void StartSyncToServer()
        {
            try
            {
                var list = Model.DataProvider.Ins.DB.SYNCQUEUES.Where(p => p.status == false).OrderBy(p => p.id);

                foreach (Model.SYNCQUEUE item in list)
                {
                    string url = "";

                    if (item.type == 1)
                    {
                        var even = Model.DataProvider.Ins.DB.EVENS.Where(p => p.id == item.evenid).FirstOrDefault();
                        if (even != null)
                        {
                            url = String.Format(push_url + "?evenid={0}&evenname={1}&type={2}&daytime={3}&notiday={4}&status={5}&color={6}&objectid={7}&comment={8}",
                                even.evenid, even.evenname, even.type, even.daytime, even.notiday, even.status ? 1 : 0, even.color, even.objectid, even.comment);
                        }
                    }
                    else if (item.type == 2)
                    {
                        url = $"{push_url}?delete_id={item.evenid}";
                    }

                    var result = Request(url);

                    if (result.Equals("OK"))
                    {
                        item.status = true;
                    }
                }


                Model.DataProvider.Ins.DB.SaveChanges();
            }
            catch (Exception e) { }
        }

        public static int getLocalVersion()
        {
            var even = Model.DataProvider.Ins.DB.EVENS.OrderBy(p => -p.id).FirstOrDefault();

            if (even != null)
            {
                return even.evenid;
            }

            return 0;
        }

        public static void StartSyncFromServer(DateTime date)
        {
            try
            {
                int localVersion = getLocalVersion();
                int serverVersion = getServerVersion();
                if (getLocalVersion() != getServerVersion())
                {
                    string result = Request(get_url + String.Format("?date={0}", date));

                    if (!String.IsNullOrEmpty(result))
                    {
                        string[] pre = { "</br>" };
                        string[] list = result.Split(pre, StringSplitOptions.None);

                        if (list[0].Equals("SUCCESS") && list[list.Length - 1].Equals("END"))
                        {
                            Model.DataProvider.Ins.DB.Database.ExecuteSqlCommand("DELETE FROM EVENS");

                            for (int i = 1; i < list.Length - 1; i++)
                            {
                                string[] evenItem = list[i].Split('|');

                                Model.EVEN even = new Model.EVEN();
                                even.evenid = int.Parse(evenItem[0]);
                                even.evenname = evenItem[1];
                                even.type = int.Parse(evenItem[2]);
                                even.daytime = DateTime.Parse(evenItem[3]);
                                even.notiday = int.Parse(evenItem[4]);
                                even.status = evenItem[5] == "0" ? false : true;
                                even.color = int.Parse(evenItem[6]);
                                even.objectid = int.Parse(evenItem[7]);
                                even.comment = evenItem[8];

                                Model.DataProvider.Ins.DB.EVENS.Add(even);

                            }
                        }

                        Model.DataProvider.Ins.DB.SaveChanges();
                    }
                }

                Properties.Settings.Default.lastUpdate = DateTime.Now;
            }
            catch (Exception e) { }
        }

        public static int getServerVersion()
        {
            try
            {
                string result = Request(check_version_url);
                int re;
                int.TryParse(result, out re);
                return re;
            }
            catch (Exception e) { }

            return -1;
        }

        public static string Request(string url)
        {
            string responseFromServer = "";
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            return responseFromServer;
        }
    }
}
