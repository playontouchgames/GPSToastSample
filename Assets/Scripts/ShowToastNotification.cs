using UnityEngine;

namespace RishadTest
{
    public class ShowToastNotification : MonoBehaviour
    {
        // Start is called before the first frame update
        public void ButtonPressed()
        {
            Utils.ShowAndroidToastMessage("This is a toast notification sample");
        }
    }
}