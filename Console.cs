using System;
using UnityEngine;
using MelonLoader;
using UnhollowerBaseLib;
using CodeStage.AntiCheat.Storage;
using System.IO;
using System.Collections;
using UnityEngine.AI;
using HarmonyLib;

namespace Master
{
    public class Console : MelonMod
    {
        private GameObject _cursorObject;
        private GameObject _player => UnityEngine.Object.FindObjectOfType<FPScontroller>().gameObject;
        private WeaponManager _weaponManager => UnityEngine.Object.FindObjectOfType<WeaponManager>();
        private WeaponScript _weaponScript => UnityEngine.Object.FindObjectOfType<WeaponScript>();
        private KeyCode _consoleButton = KeyCode.F1;
        private KeyCode _kickConsoleButton = KeyCode.F2;
        private KeyCode _infoBarButton = KeyCode.F3;
        private KeyCode _mainMenuButton = KeyCode.F4;
        private KeyCode _spawnButton = KeyCode.P;
        private KeyCode _regenButton = KeyCode.K;
        private KeyCode _proneButton = KeyCode.C;
        private KeyCode _timeButton = KeyCode.G;
        private KeyCode _cursorButton = KeyCode.L;
        private KeyCode _flyButton = KeyCode.Z;
        private KeyCode[] _chatButtons;
        public Rect _consoleRect = new Rect(10f, 10f, 200f, 630f);
        private Rect _kickConsoleRect = new Rect(250f, 10f, 550f, 560f);
        private Rect _infoMenuRect = new Rect(840f, 10f, 300f, 630f);
        private bool _сonsole = false;
        private bool _infoBar = false;
        private bool _kickConsole = false;
        private bool _mainMenu = true;
        private bool _playerMenu = false;
        private bool _serverMenu = false;
        private bool _weaponMenu = false;
        private bool _teamMenu = false;
        private bool _hatMenu = false;
        private bool _logMenu = false;
        private bool _helpMenu = false;
        private bool _monsterMenu = false;
        private bool _cursor = false;
        private bool _fly = false;
        private bool _spawnCustard = false;
        private bool _helicopter = false;
        private bool _rotator = false;
        private bool _infHp = false;
        private bool _regen = false;
        private bool _recoil = true;
        private bool _rapidFire = false;
        private bool _infMonsterHp = false;
        private bool _disablePowerReload = false;
        private bool _monsterRapidFire = false;
        private float _flySpeed = 50f;
        private float _walkRunSpeed = 1f;
        private string[] _chatMessages;
        public static bool Protection = false;
        public static float Fov = 60f;
        public int Fps = 60;
        public string RoomMessage = "";
        public string RoomMsgKiller = "";
        public string RoomMsgKilled = "";
        public string ChatMessage = "";
        public string ChatMsgTeam = "";
        public string Custards = "0";
        public string Hat = "1";
        public string FlyText = "50";
        public string Health = "100";
        public string MonsterHealth = "11500";
        public string ConsoleName = "<color=red>Master</color> Console";

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            SetupConfig();
            if (sceneName != "MainMenu" && sceneName != "Updater" && sceneName != "Banned")
            {
                _cursorObject = new GameObject("Cursor");
                _cursorObject.tag = "Menu";
                _cursorObject.SetActive(false);
            }
            else
            {
                if (_cursorObject != null)
                {
                    UnityEngine.Object.Destroy(_cursorObject);
                }
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(_consoleButton))
            { 
                _сonsole = !_сonsole; 
            }

            if (Input.GetKeyDown(_kickConsoleButton))
            { 
                _kickConsole = !_kickConsole; 
            }

            if (Input.GetKeyDown(_infoBarButton))
            { 
                _infoBar = !_infoBar; 
            }

            if (Input.GetKeyDown(_mainMenuButton))
            { 
                Application.LoadLevel("MainMenu"); 
            }

            if (Input.GetKey(_spawnButton))
            {
                if (!_spawnCustard)
                {
                    PhotonNetwork.NOOU("Toast", _player.transform.position + _player.transform.forward * 3f, Quaternion.identity, 0, null);
                }
                else
                {
                    PhotonNetwork.NOOU("Custard", _player.transform.position + _player.transform.forward * 3f, Quaternion.identity, 0, null);
                }
            }

            if (Input.GetKeyDown(_regenButton))
            { 
                _regen = true; 
            }

            if (Input.GetKeyDown(_proneButton))
            { 
                _player.GetComponent<FPScontroller>().NDBHHAHAAKN = !_player.GetComponent<FPScontroller>().NDBHHAHAAKN; 
            }

            if (Input.GetKeyDown(_timeButton))
            { 
                PhotonNetwork.photonMono.enabled = !PhotonNetwork.photonMono.enabled; 
            }

            if (Input.GetKeyDown(_cursorButton))
            {
                _cursor = !_cursor;
                _cursorObject.SetActive(_cursor);
            }

            if (Input.GetKeyDown(_flyButton))
            {
                _fly = !_fly;
                if (!_fly)
                {
                    _player.GetComponent<FPScontroller>().LGIGJCDJMNO.fallDamageMultiplier = 1f;
                    _player.GetComponent<FPScontroller>().enabled = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.F5) && _chatMessages[0] != null)
            {
                SendChatMessage(_chatMessages[0]);
            }

            if (Input.GetKeyDown(KeyCode.F6) && _chatMessages[1] != null)
            {
                SendChatMessage(_chatMessages[1]);
            }

            if (Input.GetKeyDown(KeyCode.F7) && _chatMessages[2] != null)
            {
                SendChatMessage(_chatMessages[2]);
            }

