using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
namespace RishadTest
{
    
    [System.Serializable]
    public class DailyData
    {
        public string time;
        public string temperature_2m_max;
    }

    [System.Serializable]
    public class CustomWeatherData
    {
        public double latitude;
        public double longitude;
        public double generationtime_ms;
        public int utc_offset_seconds;
        public string timezone;
        public string timezone_abbreviation;
        public double elevation;
        public DailyData daily_units;
        public Daily daily;
    }

    [System.Serializable]
    public class Daily
    {
        public List<string> time;
        public List<float> temperature_2m_max;
    }
    public class CheckWeatherScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text output;
        private string _prefferenceTodaysTemp = "TODAYS TEMPERATURE";
        private string URL;
        void Start()
        {
            TestLocationService testLocationService = new TestLocationService();
            output.text = "fetching location";
            StartCoroutine(testLocationService.GetLocation(this));  
            
        }
        private void BuildURI(float latitude, float longitude)
        {
            URL = "https://api.open-meteo.com/v1/forecast?latitude="+latitude +"&longitude=" + longitude +"&timezone=IST&daily=temperature_2m_max";
        }

        public void GetRequest(float latitude, float longitude)
        {
            output.text = "Fetching Temperature";
            BuildURI(latitude, longitude);
            StartCoroutine(GetRequestDelayed());
        }

        void StoreWeatherData(string todaysTemp)
        {
            PlayerPrefs.SetString(_prefferenceTodaysTemp, todaysTemp );
            Debug.Log("storedData: " + todaysTemp);
            output.text = todaysTemp;
        //    ShowAndroidToastMessage("temp is: "+ todaysTemp);
        }
        void ParseRequest(string jsonData)
        {
            CustomWeatherData data = JsonUtility.FromJson<CustomWeatherData>(jsonData);
            // print the fields
            // get today's temperature (1st value)
            output.text = data.daily.temperature_2m_max[0] + "";
            if (data.daily.temperature_2m_max.Count > 0)
            {
                Debug.Log("First Daily Max Temperature: " + data.daily.temperature_2m_max[0]);
                StoreWeatherData(data.daily.temperature_2m_max[0] + data.daily_units.temperature_2m_max);
            }
            else
            {
                Debug.Log("No temperature data available.");
            }
        }
        IEnumerator GetRequestDelayed()
        {
            output.text = URL;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = URL.Split('/');
                int page = pages.Length - 1;
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        // if true, print the string
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        // convert the string into JSON data
                        ParseRequest(webRequest.downloadHandler.text);
                        break;
                }
            }
         }
        
        
        // Update is called once per frame

    }
}