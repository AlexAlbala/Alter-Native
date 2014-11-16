var 
$ = require('jquery');
fs = require('fs');//helps with file system tasks
const path = './hidden/users/';

function checkUserFolder(folderName){
    if(fs.existsSync(path+folderName)){
        //aquí miramos cuanto tiempo falta para borrar la carpeta en el caso de que ya no se utilice
    }
    else{
        console.log(path+folderName+'/welcome.cs');
        fs.mkdirSync(path+folderName);
        console.log('creada');
        fs.writeFileSync(path+folderName+'/welcome.cs', fs.readFileSync('./hidden/default/welcome.cs'));
    }
}

function getDirList(dir) {
    var r = '<ul class="jqueryFileTree" style="display: none;">';
    try {
        r = '<ul class="jqueryFileTree" style="display: none;">';
        var files = fs.readdirSync(dir);
        files.forEach(function(f){
            var ff = dir + f;
            var show = 'uFile/' + f;
            var stats = fs.statSync(ff)
            if (stats.isDirectory()) { 
                r += '<li class="directory collapsed"><a href="#" rel="' + show  + '/">' + f + '</a></li>';
            } else {
                var e = f.split('.')[1];
                r += '<li class="file ext_' + e + '"><a href="#" rel='+ show + '>' + f + '</a></li>';
            }
        });
        r += '</ul>';
    } catch(e) {
        r += 'Could not load directory.\n'+e;
        r += '</ul>';
    }
    return r;
}

var _serve = function(req, res){
    pth='web/' + req.path;
    console.log(pth);
    checkUserFolder(req.sessionID);
    res.end(fs.readFileSync(pth));
};

var _uFile = function(req, res){
    console.log('1');
    pth=req.path.replace(/\/uFile/, path+ req.sessionID);
    console.log(pth);
    checkUserFolder(req.sessionID);
    res.end(fs.readFileSync(pth));
};

var _uLoad = function(req, res){
    console.log('1');
    pth=req.path.replace(/\/uFolder/, path+ req.sessionID +'/');
    console.log(pth);
    checkUserFolder(req.sessionID);
    res.send(getDirList(pth))
};

module.exports.serve = _serve;
module.exports.uFile = _uFile;
module.exports.uLoad = _uLoad;