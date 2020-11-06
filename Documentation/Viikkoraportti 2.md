

Klassinen oivallus: tämä algoritmi onkin aika kinkkinen, mutta ehkä sitäkin mielenkiintoisempi :) Vaikka voronoi-diagrammi sinänsä on simppeli juttu ja toteutettavissa bruteforcella laskemalla jokaiselle pikselille lähin site, tämä algoritmi on kuitenkin aikavaativuutensa vuoksi järkevämpi valinta. Edellisen tapaisen virityksen olenkin joskus vuosia sitten koodannut, ja se oli armottoman hidas jo aika pienillä resoluutioilla. Tavoitteena itselle nyt ymmärtää tämä kunnolla ja implementoida se, ei niin väliä kurssiarvosanasta.

Aikatauluongelmia näin kahden lapsen kotiäitinä on merkittävästi -> 5-vuotias saa pelata Minecraftia 2 h päivässä ja lupaan itselleni edistää projektia päivittäin.

Täältä löytyi hyvä demo jossa selitettiin algoritmin toiminta: https://jacquesheunis.com/post/fortunes-algorithm/

Hyvin korkean tason pseudokoodi siis menee näin:


Fill the event queue with site events for each input site.

While the event queue still has items in it:

    If the next event on the queue is a site event:
	
        Add the new site to the beachline
		
    Otherwise it must be an edge-intersection event:
	
        Remove the squeezed cell from the beachline
		
Cleanup any remaining intermediate state


Täydensin tätä vähän tutoriaalin perusteella. Tässä vaiheessa isoja ongelmia NetBeansin kanssa, joten keskityn nyt pseudokoodiin...


Two ordered collections: event queue and beachline

Fill the event queue with site events for each input site.


	-order by y-coordinate of the site	
	
While the event queue still has items in it:

    If the next event on the queue is a site event:
	
        Add the new site to the beachline
		
		-add new arc
		
		-remove the arc right above it
		
		-add two copies of the removed arc
		
		-add two half-edges starting from the point in the original arc right above the new site
		
		-add possible circle events to the event queue
		
			-check if the lines next to the new arcs are going to intersect
			
			-if yes, add circle event to queue
			
			-y-coordinate of event (sweepline location) is point of intersection minus distance to endpoint
			
    Otherwise it must be an edge-intersection (circle) event:
	
        Remove the squeezed cell from the beachline
		
		-remove arc
		
		-the half-edges become finished edges, remove from beachline
		
		-add new single half-edge starting from intersection point
		
		-check both arcs for new future intersections
		
Cleanup any remaining intermediate state

	-remaining collisions must only have one arc in between

Tämän perusteella aloin siiten kirjoittaa C#-koodia rakkaalla Visual Studiolla päästäkseni nyt edes itse algoritmin kimppuun. Ensimmäinen ongelma on löytää site eventin yhteydessä oikea kaari uuden siten yläpuolelta. Tuolta demosivulta löytyi kaava, jolla oikea paraabeli löytyy, mutta identtisiä paraabeleja saattaa joskus olla beachlinessa useampi... no, päätin ratkaista ensin vain ne vaihtoehdot, joissa näin ei ole.

RATKAISU: lisätään kaarille tieto vasemmasta ja oikeasta rajasta, leftLimit ja rightLimit tjsp. Näin yksiselitteinen kaari löytyy \o/

Vektorit: miten ilmaistaan suunta? Ratkaisu: Alku- ja suuntapiste, esim yksikkövektori (lähinnä kysymys onko tarkkuus riittävä, pitäisikö olla pidempi?)

Sain algoritmin kaikki osat kirjoitettua ja ohjelman jopa ajettua kaatumatta muutamalla muutoksella. Algoritmi ei vielä toimi oikein, yhtään FinishedEdgeä ei muodostu. Tämä oli oletettavaa, sillä kirjoitin kaiken koodin ajamatta sitä kertaakaan välissä... Seuraavaksi sitten debuggerin kanssa pitää käydä koko homma läpi ja katsoa, mitä oikeasti tapahtuu.

Käytetty aika tällä viikolla noin 10 h.