const request  = require('request');
const cheerio  = require('cheerio');
const orm      = require('./orm');
const socket   = require('./sockethandler');

module.exports = (function() {

	//user-id : translation mode
	let users_translation_mode = {};

	const default_mode = 'spa';

	const translations = {
		'eng' : 'http://www.wordreference.com/es/en/translation.asp?spen=',
		'spa' : 'http://www.wordreference.com/es/translation.asp?tranword='
	}

	const Parser = {
		is_a_title : function (string, $)
		{
			aux = false;
			possibleTitles = ["Principal Translations",
							  "Additional Translations", "Compound Forms"];

			for (let i in possibleTitles) {
				if (string == possibleTitles[i])
					aux = true;
			}

			return aux;
		},
		parse_from_word : function (obj, $)
		{
			let message = '';

			let cell =  $('<textarea />').html($(obj).children('strong')
										 .html()).text();
			message += '*' + cell + '* ';

			//next td
			cell =  $('<textarea />').html($(obj).next('td').html()).text();
			message += '_' + cell +'_' + '\n ';

			return message;
		},

		parse_to_word : function (obj, $)
		{
			//There is an em child on that cell that
			// has additional info that we do not want to show
			let cell =  $('<textarea />').html($(obj).clone()
			    .children() //select all the children
			    .remove()   //remove all the children
			    .end().html()).text();

			let message = cell + '\n ';

			return message;
		},

		parse_title : function (obj, $)
		{
			let message = '\n----------------------------------\n';
			message +=  $(obj).attr('title');
			message += '\n----------------------------------\n ';
			return message;
		},

		parse_translation : function ($, word_being_searched)
		{
			let message ='';
			let translation_found = false;

			/*We need to extract all the rows*/
			$(this).children('tr').each(function(){
			/*We want to separate the cells for output*/
				if ( !($(this).hasClass('langHeader') ) ) {

				        translation_found = true;
				        $(this).children('td').each(function() {

				            //From the original we need to parse
				            //the word + next td (meaning)
				            if ($(this).hasClass('FrWrd')) {
				            	message += Parser.parse_from_word(this, $);
				            }
				            if ($(this).hasClass('ToWrd')) {
				            	message += Parser.parse_to_word(this, $);
				            } else {
				                if (Parser.is_a_title($(this).attr('title'))) {
				                     message += Parser.parse_title(this, $);
				                }
				            }
				    });
				}
			});

			if(!translation_found)
				message = '*There is no translation for ' + word_being_searched
							+ '*';
			return message;
		},

		parse_request : function (msg, word_being_searched, lang,
								  sender_function, error, response, body)
		{

			if(!error && response.statusCode == 200){
				let $ = cheerio.load(body);

				let message = Parser.parse_translation
						.call( $('table.WRD').first(), $, word_being_searched);


				//Message back
				const fromId = msg.chat.id;
				const options = {
				    parse_mode : 'Markdown'
				};

				sender_function(fromId, message, options);
				
				//Time to store the message
				orm.register(word_being_searched, message, lang);
			}

		}
	};
	function translate(msg, lang, word, sender_function)
	{
		//First we should look in the database
		const word_being_searched = word;
		const destiny = translations[lang] + word;

		orm.recover(word_being_searched, callback,
			{ 'msg' : msg,
			  'senderf' : sender_function,
			  'lang' : lang
			});

		function callback(message, msg, lang, sender_function) {

			if (message != null) {
				//Message back
				const fromId = msg.chat.id;
				const options = {
				    parse_mode : 'Markdown'
				};
				sender_function(fromId, message.msg, options);
				
			} else {

			    //We need to send an user-agent for get wordreference's answer
			    const options = {
			      url: destiny,
			      headers: {
			        'User-Agent': 'request'
			      },
			      enconding: 'ascii',
			      timeout: 500
			    };
			    //Inject msg and sender_function
			    request(options, Parser.parse_request.bind(null, msg,
			    										   word_being_searched,
			    										   lang,
			    										   sender_function));
				}
		}

	}

	function translate_using_mode(msg, word, sender_function)
	{
		if(!users_translation_mode[msg.chat.id])
			users_translation_mode[msg.chat.id] = default_mode;

		translate(msg, translations[users_translation_mode[msg.chat.id]],
				  word, sender_function);
	}

	function set_user_translation(id, mode)
	{
		users_translation_mode[id] = translations[mode];
	}

	return {
		translate : translate,
		translate_using_mode : translate_using_mode,
		set_user_translation: set_user_translation
	}

})();
