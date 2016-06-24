using UnityEngine;

namespace gametheory.iOS.HealthKit
{
	[System.Obsolete]
    public class HealthKitManager : MonoBehaviour 
    {
        #region Events
        public static event System.Action<string> receivedAge;
        public static event System.Action<string> receivedHeight; //cm
        public static event System.Action<string> receivedWeight; //kilograms
        public static event System.Action<string> receivedBioSex; //male, female, not available
        public static event System.Action<string> receivedDOB; //pre-formatted string
        public static event System.Action<int> dailySteps;
        public static event System.Action initialized;
        #endregion

        #region Private Vars
        static HealthKitManager instance = null;
        #endregion

        #region Unity Methods
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        #endregion

        #region iOS Callbacks
        void Initialized(string allowed)
        {
            if(initialized != null)
                initialized();
        }
        void ReceivedAge(string age)
        {
            if(receivedAge != null)
            {
                receivedAge(age);
            }
        }
        void ReceivedHeight(string height)
        {
            if(receivedHeight != null)
            {
                receivedHeight(height);
            }

        }
        void ReceivedWeight(string weight)
        {
            if(receivedWeight != null)
            {
                receivedWeight(weight);
            }
        }
        void ReceivedBiologicalSex(string bioSex)
        {
            if(receivedBioSex != null)
                receivedBioSex(bioSex);
        }
        void ReceivedDOB(string dob)
        {
            if(receivedDOB != null)
                receivedDOB(dob);
        }
        void DailyStepsCallback(string steps)
        {
            int value = 0;
            int.TryParse(steps, out value);

            if(dailySteps != null)
                dailySteps(value);
        }
        #endregion
    }
}
