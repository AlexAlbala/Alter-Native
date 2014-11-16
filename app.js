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

if (!fs.existsSync(__dirname+'/hidden/users')) fs.mkdirSync(__dirname+'/hidden/users');

//app.post('/getDir', require(__dirname + '/hidden/jqueryFileTreeConnector.js').getDirList);
app.post('/compile-csharp', require(__dirname + '/hidden/compiler.js').compile);
app.post(/\/uFolder/, require(__dirname + '/hidden/serve.js').uLoad);

app.get(/\/uFile/, require(__dirname + '/hidden/serve.js').uFile);
app.get(/\/file/, require(__dirname + '/hidden/serve.js').serve);
 
app.use(express.static(__dirname + '/web'));

//step 2) create the server
http.createServer(app)
//step 3) listen for an HTTP request on port 3000
.listen(3000);
console.log("server working at localhost:3000");
