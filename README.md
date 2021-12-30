## Sistema Distribuito di notifiche basato su database realtime

### Overview

- Installare `Docker Desktop` per eseguire il server RethinkDB.

- E' possibile eseguire il server RethinkDB "dockerizzato" a prescindere dal sistema operativo della propria macchina.
  * Quando si passa da Windows a MacOs l'unico problema è che nei casi del server a più nodi bisogna crearsi un nuovo file .sh e riscriverselo per non avere problemi.
  * In questo modo l'esecuzione del comando "docker-compose -f docker-compose.yml up -d" va a buon fine e i container rimangono "up" senza terminare subito!!

- Aprire un terminale e dirigersi sulla cartella Cluster_Docker_rethinkdb.
  * E' possibile ora scegliere, in base alle proprie esigenze, di utilizzare un Cluster Rethinkdb a 1, 2 o 5 nodi andando via terminale sulla cartella corrispondente:

   + `Cluster` : 5 nodi.
   + `twoNode` : 2 nodi.
   + `oneNode` : 1 nodo.


- Una volta scelto, eseguire i seguenti comandi, validi per tutti e 3 i casi:
  * `docker-compose -f docker-compose.yml build` per costruire l'immagine --> è necessario usarlo solo la prima volta, una volta eseguito il comando l'immagine viene "salvata".

  * `docker-compose -f docker-compose.yml up -d` per mettere in esecuzione il cluster.

- Aprire un browser e digitare "proprioIndirizzoDiRete:8081", su questa porta infatti in tutti e tre i casi c'è un nodo Rethink in ascolto.

  Nel caso a due nodi è possibile digitare anche "proprioIndirizzoDiRete:8082" mentre nel caso a 5 nodi si possono digitare anche: "proprioIndirizzoDiRete:8083", "proprioIndirizzoDiRete:8084" "proprioIndirizzoDiRete:8085"

- A questo punto il tipo di server RethinkDB scelto è in esecuzione.
  * Aprire il progetto di libreria e seguire le istruzioni per utilizzarlo lato client nel README.md

- Per stoppare il server in esecuzione: `docker-compose -f docker-compose.yml stop` 