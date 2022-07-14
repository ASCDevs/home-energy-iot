var nomeConexao = prompt("Dê um nome para a conexão. ");
var socket = new WebSocket("wss://localhost:7056/consocket");
var logArea = document.getElementById("log-server");

socket.onopen = function(e){
    
    let log = "<p>[open] Connection established</p>"    ;
    logArea.insertAdjacentHTML('afterend',log);
    socket.send(nomeConexao+" entroucon");
    socket.send("server>idfinal>"+nomeConexao);
};

socket.onmessage = function (event){
    let log = "<p>[message] "+event.data+"</p>"    ;
    logArea.insertAdjacentHTML('afterend',log);
}

socket.onclose = function (event){
    if(event.wasClean){
        let log = "<p>[close] "+event.code+" - reason = "+event.reason+"</p>"    ;
        logArea.insertAdjacentHTML('afterend',log);
    }else{
        let log = "<p>[close] connection died</p>"    ;
        logArea.insertAdjacentHTML('afterend',log);
    }
}

socket.onerror = function(error){
    let log = "<p>[close] "+error.message+"</p>";
    logArea.insertAdjacentHTML('afterend',log);
}


function MandarMensagem(mensage){
    socket.send("mensagem")
}