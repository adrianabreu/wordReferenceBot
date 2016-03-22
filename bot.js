#!/bin/env node
var TelegramBot = require('node-telegram-bot-api');
var request = require('request');
var cheerio = require('cheerio');

/* Bot setup */
var token = '218594964:AAFv56r5Sn71KmFMMtOfKOVjyzhKip7wp7M'; //We read the token from a gitignored file

// Setup polling way
var bot = new TelegramBot(token);
bot.setWebHook('https://agile-shore-43390.herokuapp.com/' + bot.token);


console.log('bot server started...');
/* End bot setup */

function isATitle(string) {
    aux = false;
    possibleTitles = ["Principal Translations", "Additional Translations",
    "Compound Forms"];

    for (var i in possibleTitles) {
        if (string == possibleTitles[i]) 
            aux = true;
    }

    return aux;
}

function translate(msg, destiny) {
    //We need to send an user-agent for get wordreference's answer
    var options = {
      url: destiny,
      headers: {
        'User-Agent': 'request'
      },
      enconding: 'ascii'
    };

    function callback(error, response, body){
        if(!error && response.statusCode==200){
            var $ = cheerio.load(body);
            var message='';

            (function(){
                /*We need to extract all the rows*/
                $(this).children('tr').each(function(){
                    /*We want to separate the cells for output*/
                    if ( ($(this).attr('class') != "langHeader") && ($(this).attr('class') != "odd") ) {
                            $(this).children('td').each(function() {
                                
                                //From the original we need to parse the word + next td (meaning)
                                if ($(this).attr('class') == "FrWrd") {
                                
                                    cell =  $('<textarea />').html($(this).children('strong').html()).text();
                                    message += '*' + cell + '* ';
                                    //next td
                                    cell =  $('<textarea />').html($(this).next('td').html()).text();
                                    message += '_'+cell+'_' + '\n';
                                }
                                if ($(this).attr('class') == "ToWrd") {
                                    //There is an em child on row that has additional info that we do not want to show
                                    cell =  $('<textarea />').html($(this).clone()
                                        .children() //select all the children
                                        .remove()   //remove all the children
                                        .end().html()).text();
                                    message += cell;
                                    message += '\n'; 
                                   
                                } else {
                                    if (isATitle($(this).attr('title'))) {
                                        message += '\n----------------------------------\n';
                                        message +=  $(this).attr('title');
                                        message += '\n----------------------------------\n';                                         
                                    } 
                                }
                        });
                    }            
                });
                
            }).call($('table.WRD').first());

            //Message back
            var fromId = msg.chat.id;
            var options = {
                parse_mode : 'Markdown'
            };

            bot.sendMessage(fromId, message, options);
        }
    }request(options, callback.bind(this)); //We need to inject msg
}



//Matches /echo [whatever]
bot.onText(/\/eng (.+)/, function (msg, match) {
  var wordsToSearch = match[1].split(','); //Array with words to traduce
  for (word in wordsToSearch) {
    translate(msg, 'http://www.wordreference.com/es/en/translation.asp?spen=' + wordsToSearch[word]);
  }
});

bot.onText(/\/spa (.+)/, function (msg, match) {
  var wordsToSearch = match[1].split(','); //Array with words to traduce
  for (word in wordsToSearch) {
    translate(msg, 'http://www.wordreference.com/es/translation.asp?tranword=' + wordsToSearch[word]);
  }
  
});

bot.onText(/\/help/, function (msg, match) {
  var fromId = msg.chat.id;
  var resp = 'Modo de uso - Usage mode: \n' + 
            '/eng lista,de,palabras : Traduce al Inglés la lista de palabras separadas por comas\n' +
            '/spa list,of,words : Translate from English the list of words separated by commas\n' +
            '/help display this message';
  bot.sendMessage(fromId, resp);
});

module.exports = bot;