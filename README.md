# Unity-peliprojekti
2D mobiilipelin tekoälyn testausta
Tässä testataan 2D mobiilipelin tekoälyä, jossa ukkelille on annettu 2 pistettä, jonka välillä se liikkuu. Välissä on este, jonka se havaitsee ja hyppää yli. Esteenhavaitsija toimii siedettävästi, mutta on siinä vielä kehitettävää luotettavuudessa ja toimivuudessa.
Esteenhavaitsija on totetettu Plugins->Artifacial_Intelligence_Handling.cs ja siellä Esteenhavaitsija-luokassa, joka havaitsee esteen ja antaa ohjauskoordinaatteja.
Esteenhavaitsija on testivalmiina. Riittää, kun avaa tESTI-scenen ja käynnistää sen. Myös peliukkelin ohjausta nuolinäppäimillä voi kokeilla. Ohjaus on kytketty valmiiksi Player-hahmoon. Testausta varten kamera on kytkettävä Player-hahmoon, joten avaa Hierarchy->Mokkuloiden_hallinta->Inspector->Smooth_camera_asetukset -> vie Hierarchy->Player raahaamalla Target-kenttään.
Tämä projekti toimii Unityn versiossa 2018.3.0f2
Projekti on Wildman Games Oy:n tuotantoa. Kaikesta koodauksesta on vastannut Joni Taipale. Grafiikasta ja pelisuunnittelusta on vastannut Juhamatti Ollikainen.
Mikäli projekti kiinnostaa, voin tehdä tarkemmat ohjeet projektin käyttöönottamiseksi.
Plugins-kansiossa olevien dll-palikoiden koodit on saatavilla: https://github.com/joni-t/2D-pelin-koodikirjasto-Unityyn
Valmiit pelit voi ladata tarkasteltavaksi. https://play.google.com/store/apps/developer?id=Wildman+Games&hl=en-gb
