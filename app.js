//step 1) require the modules we need
var
http = require('http'),//helps with http methods
path = require('path'),//helps with file paths
fs = require('fs');//helps with file system tasks
express = require('express');
session = require('express-session');
//routes = require('./routes');

app = express();

var bodyParser = require('body-parser')
app.use(bodyParser.json());       // to support JSON-encoded bodies
app.use(bodyParser.urlencoded()); // to support URL-encoded bodies
app.use(session({name: "uCookie", secret: "test", resave: true, saveUninitialized: true}));

app.use(express.static(__dirname + '/web'));
app.use(express.static(__dirname + '/temp'));

app.post('/getDir', require(__dirname + '/web/lib/jquery/jqueryFileTreeConnector.js').getDirList);
app.post('/compile-csharp', require(__dirname + '/web/lib/compiler.js').compile);

app.get(/\/file/, require(__dirname + '/web/lib/serve.js').serve);
 
//step 2) create the server
http.createServer(app)
//step 3) listen for an HTTP request on port 3000
.listen(3000);
console.log("server working at localhost:3000");