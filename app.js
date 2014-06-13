//step 1) require the modules we need
var
http = require('http'),//helps with http methods
path = require('path'),//helps with file paths
fs = require('fs');//helps with file system tasks
 
//a helper function to handle HTTP requests
function requestHandler(req, res) {
    var
    content = '',
    fileName = path.basename(req.url),//the file that was requested
    localFolder = __dirname + '/web/';//where our public files are located
 
    //NOTE: __dirname returns the root folder that this javascript file is in.
 
    if(fileName === 'index2.html' || fileName === ""){//if index.html was requested...
        content = localFolder + "index2.html";//setup the file name to be returned
 
        //reads the file referenced by 'content' and then calls the anonymous function we pass in
        fs.readFile(content,function(err,contents){
            //if the fileRead was successful...
            if(!err){
                //send the contents of index.html and then close the request
                res.end(contents);
            } else {
                //otherwise, let us inspect the error n the console
                console.dir(err);
            };
        });
    } 
    else {
        console.log("PATH: " + req.url)
         content = path.dirname(localFolder + req.url) + path.sep + path.basename(req.url);//setup the file name to be returned

         //content = path.relative(localFolder, content);
         console.log(content);
         console.log("file: " + fileName);
         console.log("localFolder: " + localFolder);

         //console.log("relative", path.relative(localFolder, ))
    
        //reads the file referenced by 'content' and then calls the anonymous function we pass in
        fs.readFile(content,function(err,contents){
            //if the fileRead was successful...
            if(!err){
                //send the contents of index.html and then close the request
                res.end(contents);
            } else {
                //otherwise, let us inspect the error n the console
                console.error(err);
                res.statusCode = 404;                
                res.end();
            };
        });
    };
};
 
//step 2) create the server
http.createServer(requestHandler)
 
//step 3) listen for an HTTP request on port 3000
.listen(3000);