var http = require('http');

var url = require('url');

function startServer(route){

    function createServer(req,res){

var pathname = url.parse(req.url).pathname;

console.log("pathname is",pathname);
route(pathname);


        res.writeHead(200,{"Content-Type":"text/plain"});
        res.write("Server Starts Here");
        res.end();
    }

    http.createServer(createServer).listen(8888);

}

module.exports.startServer = startServer;