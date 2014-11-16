var
terminal = require('child_process').spawn('cmd');
const path = './hidden/users/';
        
var _compile = function(req, res){
    console.log('COMPILANDO');  
    terminal.stdout.on('data', function (data) {
        console.log('stdout: ' + data);
    });

    var msg = req.param('texto');
    fs.writeFileSync('./hidden/users/'+req.sessionID+"/code.cs", msg); 

    terminal.stdin.write('cd '+__dirname+'/users/'+req.sessionID+'\n');
    //compilamos con el terminal
    terminal.stdin.write('csc code.cs\n');

    //ejecutamos el programa
    terminal.stdin.write('code.exe\n');

    //terminal.stdin.end();
    res.end();
}

module.exports.compile = _compile;