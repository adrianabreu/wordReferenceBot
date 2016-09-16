const express     = require('express');
const bodyParser  = require('body-parser');
const app         = express();
const socket      = require('./sockethandler');
const logger      = require('./logger');
const config      = require('../config/config');

//App config
app.use(bodyParser.json());
app.set('view engine', 'ejs');
app.set('views', __dirname + '/../views');
app.use(express.static(__dirname + '/../public'));

//Start server and socket
var server = app.listen(config.port, config.host, function () {
  logger.info('Web server started at http://%s:%s', config.host, config.port);
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