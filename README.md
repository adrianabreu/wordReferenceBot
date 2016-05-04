# wordReferenceBot

This is a telegram bot made on Node.js for helping with translations.

I have trying to learn Englosh on my own. But I need to translate a lot of new words and with this bot, it's extremely easy :)

Since wordreference's api is no longer available, I'm doing web scraping using [request](https://github.com/request/request) + [cheerio](https://github.com/cheeriojs/cheerio).

## New features!

1. Short usage mode
2. Code separated into modules!
3. Better reg exp for matching messages

## Commands

```
Long usage mode:
/eng list,of,words: Translate TO English, the words separated by commas
/spa list,of,words: Translates TO Spanish, the words separated by commas
/help: Display help message

Short usage mode:
/spa: Activate mode eng -> spa
/eng: Activa mode spa-> eng
word,to,search for: translate words using the active mode
eng -> spa is active by default
```

## Features

This bot uses a webhook for better response time, it also uses 

## Run your own

I have deployed it to Heroku with almost no problem.

1. Get your own bot via [@BotFather](https://telegram.me/BotFather), make your own bot and get your token.
2. Place your token in bot.js file.
3. Create a heroku account and copy node's example.
4. Replace those files with these. 
5. Make a push to heroku master `git push heroku master`
6. Enjoy

## Working!

![bot chat](botworks.png)
![bot v2](botv2.png)

## Documentation

For more info of everything that has struggled me:

- [Problems crawling wordreference](http://stackoverflow.com/questions/34860760/problems-crawling-wordreference)
- [Configure a telegram bot webhook into an existing express app](http://mvalipour.github.io/node.js/2015/12/06/telegram-bot-webhook-existing-express/)

 
