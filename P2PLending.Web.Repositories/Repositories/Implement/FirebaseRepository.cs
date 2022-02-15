using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class FirebaseRepository : IFirebaseRepository
    {
        public async Task<string> GetFirebaseToken(string deviceId)
        {
            var firebaseToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(deviceId);

            return firebaseToken;
        }

        public async Task<string> SendMessage(string token, string title, string body, Dictionary<string, string> datas)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Notification = new AndroidNotification()
                    {
                        ClickAction = FirebaseConfig.ClickAction,
                        ChannelId = FirebaseConfig.ChannelId
                    }
                },
                Data = datas,
                Token = token,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return response;
        }

        public async Task<string> SendTopicMessage(string topic, string title, string body, Dictionary<string, string> datas)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Notification = new AndroidNotification()
                    {
                        ClickAction = FirebaseConfig.ClickAction,
                        ChannelId = FirebaseConfig.ChannelId
                    }
                },
                Data = datas,
                Topic = topic
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return response;
        }

        public async Task<BatchResponse> SendMessages(List<string> tokens, string title, string body, Dictionary<string, string> datas)
        {
            var message = new MulticastMessage()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Notification = new AndroidNotification()
                    {
                        ClickAction = FirebaseConfig.ClickAction,
                        ChannelId = FirebaseConfig.ChannelId
                    }
                },
                Data = datas,
                Tokens = tokens,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

            return response;
        }
    }
}
