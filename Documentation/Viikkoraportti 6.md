Algoritmista paljastui jonkin verran bugeja kun aloin testailla satunnaisilla syötteillä, näyttäisi siltä että reunoilla tapahtuu välillä outoja asioita. Lisäksi käyttämäni valmis tietorakenne ei hyväksy tapahtumia, joilla on sama y-koordinaatti: tässäpä onkin luonteva vaihe toteuttaa itse max-heap even queueta varten.

Lisäsin UI:hin nappulan, jolla käytetyn inputin saa tallennettua leikepöydälle debuggausta varten. Bugisia tapauksia löytyy helposti pienelläkin määrällä siteja.

Asensin StyleCop-lisäosan Visual Studioon. Se tekee siis koodin laaduntarkistusta vastaavasti kuin Checkstyle.

Kahden pienen lapsen kanssa kotona ollessa on edelleen vaikea löytää tarpeeksi häiriötöntä työskentelyaikaa, joten muistutan taas itselleni, että ykkönen riittää hyvin ja kaikki sen päälle on plussaa. Ilmeisesti läpipääsyn estäviä juttuja on vielä tuo omien tietorakenteiden toteutus? Nyt on vielä käytössä valmis List<> ja SortedSet<>.
