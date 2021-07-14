using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Menu : MonoBehaviour
{
    public GameObject panel; // Main panel
    public GameObject panelNG; // Panel new game
    public GameObject totalRang; // Panel table total rang champion
    public GameObject panelMarket; // Panel market buy GOF
    public GameObject panelSetting; // Panel preferens game
    public GameObject panelName; // Panel set user name
    public GameObject start; // Buttom start
    public GameObject start2; // Button start in table rang
    public GameObject reStartAds; // Button restart game and add 100hp
    public GameObject newGame; // Button new game, question a not save score
    public GameObject exit; // Button exit application
    public GameObject setting; // Button preferens applicaion
    public GameObject market; // Button show buy GOF
    public GameObject champion; // Button show table rang
    public GameObject okNG; // Button new game, no save score
    public GameObject backNG; // Button back to panel restart game
    public Button okName; // Button save user name
    public Button gofBtn; // Button activate GOF
    public GameObject gofArrow; // Arrow GOF
    public Button buy; // Button buy GOF
    public Text gofTxt; // Label text GOF
    public Text gofTxt2; // Label text GOF in buy panel
    public Text gofCoin; // Label text coin in buy panel
    public InputField uName; // Label text coin in buy panel
    public RectTransform rang; // Content table total rang
    public Image[] panelScores; // Array panels scores users
    [Space]
    public Toggle sound_mute;
    public Toggle rain_mute;
    public Toggle mus1;
    public Toggle mus2;
    public Toggle mus3;
    [Space]
    public GameObject player;
    public GameObject cam;
    public GameObject gof;
    public GameObject generate;
    [Space]
    public bool newUSR = false;
    [Space]
    [SerializeField] private AudioClip[] sounds = { };
    private AudioSource source;

    private Image gofImg;

    private int viewADS = 3;

    private bool startMusic = false;

    private void Start()
    {
        source = cam.GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.mute = false;
        source.loop = true;

        start.GetComponent<Button>().onClick.AddListener(StartGame);
        start2.GetComponent<Button>().onClick.AddListener(OkNG);
        newGame.GetComponent<Button>().onClick.AddListener(NewGame);

        exit.GetComponent<Button>().onClick.AddListener(ExitGame);

        setting.GetComponent<Button>().onClick.AddListener(Setting);

        market.GetComponent<Button>().onClick.AddListener(Market);

        champion.GetComponent<Button>().onClick.AddListener(Champion);

        okNG.GetComponent<Button>().onClick.AddListener(OkNG);
        backNG.GetComponent<Button>().onClick.AddListener(BackNG);

        okName.onClick.AddListener(OkName);

        gofBtn.onClick.AddListener(GOF);
        buy.onClick.AddListener(BuyGOF);

        sound_mute.onValueChanged.AddListener(delegate { SoundMute(sound_mute); });
        rain_mute.onValueChanged.AddListener(delegate { RainMute(rain_mute); });
        mus1.onValueChanged.AddListener(delegate { Music(0); });
        mus2.onValueChanged.AddListener(delegate { Music(1); });
        mus3.onValueChanged.AddListener(delegate { Music(2); });

        panelNG.SetActive(false);

        reStartAds.SetActive(false);
        newGame.SetActive(false);

        gof.SetActive(false);
        totalRang.SetActive(false);

        panelMarket.SetActive(false);

        if(SinglVar.gofMana > 0)
            gofBtn.interactable = true;

        else
            gofBtn.interactable = false;

        gofTxt.text = SinglVar.gofMana.ToString();
        gofImg = gofBtn.GetComponent<Image>();

        SinglVar.level = 1;

        SinglVar.uid = PlayerPrefs.GetString("uid", "-1");
        if (SinglVar.uid == "-1" || newUSR) {
            PlayerPrefs.DeleteAll();
            webGame("add-usr");
            SinglVar.gofMana = 3;
        }

        viewADS = 3;

        if(SinglVar.sound == 1)
            source.Play();

        if(SinglVar.sound == 0) sound_mute.isOn = false;
        if(SinglVar.rain == 0) rain_mute.isOn = false;

        if (SinglVar.music == 0) mus1.isOn = true;
        else if (SinglVar.music == 1) mus2.isOn = true;
        else if (SinglVar.music == 2) mus3.isOn = true;
    }

    private void Pause()
    {
        start.SetActive(false);

        panel.SetActive(true);

        if (viewADS > 0) {
            reStartAds.SetActive(true);
            newGame.SetActive(true);

        } else {
            reStartAds.SetActive(false);
            newGame.SetActive(false);

            if (SinglVar.uname == "") {
                panelName.SetActive(true);

            } else {
                start2.SetActive(true);
                totalRang.SetActive(true);

                webGame("score-usr", SinglVar.uid, "", SinglVar.level.ToString(), SinglVar.scores.ToString());
            }
        }

        source.clip = sounds[0];
        if (SinglVar.sound == 1)
            source.Play();

        viewADS--;
    }

    private void StartGame()
    {
        panel.SetActive(false);
        totalRang.SetActive(false);

        player.SendMessage("Live", SendMessageOptions.DontRequireReceiver);

        source.clip = sounds[SinglVar.music];
        if (SinglVar.sound == 1)
            source.Play();

        viewADS = 3;

        if (PlayerPrefs.GetInt("gofArrow", 1) == 1)
            gofArrow.SetActive(true);
    }

    private void ReStartGameAds()
    {
        panel.SetActive(false);

        player.SendMessage("Live", SendMessageOptions.DontRequireReceiver);

        source.clip = sounds[SinglVar.music];
        if (SinglVar.sound == 1)
            source.Play();
    }

    private void NewGame()
    {
        panel.SetActive(false);
        totalRang.SetActive(false);
        panelNG.SetActive(true);
    }

    private void ExitGame() //Application.Quit();
    {
        Application.OpenURL("market://details?id=ID");
    }

    private void Setting()
    {
        totalRang.SetActive(false);
        panelMarket.SetActive(false);
        panelNG.SetActive(false);

        if (panelSetting.activeSelf) {
            if (viewADS == 3)
                start.SetActive(true);

            else if (viewADS < 0)
                totalRang.SetActive(true);

            else {
                reStartAds.SetActive(true);
                newGame.SetActive(true);
            }

            panelSetting.SetActive(false);

        } else {
            start.SetActive(false);
            reStartAds.SetActive(false);
            newGame.SetActive(false);

            panelSetting.SetActive(true);

            startMusic = true;
        }
    }

    private void Market()
    {
        totalRang.SetActive(false);
        panelSetting.SetActive(false);
        panelNG.SetActive(false);

        if (panelMarket.activeSelf) {
            if (viewADS == 3)
                start.SetActive(true);

            else if (viewADS < 0)
                totalRang.SetActive(true);

            else {
                reStartAds.SetActive(true);
                newGame.SetActive(true);
            }

            panelMarket.SetActive(false);

        } else {
            start.SetActive(false);
            reStartAds.SetActive(false);
            newGame.SetActive(false);

            panelMarket.SetActive(true);

            CalcGOF();
        }
    }

    private void Champion()
    {
        panelMarket.SetActive(false);
        panelSetting.SetActive(false);
        panelNG.SetActive(false);

        if (totalRang.activeSelf && viewADS > 0) {
            if (viewADS == 3)
                start.SetActive(true);

            else {
                reStartAds.SetActive(true);
                newGame.SetActive(true);
            }

            totalRang.SetActive(false);

        } else {
            start.SetActive(false);
            reStartAds.SetActive(false);

            if (viewADS < 0 || viewADS == 3) {
                newGame.SetActive(false);
                start2.SetActive(true);

            } else {
                start2.SetActive(false);
                newGame.SetActive(true);
            }

            totalRang.SetActive(true);

            webGame("scores", SinglVar.uid);
        }
    }

    private void OkNG()
    {
        panelNG.SetActive(false);
        totalRang.SetActive(false);

        generate.SendMessage("startGame", SendMessageOptions.DontRequireReceiver);

        StartGame();
    }

    private void BackNG()
    {
        panelNG.SetActive(false);
        reStartAds.SetActive(true);
        panel.SetActive(true);
    }

    private void OkName()
    {
        if (uName.text.Length > 0) {
            panelName.SetActive(false);

            SinglVar.uname = uName.text;
            PlayerPrefs.SetString("uname", SinglVar.uname);

            webGame("name-usr", SinglVar.uid, uName.text);
        }
    }

    void SoundMute(Toggle change)
    {
        if (startMusic) {
            if (change.isOn) {
                SinglVar.sound = 1;
                if (!source.isPlaying) source.Play();

            } else {
                SinglVar.sound = 0;
                source.Stop();
            }

            PlayerPrefs.SetInt("sound", SinglVar.sound);
        }
    }

    void RainMute(Toggle change)
    {
        if (startMusic) {
            if (change.isOn)
                SinglVar.rain = 1;

            else
                SinglVar.rain = 0;

            PlayerPrefs.SetInt("rain", SinglVar.rain);
        }
    }

    void Music(int change)
    {
        if (startMusic) {
            SinglVar.music = change;
            PlayerPrefs.SetInt("music", SinglVar.music);
        }
    }

    private void CalcGOF()
    {
        gofCoin.text = SinglVar.coin.ToString();
        gofTxt.text = SinglVar.gofMana.ToString();
        gofTxt2.text = SinglVar.gofMana.ToString();

        if (SinglVar.coin < 25)
            buy.interactable = false;

        else
            buy.interactable = true;

        if (SinglVar.gofMana > 0)
            gofBtn.interactable = true;

        else
            gofBtn.interactable = false;
    }

    private void BuyGOF()
    {
        if (SinglVar.coin >= 25 && SinglVar.gofMana < 99) {
            SinglVar.coin -= 25;
            SinglVar.gofMana++;

            PlayerPrefs.SetInt("gof", SinglVar.gofMana);
            PlayerPrefs.SetInt("coin", SinglVar.coin);

            webGame("gof", SinglVar.uid);
        }

        CalcGOF();
    }

    private void GOF()
    {
        gofBtn.interactable = false;

        gof.SetActive(true);

        SinglVar.gof = 1;
        SinglVar.gofMana--;

        PlayerPrefs.SetInt("gof", SinglVar.gofMana);

        StartCoroutine(GodOfFire());

        if (gofArrow.activeSelf) {
            gofArrow.SetActive(false);
            PlayerPrefs.SetInt("gofArrow", 0);
        }
    }

    IEnumerator GodOfFire()
    { 
        yield return new WaitForSeconds(.2f);

        SinglVar.gof -= .01f;
        gofImg.fillAmount = SinglVar.gof;

        if (SinglVar.gof > 0)
            StartCoroutine(GodOfFire());
        else {
            gof.SetActive(false);

            if (SinglVar.gofMana > 0)
                gofBtn.interactable = true;
            else
                gofBtn.interactable = false;

            gofTxt.text = SinglVar.gofMana.ToString();
            gofImg.fillAmount = 1;
            SinglVar.gof = 0;
        }
    }

    void webGame(string cmd, string uid = "", string uname = "", string ulvl = "", string uscore = "")
    {
        StartCoroutine(Upload(
            cmd,
            uid,
            uname,
            ulvl,
            uscore));
    }

    IEnumerator Upload(string cmd, string uid, string uname, string ulvl, string uscore)
    {
        WWWForm form = new WWWForm();
        form.AddField("cmd", cmd);
        if (uid != "") form.AddField("u_id", uid);
        if (uname != "") form.AddField("u_name", uname);
        if (ulvl != "") form.AddField("u_lvl", ulvl);
        if (uscore != "") form.AddField("u_score", uscore);

        UnityWebRequest www = UnityWebRequest.Post("https://website/magma.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            //Debug.Log(www.error);
        } else {
            // Debug.Log("complete: " + www.downloadHandler.text);

            if (cmd == "add-usr") {
                SinglVar.uid = www.downloadHandler.text;
                PlayerPrefs.SetString("uid", SinglVar.uid);
            } else if (cmd == "score-usr" || cmd == "scores") {

                string[] c = www.downloadHandler.text.Split('|');

                int idimg = 0;
                int scroll = 0;
                for (int i = 0; i < 30; i++) {
                    string[] s = { "", "", "", "" };
                    try { s = c[i].Split('‼'); } catch { }

                    if (s[0] == uid) {
                        panelScores[i].SendMessage("SetElements", (1, "I'm, " + s[1], s[2], s[3], i), SendMessageOptions.DontRequireReceiver);
                        idimg = 2;
                        scroll = i;
                    } else
                        panelScores[i].SendMessage("SetElements", (idimg, s[1], s[2], s[3], i), SendMessageOptions.DontRequireReceiver);

                }

                Vector3 v3 = rang.position;
                v3.y = 132 * scroll;
                rang.localPosition = v3;

            } else if (cmd == "name-usr") {
                Pause();
            }
        }
    }
}
