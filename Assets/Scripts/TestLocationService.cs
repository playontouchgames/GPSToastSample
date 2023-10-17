using UnityEngine;
using System.Collections;

namespace RishadTest
{
    public class TestLocationService : MonoBehaviour
    {
        public IEnumerator GetLocation(CheckWeatherScript _chk)
        {
            ///_chk.GetRequest(17.28f, 72.32f);
            // Check if the user has location service enabled.
            if (!Input.location.isEnabledByUser)
                Debug.Log("Location not enabled on device or app does not have permission to access location");
                //ShowAndroidToastMessage("Location not enabled on device or app does not have permission to access location");
            // Starts the location service.
            Input.location.Start();

            // Waits until the location service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // If the service didn't initialize in 20 seconds this cancels location service use.
            if (maxWait < 1)
            {
                Debug.Log("Timed out");
                Utils.ShowAndroidToastMessage("Timed out");
                yield break;
            }

            // If the connection failed this cancels location service use.
            if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
            {
                Debug.LogError("Unable to determine device location");
                Utils.ShowAndroidToastMessage("Unable to determine device location, please enable location sharing");
                yield break;
            }
            else
            {

                //Debug.Log("Status: " + Input.location.status);
               // ShowAndroidToastMessage("Status: " + Input.location.status);
                // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                Utils.ShowAndroidToastMessage("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                _chk.GetRequest(Input.location.lastData.latitude, Input.location.lastData.longitude);
                //CheckWeatherScript checkWeatherScript = new CheckWeatherScript();
                Utils.ShowAndroidToastMessage("Location2: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            }

            // Stops the location service if there is no need to query location updates continuously.
            Input.location.Stop();
        }
       
    }
}