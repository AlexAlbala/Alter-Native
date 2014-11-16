var CODE;
var CODE2;

function openFile(file){
    file2 = file.replace(/..\/Alter-Native/,"/uFolder");
    $.get(file2, function (data){
        CODE.getDoc().setValue(data);
    });
} 

$('#folders').fileTree({ root: "./", script: 'uFolder', folderEvent:'click' }, function(file) {
    openFile(file);
});

$('#btn-compile').click(function(){
    var txt =  CODE.getDoc().getValue();
    $.post("compile-csharp", {texto:txt});
});

openFile('/uFile/welcome.cs');

var codeConsole = document.getElementById("code");
CODE = CodeMirror.fromTextArea(codeConsole, {lineNumbers: true, readOnly: false, mode: "text/x-csharp"});

codeConsole = document.getElementById("code2");
CODE2 = CodeMirror.fromTextArea(codeConsole, {lineNumbers: true, readOnly: true, mode: "text/x-c++src"});