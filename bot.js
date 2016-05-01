#!/bin/env node
var TelegramBot = require('node-telegram-bot-api');
var translator = require('./translator');


// Bot setup
var token = 'YOUR TOKEN';

// Setup webhook
var bot = new TelegramBot(token);
bot.setWebHook('YOUR WEBHOOK' + bot.token);


console.log('bot server started...');
// End bot setup

//Callback for wrapping bot.sendMessage
function sendMessageBack(id, message, options) {
    //console.log(id,message,options); //debug purposes
    bot.sendMessage(id, message, options);
}

// Set eng for the user
bot.onText(/\/eng/, function (msg, match) {
    translator.set_user_translation(msg.chat.id,'eng');
});

// Set spa for the user
bot.onText(/\/spa/, function (msg, match) {
    translator.set_user_translation(msg.chat.id,'spa');
});

// Matches word 
bot.onText(/(^[a-zA-Z\s*\,*]+)/, function(msg, match) {

    match[1].split(',').map(function(word)
    {
        translator.translate_using_mode(msg,word,sendMessageBack);       
    });

});

// Matches /eng [list,of,words]
bot.onText(/\/eng ([a-zA-Z\s*\,*]+)/, function (msg, match) {

    match[1].split(',').map(function(word)
    {
        translator.translate(msg,
            'http://www.wordreference.com/es/en/translation.asp?spen=' + word,
            sendMessageBack);       
    });

});

// Matches /spa [lista,de,palabras]
bot.onText(/\/spa ([a-zA-Z\s*\,*]+)/, function (msg, match) {

    match[1].split(',').map(function(word)
    {
        translator.translate(msg,
            'http://www.wordreference.com/es/translation.asp?tranword=' + word,
            sendMessageBack);       
    });
  
});

// Matches /help
bot.onText(/\/help/, function (msg, match) {

  var resp = '*Modo de uso - Usage mode:*\n' + 
            '/eng lista,de,palabras : Traduce al Inglés dla lista de palabras' +
                'separadas por comas\n' +
            '/spa list,of,words : Translate from English the list of words' + 
                'separated by commas\n' +
            '/help display this message\n' + 
            '*Short usage mode:*\n' +
            '/eng: Active mode spa -> eng\n' +
            '/spa: Activa el modo eng -> eng\n' +
            'word,to,search for: translate words using the active mode\n' +
            '*eng -> spa* is active by default\n';
  
    var options = {
        parse_mode : 'Markdown'
    };
  
  bot.sendMessage(msg.chat.id, resp, options);
});

module.exports = bot;