const  express = require('express');
const mustacheExpress = require('mustache-express');
const bodyParser = require('body-parser');
const mongoose = require('mongoose');
const routes = require('./routes/routes');

// mongoose.Promise = global.Promise;

//Connecting to Mongoose

mongoose.connect('mongodb://localhost:27017/mongoose_express_shares').then(function(err){

    console.log("MongooDb Connected");
}).catch(function(err){

    console.log(err);
});

// mongoose.connect('mongodb:')


const app = express();
// app.use(bodyParser.json);
app.use(bodyParser.urlencoded({extended:true}));

//setting up view engine

const mustacheExpressInstance = mustacheExpress();
mustacheExpressInstance.cache = null;

app.engine('mustache',mustacheExpressInstance);//tell mustache is our view engine
app.set('view engine','mustache');// any file with mustache extension will be considered as template by view engine.
app.set('views', __dirname +'/views');
//
// var shares =[
//     {
//         shareName:'Ford',
//         price:14,
//         holdings:10,
//         date:'12/27/2017'
//     },
//     {
//         shareName: 'Apple',
//         price: 277,
//         holdings:10,
//         date:'1/27/2018'
//     }
// ];



app.use('/',routes);

app.listen(8080,function(err){

    if(err)
{
    console.log(err);

}
    console.log('listening to port 8080');
});