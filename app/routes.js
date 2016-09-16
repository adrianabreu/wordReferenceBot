
const orm    = require('./orm');
const logger = require('./logger');


module.exports = function(app) {

	//Redirect http to https
	app.all('*', (req, res, next) => {

	if (!req.secure && (req.get('X-Forwarded-Proto') !== 'https'))
		res.redirect(301,"https://" + req.headers.host + req.url);
	else
		next();
	});

	app.get('/', (req, res) => {

	  orm.count(res, (res, count) => {
	  	res.render('index', { totalwords : count });
	  });

	});

	app.get('/parser', (req, res) => {

	  res.render('parser');

	});

	app.get('/stats', (req, res) => {

	  res.render('stats');

	});

	app.get('/ddbb/word/:wordname', (req, res) => {

		orm.recover(req.params.wordname, function(translation, res) {
	    		res.json(translation);
			}, { 'msg' : res  }
		); 
	
	});

	app.get('/ddbb/stats', (req, res) => {
		
		orm.countByLang(res);
		
	});

}