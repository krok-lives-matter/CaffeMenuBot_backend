using CaffeMenuBot.Bot.Actions.Interface;
using CaffeMenuBot.Data;
using CaffeMenuBot.Data.Models.Bot;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CaffeMenuBot.Bot.Actions.Callbacks
{
    /// <summary>
    /// Handles inline rating keyboard comment button press
    /// </summary>
    public sealed class AwaitCommentCallback : IChatAction
    {
        private CaffeMenuBotContext Context { get; }
        private ITelegramBotClient Client { get; }
        
        // Contains uses this identifier instead of COMMAND_NAME
        private const string CALLBACK_ID = "CCC";
        private const string MESSAGE_TITLE = "Send your comment by utilizing text input of telegram below";

        public AwaitCommentCallback(CaffeMenuBotContext context, ITelegramBotClient client)
        {
            Context = context;
            Client = client;
        }

        public bool Contains(BotUser user, Update update)
        {
            if (update.CallbackQuery == null) return false;
            return update.CallbackQuery.Data.Contains(CALLBACK_ID);
        }

        public async Task ExecuteAsync(BotUser user, Update update, CancellationToken ct)
        {
            // set user state to pending_comment, so state handler could handle it
            user.State = ChatState.pending_comment;
            Context.Entry(user).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            await Client.AnswerCallbackQueryAsync(
                update.CallbackQuery.Id,
                cancellationToken: ct);

            await Client.SendTextMessageAsync(
                update.CallbackQuery.From.Id,
                MESSAGE_TITLE,
                cancellationToken: ct);
        }
    }
}