var mongoose     = require('mongoose');
var Schema       = mongoose.Schema;

var MessagesSchema  = new Schema({
    name: String,
    msg: String,
    lang : String,
    date : Date
});

module.exports = mongoose.model('Messages', MessagesSchema);