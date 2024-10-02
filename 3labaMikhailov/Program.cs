using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7810865647:AAFnK8PKQERf5kBfKY9ZIotzhUro93SQDtI", cancellationToken:cts.Token);
var me = await bot.GetMeAsync();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method that handle messages received by the bot:

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;	// we only handle Text messages here
    Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
    // let's echo back received text in the chat
    await bot.SendTextMessageAsync(msg.Chat, $"{msg.From} said: {msg.Text}");

    if (msg.Text == "/start")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Welcome! Pick one direction",
            replyMarkup: new InlineKeyboardMarkup().AddButtons("Left", "Right"));
    }

    // Добавляем данный фрагмент
    if (msg.Text == "Проверка")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Проверка бота: работа корректна");
    }
    if (msg.Text == "Привет")
    {
        await bot.SendTextMessageAsync(msg.Chat, "Ку,здарова");
    }
    if (msg.Text == "Картиночка")
    {
        await bot.SendStickerAsync(msg.Chat, "https://telegrambots.github.io/book/docs/sticker-dali.webp");

    }
    if (msg.Text == "Видосик")
    {
        await bot.SendVideoAsync(msg.Chat, "https://telegrambots.github.io/book/docs/video-hawk.mp4");

    }
    var message = await bot.SendTextMessageAsync(msg.Chat, "Trying <b>all the parameters</b> of <code>sendMessage</code> method",
    parseMode: ParseMode.Html,
    protectContent: true,
    replyParameters: msg.MessageId,
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl("Check sendMessage method", "https://core.telegram.org/bots/api#sendmessage"),
        InlineKeyboardButton.WithCallbackData("Data")));
}
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        await bot.AnswerCallbackQueryAsync(query.Id, $"You picked {query.Data}");
        await bot.SendTextMessageAsync(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}

// условная телефонная книга
Dictionary<string, string> phoneBook = new Dictionary<string, string>();

// добавляем элемент: ключ - номер телефона, значение - имя абонента
phoneBook.Add("+123456", "Tom");
// альтернативное добавление
// phoneBook["+123456"] = "Tom";

// Проверка наличия
bool phoneExists1 = phoneBook.ContainsKey("+123456");    // true
Console.WriteLine($"+123456: {phoneExists1}");
bool phoneExists2 = phoneBook.ContainsKey("+567456");    // false
Console.WriteLine($"+567456: {phoneExists2}");
bool abonentExists1 = phoneBook.ContainsValue("Tom");      // true
Console.WriteLine($"Tom: {abonentExists1}");
bool abonentExists2 = phoneBook.ContainsValue("Bob");      // false
Console.WriteLine($"Bob: {abonentExists2}");

// удаление элемента
phoneBook.Remove("+123456");

// проверяем количество элементов после удаления
Console.WriteLine($"Count: {phoneBook.Count}"); // Count: 0