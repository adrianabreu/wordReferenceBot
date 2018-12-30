# wordReferenceBot

## Table of Contents
1. [Description](#description)
2. [Major update!](#major-update)
3. [Commands](#commands)
4. [Wrefbot web](#wrefbot-web)
5. [Screenshots](#screenshots)
6. [Documentation](#documentation)

## Description

This is a telegram bot made with .Net Core for helping on translations.

I have trying to learn English on my own. But I need to translate a lot of new words and with this bot, it's extremely easy :)


## Commands

```
Long usage mode:
/eng list,of,words: Translate TO English, the words separated by commas
/spa list,of,words: Translates TO Spanish, the words separated by commas
/help: Display help message
```

## [Wrefbot web](http://wrefbot-wordreferencebot.7e14.starter-us-west-2.openshiftapps.com/)

## Screenshots

![bot chat](botworks.png)
![bot v2](botv2.png)


## Documentation

If you want to run the project locally, I recommend you to investigate how to register a bot on telegram.

After receiving your token, you can use ngrok for getting a valid https address (telegram needs it for its webhook):

You can launch ngrok with

`ngrok http 8443 -host-header="localhost:8443"`

Make a post request to the telegram api using the following params

`https://api.telegram.org/bot{TOKEN}/setWebhook?url={NGROK_URL}/api/translate`

Enjoy!