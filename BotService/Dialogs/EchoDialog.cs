﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecivedAsync);
        }

        public virtual async Task MessageRecivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(context, AfterResetAsync, "Are you sure you want to reset the count?",
                    "Didn't get that!", promptStyle: PromptStyle.None);
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(MessageRecivedAsync);
            }
        }

        private async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                count = 1;
                await context.PostAsync("Reset count!");
            }
            else
            {
                await context.PostAsync("Did not reset!");
            }
            context.Wait(MessageRecivedAsync);
        }
    }
}