#!/bin/env node
var TelegramBot = require('node-telegram-bot-api');
var translator = require('./translator');

/* Bot setup */
var token = 'YOUR_TOKEN';

// Setup polling way
var bot = new TelegramBot(token);
bot.setWebHook('YOUR_WEBHOOK' + bot.token);


console.log('bot server started...');
/* End bot setup */

//Callback for wrapping bot.sendMessage
function sendMessageBack(id, message, options) {
    console.log(id,message,options);
    bot.sendMessage(id, message, options);
}


//Matches /eng [list,of,words]
bot.onText(/\/eng (.+)/, function (msg, match) {
  var wordsToSearch = match[1].split(','); //Array with words to traduce

  for (word in wordsToSearch) {
    translator.translate(msg, 'http://www.wordreference.com/es/en/translation.asp?spen=' 
        + wordsToSearch[word], sendMessageBack);
  }
});

//Matches /spa [lista,de,palabras]
bot.onText(/\/spa (.+)/, function (msg, match) {
  var wordsToSearch = match[1].split(','); //Array with words to traduce

  for (word in wordsToSearch) {
    translator.translate(msg, 'http://www.wordreference.com/es/translation.asp?tranword=' 
        + wordsToSearch[word], sendMessageBack);
  }
  
});
//Matches /help
bot.onText(/\/help/, function (msg, match) {
  var fromId = msg.chat.id;
  var resp = 'Modo de uso - Usage mode: \n' + 
            '/eng lista,de,palabras : Traduce al Inglés la lista de palabras' +
                'separadas por comas\n' +
            '/spa list,of,words : Translate from English the list of words' + 
                'separated by commas\n' +
            '/help display this message';
  bot.sendMessage(fromId, resp);
});
module.exports = bot;
