var 
$ = require('jquery');
fs = require('fs');//helps with file system tasks
const path = './hidden/users/';

function checkUserFolder(folderName){
    if(fs.existsSync(path+folderName)){
        //aquí miramos cuanto tiempo falta para borrar la carpeta en el caso de que ya no se utilice
    }
    else{
        if(!fs.existsSync(path)){
            fs.mkdirSync(path);
        }
        console.log(path+folderName+'/input/HelloWorld/Program.cs');
        fs.mkdirSync(path+folderName);
        fs.mkdirSync(path+folderName+'/input/');
        fs.mkdirSync(path+folderName+'/log/');
        //fs.mkdirSync(path+folderName+'/input/HelloWorld/');
        console.log('creada');
        serve("000.HelloWorld", path+folderName);
        //fs.writeFileSync(path+folderName+'/input/HelloWorld/Program.cs', fs.readFileSync('./hidden/default/000.HelloWorld/HelloWorld/Program.cs'));
    }
}

function getDirList(dir,rel) {
    var r = '<ul class="jqueryFileTree" style="display: none;">';
    try {
        r = '<ul class="jqueryFileTree" style="display: none;">';
        var files = fs.readdirSync(dir);
        files.forEach(function(f){
            var ff = dir + f;
            var stats = fs.statSync(ff)
            if (stats.isDirectory()) { 
                r += '<li class="directory collapsed"><a href="#" rel="./uFolder' + rel + f + '/">' + f + '</a></li>';
            } else {
                var e = f.split('.')[1];
                r += '<li class="file ext_' + e + '"><a href="#" rel="./uFile'+ rel + f + '">' + f + '</a></li>';
            }
        });
        r += '</ul>';
    } catch(e) {
        //r += 'Could not load directory.\n'+e;
        r += '</ul>';
    }
    return r;
}

var _uFile = function(req, res){
    console.log('uFile');
    
    //carpeta=req.query.carpeta;
    //console.log('carpeta: '+carpeta);
    
    pth=req.path.replace(/\.?\/?uFile/, path + req.sessionID);
    console.log(pth);
    checkUserFolder(req.sessionID);
    res.end(fs.readFileSync(decodeURIComponent(pth)));
};

var _uLoad = function(req, res){
    console.log('uLoad');

    //carpeta=req.query.carpeta;
    //console.log('carpeta: '+carpeta);
    
    checkUserFolder(req.sessionID);
    if (req.body.dir){
        console.log("dir:" +req.body.dir);
        pth=req.body.dir.replace(/\.\/uFolder/, path + req.sessionID + '/');
        var rel = req.body.dir.replace(/\.\/uFolder/, "");
        res.send(getDirList(decodeURIComponent(pth),decodeURIComponent(rel)));
    }
};

var _rename = function(req, res){
    pth=req.body.dir.replace(/\.?\/?uFile/, './' + path + req.sessionID + '/');
    oldname=pth.substring(pth.lastIndexOf('/')+1);
    name=req.body.name;
    console.log("oldname: "+oldname+" newname= "+name);
    fs.renameSync(pth, pth.replace(oldname, name));
    res.end(req.body.dir.replace(oldname, name));
};

var _save = function(req, res){
    text=req.body.text;
    pth=req.body.dir.replace(/\.?\/?uFile/, './' + path + req.sessionID + '/');
    fs.unlinkSync(pth);
    fs.writeFileSync(pth, text);
    res.end(req.body.dir);
};

var _delete = function(req, res){
    pth=req.body.dir.replace(/\.?\/?uFile/, './' + path + req.sessionID + '/');
    _empty(pth);
    res.end();
};

var _loadTests = function(req,res){
    var carpetas = fs.readdirSync('./hidden/default/');
    var codigo = '';
    carpetas.forEach(function (f){
        codigo += '<li role="presentation"><a role="menuitem" tabindex="-1" href="#" cargar="'+f+'" >'+f.substring(4)+'</a></li>';
    });
    res.end(codigo);
}

var recursiveCopy = function(src, dst){
    content = fs.readdirSync(src);
    content.forEach(function (f){
        if (fs.statSync(src+f).isDirectory()){
            if(!fs.existsSync(dst+f))
                fs.mkdirSync(dst+f);
            recursiveCopy(src+f+'/',dst+f+'/');
        }
        else
        {
            if(fs.existsSync(dst+f))
                fs.unlinkSync(dst+f);
            fs.writeFileSync(dst+f, fs.readFileSync(src+f));
        }
    });
}

var _empty = function(name){
    if (fs.existsSync(name)){
        if (fs.statSync(name).isDirectory()){
            content=fs.readdirSync(name);
            content.forEach(function (f){
                _empty(name+'/'+f);
            });
            fs.rmdirSync(name);
        }
        else
        {
            fs.unlinkSync(name);
        }
    }
}

var serve = function (test, pth){
    _empty(pth+'/input');
    _empty(pth+'/output');
    fs.mkdirSync(pth+'/input');
    recursiveCopy('./hidden/default/'+test+'/', pth+'/input/');
}

var _serveTest = function(req,res){
    test=req.body.test;
    serve(test, path+req.sessionID);
    res.end();
}

var _checkCompilation = function(req,res){
    var ncarpeta=fs.readdirSync(__dirname+'/users/'+req.sessionID+'/input/') + req.sessionID;
    var newDIR='';
    if (!fs.existsSync(__dirname+'/backup')){
        fs.mkdirSync(__dirname+'/backup');
    }
    if (!fs.existsSync(__dirname+'/backup/success-cs')){
        fs.mkdirSync(__dirname+'/backup/success-cs');
    }
    if (!fs.existsSync(__dirname+'/backup/success-cpp')){
        fs.mkdirSync(__dirname+'/backup/success-cpp');
    }
    if (!fs.existsSync(__dirname+'/backup/failure')){
        fs.mkdirSync(__dirname+'/backup/failure');
    }
    if (fs.existsSync(__dirname+'/users/'+req.sessionID+'/code.exe')){
        newDIR=__dirname+'/backup/success-cs/'+ncarpeta+'/';
        fs.mkdirSync(newDIR);
        recursiveCopy(__dirname+'/users/'+req.sessionID+'/input/', newDIR);
    }
    if (fs.existsSync(__dirname+'/users/'+req.sessionID+'/output')){
        newDIR=__dirname+'/backup/success-cpp/'+ncarpeta+'/';
        fs.mkdirSync(newDIR);
        recursiveCopy(__dirname+'/users/'+req.sessionID+'/output/', newDIR);
        
    }
    else{
        newDIR=__dirname+'/backup/failure/'+ncarpeta+'/';
        fs.mkdirSync(newDIR);
        recursiveCopy(__dirname+'/users/'+req.sessionID+'/input/', newDIR);
    }
    fs.writeFileSync(newDIR + '/userinfo.txt' , req.sessionID);
    fs.mkdirSync(newDIR + '/log');
    recursiveCopy(__dirname+'/users/'+req.sessionID+'/log/', newDIR +'/log/');
}

module.exports.uFile = _uFile;
module.exports.uLoad = _uLoad;
module.exports.rename = _rename;
module.exports.save = _save;
module.exports.delete = _delete;
module.exports.loadTests = _loadTests;
module.exports.serveTest = _serveTest;
module.exports.checkCompilation = _checkCompilation;
module.exports.empty = _empty;