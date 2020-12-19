Map generator

Tarkoituksena on luoda yksinkertainen karttageneraattori käyttämällä Voronoi-diagrammia: https://en.wikipedia.org/wiki/Voronoi_diagram

Käyttäjä saa määritellä haluamansa määrän pisteitä, ja ohjelma arpoo pisteet satunnaisesti kartalle (välttäen samoja koordinaatteja eri pisteillä) sekä piirtää niistä Voronoi-diagrammin. Piirtämiseen käytetään Fortune's algoritmia: https://en.wikipedia.org/wiki/Fortune's_algorithm

Algoritmin toteutukseen on käytetty lähteenä pääasiassa sivua https://jacquesheunis.com/post/fortunes-algorithm/. Lisäksi yleistä googlailua paljon.

Fortunen algoritmin aikavaativuuden tulisi olla O(log n), mutta oma toteutukseni ei yllä tähän. Ero johtuu luullakseni siitä, että uutta sitea lisätessä toteutus ei hyödynnä binäärihakua etsiessään paraabelia yläpuoleltaan, vaan kaikki mahdolliset paraabelit käydään läpi. Tämän korjaaminen olisi seuraava askel algoritmin kehityksessä. Tarvitun kokoisilla syötteillä tällä ei kuitenkaan ole juuri merkitystä, vaan toteutus on aivan käyttökelpoinen.

Projekti toteutetaan ~Javalla~ C#:lla. Opiskelen tietojenkäsittelytieteen kandia.