            if (Input.GetKeyDown(KeyCode.F8) && _chatMessages[3] != null)
            {
                SendChatMessage(_chatMessages[3]);
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                PhotonNetwork.networkingPeer.SetMasterClient(PhotonNetwork.player.actorID);
            }

            if (_fly)
            {
                _player.GetComponent<FPScontroller>().LGIGJCDJMNO.fallDamageMultiplier = 0f;
                _player.GetComponent<FPScontroller>().enabled = false;
                if (Input.GetKey(KeyCode.W))
                {
                    _player.transform.position += Camera.main.transform.forward * _flySpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    _player.transform.position -= Camera.main.transform.right * _flySpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    _player.transform.position -= Camera.main.transform.forward * _flySpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _player.transform.position += Camera.main.transform.right * _flySpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    _player.transform.position += Camera.main.transform.up * _flySpeed * Time.deltaTime;
                }
            }

            if (_rotator)
            { 
                _player.transform.Rotate(new Vector3(0f, 1000f, 0f) * Time.deltaTime); 
            }

            if (_helicopter)
            {
                _player.GetComponent<CharacterController>().enabled = false;
                _player.transform.position += Camera.main.transform.up * 3f * Time.deltaTime;
            }

            if (_regen)
            {
                for (int i = 0; i <= 100; i++)
                {
                    SetHealth(Convert.ToSingle(i));

                    if (i >= 100)
                        _regen = false;
                }
                _infHp = false;
            }

            if (_infHp)
            { 
                SetHealth(float.NaN); 
                _regen = false; 
                _infHp = false; 
            }

            if (_weaponScript.gameObject)
            { 
                _weaponScript.BFMOECIAGFN = _recoil; 
            }
        }

        public override void OnGUI()
        {
            if (_сonsole)
            { 
                GUI.Window(0, _consoleRect, new Action<int>(Window), ConsoleName); 
            }

            if (_kickConsole)
            { 
                GUI.Window(1, _kickConsoleRect, new Action<int>(Window), "<color=blue>Kick</color> Console"); 
            }

            if (_infoBar)
            {
                GUI.Window(2, _infoMenuRect, new Action<int>(Window), "<color=green>Info</color> Menu"); 
            }
        }

