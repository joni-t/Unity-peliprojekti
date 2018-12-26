using UnityEngine;
using System;
using Ohjaus_2D;
using Target_ohjaus;
using Level_hallinta;
using Ohjaus_laite;
using MenuFactory;

//scripti luo, tekee määritykset ja liittää target_ohjauksen sekä levelin hallinnan peliin
public class Ohjausmokkuloiden_asetukset : MonoBehaviour {
	//Ohjauslaitteet
	public bool Kosketusnaytto=true;
	public bool Hiiriohjaus=false;
	//parametrit
	public bool use_allowed_area;
	public Rect public_allowed_area=new Rect(); //yleinen sallittu alue peliobjekteille
	//kamera
	public SmoothCamera_Asetukset_Hallintapaneeliin smooth_camera_asetukset;
	//levelin lisäasetukset
	public Level_hallinta.LisaAsetukset_Parametrit_Hallintapaneeliin levelin_lisa_asetukset;
	public Ohjaus_laite.Parametrit_Hallintapaneeliin pausenappulan_asetukset; //pausenappulan määritykset
	//skip level
	public MenuFactory.Skip_Level_asetukset skip_level_asetukset;
	//kuorman mittaus
	public Ohjaus_2D.KuormanMittaaja_Parametrit_Hallintapaneeliin kuorman_mittaus_parametrit;

	[HideInInspector]
	Ohjaus_2D_rajapinta ohjaus_2d;
	[HideInInspector]
	public Target_ohjaus.Target_ohjaus target_ohjaus;
	[HideInInspector]
	public Level_hallinta.Level_hallinta level_hallinta;
	[HideInInspector]
	public MenuFactory.MenuFactory menu_factory;
	
	void Awake () {
		ohjaus_2d=Ohjaus_2D_rajapinta.Instance;
		target_ohjaus = Target_ohjaus.Target_ohjaus.Instance;
		level_hallinta=Level_hallinta.Level_hallinta.Instance;
		menu_factory=MenuFactory.MenuFactory.Instance;
		//siivotaan target-ohjauksesta rojut pois
		Debug.Log("Ohjausmokkuloiden_asetukset: Clean and Awake");
		target_ohjaus.Clean(true);
		Level_hallinta.Level_hallinta.Instance.Clean();
		Ohjaus_laite.Ohjauslaitteet.Instance.Clean();
		//asetukset ohjaus_2D:lle
		if(smooth_camera_asetukset.kamera_kaytossa)
			target_ohjaus.AsetaSmoothCamera(new SmoothCamera_Hallintapaneelista_Rakentaja().Rakenna(smooth_camera_asetukset));
		if(Kosketusnaytto) target_ohjaus.Lisaa_Ohjaus_Input_elem(new Multitouch_input_2D()); // lisätään multitouch-ohjaus
		if(Hiiriohjaus) target_ohjaus.Lisaa_Ohjaus_Input_elem(new Mouse_input_2D(0)); // lisätään hiiri-ohjaus
		if(public_allowed_area.width==0 && public_allowed_area.height==0) //jos asetusta ei asetettu -> asetetaan kameran alue
			public_allowed_area=target_ohjaus.ScreenToWorldRect(new Rect(0, 0, Screen.width, Screen.height));
		target_ohjaus.public_allowed_area=public_allowed_area;
		target_ohjaus.use_allowed_area=use_allowed_area;
		//screen-ohjaustapahtumien suorittajan asetus jostain tarvittaessa
		//target_ohjaus.Aseta_ScreenOhjaustapahtumien_Suorittaja(ScreenOhjaustapahtumien_suorittaja);
		//rakennetaan kuorman mittaaja
		//ohjaus_2d.AsetaKuormanMittaaja(new Ohjaus_2D.KuormanMittaaja_Hallintapaneelista_Rakentaja().Rakenna(kuorman_mittaus_parametrit));
	}

	void Start() {
		Debug.Log("Ohjausmokkuloiden_asetukset Start");
		target_ohjaus.clean_suoritettu=false; //seuraavan levelin käynnistystä varten
		//levelin hallinta
		new Level_hallinta.LisaAsetukset_Hallintapaneelista_Rakentaja().Rakenna(levelin_lisa_asetukset);
		//rakennetaan pausenappula
		Ohjaus_laite.Hallintapaneelista_Rakentaja nappulan_rakentaja=new Ohjaus_laite.Hallintapaneelista_Rakentaja();
		Ohjaus_laite.Ohjaus_laite pausen_ohjauslaite=new Ohjaus_laite.Ohjaus_laite("Pausenappula");
		Menu.Button_elem pause_button=nappulan_rakentaja.RakennaNappula_Hallintapaneelista(pausenappulan_asetukset, pausen_ohjauslaite);
		MenuFactory.PausenappulaRajapinta pausenappula_rajapinta=new MenuFactory.PausenappulaRajapinta(pausen_ohjauslaite, pause_button);
		MenuFactory.MenuFactory.Instance.AsetaPausenappi(pausenappula_rajapinta);
		//rakennetaan skip level nappula
		MenuFactory.MenuFactory.Instance.skip_level_hallinta=new Skip_Level_Rakentaja().Rakenna(skip_level_asetukset);
		Debug.Log("Level initialized");
		menu_factory.Set_to_inGame_State();
		menu_factory.Preparing_Level(); //asetetaan menu pois heti, leveli käynnistetään viiveellä
	}

	void Update() {
		if(level_hallinta.level_kaynnissa) {
			ohjaus_2d.SuoritaToiminnot(); //päivittää ohjauksien tilat ja suorittaa ohjattavien toiminnot
			if(level_hallinta.levelin_kello!=null)
				level_hallinta.levelin_kello.Paivita();
		} else if((!level_hallinta.level_ended_message_sended) & (!menu_factory.game_paused)) { //jos ei levelin päättymistä ole vielä otettu vastaan
			level_hallinta.level_ended_message_sended=true;
			if(level_hallinta.is_level_completed) {
				Debug.Log("Level complete");
				menu_factory.LevelComplete(); //asettaa menu-sivun näkyville
			} else if(level_hallinta.is_level_failed) {
				Debug.Log("Level failed");
				//if(menu_factory.LevelFailed()) { //asettaa menu-sivun näkyville tai käynnistää resume-toiminnon
				//	Invoke("Resume", menu_factory.level_failed_valinnat.resume_viive);
				//}
			}
		}
		//float start_time=Time.realtimeSinceStartup;
		//target_ohjaus.Move_All(Time.deltaTime); //suoritetaan, vaikka on tullut levelcomplete /-failed
		//täydennetään kuorman mittaukseen vielä move_all
		/*if(ohjaus_2d.kuorman_mittaaja!=null) {
			ohjaus_2d.kuorman_mittaaja.OtaSeuraavaMittausaikaVastaan("Move_All: ");
			ohjaus_2d.kuorman_mittaaja.Paivita(); //näytön päivitys, mikäli on aika
		}*/
	}

	//Toimintoja *****************
	//resume game
	void Resume() {
		menu_factory.Play_again(null);
	}
}