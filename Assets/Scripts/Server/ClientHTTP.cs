using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Globalization;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Compression;

namespace HttpClientStatus
{
    
    public class ClientHTTP : MonoBehaviour
    {
        public string greenhouseID="0";
        internal string userToken="admin";
        internal string passToken="admin";
        private UriFactory _uriFactory;
        List<CircuitItems> jsonLoads;
        bool shouldLoadJson = true;
        internal void SetLogInfo(string user, string pass, string id)
        {
            userToken = user;
            passToken = pass;
            greenhouseID = id;
        }



        private void Awake()
        {
            _uriFactory = GetComponent<UriFactory>();
        }

        public void Update()
        {
            if(jsonLoads!= null&& shouldLoadJson)
            {
                shouldLoadJson = false;
                FindObjectOfType<JsonReader>().LoadJsonFile(jsonLoads[0]);

            }
        }

        public void Decompress(string source, string destination, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("Source file path is null or empty.");
            }

            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentNullException("Destination path is null or empty.");
            }

            if (File.GetAttributes(source).HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("Source is a directory. You need to specify the name of a zip file, instead.");
            }

            if (!File.GetAttributes(destination).HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("Destination is not a directory. You need to specify the name of a directory, instead.");
            }

            if (!File.Exists(source))
            {
                throw new FileNotFoundException("Source file does not exist.");
            }

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            if (overwrite)
            {
                using (ZipArchive archive = ZipFile.OpenRead(source))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {

                        //decompressionStream.CopyTo(decompressedFileStream);
                        //print($"Decompressed: {fileToDecompress.Name}");

                        string targetFile = Path.Combine(destination, entry.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                        entry.ExtractToFile(targetFile, true);

                    }
                }
            }
            else
            {
                ZipFile.ExtractToDirectory(source, destination);
            }
        }


        public  void ThrowError(string content)
        {
            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                throw new Exception("Server Error:" + content);
            });
        }
        async private Task<string> GetTokenAsync()
        {
            print("inicio token");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    print(_uriFactory.GetAutenticateUri());
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = _uriFactory.GetAutenticateUri(),
                        Content = new StringContent("{\n\"Username\" : \"" + userToken + "\",\n\"Password\" : \"" + passToken + "\"\n}")
                        {
                            Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                        }
                    };
                    print("Seteo el token");
                    using (var response = await httpClient.SendAsync(request))
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            ThrowError("Log in Error. Code " + response.StatusCode);
                            return "";
                        }

                        print("consigo el token : " + response.Content.ReadAsStringAsync());
                        return await response.Content.ReadAsStringAsync();


                    }
                }
                catch (Exception e)
                {

                    print ("UPS Error "+ e.Message + e.StackTrace + e.Source);
                }
                print("Seteo el token");
                return "";

            }
        }


        public void SetJsonConstructionfiles()
        {
            {
                Task<string> tokenTask = GetTokenAsync();
                Task response = tokenTask.ContinueWith(task =>
                {
                    print("token is: " + task.Result);
                    try
                    {

                        using (var client = new WebClient())
                        { 
                            var request = _uriFactory.GetConstructionJsonUri();//V
                            print("antes de mandar la info: " + request);
                            client.Headers[HttpRequestHeader.Authorization] = "Bearer " + task.Result;
                            while (client.IsBusy) { System.Threading.Thread.Sleep(1000); }

                            Stream data = client.OpenRead(request);
                            StreamReader reader = new StreamReader(data);
                            string s = reader.ReadToEnd();
                            Console.WriteLine(s);
                            data.Close();
                            reader.Close();

                            ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
                            {
                                print("ok, recibi esto: "+ s);
                                jsonLoads= JsonConvert.DeserializeObject<List<CircuitItems>>(s);
                                //T test = JsonUtility.FromJson<T>(s);
                                // result = JsonConvert.DeserializeObject<List<T>>(s);
                                //ACA VA LA FUNCION
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        print("Conecting to the server failed: " + e.Message + e.StackTrace);
                    }
                });
            }
        }


        public async Task<String> GetDataPointFromAsync(Uri uri, string token)
        {
            using (var httpClient = new HttpClient()) { 
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = uri,
                    Headers =
                    {
                        { "Authorization", "Bearer " + token },
                    },
                };
                using (var response = await httpClient.SendAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ThrowError("Unauthorized");
                        return null;
                    }
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ThrowError("Request Error");
                        return null;
                    }
                    string res = await response.Content.ReadAsStringAsync();
                    return res;
                }
            }
        }
        public Task<T> GetFromServer<T>(Uri uri)
        {
            return GetFromServerString(uri)
                .ContinueWith(t => JsonConvert.DeserializeObject<T>(t.Result));
        }

        public Task<string> GetFromServerString(Uri uri)
        {
            Task<string> tokenTask = GetTokenAsync();
            Task<string> responseTask = tokenTask.ContinueWith(async token =>
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = uri,
                        Headers =
                            {
                                { "Authorization", "Bearer " + token.Result },
                            },
                    };
                    using (var response = await httpClient.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            ThrowError("Unauthorized");
                        }
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            ThrowError("Request Error");
                        }
                        string res = await response.Content.ReadAsStringAsync();

                        return res;
                    }

                }

            }).Unwrap();

            return responseTask;
        }
    }
}
