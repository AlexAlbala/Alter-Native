var CODE;
var CODE2;
var path;

function openFile(file, folder){
    file2 = file.replace(/..\/Alter-Native/,"./uFolder");
    $.get(file2, function (data){
        if (folder=='input'){
            CODE.getDoc().setValue(data);
            $('.filename input').val(file.substring(file.lastIndexOf('/')+1));
            path=file;
        }
        else
            CODE2.getDoc().setValue(data);
    });
} 

function loadFolders1(){
    $('.folders').fileTree({ root: "./uFolder/input/", script: 'uFolder', folderEvent:'click'}, function(file) {
        openFile(file, 'input');
    });
}
function loadFolders2(){
    $('.folders2').fileTree({ root: "./uFolder/output/", script: 'uFolder', folderEvent:'click'}, function(file) {
        openFile(file, 'output');
    });
}

$('#rename').click(function(){
    var newname = $('.filename input').val() ;
    //alert("newname = "+ newname +" path = "+path);
    $.post("rename", {name:newname, dir:path}, function(data){
    	$.toaster({ priority : 'success', title : 'Success', message : 'Name changed!'});
    	//$('.folders').addClass('oldtree').removeClass('folders').hide().before($('<div>').addClass('folders').addClass('col-md-2').addClass('columna').addClass('demo'));
        loadFolders1();
    });
});

$('#save').click(function(){
    var txt =  CODE.getDoc().getValue();
    $.post("save", {text:txt, dir:path}, function(data){
        name=path.substring(path.lastIndexOf('/')+1);
    	$.toaster({ priority : 'success', title : 'Success', message : 'File "'+name+'" saved!'});
        //$('.folders').addClass('oldtree').removeClass('folders').hide().before($('<div>').addClass('folders').addClass('col-md-2').addClass('columna').addClass('demo'));
        //loadFolders1();
    });
});

$('#delete').click(function(){
    $.post("delete", {dir:path}, function(data){
    	$.toaster({ priority : 'danger', title : 'Success', message : 'File deleted!'});
    	CODE.getDoc().setValue('');
    	$('#filename').text('');
        $('.folders').addClass('oldtree').removeClass('folders').hide().before($('<div>').addClass('folders').addClass('col-md-2').addClass('columna').addClass('demo'));
        loadFolders1();
    });
});

document.consola = $('.win').dialog({title: 'Consola', autoOpen: false, width: 550, height: 250, resizable: false, close: function() {$(this).children('p').html('');}});

document.b = function (){
    $.get("consola", function(data){
        document.consola.children('p').html(data.substring(1));
        if(data.substring(0,1) == "0"){
            setTimeout(document.b, 250);
        }
    });
}

$('#btn-compile').click(function(){
   
    document.consola.dialog('open');
   
    document.recargar=true;
    setTimeout(document.b,250);
    $.get("compile-csharp", function(data) {
        //document.recargar=false;
        $('.folders2').addClass('oldtree').removeClass('folders2').hide().before($('<div>').addClass('folders2').addClass('col-md-2').addClass('columna').addClass('demo'));
        loadFolders2();
    });
    
});

$('#file').change(function(){

    alert(this.value);
});

$('#upload').fancybox({type:'iframe', width:500, height:100});

var codeConsole = document.getElementById("code");
CODE = CodeMirror.fromTextArea(codeConsole, {lineNumbers: true, readOnly: false, mode: "text/x-csharp"});

codeConsole = document.getElementById("code2");
CODE2 = CodeMirror.fromTextArea(codeConsole, {lineNumbers: true, readOnly: true, mode: "text/x-c++src"});

$(document).ready(function (){
    var funcionCargar = function(){
        nombre = $(this).attr('cargar');
        $.post("serve-test", {test:nombre}, function() {
            $.toaster({ priority : 'success', title : 'Success', message : 'Test successfully loaded!'});
            CODE.getDoc().setValue('');
            $('#filename').text('');
            $('.folders').addClass('oldtree').removeClass('folders').hide().before($('<div>').addClass('folders').addClass('col-md-2').addClass('columna').addClass('demo'));
            loadFolders1();
            $('.folders2').addClass('oldtree').removeClass('folders2').hide().before($('<div>').addClass('folders2').addClass('col-md-2').addClass('columna').addClass('demo'));
        });
    }
    $.post("load-tests", function(data) {
        $('#testList').html(data);
        $('[cargar]').off('click', funcionCargar).on('click', funcionCargar);
    });

    loadFolders1();
    loadFolders2();

    /*setInterval(function(){
        $('.folders2').addClass('oldtree').removeClass('folders2').hide().before($('<div>').addClass('folders2').addClass('col-md-2').addClass('columna').addClass('demo'));
        loadFolders2();
    }, 2000);*/
});

