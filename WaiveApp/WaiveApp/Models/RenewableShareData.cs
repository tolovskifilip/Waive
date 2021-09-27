using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace WaiveApp.Models
{
    public class RenewableShareData
    {
        [Required]
        [JsonProperty("id")]
        public int id { get; set; }

        [Required]
        [JsonProperty("dateTime_of_Prediction")]
        public DateTime dateTime_of_Prediction { get; set; }

        [Required]
        [JsonProperty("renewableShare")]
        public double renewableShare { get; set; }

        [Required]
        [JsonProperty("region")]
        public string region { get; set; }

        //[Required]
        //public bool IsGreenOrNot { get; set; }

        public RenewableShareData(int id_input, DateTime dateTime_of_Prediction_input, double renewableShare_input, string region_input)
        {
            id = id_input;
            dateTime_of_Prediction = dateTime_of_Prediction_input;
            renewableShare = renewableShare_input;
            region = region_input;
        }

    }
}
