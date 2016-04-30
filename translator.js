var request = require('request');
var cheerio = require('cheerio');

var translator = (function() {

	var mode = 'eng';

	var translations = {
		'eng' : 'http://www.wordreference.com/es/en/translation.asp?spen=',
		'spa' : 'http://www.wordreference.com/es/translation.asp?tranword='
	}

	function isATitle(string, $)
	{
		aux = false;
		possibleTitles = ["Principal Translations", "Additional Translations",
		 "Compound Forms"];

		for (var i in possibleTitles) {
			if (string == possibleTitles[i]) 
				aux = true;
		}

		return aux;
	}

	function parese_from_word(obj, $)
	{
		var message = '';

		var cell =  $('<textarea />').html($(obj).children('strong')
									 .html()).text();
		message += '*' + cell + '* ';

		//next td
		cell =  $('<textarea />').html($(obj).next('td').html()).text();

		message += '_'+cell+'_' + '\n';

		return message;
	}

	function parse_to_word (obj, $)
	{
        //There is an em child on that cell that
        // has additional info that we do not want to show
        var message = '';
        var cell =  $('<textarea />').html($(obj).clone()
            .children() //select all the children
            .remove()   //remove all the children
            .end().html()).text();
        
        message += cell;
        message += '\n'; 

        return message;
	}

	function parse_title(obj, $)
	{
		var message = '';
        message += '\n----------------------------------\n';
        message +=  $(obj).attr('title');
        message += '\n----------------------------------\n';  
        return message;  
	}

	function parse_translation($, word_being_searched)
	{
        var message ='';
        var translation_found = false;
 
	    /*We need to extract all the rows*/
	    $(this).children('tr').each(function(){
	        /*We want to separate the cells for output*/
	        if ( ($(this).attr('class') != "langHeader") ) {
	                console.log("There is translation!");
	                translation_found = true;
	                $(this).children('td').each(function() {
	                    
	                    //From the original we need to parse 
	                    //the word + next td (meaning)
	                    if ($(this).attr('class') == "FrWrd") {
	                    	message += parese_from_word(this, $);
	                    }
	                    if ($(this).attr('class') == "ToWrd") {
	                    	message += parse_to_word(this, $);
	                    } else {
	                        if (isATitle($(this).attr('title'))) {
	                             message += parse_title(this, $);
	                        } 
	                    }
	            });
	        }            
	    });
	    if(!translation_found)
	        message = '*There is no translation for ' + word_being_searched 
	    			+ '*';
	    console.log("I will return this message", message);
	    return message;

	}

	function parse_request(msg, word_being_searched, sender_function, error,
						   response, body)
	{

		if(!error && response.statusCode==200){
            var $ = cheerio.load(body);

            var message = parse_translation.call( $('table.WRD').first(), $,
            										word_being_searched);
            
            //Message back
            var fromId = msg.chat.id;
            var options = {
                parse_mode : 'Markdown'
            };

            sender_function(fromId, message, options);
        }

	}

	function translate(msg, destiny, sender_function)
	{

	    //We need to send an user-agent for get wordreference's answer
	    var options = {
	      url: destiny,
	      headers: {
	        'User-Agent': 'request'
	      },
	      enconding: 'ascii'
	    };

	    var word_being_searched = destiny.split('=')[1];
	    //Inject msg and sender_function
	    request(options, parse_request.bind(null,msg,word_being_searched,
	    									sender_function)); 
	    
	    
	}

	return {
		translate : translate
	}

})();

module.exports = translator;