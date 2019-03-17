const router = require('express').Router();
const _ = require('underscore');

const Client = require('node-rest-client').Client;
var client = new Client();

const Share =require('../models/share');  /*to use mongoose schema*/

var liveSharePrices =[
    {
        "shareName":"Ethereum",
        "shareCode":"ETH",
        "shareType":"Crypto",
        "livePrice":830

    },
    {
        "shareName":"Bitcoin",
        "shareCode":"BTC",
        "shareType":"Crypto",
        "livePrice":8030
    },
    {
        "shareName":"Bitcoin-Cash",
        "shareCode":"BCH",
        "shareType":"Crypto",
        "livePrice":1000
    },
    {
        "shareName": "Litecoin",
        "shareCode": "LTC",
        "shareType": "Crypto",
        "livePrice": 150
    },
    {
        "shareName":"Ford",
        "shareCode":"F",
        "shareType":"Share",
        "livePrice":10.85
    },
    {
        "shareName":"Sprint",
        "shareCode":"S",
        "shareType":"Share",
        "livePrice":5.44
    },
    {
        "shareName":"Apple",
        "shareCode":"AAPL",
        "shareType":"Share",
        "livePrice":161
    },
    {
        "shareName":"Tesla",
        "shareCode":"TSLA",
        "shareType":"Share",
        "livePrice":342.50
    }

];
router.get('https://api.coinmarketcap.com/v1/ticker/', function(req,resp,next){

resp.send()
    // console.log(data);
    // console.log(resp);

    // resp.render('index',{liveData:data});
});

router.get('/livePrices', function(req,res){

res.send("Hi live prices");

});


router.get('/',function(req,res){

    Share.find({}).then(function(results){
const shares = results.filter(function(share){

    return !share.done;
});
//Calculating profit/Loss
        const holdingShareNames =[];
        const totalShareHoldings =[];
//getting Total Holdings of each Share
 shares.forEach(function(value){

     var totalPrice = value.price* value.holdings;
     const y =_.contains(holdingShareNames,value.shareName); //finds if the value exists in array or not
     const livePrice = getLivePrice(liveSharePrices,value.shareName);
     if(y==false)
     {
         const obj ={
             "shareName":value.shareName,
             "totalPriceUSD":totalPrice,
             "totalHoldings":value.holdings,
             "currentLivePrice":livePrice,
             "profit/loss":"",
             "profit/loss%":""
         };
         holdingShareNames.push(value.shareName);
         totalShareHoldings.push(obj);
     }
     if(y==true)
     {
         for (var i in totalShareHoldings){

             if (totalShareHoldings[i].shareName ==value.shareName){

                 totalShareHoldings[i].totalPriceUSD += totalPrice;
                 totalShareHoldings[i].totalHoldings +=value.holdings;
             }

         }

     }



 });
        console.log(holdingShareNames);
        console.log(totalShareHoldings);


var doneShares = results.filter(function(share){

    return share.done;
});



        //profit or Loss Percentage Calculation

        totalShareHoldings.forEach(function (value, index) {

            const livePrice = getLivePrice(liveSharePrices,value.shareName);


            const profitOrLoss = (livePrice*value.totalHoldings) -(value.totalPriceUSD*value.totalHoldings);
            console.log(livePrice+" "+ value.shareName+" "+profitOrLoss);
        });


        //Getting live prices from LiveSharePrices Array
        function getLivePrice(array, shareName){

            for(var i in array)
            {
                if (array[i].shareName==shareName || array[i].shareCode ==shareName)
                {

                    return array[i].livePrice;
                }

            }
        }

// function findProfitOrLoss(liveprice,ttlShldngs){
//
//             for (var i in ttlShldngs)
//             {
//
//
//             }
// }

    res.render('index',{shares:shares,doneShares:doneShares,totalShareHoldings:totalShareHoldings}); /* object that has property of shares and taking results*/
       // res.render('d3script',{shares:shares,doneShares:doneShares});

    });
   });



router.post('/livePrices',function(req,res){



    res.redirect('https://api.coinmarketcap.com/v1/ticker/');
    // console.log(data);

});


router.post('/shares',function(req,res){

   //  /*Array to store*/
   //  const _shareName = req.body.shareName;
   //  const _price = req.body.price;
   //  const _holdings = req.body.holdings;
   //  const _date = req.body.date;
   //
   //  var obj ={shareName:_shareName,price:_price,holdings:_holdings,dat:_date};
   //  shares.push(obj);
   // res.json(shares);

    /* To Use Mongoose Model */
    var newShare = new Share({

        shareName: req.body.shareName,
        price : req.body.price,
        shareType : req.body.shareType,
        action: req.body.action,
        holdings : req.body.holdings,
        date : req.body.date
    });

    newShare.save().then(function(result){
           console.log(result);
           res.redirect('/');
        })
        .catch(function(err){        /* Used to catch error while posting  */
        console.log(err);
        res.redirect('/');

    });

});

router.post('/shares/:id/completed',function(req,res){

    var shareId = req.params.id;

    Share.findById(shareId)
        .exec()
        .then(function(result){

            result.done = !result.done;
            return result.save();
        })
        .then(function(result){

    });
    res.redirect('/');
});

module.exports = router;