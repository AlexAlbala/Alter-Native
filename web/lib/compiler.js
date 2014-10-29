var terminal = require('child_process').spawn('cmd');
        
var _compile = function(req, res){
    console.log('COMPILANDO');  
    terminal.stdout.on('data', function (data) {
        console.log('stdout: ' + data);
    });

    //leemos el post y lo guardamos en un archivo
    var msg;
    req.on('data', function(chunk) {
      msg=chunk.toString();
    });

    req.on('end', function() {
        fs.writeFile("code.cs", msg, function(err) {
            if(err) {
                console.log(err);
            } else {
               console.log("The file was saved!");
            }
        }); 
    });

    //compilamos con el terminal
    terminal.stdin.write('csc code.cs\n');

    //ejecutamos el programa
    terminal.stdin.write('code.exe\n');

    terminal.stdin.end();
    res.end();
}

module.exports.compile = _compile;