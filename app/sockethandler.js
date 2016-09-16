const orm = require('./orm');

module.exports = (function() {

	let io;
	let counter;

	function createServer(server) {

		io = require('socket.io')(server);

	}

	function emit(data) {
		io.emit('counter', data);
	}

	function getSocket() {
		return io;
	}

	function updateAndEmit() {
		if (counter) {
			counter++;
			emit(counter);
 		} else {
			orm.count(null, (trash, result) => {
				counter = result;
				emit(counter);
			});
		}

	}
	return {
		createServer : createServer,
		emit : emit,
		getSocket : getSocket,
		updateAndEmit : updateAndEmit
	}

})();