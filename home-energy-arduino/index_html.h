const char *pageLogin = R"raw(
	<!DOCTYPE html>
	<html>
		<head>
			<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

			<meta name="viewport" content="width=device-width, initial-scale=1.0" />

			<title> Autenticar Wi-Fi </title>

			<style>
				#loader {
					position: absolute;
					left: 50%;
					top: 50%;
					z-index: 1;
					width: 60px;
					height: 60px;
					margin: -76px 0 0 -76px;
					border: 16px solid #f3f3f3;
					border-radius: 50%;
					border-top: 16px solid #26b36c;
					-webkit-animation: spin 2s linear infinite;
					animation: spin 2s linear infinite;
				}

				@-webkit-keyframes spin {
					0% {
						-webkit-transform: rotate(0deg);
					}

					100% {
						-webkit-transform: rotate(360deg);
					}
				}

				@keyframes spin {
					0% {
						transform: rotate(0deg);
					}
					
					100% {
						transform: rotate(360deg);
					}
				}

				/* Add animation to "page content" */
				.animate-bottom {
					position: relative;
					-webkit-animation-name: animatebottom;
					-webkit-animation-duration: 1s;
					animation-name: animatebottom;
					animation-duration: 1s;
				}

				@-webkit-keyframes animatebottom {
					from {
						bottom: -100px;
						opacity: 0;
					}

					to {
						bottom: 0px;
						opacity: 1;
					}
				}

				@keyframes animatebottom {
					from {
						bottom: -100px;
						opacity: 0;
					}

					to {
						bottom: 0;
						opacity: 1;
					}
				}

      #myDiv {
        display: none;
        text-align: center;
      }

      .center {
        text-align: center;
      }
      body {
        margin: 0;
        padding: 0;
        background-color: #b5de6aba;
        font-family: "Arial";
      }
      .login {
        width: 382px;
        overflow: hidden;
        margin: auto;
        margin: 20 0 0 450px;
        padding: 80px;
        background: #23463f;
        border-radius: 15px;
      }
      h2 {
        text-align: center;
        color: #277582;
        padding: 20px;
      }
      label {
        color: #08ffd1;
        font-size: 17px;
      }
      #selectSSIDS {
        position: relative;
        left: 10px;
        width: 300px;
        height: 30px;
        border: none;
        border-radius: 3px;
        padding-left: 8px;
        margin-left: 10px;
      }
      #Pass {
        position: relative;
        left: 10px;
        width: 290px;
        height: 30px;
        border: none;
        border-radius: 3px;
        padding-left: 8px;
      }
      #post-btn {
        position: relative;
        top: 15px;
        width: 300px;
        height: 30px;
        border: none;
        border-radius: 17px;
        background-color: blue;
        color: white;
        cursor: pointer;
      }
      #post-btn:hover {
        background-color: rgb(87, 87, 220);
        color: white;
      }
      #informacao {
        position: relative;
        height: 30px;
        color: red;
        text-align: center;
        top: 30px;
      }
    </style>
  </head>
  <body>
    <div id="loader" style="display: none"></div>

			<h2> PowerMetrics - Authentication </h2>
		
			<br/>

			<div class="responsive">
				<div class="login" style="display: block;" id="myDiv">
					<form id="login">
						<div>
							<div>
								<label>
									<b> SSID: </b> 
								</label>
							</div>

							<div>
								<select name="Ssid" id="selectSSIDS" style="width: 100%" required>
									<option id="def"> Selecione sua Rede (ssid) </option>
								</select>
							</div>
						</div>

						<br/> <br/>

						<div>
							<div style="margin-bottom: 10px;">
								<label> 
									<b> SENHA: </b> 
								</label>
							</div>

							<div>
								<input style="width: 100%" type="Password" name="Pass" id="Pass" placeholder="Informe a Senha" required />
							</div>
						</div>

						<br/>

						<div class="center" id="informacao" disabled></div>
						
						<div class="center">
							<input type="submit" name="post-btn" id="post-btn" value="Autenticar" />
						</div>
					</form>
				</div>
			</div>

			<script>
				let mensagemDinamica = document.getElementById("informacao");

				let frm = document.getElementById("login");

				let btn = document.getElementById("post-btn");

				btn.addEventListener("click", (e) => {
					e.preventDefault();
					enviandoDados();
				});

				async function enviandoDados() {
					const formDataJsonString = JSON.stringify(
						Object.fromEntries(new FormData(frm).entries())
					);

					mensagemDinamica.hidden = true;

					hidePage();

					const response = fetch("http://192.168.4.1/senddata", {
						headers: {
							"Content-Type": "text/plain"
						},

						method: "POST",

						body: formDataJsonString,

					}).then((response) => {
						mensagemDinamica.hidden = false;

						if(response.status == 200) {
							ShowPage();

							mensagemDinamica.innerHTML = "AUTENTICADO COM SUCESSO - AGUARDE";

							window.location.href = "http://powermetrics.local/autenticado";
						}

						if(response.status == 404) {
							ShowPage();

							mensagemDinamica.innerHTML = "Erro - dados inválidos";
						}

						if(response.status == 405) {
							ShowPage();

							mensagemDinamica.innerHTML = "Conecte-se a rede PowerControl !!!";
						}
					}).catch(() => {
						ShowPage();

						mensagemDinamica.innerHTML = "Serviço indisponível !!!";
					});
				}

				async function insertOptionsValues() {
					event.preventDefault();

					let response = await fetch("http://192.168.4.1/ssids");

					let dados = await response.text();

					let array = dados.split("<br>").filter((value) => value !== "");

					select = document.getElementById("selectSSIDS");

					document.querySelectorAll("#selectSSIDS option").forEach((r) => {
						if(r.id !== "def") {
							r.remove();
						}
					});

					for(var i = 0; i <= array.length - 1; i++) {
						var opt = document.createElement("option");
						opt.value = array[i];
						opt.innerHTML = array[i];
						select.appendChild(opt);
					}
				}

				function hidePage() {
					document.getElementById("loader").style.display = "block";
					document.getElementById("myDiv").style.display = "none";
				}

				function ShowPage() {
					document.getElementById("loader").style.display = "none";
					document.getElementById("myDiv").style.display = "block";
				}

       window.addEventListener("load", function (event) {
        insertOptionsValues();
      });
    </script>
  </body>
</html>)raw";
