var express = require('express');
var engines = require('consolidate');
var mongoDb = require('mongodb').MongoClient;
var bodyParser = require('body-parser');

var app  = express();
app.use(bodyParser.json);
app.use(bodyParser.urlencoded({extended:true}));

app.engine('html',engines.nunjucks);
app.set('view engine','html');
app.set('views',__dirname +'/views');


app.get('/',function(req,res){

mongoDb.connect('mongodb://localhost:27017/shares',function(err,db){
if(err)
{
    console.log(err);

}
    else

        db.collection('shares').find().toArray(function(err,docs){

res.render('index',{'shares':docs});

        })
})

});

app.post('/add',function(req,res){
    var to_coinName = req.body.coinName;
    var to_coinPrice = req.body.coinPrice;
    var to_holdings = req.body.holdings;
    var to_date = req.body.date;

var obj = {'coinName':to_coinName,'coinPrice':to_coinPrice,'holdings':to_holdings,'date':to_date};
mongodb.connect('mongodb://localhost:27017/shares',function(err,db){

    if(err)
    {
        console.log(err);
    }
    else {
        db.collection('shares'.insertOne(obj));
        res.redirect('/');
    }
})
})

app.listen(9000,function(){

    console.log("on 9000");
})
