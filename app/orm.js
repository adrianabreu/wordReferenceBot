const mongoose = require('mongoose');
const config   = require('../config/config');
const logger   = require('./logger');
const socket   = require('./sockethandler');

module.exports = (function() {

	const Messages = require('./models/Messages');
 
	if (config.ddbb.url)
		mongoose.connect(config.ddbb.fullurl);
	else
		mongoose.connect(config.ddbb.localurl, config.ddbb.options);
    
	function register (word, msg, lang) {

		let message  = new Messages();

		message.name = word;
		message.msg  = msg;
		message.lang = lang;
		message.date = new Date();

		message.save()
		.then( () => {
			count( null, (trash, result) => {
				socket.emit(result);
			})
		})
		.catch( err => {
			logger.error(err) 
		});
	}

	function recover (word, callback, obj) {

		Messages.findOne({ name : word})
		.then( message => {
				(obj.senderf) ? callback(message, obj.msg, obj.lang, obj.senderf) 
				: callback(message, obj.msg);
		})
		.catch( err => {
			logger.error(err);
		});
	}

	function recoverAll (res) {

		Messages.find()
		.then( messages => {

			res.json(messages);

		}).catch( err => {

			logger.error(err);

		});
	}

	function count (res, callback) {
		
		Messages.count({})
		.then( count => {
			logger.info('From count ' + count);
			callback(res, count);

		})
		.catch( err => {

			logger.error(err);
			if (res != null)
				res.json(err);

		});		
	}

	function countByLang (res) {

		Messages.aggregate([ {
			$group: {
				_id: '$lang',
				count: {$sum: 1}
			}
		}], (result, err) => {
			if (err) {
				logger.error(err);
				res.send(err) 
			} else {
				res.json(result);
			}
		});

	}

	return {
		register : register,
		recover : recover,
		recoverAll : recoverAll,
		count : count,
		countByLang : countByLang
	}

})();