        private void Window(int pageNumber)
        {
            if (pageNumber == 0)
            {
                if (_mainMenu)
                {
                    if (GUILayout.Button("Player Menu", new GUILayoutOption[0]))
                    { 
                        _playerMenu = true;
                        _mainMenu = false;
                    }
                    if (GUILayout.Button("Server Menu", new GUILayoutOption[0]))
                    {
                        _serverMenu = true;
                        _mainMenu = false;
                    }
                    if (GUILayout.Button("Network Messages Menu", new GUILayoutOption[0]))
                    { 
                        _logMenu = true; 
                        _mainMenu = false; 
                    }
                    if (GUILayout.Button("Monster Menu", new GUILayoutOption[0]))
                    { 
                        _monsterMenu = true; 
                        _mainMenu = false; 
                    }
                    if (GUILayout.Button("Help Menu", new GUILayoutOption[0]))
                    { 
                        _helpMenu = true; 
                        _mainMenu = false; 
                    }
                    GUILayout.Space(10f);
                    GUILayout.Label($"Fly Speed: {_flySpeed}", new GUILayoutOption[0]);
                    FlyText = GUILayout.TextField(FlyText, 3, new GUILayoutOption[0]);
                    _flySpeed = Convert.ToSingle(FlyText);
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Freeze Collect Bot", new GUILayoutOption[0]))
                    {
                        foreach (CustardBot custardBot in UnityEngine.Object.FindObjectsOfType<CustardBot>())
                        {
                            custardBot.enabled = false;
                            custardBot.GetComponent<NavMeshAgent>().enabled = false;
                        }
                    }
                    if (GUILayout.Button("Destroy All NPC'S", new GUILayoutOption[0]))
                    { 
                        DestroyAllNPCs(); 
                    }
                    if (GUILayout.Button("Disable Kill Zone's", new GUILayoutOption[0]))
                    { 
                        foreach (KillZone killZone in UnityEngine.Object.FindObjectsOfType<KillZone>())
                        { 
                            killZone.enabled = false; 
                        }
                    }
                    if (GUILayout.Button("Destroy All Custards", new GUILayoutOption[0]))
                    {
                        foreach (Custard custard in UnityEngine.Object.FindObjectsOfType<Custard>())
                        { 
                            PhotonNetwork.Destroy(custard.gameObject); 
                        }
                    }
                    if (GUILayout.Button("Destroy All (Master-Client)", new GUILayoutOption[0]))
                    { 
                        PhotonNetwork.DestroyAll(); 
                    }
                    if (GUILayout.Button("Get Fake MC(Master-Client)", new GUILayoutOption[0]))
                    {
                        PhotonNetwork.player.playerPropertiesCache.photonView.ownerId = PhotonNetwork.player.actorID;
                        PhotonNetwork.player.playerPropertiesCache.photonView.currentMasterID = PhotonNetwork.player.actorID;
                        PhotonNetwork.room.masterClientId = PhotonNetwork.player.ID;
                        PhotonNetwork.room.masterClientIdField = PhotonNetwork.player.ID;
                    }
                    GUILayout.Space(10f);
                    PhotonNetwork.photonMono.enabled = GUILayout.Toggle(PhotonNetwork.photonMono.enabled, "Time",
                    new GUILayoutOption[0]);
                    RenderSettings.fog = GUILayout.Toggle(RenderSettings.fog, "Fog", new GUILayoutOption[0]);
                    _helicopter = GUILayout.Toggle(_helicopter, "Helicopter", new GUILayoutOption[0]);
                    if (!_helicopter) 
                    { 
                        _player.GetComponent<CharacterController>().enabled = true; 
                    }
                    _rotator = GUILayout.Toggle(_rotator, "Rotator", new GUILayoutOption[0]);
                    _spawnCustard = GUILayout.Toggle(_spawnCustard, "Spawn Custard(MC)", new GUILayoutOption[0]);
                }
                if (_playerMenu)
                {
                    GUILayout.Label("Health: " + Health, new GUILayoutOption[0]);
                    Health = GUILayout.TextField(Health, new GUILayoutOption[0]);
                    if (GUILayout.Button("Set Health", new GUILayoutOption[0]))
                    { 
                        SetHealth(Convert.ToSingle(Health)); 
                    }
                    _infHp = GUILayout.Toggle(_infHp, "Infinite Health", new GUILayoutOption[0]);
                    _regen = GUILayout.Toggle(_regen, "Regeneration", new GUILayoutOption[0]);
                    GUILayout.Space(10f);
                    GUILayout.Label("Custards:", new GUILayoutOption[0]);
                    Custards = GUILayout.TextField(Custards, 100, new GUILayoutOption[0]);
                    if (GUILayout.Button("Set Custards", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("Custards", Convert.ToInt32(Custards)); 
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Hat Menu", new GUILayoutOption[0]))
                    { 
                        _hatMenu = true; 
                        _playerMenu = false; 
                    }
                    if (GUILayout.Button("Team Menu", new GUILayoutOption[0]))
                    { 
                        _teamMenu = true; 
                        _playerMenu = false; 
                    }
                    if (GUILayout.Button("Weapon Menu", new GUILayoutOption[0]))
                    { 
                        _weaponMenu = true; 
                        _playerMenu = false; 
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _playerMenu = false; 
                        _mainMenu = true; 
                    }
                }
                if (_serverMenu)
                {
                    if (_cursorObject != null && !PhotonNetwork.isOfflineMode)
                    {
                        GUILayout.Label("Room Settings:", new GUILayoutOption[0]);
                        PhotonNetwork.networkingPeer.CurrentRoom.IsOpen =
                        GUILayout.Toggle(PhotonNetwork.networkingPeer.CurrentRoom.IsOpen, "Is Open", new GUILayoutOption[0]);
                        PhotonNetwork.networkingPeer.CurrentRoom.IsVisible =
                        GUILayout.Toggle(PhotonNetwork.networkingPeer.CurrentRoom.IsVisible, "Is Visible", new GUILayoutOption[0]);
                        GUILayout.Label("Max Players: " + PhotonNetwork.networkingPeer.CurrentRoom.maxPlayers, new GUILayoutOption[0]);
                        PhotonNetwork.networkingPeer.CurrentRoom.maxPlayers =
                        (int)GUILayout.HorizontalSlider(PhotonNetwork.networkingPeer.CurrentRoom.maxPlayers, 1, 12, new GUILayoutOption[0]);
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    {
                        _serverMenu = false;
                        _mainMenu = true;
                    }
                }
                if (_weaponMenu)
                {
                    string[] weapons = new string[]
                    {
                        "XIX",
                        "XIX II",
                        "MK16",
                        "AKM",
                        "M249-Saw",
                        "M40A3",
                        "MP5N",
                        "MCS870",
                        "44 Combat",
                        "VZ61",
                        "Shorty",
                        "RPG",
                        "Chainsaw",
                        "Knife",
                        "Katana",
                        "Machete",
                        "Fireaxe",
                        "Grenade Launcher",
                    };
                    foreach (string text in weapons)
                    {
                        if (GUILayout.Button(text, new GUILayoutOption[0]))
                        {
                            _weaponManager.DHFOMKJHLBM.Add
                            (_weaponManager.transform.Find(text).GetComponent<WeaponScript>());
                        }
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Infinite Ammo", new GUILayoutOption[0]))
                    {
                        for (int i = 0; i < _weaponManager.DHFOMKJHLBM.Count; i++)
                        {
                            if (_weaponManager.DHFOMKJHLBM[i].FMPLNFBLHKJ != null)
                            {
                                _weaponManager.DHFOMKJHLBM[i].FMPLNFBLHKJ.bulletsLeft = int.MaxValue;
                                _weaponManager.DHFOMKJHLBM[i].FMPLNFBLHKJ.patchedClips = int.MaxValue;
                            }
                            if (_weaponManager.DHFOMKJHLBM[i].MMIKHOPCECE != null)
                            {
                                _weaponManager.DHFOMKJHLBM[i].MMIKHOPCECE.bulletsLeft = int.MaxValue;
                                _weaponManager.DHFOMKJHLBM[i].MMIKHOPCECE.patchedClips = int.MaxValue;
                            }
                            if (_weaponManager.DHFOMKJHLBM[i].MJIKHNADGAG != null)
                            {
                                _weaponManager.DHFOMKJHLBM[i].MJIKHNADGAG.ammoCount = int.MaxValue;
                            }
                        }
                    }
                    if (GUILayout.Button("Weapon Damage Buff", new GUILayoutOption[0]))
                    {
                        foreach (HitBox hitBox in UnityEngine.Object.FindObjectsOfType<HitBox>())
                        { 
                            hitBox.DHGDMKPLBJH = float.PositiveInfinity; 
                        }
                        foreach (HitboxBoss hitboxBoss in UnityEngine.Object.FindObjectsOfType<HitboxBoss>())
                        { 
                            hitboxBoss.DHGDMKPLBJH = float.PositiveInfinity; 
                        }
                        foreach (HitBoxBot hitBoxBot in UnityEngine.Object.FindObjectsOfType<HitBoxBot>())
                        { 
                            hitBoxBot.DHGDMKPLBJH = float.PositiveInfinity; 
                        }
                    }
                    if (GUILayout.Button("Rapid Fire: " + _rapidFire, new GUILayoutOption[0]))
                    {
                        _rapidFire = !_rapidFire;
                        if (_rapidFire)
                        {
                            _weaponScript.FMPLNFBLHKJ.fireRate = 0.001f;
                            _weaponScript.FMPLNFBLHKJ.bulletsPerClip = 1000;
                            _weaponScript.FMPLNFBLHKJ.reloadTime = 0.001f;
                            _weaponScript.APFMIOFJJNC.recoilPower = 0f;
                            _weaponScript.APFMIOFJJNC.shakeAmount = 0f;
                            _weaponScript.MJIKHNADGAG.reloadTime = 0.02f;
                            _weaponScript.MJIKHNADGAG.waitBeforeReload = 0.01f;
                            _weaponScript.MJIKHNADGAG.initialSpeed = 100;
                            _weaponScript.MMIKHOPCECE.fireRate = 0.001f;
                            _weaponScript.MMIKHOPCECE.bulletsPerClip = 1000;
                            _recoil = false;
                            _weaponScript.HHAMHPCAHNK = false;
                        }
                        else
                        {
                            _weaponScript.FMPLNFBLHKJ.fireRate = 0.05f;
                            _weaponScript.FMPLNFBLHKJ.bulletsPerClip = 7;
                            _weaponScript.FMPLNFBLHKJ.reloadTime = 2f;
                            _weaponScript.APFMIOFJJNC.recoilPower = 0.45f;
                            _weaponScript.APFMIOFJJNC.shakeAmount = 0.6f;
                            _weaponScript.MJIKHNADGAG.reloadTime = 0.5f;
                            _weaponScript.MJIKHNADGAG.waitBeforeReload = 0.5f;
                            _weaponScript.MJIKHNADGAG.initialSpeed = 20;
                            _weaponScript.MMIKHOPCECE.fireRate = 1f;
                            _weaponScript.MMIKHOPCECE.bulletsPerClip = 40;
                            _recoil = true;
                            _weaponScript.HHAMHPCAHNK = true;
                        }
                    }
                    GUILayout.Space(5f);
                    _recoil = GUILayout.Toggle(_recoil, "Recoil", new GUILayoutOption[0]);
                    GUILayout.Space(20f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _weaponMenu = false; 
                        _playerMenu = true; 
                    }
                }
                if (_teamMenu)
                {
                    if (GUILayout.Button("Team <color=red>B</color>", new GUILayoutOption[0]))
                    { 
                        UnityEngine.Object.FindObjectOfType<RoomMultiplayerMenu>().SpawnPlayer("Team B"); 
                    }
                    if (GUILayout.Button("Respawn", new GUILayoutOption[0]))
                    { 
                        UnityEngine.Object.FindObjectOfType<RoomMultiplayerMenu>().RespawnPlayer2(); 
                    }
                    if (GUILayout.Button("Team <color=cyan>A</color>", new GUILayoutOption[0]))
                    { 
                        UnityEngine.Object.FindObjectOfType<RoomMultiplayerMenu>().SpawnPlayer("Team A"); 
                    }
                    GUILayout.Space(20f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { _playerMenu = true; _teamMenu = false; }
                }
                if (_hatMenu)
                {
                    GUILayout.Space(5f);
                    Hat = GUILayout.TextField(Hat, 100, new GUILayoutOption[0]);
                    if (GUILayout.Button("Set Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", Convert.ToInt32(Hat)); 
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Unlock All Hats", new GUILayoutOption[0]))
                    {
                        for (int i = 0; i < 100; i++)
                        { 
                            ObscuredPrefs.SetInt("Hat:" + i.ToString(), 1); 
                        }
                    }
                    GUILayout.Space(10f);
                    if (GUILayout.Button("None Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 90); 
                    }
                    if (GUILayout.Button("Default Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 0); 
                    }
                    if (GUILayout.Button("Ushanka Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 7); 
                    }
                    if (GUILayout.Button("Blue Pumpkin", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 71); 
                    }
                    if (GUILayout.Button("Laser Glasses", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 24); 
                    }
                    if (GUILayout.Button("Pirate Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 28); 
                    }
                    if (GUILayout.Button("Punk", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 25); 
                    }
                    if (GUILayout.Button("English Arm Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 18); 
                    }
                    if (GUILayout.Button("Sharf", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 63); 
                    }
                    if (GUILayout.Button("Top Hat", new GUILayoutOption[0]))
                    { 
                        ObscuredPrefs.SetInt("HatID", 30); 
                    }
                    GUILayout.Space(20f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _playerMenu = true; 
                        _hatMenu = false; 
                    }
                }
                if (_logMenu)
                {
                    GUILayout.Label("Room Message:", new GUILayoutOption[0]);
                    RoomMessage = GUILayout.TextField(RoomMessage, new GUILayoutOption[0]);
                    GUILayout.Label("Option 2(Killer):", new GUILayoutOption[0]);
                    RoomMsgKiller = GUILayout.TextField(RoomMsgKiller, new GUILayoutOption[0]);
                    GUILayout.Label("Option 3(Killed):", new GUILayoutOption[0]);
                    RoomMsgKilled = GUILayout.TextField(RoomMsgKilled, new GUILayoutOption[0]);
                    if (GUILayout.Button("Send", new GUILayoutOption[0]))
                    {
                        SendRoomMessage(RoomMessage, RoomMsgKiller, RoomMsgKilled);
                    }
                    GUILayout.Space(10f);
                    GUILayout.Label("Chat Message:", new GUILayoutOption[0]);
                    ChatMessage = GUILayout.TextField(ChatMessage, new GUILayoutOption[0]);
                    GUILayout.Label("Option 2(Team):", new GUILayoutOption[0]);
                    ChatMsgTeam = GUILayout.TextField(ChatMsgTeam, new GUILayoutOption[0]);
                    if (GUILayout.Button("Send", new GUILayoutOption[0]))
                    { 
                        SendChatMessage(ChatMessage, ChatMsgTeam); 
                    }
                    GUILayout.Space(20f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _mainMenu = true; 
                        _logMenu = false; 
                    }
                }
                if (_monsterMenu)
                {
                    GUILayout.Label("Monster Health:", new GUILayoutOption[0]);
                    MonsterHealth = GUILayout.TextField(MonsterHealth, new GUILayoutOption[0]);
                    if (GUILayout.Button("Set", new GUILayoutOption[0]))
                    { 
                        SetMonsterHealth(Convert.ToInt32(MonsterHealth)); 
                    }
                    GUILayout.Space(10f);
                    GUILayout.Label("Walk & Run Speed: " + _walkRunSpeed.ToString(), new GUILayoutOption[0]);
                    _walkRunSpeed = GUILayout.HorizontalSlider(_walkRunSpeed, 1f, 20f, 
                    new GUILayoutOption[0]);
                    SetMonsterSpeed(_walkRunSpeed);
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Infinity Health: " + _infMonsterHp, new GUILayoutOption[0]))
                    {
                        _infMonsterHp = !_infMonsterHp;
                        if (_infMonsterHp)
                        { 
                            SetMonsterHealth(float.NaN); 
                        }
                        else
                        { 
                            SetMonsterHealth(11500f); 
                        }
                    }
                    if (GUILayout.Button("Disable Power Reload: " + _disablePowerReload, new GUILayoutOption[0]))
                    { 
                        MonsterPowerReload(); 
                    }
                    if (GUILayout.Button("Monster Rapid Fire: " + _monsterRapidFire, new GUILayoutOption[0]))
                    { 
                        MonsterRapidFire(); 
                    }
                    GUILayout.Space(20f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _mainMenu = true; 
                        _monsterMenu = false; 
                    }
                }
                if (_helpMenu)
                {
                    GUILayout.Label("Binds:", new GUILayoutOption[0]);
                    GUILayout.Label($"{_consoleButton} - Open&Close Console",
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_kickConsoleButton} - Open&Close Server Console",
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_infoBarButton} - Open&Close Info Window",
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_mainMenuButton} - Main Menu", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_regenButton} - Regeneration", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_proneButton} - Prone", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_spawnButton} - Spawn Toast & Custards", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_timeButton} - Off&On Time", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_cursorButton} - Off&On Cursor", 
                    new GUILayoutOption[0]);
                    GUILayout.Label($"{_flyButton} - Fly", 
                    new GUILayoutOption[0]);
                    GUILayout.Label("Credits:", new GUILayoutOption[0]);
                    GUILayout.Label("Console by MasterHell", new GUILayoutOption[0]);
                    GUILayout.Label("Code taken from:", new GUILayoutOption[0]);
                    GUILayout.Label("Protivogaz(Fly)", new GUILayoutOption[0]);
                    GUILayout.Label("VantaBlack(IniFile, FOV, Anti-Crash)", new GUILayoutOption[0]);
                    GUILayout.Label("VACAT1ON(Crash, GetPlayerMonster)", new GUILayoutOption[0]);
                    GUILayout.Label("Barsik(Destroy[Loop])", new GUILayoutOption[0]);
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Back", new GUILayoutOption[0]))
                    { 
                        _helpMenu = false; 
                        _mainMenu = true; 
                    }
                }
            }
            if (pageNumber == 1)
            {
                Vector2 zero = Vector2.zero;
                GUILayout.BeginScrollView(zero, new GUILayoutOption[0]);
                if (GUILayout.Button("<b>Power Of <color=blue>Archangel</color></b>", new GUILayoutOption[0]))
                { 
                    MassCrash(); 
                }
                foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
                {
                    GUILayout.BeginHorizontal("Box", new GUILayoutOption[0]);
                    GUILayout.Label(photonPlayer.NickName, new GUILayoutOption[0]);
                    if (GUILayout.Button("Destroy", new GUILayoutOption[0]))
                    { 
                        PhotonNetwork.DestroyPlayerObjects(photonPlayer); 
                    }
                    if (GUILayout.Button("Destroy[Loop]", new GUILayoutOption[0]))
                    { 
                        MelonCoroutines.Start(DestroyLoop(photonPlayer, true)); 
                    }
                    if (GUILayout.Button("TP To", new GUILayoutOption[0]))
                    {
                        GameObject gameObject = GameObject.Find(photonPlayer.name.Split(new char[] { '|' })[0]);
                        _player.transform.position = gameObject.transform.position;
                    }
                    if (GUILayout.Button("Crash", new GUILayoutOption[0]))
                    { 
                        Crash(photonPlayer); 
                    }
                    if (GUILayout.Button("Freeze", new GUILayoutOption[0]))
                    { 
                        Freeze(photonPlayer); 
                    }
                    if (GUILayout.Button("Kill", new GUILayoutOption[0]))
                    { 
                        Kill(photonPlayer); 
                    }
                    if (GUILayout.Button("Inf. HP", new GUILayoutOption[0]))
                    { 
                        InfHealth(photonPlayer); 
                    }
                    if (GUILayout.Button("Grab Nick", new GUILayoutOption[0]))
                    { 
                        GUIUtility.systemCopyBuffer = photonPlayer.NickName; 
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            if (pageNumber == 2)
            {
                Fps = (int)(1f / Time.unscaledDeltaTime);
                if (_cursorObject != null)
                {
                    GUILayout.Label($"NickName: {PhotonNetwork.playerName}", new GUILayoutOption[0]);
                    GUILayout.Label($"Master Server: {PhotonNetwork.masterClient.NickName}", new GUILayoutOption[0]);
                    GUILayout.Label($"ServerSideMaster: {PhotonNetwork.room.serverSideMasterClient}", new GUILayoutOption[0]);
                    GUILayout.Label($"Master ID: {PhotonNetwork.room.MasterClientId}", new GUILayoutOption[0]);
                    GUILayout.Label($"Server Open: {PhotonNetwork.room.IsOpen}", new GUILayoutOption[0]);
                    GUILayout.Label($"Server Visible: {PhotonNetwork.room.IsVisible}", new GUILayoutOption[0]);
                    GUILayout.Label($"Player's Count: {PhotonNetwork.room.PlayerCount} / {PhotonNetwork.room.maxPlayersField}",
                    new GUILayoutOption[0]);
                    GUILayout.Label($"Server Name: {PhotonNetwork.room.Name}", new GUILayoutOption[0]);
                    GUILayout.Label($"Game Mode: {PhotonNetwork.room.customProperties["GM001'"].ToString()}",
                    new GUILayoutOption[0]);
                    GUILayout.Label($"Map: {PhotonNetwork.room.customProperties["MN002'"].ToString()}", new GUILayoutOption[0]);
                    GUILayout.Label($"Ping: {PhotonNetwork.GetPing()}", new GUILayoutOption[0]);
                }
                GUILayout.Label($"FPS: {Fps}", new GUILayoutOption[0]);
                GUILayout.Label($"Custards: {ObscuredPrefs.GetInt("Custards")}", new GUILayoutOption[0]);
                GUILayout.Label($"Hat: {ObscuredPrefs.GetInt("HatID")}", new GUILayoutOption[0]);
                GUILayout.Label($"Server ID: {ObscuredPrefs.GetString("ServerID")}", new GUILayoutOption[0]);
                GUILayout.Label($"Region: {ObscuredPrefs.GetString("ServerName")}", new GUILayoutOption[0]);
                GUILayout.Label($"Server Type: {ObscuredPrefs.GetString("ServerType")}", new GUILayoutOption[0]);
            }
        }

        #region Console

        private void SetMonsterSpeed(float speed)
        {
            GameObject yourPlayerMonster = GetPlayerMonster();
            if (yourPlayerMonster != null)
            {
                BossBot bossBot = yourPlayerMonster.GetComponent<BossBot>();
                if (bossBot != null)
                {
                    bossBot.JCMIJPEBOJI = speed;
                    bossBot.GKFPAHIPDOM = speed;
                }
            }
        }

        private void SetMonsterHealth(float health)
        {
            GameObject yourPlayerMonster = GetPlayerMonster();
            if (yourPlayerMonster != null)
            {
                BossBot bossBot = yourPlayerMonster.GetComponent<BossBot>();
                if (bossBot != null)
                {
                    bossBot.NNIBHOMNJHM = float.NaN;
                    bossBot.PIIDNIGPCDK = float.NaN;
                }
            }
        }

        private void MonsterPowerReload()
        {
            _disablePowerReload = !_disablePowerReload;
            GameObject yourPlayerMonster = GetPlayerMonster();
            if (yourPlayerMonster != null)
            {
                BossBot bossBot = yourPlayerMonster.GetComponent<BossBot>();
                if (bossBot != null)
                {
                    for (int i = 0; i < bossBot.OKJKGBJHKAI.Count; i++)
                    {
                        if (_disablePowerReload)
                        {
                            bossBot.OKJKGBJHKAI[i].timeout = 0f;
                        }
                        else
                        {
                            bossBot.OKJKGBJHKAI[i].timeout = 1f;
                        }
                    }
                }
            }
        }

        private void MonsterRapidFire()
        {
            _monsterRapidFire = !_monsterRapidFire;
            GameObject yourPlayerMonster = GetPlayerMonster();
            if (yourPlayerMonster != null)
            {
                BossBot bossBot = yourPlayerMonster.GetComponent<BossBot>();
                if (bossBot != null)
                {
                    if (_monsterRapidFire)
                    {
                        for (int i = 0; i < bossBot.OKJKGBJHKAI.Count; i++)
                        {
                            bossBot.OKJKGBJHKAI[i].timeout = 0f;
                            bossBot.OKJKGBJHKAI[i].specialWaitDivide = 666f;
                        }
                        for (int i = 2; i < 7; i++)
                        {
                            if (bossBot.ELPJDJFCCNJ.HJPBPFHIIMB[i] != null)
                            {
                                bossBot.ELPJDJFCCNJ.HJPBPFHIIMB[i].speed = 1f * 1000f;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < bossBot.OKJKGBJHKAI.Count; i++)
                        {
                            bossBot.OKJKGBJHKAI[i].timeout = 1f;
                        }
                        for (int i = 2; i < 7; i++)
                        {
                            if (bossBot.ELPJDJFCCNJ.HJPBPFHIIMB[i] != null)
                            {
                                bossBot.ELPJDJFCCNJ.HJPBPFHIIMB[i].speed = 1f;
                            }
                        }
                    }
                }
            }
        }

        private void DestroyAllNPCs()
        {
            foreach (Bot bot in UnityEngine.Object.FindObjectsOfType<Bot>())
            {
                PhotonNetwork.Destroy(bot.gameObject);
            }
            foreach (BossBot bossBot in UnityEngine.Object.FindObjectsOfType<BossBot>())
            {
                PhotonNetwork.Destroy(bossBot.gameObject);
            }
            foreach (CustardBot custardBot in UnityEngine.Object.FindObjectsOfType<CustardBot>())
            {
                PhotonNetwork.Destroy(custardBot.gameObject);
            }
            foreach (PlayerMonster playerMonster in UnityEngine.Object.FindObjectsOfType<PlayerMonster>())
            {
                PhotonNetwork.Destroy(playerMonster.gameObject);
            }
        }

        private void SetHealth(float health)
        {
            _player.GetComponent<PlayerDamage>().NILGDNBIFDC = health;
            _player.GetComponent<PlayerDamage>().PIIDNIGPCDK = health;
        }

        #endregion

        #region KickConsole

        private void Freeze(PhotonPlayer photonPlayer)
        {
            PhotonView photonView = GetPlayer(photonPlayer);
            if (photonView != null)
            {
                photonView.onSerializeTransformOption = OnSerializeTransform.OnlyScale;
                photonView.ownershipTransfer = OwnershipOption.Takeover;
                photonView.ownerId = _player.GetComponent<PhotonView>().ownerId;
            }
        }

        private IEnumerator ArchangelEffect()
        {
            RenderSettings.ambientIntensity = 1000f;
            yield return new WaitForSeconds(0.3f);
            RenderSettings.ambientIntensity = 1f;
        }

        private IEnumerator DestroyLoop(PhotonPlayer photonPlayer, bool value)
        {
            while (value)
            {
                PhotonNetwork.DestroyPlayerObjects(photonPlayer);
            }
            yield return null;
        }

        #endregion

        #region RPC

        private void Kill(PhotonPlayer player)
        {
            Il2CppSystem.Single single = default(Il2CppSystem.Single);
            single.m_value = Il2CppSystem.Single.PositiveInfinity;
            Il2CppSystem.Object[] parameters = new Il2CppSystem.Object[2];
            parameters[0] = single.BoxIl2CppObject();

            PhotonView photonView = GetPlayer(player);
            if (photonView != null)
            {
                photonView.RPC("KOFOOHFOGHL", player, parameters);
            }
        }

        private void InfHealth(PhotonPlayer player)
        {
            Il2CppSystem.Single single = default(Il2CppSystem.Single);
            single.m_value = float.NaN;
            Il2CppSystem.Object[] parameters = new Il2CppSystem.Object[2];
            parameters[0] = single.BoxIl2CppObject();

            PhotonView photonView = GetPlayer(player);
            if (photonView != null)
            {
                photonView.RPC("KOFOOHFOGHL", player, parameters);
            }
        }

        private void Crash(PhotonPlayer currentPlayer)
        {
            Il2CppSystem.Single single = default(Il2CppSystem.Single);
            single.m_value = 1E+09f;
            Il2CppSystem.Object[] parameters = new Il2CppSystem.Object[] { "syncShotGun", single.BoxIl2CppObject() };

            PhotonView photonView = GetPlayer();
            if (photonView != null)
            {
                photonView.RPC("BCPFIMDIMJE", currentPlayer, parameters);
            }
        }

        private void MassCrash()
        {
            MelonCoroutines.Start(ArchangelEffect());
            Il2CppSystem.Single single = default(Il2CppSystem.Single);
            single.m_value = 1E+09f;
            Il2CppSystem.Object[] parameters = new Il2CppSystem.Object[] { "syncShotGun", single.BoxIl2CppObject() };

            PhotonView photonView = GetPlayer();
            if (photonView != null)
            {
                photonView.RPC("BCPFIMDIMJE", PhotonTargets.Others, parameters);
            }
        }

        private void SendRoomMessage(string text, string player = "", string text2 = "")
        {
            PhotonView photonView = GetRoom();
            if (photonView != null)
            {
                Il2CppReferenceArray<Il2CppSystem.Object> parameters = new Il2CppReferenceArray<Il2CppSystem.Object>(new Il2CppSystem.Object[]
                {
                    player,
                    text2,
                    text,
                    string.Empty
                });
                photonView.RPC("networkAddMessage", PhotonTargets.All, parameters);
            }
        }

        private void SendChatMessage(string text, string team = "Team A")
        {
            PhotonView photonView = GetRoom();
            if (photonView != null)
            {
                Il2CppSystem.Object[] arr = new Il2CppSystem.Object[]
                {
                    text,
                    team
                };
                photonView.RPC("OHKPKNCACEK", PhotonTargets.All, arr);
            }
        }

        #endregion

        #region Other

        private GameObject GetPlayerMonster()
        {
            foreach (BossBot bossBot in UnityEngine.Object.FindObjectsOfType<BossBot>())
            {
                CharacterController characterController = bossBot.gameObject.GetComponent<CharacterController>();
                if (characterController != null)
                { 
                    return bossBot.gameObject; 
                }
            }
            return null;
        }

        private PhotonView GetRoom()
        {
            GameObject gameObject = GameObject.Find("__Room");
            if (gameObject != null)
            {
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    return photonView;
                }
            }
            return null;
        }

        private PhotonView GetPlayer(PhotonPlayer photonPlayer)
        {
            GameObject gameObject = GameObject.Find(photonPlayer.name.Split(new char[] { '|' })[0]);
            if (gameObject != null)
            {
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    return photonView;
                }
            }
            return null;
        }

        private PhotonView GetPlayer()
        {
            foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
            {
                GameObject gameObject = GameObject.Find(photonPlayer.name.Split(new char[] { '|' })[0]);
                if (gameObject != null)
                {
                    PhotonView photonView = gameObject.GetComponent<PhotonView>();
                    return photonView;
                }
            }
            return null;
        }

        private void SetupConfig()
        {
            if (!Directory.Exists("UserData/Console"))
            {
                Directory.CreateDirectory("UserData/Console");
            }

            string[,] configKeys = new string[14, 2];
            configKeys[0, 0] = "ConsoleName";
            configKeys[0, 1] = "<color=red>Master</color> Console";
            configKeys[1, 0] = "ConsoleButton";
            configKeys[1, 1] = "F1";
            configKeys[2, 0] = "KickConsoleButton";
            configKeys[2, 1] = "F2";
            configKeys[3, 0] = "InfoBarButton";
            configKeys[3, 1] = "F3";
            configKeys[4, 0] = "MainMenuButton";
            configKeys[4, 1] = "F4";
            configKeys[5, 0] = "SpawnToastButton";
            configKeys[5, 1] = "P";
            configKeys[6, 0] = "RegenButton";
            configKeys[6, 1] = "K";
            configKeys[7, 0] = "ProneButton";
            configKeys[7, 1] = "C";
            configKeys[8, 0] = "TimeButton";
            configKeys[8, 1] = "G";
            configKeys[9, 0] = "CursorButton";
            configKeys[9, 1] = "L";
            configKeys[10, 0] = "FlyButton";
            configKeys[10, 1] = "Z";
            configKeys[11, 0] = "FOV";
            configKeys[11, 1] = "60";
            configKeys[12, 0] = "CrashProtection";
            configKeys[12, 1] = "false";
            configKeys[13, 0] = "ChatMessages";
            configKeys[13, 1] = ":) gg";
            IniFile iniFile = new IniFile("UserData/Console/Config.cfg");

            if (!File.Exists("UserData/Console/Config.cfg"))
            {
                for (int i = 0; i < configKeys.GetLength(0); i++)
                {
                    if (!iniFile.KeyExists(configKeys[i, 0]))
                    {
                        iniFile.Write(configKeys[i, 0], configKeys[i, 1]);
                    }
                }
            }
            else
            {
                if (!Enum.TryParse<KeyCode>(iniFile.Read("ConsoleButton"), out _consoleButton))
                { 
                    _consoleButton = KeyCode.F1;
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("KickConsoleButton"), out _kickConsoleButton))
                { 
                    _kickConsoleButton = KeyCode.F2;
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("InfoBarButton"), out _infoBarButton))
                { 
                    _infoBarButton = KeyCode.F3;
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("MainMenuButton"), out _mainMenuButton))
                { 
                    _mainMenuButton = KeyCode.F4;
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("SpawnToastButton"), out _spawnButton))
                { 
                    _spawnButton = KeyCode.P; 
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("RegenButton"), out _regenButton))
                { 
                    _regenButton = KeyCode.K; 
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("ProneButton"), out _proneButton))
                { 
                    _proneButton = KeyCode.C; 
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("TimeButton"), out _timeButton))
                { 
                    _timeButton = KeyCode.G; 
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("CursorButton"), out _cursorButton))
                { 
                    _cursorButton = KeyCode.L; 
                }
                if (!Enum.TryParse<KeyCode>(iniFile.Read("FlyButton"), out _flyButton))
                { 
                    _flyButton = KeyCode.Z; 
                }
                ConsoleName = iniFile.Read("ConsoleName");
                Fov = Convert.ToSingle(iniFile.Read("FOV"));
                Protection = Convert.ToBoolean(iniFile.Read("CrashProtection"));
                _chatMessages = iniFile.Read("ChatMessages").Split(' ');
            }
        }

        #endregion
    }

    [HarmonyPatch(typeof(PlayerNetworkController), "BCPFIMDIMJE")]
    public static class AntiCrash
    {
        [HarmonyPrefix]
        private static bool Prefix(string JMOEKIHCLBH, float FADNANLHJHF)
        {
            bool result = true;

            if (Console.Protection)
            {
                if (FADNANLHJHF > 100f)
                    result = false;
            }

            return result;
        }
    }

    [HarmonyPatch(typeof(RoomMultiplayerMenu), "OnMasterClientSwitched")]
    public static class AntiMasterClientCrash
    {
        [HarmonyPrefix]
        private static bool Prefix(PhotonPlayer MPGACJLLAJO)
        {
            bool result = true;

            if (Console.Protection)
                result = false;

            return result;
        }
    }

    [HarmonyPatch(typeof(WeaponScript), "Start")]
    public static class FOV
    {
        [HarmonyPostfix]
        private static void Postfix(WeaponScript weaponScript)
        {
            if (Console.Fov > 65f && Console.Fov <= 100f)
            {
                weaponScript.KKCIIEDNLEO = new Vector3(0f, 0f, (65f - Console.Fov) / 75f * -1f);
                weaponScript.FFGIIOODMGK = Console.Fov;
            }
        }
    }
}