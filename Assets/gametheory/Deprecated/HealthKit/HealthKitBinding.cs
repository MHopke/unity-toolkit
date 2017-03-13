using System.Runtime.InteropServices;
using UnityEngine;

namespace gametheory.iOS.HealthKit
{
    public enum BiologicalSex { MALE, FEMALE, NOT_SET, NOT_AVAILABLE }

	[System.Obsolete]
    public static class HealthKitBinding 
    {
        #region Methods
        public static void Init()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                InitHealthKit();
        }
        public static void PostRun(string json)
        {
            //Debug.Log("posting to healthkit: " + json);
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                PostHKRun(json);
        }

        public static void GetAge()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                GetHKAge();
        }
        public static void GetHeight()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                GetHKHeight();
        }
        public static void GetWeight()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                GetHKWeight();
        }
        public static void GetBiologicalSex()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                GetHKSex();
        }
        public static void GetDOB()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
             GetHKDateOfBirth();
        }

        public static void DailySteps()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
                GetDailySteps();
        }
        #endregion

        #region Externs
        [DllImport ("__Internal")]
        static extern void InitHealthKit();

        [DllImport ("__Internal")]
        static extern void PostHKRun(string json);

        [DllImport ("__Internal")]
        static extern void GetHKAge();

        [DllImport ("__Internal")]
        static extern void GetHKHeight();

        [DllImport ("__Internal")]
        static extern void GetHKWeight();

        [DllImport ("__Internal")]
        static extern void GetHKSex();

        [DllImport ("__Internal")]
        static extern void GetHKDateOfBirth();

        [DllImport ("__Internal")]
        static extern void GetSteps(string dateString, int timeInSeconds);

        [DllImport ("__Internal")]
        static extern void GetDailySteps();
        #endregion
    }
}
