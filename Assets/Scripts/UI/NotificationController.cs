using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;


public enum NotificationType
{
    Warning,
    Advisor,
    Default
}

public class NotificationController : MonoBehaviour
{
    public GameObject NotificationContainer;
    public Notification WarningNotificationPrefab;
    public Notification AdvisorNotificationPrefab;
    public Notification DefaultNotificationPrefab;

    private List<Notification> _notifications = new List<Notification>();
    
    public void DisplayNotification(string text, NotificationType type)
    {
        var note = Instantiate(
            GetNotificationPrefabByType(type), 
            NotificationContainer.transform);

        note.Controller = this;
        note.SetText(text);

        _notifications.Add(note);

        if (_notifications.Count > 2)
        {
            var notification = _notifications[0];
            RemoveNotification(notification);
            notification.Dismiss();
        }
    }

    public void RemoveNotification(Notification notification)
    {
        _notifications.Remove(notification);
    }

    private Notification GetNotificationPrefabByType(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.Warning:
                return WarningNotificationPrefab;
            case NotificationType.Advisor:
                return AdvisorNotificationPrefab;
            case NotificationType.Default:
            default:
                return DefaultNotificationPrefab;
        }
    }
}