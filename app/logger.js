const winston  = require('winston');

module.exports = (function() {

    const logger = new (winston.Logger)({
        transports: [
            new (winston.transports.Console)({
                timestamp: function() {
                    const d = new Date();
                    return d.toUTCString();
                },
            formatter : function(options) {
                return options.timestamp() + ' ' + options.level.toUpperCase() + ' '
                +  (undefined !== options.message ? options.message : '');
                }
            })
        ]
    });

    return logger;
})();