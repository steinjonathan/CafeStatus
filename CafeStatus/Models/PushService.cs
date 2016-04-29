using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CafeStatus.Models
{
    public static class PushService
    {
        private static GcmServiceBroker gcmBroker;

        public static void Send()
        {
            using (var contexto = new CafeDBContext())
            {
                var ids = contexto.PushDevices.Select(a => a.Endpoint).ToList();
                foreach (var regId in ids)
                {
                    // Queue a notification to send
                    gcmBroker.QueueNotification(new GcmNotification
                    {
                        RegistrationIds = new List<string> {
                        regId
                    }
                    });
                }
            }
        }

        public static void Stop()
        {
            gcmBroker.Stop();
            gcmBroker = null;
        }

        public static void Configure(string senderId, string authToken)
        {
            var config = new GcmConfiguration(senderId, authToken, null);
            gcmBroker = new GcmServiceBroker(config);
            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is GcmNotificationException)
                    {
                        var notificationException = (GcmNotificationException)ex;

                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;

                        Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        var multicastException = (GcmMulticastResultException)ex;

                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            Console.WriteLine($"GCM Notification Failed: ID={succeededNotification.MessageId}");
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;

                            Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Message}");
                        }

                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        var expiredException = (DeviceSubscriptionExpiredException)ex;

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;


                        if (!string.IsNullOrEmpty(newId))
                        {
                            using (var contexto = new CafeDBContext())
                            {
                                var oldPushDevice = contexto.PushDevices.FirstOrDefault(a => a.Endpoint == oldId);
                                if (oldPushDevice != null)
                                {
                                    oldPushDevice.Endpoint = newId;
                                    contexto.SaveChanges();
                                }
                            }
                        }


                        Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException)
                    {
                        var retryException = (RetryAfterException)ex;
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        Console.WriteLine("GCM Notification Failed for some unknown reason");
                    }

                    // Mark it as handled
                    return true;
                });
            };
            gcmBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("GCM Notification Sent!");
            };
            gcmBroker.Start();
        }
    }


}