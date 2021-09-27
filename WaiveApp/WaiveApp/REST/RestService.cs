using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WaiveApp.Models;

namespace WaiveApp.REST
{
        public class RestService  : IRestService
        {
            HttpClient client;
            private JsonSerializer _serializer = new JsonSerializer();

            public List<RenewableShareData> Items { get; private set; }
            public RestService()
            {
                client = new HttpClient();
                client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4eeb736011f545a8a4d5ccc444ea9152");
                
            }

       
        //When to use NSUrlSessionHandler and AndroidClientHandler
        //As general guidance, always use NSUrlSessionHandler and AndroidClientHandler when you use HttpClient.They both provide benefits from the native networking stack.
        //Using these handlers is the default for new Xamarin.Forms projects.There are project settings to change the HttpClient default.
        //To change the default HttpClient handler for Xamarin.iOS, open iOS project's properties.
        //Under the iOS Build tab, there's an HttpClient Implementation option.If you set this option to NSUrlSession, HttpClient uses the native iOS handler without passing it into the constructor.
        //To change the default HttpClient handler for Xamarin.Android, open the Android project's properties. Under the Android Options tab, click the Advanced button at the bottom. In the Advanced Android Options dialog, there's an SSL/TLS implementation option.Setting this value to Native TLS 1.2+ makes HttpClient default to using the message handler.
            public async Task<List<RenewableShareData>> RefreshDataAsync(string region)
            {
                //Newtonsoft.Json.JsonSerializer _serializer = new Newtonsoft.Json.JsonSerializer();
                List<RenewableShareData> Items=new List<RenewableShareData>();
                Uri uri = new Uri(Constants.RestUrl+region);
                Newtonsoft.Json.JsonSerializer _serializer = new Newtonsoft.Json.JsonSerializer();

                //Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                //Debug.WriteLine(uri.ToString());
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    //Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    //Debug.WriteLine(region.ToString());
                    //Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    //Debug.WriteLine(response.StatusCode.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        //Using the ReadAsStringAsync method to retrieve a large response can have a negative performance impact.
                        //In such circumstances the response should be directly deserialized to avoid having to fully buffer it.
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new StreamReader(stream))
                        using (var json_content = new JsonTextReader(reader))
                        Items=_serializer.Deserialize<List<RenewableShareData>>(json_content);
                        

                        //Items = await System.Text.Json.JsonSerializer.DeserializeAsync<List<RenewableShareData>>(json_content, serializerOptions);
                        //Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        //Debug.WriteLine(Items[6].dateTime_of_Prediction.ToString()); //.Content.ReadAsStringAsync());
                    }
                
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }

                return Items;
            }

            

           
       
    }
}
