﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using BankingChatbot.TextStorage;
using BotService.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new IntentDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity activity)
        {
            string messageType = activity.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                IConversationUpdateActivity update = activity;
                using (ILifetimeScope scope = Microsoft.Bot.Builder.Dialogs.Internals.DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    IConnectorClient client = scope.Resolve<IConnectorClient>();
                    if (update.MembersAdded.Any())
                    {
                        Activity reply = activity.CreateReply();
                        foreach (ChannelAccount newMember in update.MembersAdded)
                        {
                            if (newMember.Id != activity.Recipient.Id)
                            {
                                reply.Text = TextProvider.Provide(TextCategory.GREETING_UserAdded);
                                await client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }

                    }
                }
            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }

            return null;
        }
    }
}