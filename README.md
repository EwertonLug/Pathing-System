# Pathing-System - Simples Sistema de Pathing para jogos de Plataforma 2D
![](https://github.com/EwertonLug/Pathing-System/blob/master/Assets/Images/bg.png)
<h5>Demostração</h5>

[Ver video de Demostração](https://youtu.be/iqOZyLQZZC0)
<h5>Fluxo</h5>
<p>1 - Usuario Criar o path manualmente na cena</p>
<p>2 - Agente(IA) usa esse path para encontrar o melhor caminho até o alvo.(à cada "n" Waypoints percorridos)</p>
<p>3 - O Agente sabe qual é o atual e o próximo Waypoint, com isso decide se vai pular, descer ou andar</p>

<h5>Tutorial</h5>
<p>1 - Crie uma nova Cena</p>
<p>2 - Crie um novo path "Windows/Path System/Create Init Path"</p>
<p>3 - Clique sobre o Waypoint criado e use a opção "Create neighbor" no "Inspector" para criar vizinhos.</p>
<p>4 - Posicione os waypoints conforme necessário</p>
<p>5 - Crei dois objetos na cena, um para o Agente e outro para o Alvo.</p>
<p>6 - Adicione o script PathManager no objeto do Agente e enexe o alvo à variável.</p>
