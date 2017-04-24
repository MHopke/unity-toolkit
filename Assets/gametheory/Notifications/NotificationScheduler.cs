using UnityEngine;

#if UNITY_ANDROID
using System.Collections.Generic;

using Area730.Notifications;
#endif

using Newtonsoft.Json;


//Requires Android Notification plugin found at https://www.assetstore.unity3d.com/en/#!/content/45507
//use Birnamwoodgames@gmail.com : standard password
public class NotificationScheduler : MonoBehaviour
{
    #region Constants
    #if UNITY_ANDROID
    const string ID_KEY = "ids";
    #endif
    #endregion

    #region Public Vars
    public static NotificationScheduler Instance = null;
    #endregion

    #region Private Vars
#if UNITY_ANDROID
    List<int> _notificationIds;
#endif
    #endregion

    #region Unity Methods
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

#if UNITY_IPHONE
            //only use the notification type you need
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
                UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
#endif

#if UNITY_ANDROID
            LoadIds();
#endif
        }
        else
            Destroy(gameObject);
    }
#endregion

#region Methods
    //Sample method
    /*public void ScheduleDailyNotification(DateTime time,bool repeat)
    {
#if UNITY_IPHONE
        UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
        notification.fireDate = time;
        notification.alertAction = DAILY_ACTION;
        notification.alertBody = DAILY_BODY;
        if(repeat)
        {
            notification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.GregorianCalendar;
            notification.repeatInterval = UnityEngine.iOS.CalendarUnit.Week;
        }

        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
#elif UNITY_ANDROID
        string NOTIFICATION_TYPE = "type";
        string ICON_NAME = "ic_stat_icon";
        string LAUNCHER_NAME = "ic_launcher";
        string ANDROID_TITLE = "FitStop";
        int DAILY_BASE = 0;

        TimeSpan delay  = time - DateTime.Now;
        int id = DAILY_BASE + _notificationIds.Count;
        _notificationIds.Add(id);
        SaveIds();

        NotificationBuilder builder = new NotificationBuilder(id, ANDROID_TITLE, DAILY_ACTION);
        builder
            .setTicker("")
            .setDefaults(NotificationBuilder.DEFAULT_ALL)
            .setRepeating(repeat)
            .setInterval(new TimeSpan(1,0,0,0))
            .setAlertOnlyOnce(true)
            .setDelay(delay)
            .setAutoCancel(true)
            .setGroup(NotificationType.Daily.ToString())
            .setColor("#B30000")
            .setSmallIcon(ICON_NAME) 
            .setLargeIcon(LAUNCHER_NAME);
        
        AndroidNotifications.scheduleNotification(builder.build());
#endif
    }*/

    void ClearNotifications()
    {
#if UNITY_IPHONE
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#elif UNITY_ANDROID
        AndroidNotifications.clearAll();
        _notificationIds.Clear();
        SaveIds();
#endif
    }

#if UNITY_ANDROID
    void SaveIds()
    {
        PlayerPrefs.SetString(ID_KEY,JsonConvert.SerializeObject(_notificationIds));
        PlayerPrefs.Save();
    }
    void LoadIds()
    {
        string json = PlayerPrefs.GetString(ID_KEY, "");
        if(string.IsNullOrEmpty(json))
            _notificationIds = new List<int>();
        else
            _notificationIds = JsonConvert.DeserializeObject<List<int>>(json);
    }
#endif
#endregion
}
