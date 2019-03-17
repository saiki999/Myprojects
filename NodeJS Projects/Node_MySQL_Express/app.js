const express= require('express');
const mysql = require('mysql');

//create connection

const dbConnection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: ''

});


const app = express();


app.listen('3000',function(){

   console.log("Server started at port 3000");
});