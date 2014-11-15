var 
$ = require('jquery');
fs = require('fs');//helps with file system tasks

var _serve = function(req, res){	
    var us = req.session;
	pth=req.path.replace(/\/file/, '');
	console.log(pth);
	
//	if (pth=="/temp/newfile.txt"){
//		us.carpeta="nombre";
//	}else{
//		res.send(app.get('/'));
//	}
    res.end(fs.readFileSync('./' + pth));

    /*fs.readFileSync('./' + pth,function(err,contents){
        if(!err){
            res.end(contents);
        } else {
            //otherwise, let us inspect the error n the console
            console.dir(err);
        };
    });

	//res.send(pth);*/
};

module.exports.serve = _serve;