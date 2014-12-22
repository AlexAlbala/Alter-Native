var
terminal = require('child_process').spawn('cmd');
fs = require('fs');//helps with file system tasks
const path = './hidden/users/';
nombre_programa='';
        
var _consola = function(req, res){
    if (fs.existsSync(__dirname + '/users/' + req.sessionID + '/log/out.log')){
        var txt=fs.readFileSync(__dirname + '/users/' + req.sessionID + '/log/out.log', {encoding:'utf8'});
        var fin='0';
        if(txt.match(/CMakeLists/)!=null){
            fin='1';
            require(__dirname+'/fileStorage.js').checkCompilation(req,res);  //backup
        }
        var rep = RegExp('\r\n.*' + req.sessionID + '.*\r\n', 'g');
        console.log(rep.toString());
        //var data = fin + txt.replace(rep, "!");
        var data = fin+txt.replace(rep, "\r\nFolders created\r\n").replace(/\r\n/g, '<br>');

        data += (fin == '1' ? '<br>Done.' : '');

        res.end(data);
    }
    else
        res.end('0');
}

var _compile = function(req, res){
    console.log('COMPILANDO');  
    
    /*terminal.stdout.on('data', function (data) {
        console.log('stdout: ' + data);
    });*/
    
    req.session.consola='';
    var fin=false;

    x = require('child_process').exec('cmd', function(error, stdout, stderr){
        console.log(stdout);
        res.end();
    });

    funciona = x.stdin;

    funciona.write('cd '+__dirname+'/users/'+req.sessionID+'\n');
    //compilamos con el terminal
    funciona.write('csc /optimize /out:code.exe /recurse:input\\*.cs\n');
    //el programa se llama code.cs

    //alternative
    funciona.write('cd C:/Alter-Native/\n');
    funciona.write('alternative-init.bat\n');
    funciona.write('cd '+__dirname+'/users/'+req.sessionID+'\n');
    
    //vaciamos el log
    require(__dirname+'/fileStorage.js').empty(__dirname + '/users/' + req.sessionID + '/log/');
    fs.mkdirSync(__dirname + '/users/' + req.sessionID + '/log');

    nombre_programa=fs.readdirSync(__dirname+'/users/'+req.sessionID+'/input');
    //funciona.write('alternative code.exe '+__dirname+'/users/'+req.sessionID+'/output/'+nombre_programa+'/\n');
    funciona.write('alternative code.exe '+__dirname+'/users/'+req.sessionID+'/output/'+nombre_programa+'/ > ' + __dirname + '/users/' + req.sessionID + '/log/out.log 2>> ' + __dirname + '/users/' + req.sessionID + '/log/err.log &\n');

    //funciona.write('ping 127.0.0.1 > ' + __dirname + '/users/' + req.sessionID + '/log/out.log 2>> ' + __dirname + '/users/' + req.sessionID + '/log/err.log &\n');

    //funciona.write('echo "finaldelmensaje" >> '+ __dirname + '/users/' + req.sessionID + '/log/out.log &\n');
    funciona.write('exit\n');
}

var _zip = function(req,res){
    var zip = new require('node-zip')();
    zip.folder(__dirname+'/users/'+req.sessionID+'/output');
    var data = zip.generate({base64:false,compression:'DEFLATE'});
    fs.writeFileSync('./AlterNative-out.zip', data, 'binary');
    
    var header = {
        "Content-Type": "application/x-zip",
        "Pragma": "public",
        "Expires": "0",
        "Cache-Control": "private, must-revalidate, post-check=0, pre-check=0",
        "Content-disposition": 'attachment; filename="AlterNative-out.zip"',
        "Transfer-Encoding": "chunked",
        "Content-Transfer-Encoding": "binary"
    };

    res.writeHead(200, header);
    res.end(data);
}

module.exports.compile = _compile;
module.exports.consola = _consola;
module.exports.zip = _zip;