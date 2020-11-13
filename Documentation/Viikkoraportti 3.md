La-su: Debuggasin koodin siihen malliin, että ainakin jotain tuloksia tulee ulos. Virheitä löytyi about kaikkialta, kuten oli odotettavissa :) Graafinen debuggaustyökalu olisi ollut hyvä, nyt naputtelin arvoja GeoGebraan kun halusin varmistua siitä, että kaikki näyttää oikealta... Tiedossa on, että algoritmi ei nyt pysty käsittelemään erikoistapauksia, joissa eri siteilla on sama x- tai y-koordinaatti. Ohjelman käyttötarkoituksen kannalta näitä ei tarvitsekaan käsitellä, vaan riittää varmistaa, että liian lähellä olevia koordinaatteja ei aineistossa ole. Aika-arvio 4 h.

Su: loin UI:n ja testasin sitä algoritmin tuottamilla arvoilla. Osa on aivan pielessä mutta myös oikeansuuntaista tulosta näkyy. UI:n avulla on helppo alkaa testata algoritmin toimintaa vaihe vaiheelta. 2 h

Su-ti: Graafinen UI iteroivaksi ym sälää, hirveästi säätöä 4 h

ke-pe: pään lyömistä seinään lasten huutaessa vieressä kunnes hiffasin, mikä mättää. Koodissa oli virheitä circle eventtien luonnissa, ja loppujen lopuksi virheet poistuivat parilla pienellä korjauksella. Muutin koodia niin, että se poistaa sivureunojen yli menneet paraabelit, jotka eivät enää tee muuta kun jatkavat välissään olevaa half edgeä äärettömyyteen. Tämä helpotti debuggausta, kun ruutuun ei piirtynyt enää niin paljon sälää. 9 h

Tässä vaiheessa kaikki näyttäisi toimivan tiettyjä erikoistapauksia lukuunottamatta. Händläämättömiä erikoistapauksia ovat ainakin identtiset pisteet ja (lähes) samalle suoralle osuvat pisteet. Toteuttamatta on vielä viimeisten viivojen päättely, mutta se on arvioni mukaan hyvin pieni homma, ne pitää vain säätää oikean pituisiksi ja merkitä "valmiiksi"

Käänsin käyttöliittymän ylösalaisin, jotta koordinaatisto olisi oikein päin (y-akselin arvot kasvavat ylöspäin). Jostain syystä nappuloiden siirtäminen toiseen gridiin ei heti onnistunut, joten nekin ovat nyt ylösalaisin.

Beachline ei ole tällä hetkellä optimaalinen eikä siitä haun tekeminen toteuta Fortunen algoritmin aikavaativuutta. Haku pitäisi toteuttaa tehokkaammin kuin käymällä jokainen alkio läpi. Tässä kohtaa kannattaisi varmasti kirjoittaa testit, joiden avulla varmistetaan perustoiminnallisuuden säilyminen, kun beachlinea ja hakua aletaan toteuttaa uudestaan.

Loin testiprojektin ja kirjoitin siihen yhden testin.

Mietin nyt, että teenkö projektin loppuun C#:lla vai yritänkö saada Javan toimimaan. Olen kyllä kirjoittanut testejä C#:lle aiemminkin työn puolesta, Java on itselle vieraampi. Testikattavuutta en ole aiemmin raportoinut C#:lla. Se pitää ilmeiseti tehdä jollain ulkoisella työkalulla, koska testikattavuusraportointi Visual Studiossa on Enterprise-tason ominaisuus... (selvittely 2 h)

Yhteensä noin 21 h työtä