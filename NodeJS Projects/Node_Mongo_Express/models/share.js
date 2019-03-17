const mongoose = require('mongoose');

const Schema = mongoose.Schema;

var shareSchema = new Schema({

    shareName : {

        type:String,
    required : true
    },
    shareCode : {

        type:String,
            },
    price:{

        type:Number,
        required : true
    },
    action:String,

    shareType:String,
    holdings: {
        type: Number,
        required : true

    },
    date:Date,
    done:{
        type:Boolean,
        default:false

    }

});

module.exports = mongoose.model('Share',shareSchema);