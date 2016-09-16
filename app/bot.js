#!/bin/env node
const TelegramBot = require('node-telegram-bot-api');
const translator  = require('./translator');
const config      = require('../config/config');
const logger      = require('./logger');
const bot         = new TelegramBot(config.token);

logger.info('bot server started...');

/* --------------------------------------------
 * HELPERS
 * --------------------------------------------*/

function sendMessageBack(id, message, options) {
    bot.sendMessage(id, message, options);
}

function wrapTranslation(match, msg, lang) {

    match.split(',').map(function(word)
    {
        translator.translate(msg,
            lang, 
            word,
            sendMessageBack);       
    });
}

/* --------------------------------------------
 *   COMMANDS
 ^ --------------------------------------------*/

//Commands for single user mode set translation mode for them
bot.onText(/\/eng$/, function (msg, match) {
    translator.set_user_translation(msg.chat.id,'eng');
});

bot.onText(/\/spa$/, function (msg, match) {

    translator.set_user_translation(msg.chat.id,'spa');

});


// Matches word 
bot.onText(/(^[a-zA-Zá-úñ\s*\,*]+)/, function(msg, match) {

    match[1].split(',').map(function(word)
    {
        translator.translate_using_mode(msg,word,sendMessageBack);       
    });

});

// Matches /eng [list,of,words]
bot.onText(/\/eng ([a-zA-Zá-úñ\s*\,*]+)/, function (msg, match) {

    if(match[1]) {
        wrapTranslation(match[1], msg, 'eng');
    }

});

//Matches /eng@wrefbot [list,of,words]
bot.onText(/\/eng@wrefbot ([a-zA-Zá-úñ\s*\,*]+)/, function (msg, match) {

    if(match[1]) {
        wrapTranslation(match[1], msg, 'eng');
    }
});

// Matches /spa [lista,de,palabras]
bot.onText(/\/spa ([a-zA-Zá-úñ\s*\,*]+)/, function (msg, match) {

    if(match[1]) {
        wrapTranslation(match[1], msg, 'spa');
    }
});

// Matches /spa@wrefbot [lista,de,palabras]
bot.onText(/\/spa@wrefbot ([a-zA-Zá-úñ\s*\,*]+)/, function (msg, match) {
    if(match[1]) {
        wrapTranslation(match[1], msg, 'spa');
    }  
});

// Matches /help or /start
bot.onText(/\/help|\/start/, function (msg, match) {

    const resp = '*Modo de uso - Usage mode:*\n' + 
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
  
    const options = {
        parse_mode : 'Markdown'
    };
  
  bot.sendMessage(msg.chat.id, resp, options);
});

module.exports = bot;