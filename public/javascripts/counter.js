var socket = io.connect('wss://wrefbot-aabreuglez.rhcloud.com:8443');
  
socket.on('counter', function (data) {
	document.getElementById('totalWords').innerHTML = data;
});