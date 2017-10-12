const express = require('express');
const bodyParser = require('body-parser');
const app = express();
const socket = require('./sockethandler');
const logger = require('./logger');

//App config
app.use(bodyParser.json());
app.set('view engine', 'ejs');
app.set('views', __dirname + '/../views');
app.use(express.static(__dirname + '/../public'));

//Start server and socket
var server = app.listen(process.env.PORT, process.env.HOST, function () {
	logger.info('Web server started at http://%s:%s', process.env.HOST, process.env.PORT);
});

socket.createServer(server);

//Set up routes
require('./routes')(app);

module.exports = function (bot) {

	//Register bot petitions
	app.post('/bot' + bot.token, function (req, res) {
		bot.processUpdate(req.body);
		res.sendStatus(200);
	});
};