
$('#input').focus();

$('#input').keypress(function(e) {

    if(e.which == 13) {
  	
        //Need to conver string to lowerCase && clear inputs
        var word = $('#input').val();
                word = word.toLowerCase(); 
        word = word.replace(/([^\w\sá-úñ])/g, "");

    	$('#input').val('');

    	if (word != null) {

	        $.ajax('https://wrefbot-aabreuglez.rhcloud.com/ddbb/word/' + word)
	        .done(function (data) {

                if(data) {
                    var expression = {
                        bold : /(\*)([\¡*\[*(a-zá-úñ)+\s*\:*\,\]**\!*]+)(\*)/gi,
                        cursive : /(\_\s)([\(*\¡*\[*(a-zá-úñ)+\s*\:*\-*\,*\]*\!*\)*]+)(\_)/gi,
                        newline : /\n/g,
                        hr : /\n-+\n/g
                    }

                    newstr = String(data.msg).replace(expression.bold,"<b>$2</b>");
                    newstr = newstr.replace(expression.hr, "<hr />");
                    newstr = newstr.replace(expression.cursive, "<i>$2</i>");
                    newstr = newstr.replace(expression.newline, "<br />");

    	        	$('#result').html(newstr);
                }
                else {
                    $('#result').html('I\'m sorry but <b>' + word + '</b> is not registered <b>yet</b>.<br/ >' + 
                        'Talk with <a href="https://telegram.me/wrefbot">@wrefbot</a> and register it!');
                }
	        });
        }
    }
});