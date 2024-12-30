using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Main : MonoBehaviour
{
    public Screen screen;
    bool loopRunning;
    public Sound sound;
    int playingSound = -1;
    public UART uart;
    bool selectText;
    bool textActive;
    bool enterState;
    Textures textures = new Textures();

    player_t player;
    sprite_t enemy;
    
    int BulletSpeed = 10 << 12;
    bullet_t bullet1;
    bullet_t bullet2;
    bullet_t bullet3;
    bullet_t[] myBullets;
    bullet_t bullet4;
    bullet_t bullet5;
    bullet_t bullet6;
    bullet_t[] enemyBullets;

    sprite_t[] sortedSprites;

    bool Flag = false;
    bool RefreshScreen = true;
    bool fillScreen = true; // i did this to set the screen black when the gamestate changes

    bool[] lastButtonState = new bool[3];
    KeyCode[] buttonPins = {KeyCode.W, KeyCode.S, KeyCode.L};

    int mapS = 64;

    int levelAmount = 5;
    level_t level1 = new level_t(16, 16, 512 << 12, 96 << 12, 6434, 512 << 12, 928 << 12, 19302, 32, 32, 4, new int[256] {
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 1, 1, 2, 2, 1, 1, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1,
                                1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 1,
                                1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 1,
                                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 1, 1, 2, 2, 1, 1, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    });
    level_t level2 = new level_t(16, 16, 128 << 12, 512 << 12, 1, 896 << 12, 512 << 12, 12868, 32, 32, 4, new int[256] {
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1,
                                1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    });
    level_t level3 = new level_t(16, 16, 512 << 12, 96 << 12, 6434, 512 << 12, 928 << 12, 19302, 32, 32, 4, new int[256] {
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                                0, 0, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0
    });
    level_t level4 = new level_t(20, 20, 640 << 12, 96 << 12, 6434, 640 << 12, 1184 << 12, 19302, 34, 34, 3, new int[400] {
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 1,
                                1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1,
                                1, 0, 2, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    });
    level_t level5 = new level_t(20, 20, 128 << 12, 96 << 12, 6434, 1152 << 12, 1184 << 12, 19302, 34, 34, 3, new int[400] {
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                                1, 0, 0, 2, 2, 2, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 2, 2, 2, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
                                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    });
    level_t[] levels;

    // different language strings. remember how i said the "language" variable can just be used as an index for an array
    string[] Player_Mode = {"Singleplayer", "Multiplayer", "Un Jugador", "Multijugador"};
    string[] Player_Select = {"Press A Button For\nPlayer 1", "Presione Un Bot\u00A2n\nPara Jugador 1"};
    string[] Level_Select = {"   Select A Map   ", "Seleccione Un Mapa"};
    string[] Wait_Level = {"Player 1 Is\nSelecting A Map", "Jugador 1 Est\uE000\nSeleccionando Un\nMapa"};
    string[] Loading = {"Loading Map", "Cargando Mapa"};
    string[] Lose = {"You Are Dead", "Est\uE000s Muerto"};
    string[] Wait_Dead = {"Player 1 Is\nSelecting The Next\nStep", "Jugador 1 Est\uE000\nSeleccionando El\nEnfoque"};
    string[] Win = {"You Win", "T\u00A3 Ganas"};
    string[] Next_Step = {"Respawn", "New Map", "Revivir", "Nuevo Mapa"};
    string[] Health = {"Life: ", "Vida: "};
    string[] Score = {"Score: ", "Puntuaci\u00A2n: "};

    string[] Connecting = {"Connecting to Photon\nNetwork", "Conexi\u00A2n a Photon\nNetwork"};
    string[] Room = {"Enter A Room Code:\n\n\nAn Existing Code\nWill Join A Room", "Introduce Un C\u00A2digo:\n\n\nUn C\u00A2digo Existente\nPermitir\uE000 Unirse A\nUna Sala"};
    string[] JoinCreate = {"Joining/Creating\nRoom ", "Uni\u0182ndose/Creando\nSala "};
    string[] Waiting = {"Waiting For\nSomeone To Join\nRoom ", "Esperando A Que\nAlguien Se Una A La\nSala "};
    string[] Cancel = {"Press A Button To\nCancel", "Presione Un Bot\u00A2n\nPara Cancela"};

    // this stores the distance of each ray. when a sprite is being drawn, this array is called.
    // if the distance of the sprite is more than the distance of that specific ray, there is a wall in front of it, so it wont draw
    // it makes more sense if you look at the "drawSpriteColumn" method
    int[] depth = new int[64];

    int TurnSpeed = 71; // speaks for itself
    int Rays = 64; // number of vertical lines to draw in between each screen
    int Degree = 71; // degree between each ray

    // i just do this to prevent UART from lagging the game too much
    int messageDelay = 1;
    int messageTimer = 0;

    // it takes two UART messages to send bullet information (position and angle), and a one message is sent every interrupt
    // this means you can shoot faster than the game can send information
    // so this just keeps track of how many bullets are left to send in UART (bulletCount)
    // and whether the current bullet being sent is done or not
    int bulletCount = 0;
    //int bulletStage = 0;

    // yeah
    int hitboxSize = 15;
    int playerLife = 3;
    int myScore = 0;
    int invincibilityTime = 15;
    int invincibilityTimer = 0;
    bool flashScreen = false;
    bool flashed = false;

    int gameState = 0;

    // all the menu values
    int language = 0;
    int multiplayer = 0;
    int level = 0;
    int nextStep = 0;

    // the game changes what to render on the screen depending on this boolean
    bool isPlayer1 = false;

    // Start is called before the first frame update
    void Start()
    {
        player = new player_t(0, 0, 0, 0, 5 << 12, 0, 2, 2);
        enemy = new sprite_t(1, textures.face, 28, 37, 2, 0, 0, 30 << 12, false);

        bullet1 = new bullet_t(new sprite_t(0, textures.bullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        bullet2 = new bullet_t(new sprite_t(0, textures.bullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        bullet3 = new bullet_t(new sprite_t(0, textures.bullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        myBullets = new bullet_t[3] { bullet1, bullet2, bullet3 };
        
        bullet4 = new bullet_t(new sprite_t(0, textures.enemyBullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        bullet5 = new bullet_t(new sprite_t(0, textures.enemyBullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        bullet6 = new bullet_t(new sprite_t(0, textures.enemyBullet, 5, 5, 2, 0, 0, 5 << 12, false), 0, 0);
        enemyBullets = new bullet_t[3] { bullet4, bullet5, bullet6 };

        sortedSprites = new sprite_t[7] {enemy, bullet1.sp, bullet2.sp, bullet3.sp, bullet4.sp, bullet5.sp, bullet6.sp};

        levels = new level_t[5] {level1, level2, level3, level4, level5};
    }

    // Update is called once per frame
    float IQRTimer = 0;
    float IQRPeriod = 0.03333333333f;
    bool[] buttonInputs = new bool[3];
    int ADCinput = 0;
    void Update()
    {
        ADCinput = ADCin();
        for (int i = 0; i < buttonPins.Length; i++) {
            buttonInputs[i] = Input.GetKey(buttonPins[i]);
        }

        if(playingSound != -1) {
            sound.Sound_Play(playingSound);
            playingSound = -1;
        }

        uart.roomManager.roomCode.gameObject.SetActive(textActive);
        if(selectText) {
            uart.roomManager.roomCode.Select();
            uart.roomManager.roomCode.ActivateInputField();
            selectText = false;
        }

        enterState = Input.GetKey(KeyCode.Return);

        if(!loopRunning) {
            try {
                loopRunning = true;
                StartCoroutine(LoopHandler());
            }
            catch (DivideByZeroException ex) {
                Debug.Log("LoopHandler: " + ex.Message);
            }
        }

        if (IQRCheck()) {
            //Debug.Log(Time.realtimeSinceStartup - IQRTimer);
            try {
                IQRHandler();
            }
            catch (DivideByZeroException ex) {
                Debug.Log("IQRHandler: " + ex.Message);
            }
            IQRTimer = Time.realtimeSinceStartup;
        }
    }

    bool IQRCheck() {
        return Time.realtimeSinceStartup - IQRTimer >= IQRPeriod;
    }

    IEnumerator LoopHandler() {
        // Read UART
        if(multiplayer != 0) {
            char letter = uart.UART2_InChar();

            // so because of the way the UART is sent in our code,
            // in now send 0xFF if it is empty, because 0x00 is a possible command that can be sent
            while(letter != 0xFF) {
                // gets number from uart
                int value = ((letter & 0x0F) << 24) | GetValue();

                // depending on state and command, it does different things with the value
                if(gameState == 2) {
                    if(((letter & 0xF0) >> 4) == 0b0000) { // player1 chosen
                        if(value == 0) {
                            isPlayer1 = false;
                            enemy.texture = textures.face2;
                            gameState = 3;
                            fillScreen = true;
                            RefreshScreen = true;
                        }
                    }
                }
                else if(gameState == 3) {
                    if(!isPlayer1) {
                        if(((letter & 0xF0) >> 4) == 0b0101) { // level selected (only if you are player2)
                            level = value;
                            gameState = 4;
                            fillScreen = true;
                            RefreshScreen = true;
                        }
                    }
                }
                else if(gameState == 5) {
                    if(((letter & 0xF0) >> 4) == 0b0001) { // enemy moved
                        int xValue = (value & 0x0FFFC000) >> 2;
                        int yValue = (value & 0x00003FFF) << 12;

                        enemy.x = xValue;
                        enemy.y = yValue;

                        RefreshScreen = true;
                    }
                    else if(((letter & 0xF0) >> 4) == 0b0010) { // first stage of bullet sent (position)
                        for(int i = 0; i < 3; i++) {
                            if(enemyBullets[i].sp.state == 0) {
                                int xValue = (value & 0x0FFFC000) >> 2;
                                int yValue = (value & 0x00003FFF) << 12;

                                enemyBullets[i].sp.x = xValue;
                                enemyBullets[i].sp.y = yValue;

                                break;
                            }
                        }
                    }
                    else if(((letter & 0xF0) >> 4) == 0b0011) { // second stage of bullet (can now spawn bullet. is done sending)
                        for(int i = 0; i < 3; i++) {
                            if(enemyBullets[i].sp.state == 0) {
                                fraction_t CS = Sin(6434 - value);
                                fraction_t SN = Sin(value);

                                enemyBullets[i].dx = (BulletSpeed * CS.numerator) / CS.denominator;
                                enemyBullets[i].dy = (BulletSpeed * SN.numerator) / SN.denominator;

                                enemyBullets[i].sp.state = 1;
                                enemyBullets[i].sp.wasDrawn = false;
                                CalculateSprite(enemyBullets[i].sp);

                                RefreshScreen = true;

                                break;
                            }
                        }
                    }
                    else if(((letter & 0xF0) >> 4) == 0b0100) { // enemy died. you win
                        if(value == 0) {
                            myScore++;
                            gameState = 7;
                            fillScreen = true;
                            RefreshScreen = true;
                        }
                    }
                }
                else if(gameState == 6 || gameState == 7) {
                    if(!isPlayer1) {
                        if(((letter & 0xF0) >> 4) == 0b0110) { // player1 choosing next step in game
                            if(value == 0) {
                                gameState = 4;
                            }
                            else {
                                gameState = 3;
                            }
                            fillScreen = true;
                            RefreshScreen = true;
                        }
                    }
                }

                if (IQRCheck()) {
                    yield return null;
                }

                letter = uart.UART2_InChar(); // test if commands still being send
            }
        }

        if(Flag) {
            Flag = false;

            // fills screen if needed
            if(fillScreen) {
                fillScreen = false;
                screen.ST7735_FillScreen(screen.ST7735_BLACK);
            }

            if(RefreshScreen) {
                RefreshScreen = false;

                if(gameState == 0) { // language select
                    DrawImage(textures.title, 20, 32, 22, 13, 4);

                    screen.ST7735_SetCursor(14, 11);
                    screen.printf(" ");
                    screen.ST7735_SetCursor(14, 12);
                    screen.printf(" ");
                    screen.ST7735_SetCursor(14, 11 + language);
                    screen.printf("*");
                    screen.ST7735_SetCursor(6, 11);
                    screen.printf("English");
                    screen.ST7735_SetCursor(6, 12);
                    screen.printf("Espa\u00A4ol");
                }
                else if(gameState == 1) { // player mode select
                    screen.ST7735_FillRect(8, 24, 112, 74, screen.ST7735_BLACK);
                    if(multiplayer != 0) {
                        DrawImage(textures.multi, 8, 24, 56, 37, 2);
                    }
                    else {
                        DrawImage(textures.face2, 36, 24, 28, 37, 2);
                    }

                    screen.ST7735_SetCursor(17, 11);
                    screen.printf(" ");
                    screen.ST7735_SetCursor(17, 12);
                    screen.printf(" ");
                    screen.ST7735_SetCursor(17, 11 + multiplayer);
                    screen.printf("*");
                    screen.ST7735_SetCursor(4, 11);
                    screen.printf(Player_Mode[language * 2]);
                    screen.ST7735_SetCursor(4, 12);
                    screen.printf(Player_Mode[1 + (language * 2)]);
                }
                else if(gameState == 2) { // choose player1
                    DrawImage(textures.button, 32, 72, 16, 8, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Player_Select[language]);
                }
                else if(gameState == 3) { // player1 choose map
                    if(isPlayer1) {
                        screen.ST7735_SetCursor(2, 12);
                        screen.printf(Level_Select[language]);

                        DrawMap2D();
                    }
                    else {
                        DrawImage(textures.clock, 20, 52, 22, 22, 4);
                        screen.ST7735_SetCursor(0, 0);
                        screen.printf(Wait_Level[language]);
                    }
                }
                else if(gameState == 4) { // says loading level (just in case it breaks and i know where it broke)
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Loading[language]);
                }
                else if(gameState == 5) { // renders game
                    if(flashScreen) {
                        screen.ST7735_InvertDisplay(true);
                        flashed = true;
                    }
                    else {
                        screen.ST7735_InvertDisplay(false);
                    }

                    for(int i = 0; i < 3; i++) {
                        CalculateSprite(myBullets[i].sp);
                        CalculateSprite(enemyBullets[i].sp);
                    }
                    CalculateSprite(enemy);

                    sortSprites(sortedSprites, 0, 6);

                    yield return StartCoroutine(DrawRays());

                    /*for(int i = 0; i < 3; i++) {
                        DrawSprite(&myBullets[i].sp);
                        DrawSprite(&enemyBullets[i].sp);
                    }*/
                    //DrawSprite(&enemy);

                    screen.ST7735_SetCursor(1, 0);
                    screen.printf(Health[language] + playerLife);

                    DrawScore();
                    //DrawMap2D();
                    //ST7735_FillRect((levels[level].xOffset + ((player.x >> 12) / (mapS / levels[level].scale))) - (player.w / 2), (levels[level].yOffset + ((player.y >> 12) / (mapS / levels[level].scale))) - (player.h / 2), player.w, player.h, ST7735_Color565(255, 255, 0));
                }
                else if(gameState == 6) { // lose
                    screen.ST7735_InvertDisplay(false);
                    DrawImage(textures.grave, 0, 68, 32, 23, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Lose[language]);

                    DrawNextStep();

                    DrawScore();
                }
                else if(gameState == 7) { // win
                    screen.ST7735_InvertDisplay(false);
                    DrawImage(textures.trophy, 0, 68, 32, 23, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Win[language]);

                    DrawNextStep();

                    DrawScore();
                }
                else if (gameState == 8) { // connecting to PhotonNetwork
                    DrawImage(textures.wifi, 16, 66, 24, 19, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Connecting[language]);
                    screen.ST7735_SetCursor(0, 3);
                    screen.printf(Cancel[language]);
                }
                else if (gameState == 9) { // enter a room code
                    DrawImage(textures.clipboard, 32, 64, 16, 22, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Room[language]);
                }
                else if (gameState == 10) { // joining/creating room
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(JoinCreate[language] + uart.roomManager.roomCode.text);
                }
                else if (gameState == 11) { // waiting for player to join
                    DrawImage(textures.controllers, 20, 74, 21, 17, 4);
                    screen.ST7735_SetCursor(0, 0);
                    screen.printf(Waiting[language] + uart.roomManager.roomCode.text);
                    screen.ST7735_SetCursor(0, 4);
                    screen.printf(Cancel[language]);
                }
            }
        }
        loopRunning = false;
    }

    void IQRHandler() {
        if(gameState == 0) { // selecting language
            if(!lastButtonState[0] && buttonInputs[0]) { // Up
                if(language > 0) {
                    language--;

                    RefreshScreen = true;
                }
            }
            if(!lastButtonState[1] && buttonInputs[1]) { // Down
                if(language < 1) {
                    language++;

                    RefreshScreen = true;
                }
            }
            if(!lastButtonState[2] && buttonInputs[2]) { // Select
                gameState = 1;
                fillScreen = true;
                RefreshScreen = true;
            }
        }
        else if(gameState == 1) { // select player mode
            if(!lastButtonState[0] && buttonInputs[0]) { // Up
                if(multiplayer > 0) {
                    multiplayer--;

                    RefreshScreen = true;
                }
            }
            if(!lastButtonState[1] && buttonInputs[1]) { // Down
                if(multiplayer < 1) {
                    multiplayer++;

                    RefreshScreen = true;
                }
            }
            if(!lastButtonState[2] && buttonInputs[2]) { // Select
                if(multiplayer != 0) {
                    gameState = 8;
                    uart.networkConnection.Connect();
                }
                else { // if you're playing singleplayer, you don't need to decide who player1 is so automatically go to level select
                    gameState = 3;
                    isPlayer1 = true;
                }
                fillScreen = true;
                RefreshScreen = true;
            }
        }
        else if(gameState == 2) {
            // just tests for each button
            bool buttonPressed = false;
            for(int i = 0; i < 3; i++) {
                if(!lastButtonState[i] && buttonInputs[i]) {
                    buttonPressed = true;
                }
            }
            if(buttonPressed) {
                isPlayer1 = true;
                if(multiplayer != 0) { // honestly this if statement is useless because you can only get to this gamestate if you selected multiplayer
                    uart.UART1_Output(0);
                }
                gameState = 3;
                fillScreen = true;
                RefreshScreen = true;
            }
        }
        else if(gameState == 3) { // select map
            if(isPlayer1) {
                if(!lastButtonState[0] && buttonInputs[0]) { // Up
                    if(level > 0) {
                        level--;

                        RefreshScreen = true;
                    }
                }
                if(!lastButtonState[1] && buttonInputs[1]) { // Down
                    if(level < levelAmount - 1) {
                        level++;

                        RefreshScreen = true;
                    }
                }
                if(!lastButtonState[2] && buttonInputs[2]) { // Select
                    //set level
                    if(multiplayer != 0) { // output to player2 which level you selected
                        uart.UART1_Output(0x50000000 | level);
                    }
                    gameState = 4;
                    fillScreen = true;
                    RefreshScreen = true;
                }
            }
        }
        else if(gameState == 4) { // load level
            // just setting all the necessary values
            if(isPlayer1) {
                player.x = levels[level].player1X;
                player.y = levels[level].player1Y;
                player.a = levels[level].player1a;

                enemy.x = levels[level].player2X;
                enemy.y = levels[level].player2Y;
            }
            else {
                player.x = levels[level].player2X;
                player.y = levels[level].player2Y;
                player.a = levels[level].player2a;

                enemy.x = levels[level].player1X;
                enemy.y = levels[level].player1Y;
            }

            for(int i = 0; i < 3; i++) {
                myBullets[i].sp.state = 0;

                if(multiplayer != 0) {
                    enemyBullets[i].sp.state = 0;
                }
            }
            bulletCount = 0;

            ResetLevel(level);

            playerLife = 3;
            invincibilityTimer = 0;
            flashScreen = false;
            flashed = false;

            nextStep = 0;

            gameState = 5;

            fillScreen = true;
            RefreshScreen = true;
        }
        else if(gameState == 5) { // playing game
            if(invincibilityTimer > 0) {
                invincibilityTimer--;
            }
            if(flashScreen && flashed) {
                flashScreen = false;
                flashed = false;
            }

            // Reading Slide Pot (Turn)
            int data = ADCinput;
            if(data < 1638) {
                player.a += TurnSpeed * ((1638 - data) / 200); // the further you slide it to one side, the faster it goes. 200 was just a number i liked.
                if(player.a > 25736) {
                    player.a -= 25736;
                }
            }
            else if(data > 2458) {
                player.a -= TurnSpeed * ((data - 2458) / 200);
                if(player.a < 0) {
                    player.a += 25736;
                }
            }

            bool playerMoved = false;

            // Reading button inputs (Move)
            if(lastButtonState[0] || lastButtonState[1]) {
                // take the sin and cos of the player angle
                fraction_t CS = Sin(6434 - player.a);
                fraction_t SN = Sin(player.a);

                // finds how much you move
                player.dx = (player.s * CS.numerator) / CS.denominator;
                player.dy = (player.s * SN.numerator) / SN.denominator;

                // 20 is also just a number i liked. it means you cannot be 20 units close to a wall
                // it is not the player velocity just because if i increased the velocity by a lot, the distance you are from the walls stay constant
                int xo = 20 << 12; // bit shifted because my x and y position are 12 bit fixed point
                if(player.dx < 0) {
                    xo *= -1;
                }
                int yo = 20 << 12;
                if(player.dy < 0) {
                    yo *= -1;
                }

                // variables used to calculate which index of the level you would be in using the 20 unit offset
                int ipx = (player.x >> 12) / mapS;
                int ipx_add_xo = ((player.x + xo) >> 12) / mapS;
                int ipx_sub_xo = ((player.x - xo) >> 12) / mapS;

                int ipy = (player.y >> 12) / mapS;
                int ipy_add_yo = ((player.y + yo) >> 12) / mapS;
                int ipy_sub_yo = ((player.y - yo) >> 12) / mapS;

                // testing if offset is in a empty level index
                // if it is empty (no walls in their way), the player is allowed to move
                // we test the x and y movement separately to allow the player to slide along the walls if only one offset hits a wall
                if(lastButtonState[0]) {
                    if(levels[level].map[ipy * levels[level].mapX + ipx_add_xo] <= 0) {
                        player.x += player.dx;
                        playerMoved = true;
                    }
                    if(levels[level].map[ipy_add_yo * levels[level].mapX + ipx] <= 0) {
                        player.y += player.dy;
                        playerMoved = true;
                    }
                }
                if(lastButtonState[1]) {
                    if(levels[level].map[ipy * levels[level].mapX + ipx_sub_xo] <= 0) {
                        player.x -= player.dx;
                        playerMoved = true;
                    }
                    if(levels[level].map[ipy_sub_yo * levels[level].mapX + ipx] <= 0) {
                        player.y -= player.dy;
                        playerMoved = true;
                    }
                }
            }

            // Reading Button Inputs (Shoot)
            if(!lastButtonState[2] && buttonInputs[2]) { // Shoot
                for(int i = 0; i < 3; i++) {
                    if(myBullets[i].sp.state == 0) { // bullet is dead
                        // set bullet position to current player position
                        /*myBullets[i].sp.x = player.x;
                        myBullets[i].sp.y = player.y;*/
                        myBullets[i].sp.x = (player.x >> 12) << 12;
                        myBullets[i].sp.y = (player.y >> 12) << 12;

                        // calculate the x and y velocity of the player
                        // the bullets should always travel in a straight line, should it can only be done once
                        fraction_t CS = Sin(6434 - player.a);
                        fraction_t SN = Sin(player.a);

                        myBullets[i].dx = (BulletSpeed * CS.numerator) / CS.denominator;
                        myBullets[i].dy = (BulletSpeed * SN.numerator) / SN.denominator;

                        myBullets[i].sp.state = 1;
                        myBullets[i].sp.wasDrawn = false;

                        // i call calculatesprite here because if the while loop is in the middle of drawing the sprites,
                        // the screen variables are no longer accurate, so now it will draw the bullet correctly
                        CalculateSprite(myBullets[i].sp);

                        if(multiplayer != 0) {
                            uart.UART1_Output(0x20000000 | ((player.x << 2) & 0x0FFFC000) | ((player.y >> 12) & 0x3FFF));
                            uart.UART1_Output(0x30000000 | player.a);
                            bulletCount++;
                        }

                        playingSound = 0;

                        break;
                    }
                }
            }

            // testing alive bullets
            for(int i = 0; i < 3; i++) {
                if(myBullets[i].sp.state == 1) {
                    // move bullet
                    myBullets[i].sp.x += myBullets[i].dx;
                    myBullets[i].sp.y += myBullets[i].dy;

                    if(Distance(myBullets[i].sp.x, myBullets[i].sp.y, enemy.x, enemy.y) < hitboxSize) { // this does nothing on your side. this is only to remove the your bullet. only the enemy chooses whether the bullet hit you or not
                        myBullets[i].sp.state = 0;

                        if(multiplayer == 0) { // if it is singleplayer, end the game early. i did this just so you can atleast explore the maps if you can't play with someone
                            myScore++;
                            gameState = 7;
                            fillScreen = true;
                        }
                    }

                    // tests if the bullet hit a wall
                    int index = ((myBullets[i].sp.y >> 12) / mapS) * levels[level].mapX + ((myBullets[i].sp.x >> 12) / mapS);
                    if(index < 0 || index > (levels[level].mapX * levels[level].mapY) - 1 || levels[level].map[index] > 0) {
                        if(levels[level].map[index] == 2) {
                            // we set it to negative one instead of 0, so when the menu renders the level, it will not show the empty block
                            // also so "resetlevel" knows which block to turn it into
                            levels[level].map[index] = -1;
                        }
                        myBullets[i].sp.state = 0;
                    }
                }

                // testing multiplayer bullets
                if(multiplayer != 0) {
                    // same exact thing as regular bullets
                    if(enemyBullets[i].sp.state == 1) {
                        enemyBullets[i].sp.x += enemyBullets[i].dx;
                        enemyBullets[i].sp.y += enemyBullets[i].dy;

                        if(Distance(enemyBullets[i].sp.x, enemyBullets[i].sp.y, player.x, player.y) < hitboxSize) {
                            if(invincibilityTimer == 0) {
                                if(playerLife > 0) {
                                    playerLife--;
                                    invincibilityTimer = invincibilityTime;
                                    flashScreen = true;
                                    playingSound = 1;
                                }
                            }

                            enemyBullets[i].sp.state = 0;
                        }

                        int index = ((enemyBullets[i].sp.y >> 12) / mapS) * levels[level].mapX + ((enemyBullets[i].sp.x >> 12) / mapS);
                        if(index < 0 || index > (levels[level].mapX * levels[level].mapY) - 1 || levels[level].map[index] > 0) {
                            if(levels[level].map[index] == 2) {
                                levels[level].map[index] = -1;
                            }

                            enemyBullets[i].sp.state = 0;
                        }
                    }
                }
            }

            // send multiplayer messages
            if(multiplayer != 0) {
                messageTimer++;

                if(messageTimer > messageDelay) {
                    bool messageSent = false;

                    if(playerLife == 0) { // tell other player they win
                        // end round
                        uart.UART1_Output(0x40000000);
                        fillScreen = true;
                        gameState = 6;
                    }
                    else {
                        if(bulletCount > 0) { // send bullets. prioritize this first before movement
                            /*if(bulletStage == 0) {
                                uart.UART1_Output(0x20000000 | ((player.x << 2) & 0x0FFFC000) | ((player.y >> 12) & 0x3FFF));
                                bulletStage++;
                            }
                            else if(bulletStage == 1) {
                                uart.UART1_Output(0x30000000 | player.a);
                                bulletCount--;
                                bulletStage = 0;
                            }*/
                            bulletCount--;
                            messageSent = true;
                        }
                        //else {
                        if(playerMoved) { // send new player position. the fixed point is really only used for calculates, so to save the amount of commands sent, we remove the fixed point
                            uart.UART1_Output(0x10000000 | ((player.x << 2) & 0x0FFFC000) | ((player.y >> 12) & 0x3FFF));
                            messageSent = true;
                        }
                        //}

                        if(messageSent) { // resets timer (remember this is used to prevent lag)
                            messageTimer = 0;
                        }
                    }
                }
            }

            RefreshScreen = true;
        }
        else if(gameState == 6 || gameState == 7) { // player won or lost
            if(isPlayer1) {
                if(!lastButtonState[0] && buttonInputs[0]) { // Up
                    if(nextStep > 0) {
                        nextStep--;

                        RefreshScreen = true;
                    }
                }
                if(!lastButtonState[1] && buttonInputs[1]) { // Down
                    if(nextStep < 1) {
                        nextStep++;

                        RefreshScreen = true;
                    }
                }
                if(!lastButtonState[2] && buttonInputs[2]) { // Select
                    if(multiplayer != 0) { // sent to other player so they can also change their menu screen
                        uart.UART1_Output(0x60000000 | nextStep);
                    }

                    if(nextStep == 0) { // respawn
                        gameState = 4;
                    }
                    else { // new map
                        gameState = 3;
                    }

                    fillScreen = true;
                    RefreshScreen = true;
                }
            }
        }
        else if (gameState == 8) {
            bool buttonPressed = false;
            for(int i = 0; i < 3; i++) {
                if(!lastButtonState[i] && buttonInputs[i]) {
                    buttonPressed = true;
                }
            }
            if(buttonPressed) {
                uart.networkConnection.Cancel();
                gameState = 1;
                fillScreen = true;
                RefreshScreen = true;
            }
        }
        else if (gameState == 9) {
            if (enterState) {
                uart.roomManager.JoinRoom();
                gameState = 10;
                textActive = false;
                fillScreen = true;
            }
            else {
                textActive = true;
                selectText = true;
            }
            RefreshScreen = true;
        }
        else if (gameState == 11) {
            bool buttonPressed = false;
            for(int i = 0; i < 3; i++) {
                if(!lastButtonState[i] && buttonInputs[i]) {
                    buttonPressed = true;
                }
            }
            if(buttonPressed) {
                uart.roomManager.LeaveRoom();
                gameState = 9;
                fillScreen = true;
                RefreshScreen = true;
            }
        }

        for(int i = 0; i < lastButtonState.Length; i++) {
            lastButtonState[i] = buttonInputs[i];
        }

        Flag = true;
    }

    // each multiplayer message uses the full 32-bits
    int GetValue() {
        int value = 0;

        value |= uart.UART2_InChar() << 16;
        value |= uart.UART2_InChar() << 8;
        value |= uart.UART2_InChar();

        return value;
    }

    public void SetGameState(int state) {
        gameState = state;
        fillScreen = true;
        RefreshScreen = true;
    }

    void DrawNextStep() {
        if(isPlayer1) {
            screen.ST7735_SetCursor(11, 2);
            screen.printf(" ");
            screen.ST7735_SetCursor(11, 3);
            screen.printf(" ");
            screen.ST7735_SetCursor(11, 2 + nextStep);
            screen.printf("*");
            screen.ST7735_SetCursor(0, 2);
            screen.printf(Next_Step[language * 2]);
            screen.ST7735_SetCursor(0, 3);
            screen.printf(Next_Step[1 + (language * 2)]);
        }
        else {
            screen.ST7735_SetCursor(0, 2);
            screen.printf(Wait_Dead[language]);
        }
    }

    void DrawScore() {
        screen.ST7735_SetCursor(17, 0);
        screen.printf(("" + myScore).PadLeft(3, ' '));
    }

    void DrawSpriteColumn(Color[] buffer, sprite_t sp, int ray) {
        if(sp.state != 0) {
            // rememer that you only need to draw a sprite if its infront of you. saves execution time
            if(sp.b >> 12 > 0) {
                // honestly, the math for this is reallyyyy weird
                // i used to draw the entire sprite at once, which uses a similar method as "drawimage", but it caused flickering
                // so i reverse engineered my program which calculated all the needed pixels at once, into only the needed pixels for the current column
                // if you asked to me recreate this program in one hour or die, tell my family goodbye :]
                int screenX = ray * (128 / Rays);

                if(screenX > sp.sx - (((sp.xScale2 / 2) * sp.ps) >> 12) && screenX < sp.sx + (((sp.xScale2 / 2) * sp.ps) >> 12)) {
                    int x = (screenX - sp.sx) / sp.ps;

                    if(sp.b >> 12 < depth[(sp.sx + (x * sp.ps)) / (128 / Rays)]) {
                        sp.t_x = (sp.t_x_step * (((sp.xScale2 / 2) >> 12) + x)) >> 12;
                        sp.t_y = (sp.yScale - 1) << 12;

                        for(int y = 0; y < (sp.yScale2 >> 12); y++) {
                            if(sp.sy - (y * sp.ps) > -1 && sp.sy - (y * sp.ps) < 160) {
                                int pixel = ((sp.t_y >> 12) * sp.xScale + sp.t_x) * 3;
                                int red = sp.texture[pixel];
                                int green = sp.texture[pixel + 1];
                                int blue = sp.texture[pixel + 2];

                                if(!(red == 255 && green == 0 && blue == 255)) {
                                    int currentY = sp.sy - (y * sp.ps);

                                    //int color = ST7735_Color565(red * sp.shade, green * sp.shade, blue * sp.shade);
                                    Color color = new Color(red / 255f, green / 255f, blue / 255f);
                                    int index = ((160 - sp.ps) - currentY) * sp.ps;
                                    for(int p = 0; p < sp.ps * sp.ps; p++) {
                                        if (index + p > -1 && index + p < buffer.Length) {
                                            buffer[index + p] = color;
                                        }
                                    }
                                    //ST7735_FillRect(screenX, currentY, sp.ps, sp.ps, ST7735_Color565(red * sp.shade, green * sp.shade, blue * sp.shade));
                                }

                                sp.wasDrawn = true;
                            }
                            sp.t_y -= sp.t_y_step;

                            if(sp.t_y < 0) {
                                sp.t_y = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DrawRays() {
        // i'm just gonna type it all out here so i don't gotta read much:

        // CASTING RAY:
        // given the player angle, a ray is casted from the player
        // an offset is calculated to see how much to increment the ray by

        // TESTING RAY:
        // so, since the map is a matrix, the walls can either be vertical or horizontal (from a top-down view)
        // we have to test how far it takes for the ray to hit BOTH a vertical and horizontal wall
        // because of how your eyes work, the closest wall will be drawn

        // DRAWING WALL:
        // so when you get a texture, you already know how tall the wall will appear on the screen, you you just scale your wall texture to fit it
        // but since you're drawing your walls each vertical line by line, you have to calculate which x coordinate of your texture will the ray display
        // i don't wanna have to explain the math.

        int mx, my, mp, dof; // used to calculate which map index the ray is hitting
        mx = my = mp = dof = 0;
        int rx, ry, ra, xo, yo; // used to calculate the ray's position in the map itself. not the index of the level
        rx = ry = ra = xo = yo = 0;
        int disT = 0; // distance of the ray
        int mt = 1; // map type. this is used to determine which wall texture to draw
        int vmt = 0;
        int hmt = 0;

        // this is just a poor programming design
        // there was a bug where the screen is refreshed when a sprite is on screen
        // but when is leaves the screen, it is no longer refreshed
        // so the the sprite will still appear on your screen since it hasn't been refreshed
        // so this is just a janky way of testing if i should draw the sprite or not if it is no longer on the screen
        for(int i = 0; i < 7; i++) {
            sortedSprites[i].wasDrawn = false;
        }

        ra = player.a - ((Rays / 2) * Degree);
        if(ra < 0) {
            ra += 25736;
        }
        if(ra > 25736) {
            ra -= 25736;
        }

        for(int r = 0; r < Rays; r++) {
            fraction_t CS = Sin(6434 - ra);
            fraction_t SN = Sin(ra);

            //---Horizontal Lines---
            dof = 0;
            int disH = 1000000000;
            int hx = player.x, hy = player.y;

            // uhhh so if you are drawing a horizontal ray, it will techincally never hit a horizontal line
            // so just dont draw the ray if it is horizontal. that's the if statment. the addition of the "Degree" is to allow wiggle room
            if(!((ra > 0 - Degree && ra < 0 + Degree) || (ra > 12868 - Degree && ra < 12868 + Degree))) {
                fraction_t aTan = new fraction_t(-CS.numerator * SN.denominator, CS.denominator * SN.numerator);

                // calculate ray offset
                if(ra > 12868) { // up
                    ry = ((((player.y >> 12) / mapS) * mapS) << 12) - 4;
                    rx = (((player.y - ry) * aTan.numerator) / aTan.denominator) + player.x;
                    yo = -(mapS << 12);
                    xo = (-yo * aTan.numerator) / aTan.denominator;
                }
                else if(ra < 12868) { // down
                    ry = ((((player.y >> 12) / mapS) * mapS) << 12) + (mapS << 12);
                    rx = (((player.y - ry) * aTan.numerator) / aTan.denominator) + player.x;
                    yo = mapS << 12;
                    xo = (-yo * aTan.numerator) / aTan.denominator;
                }

                // testing ray collision
                while(dof < levels[level].mapX) {
                    mx = (rx >> 12) / mapS;
                    my = (ry >> 12) / mapS;
                    mp = my * levels[level].mapX + mx;

                    if(mp >= 0 && mp < (levels[level].mapX * levels[level].mapY) && levels[level].map[mp] > 0) {
                        hmt = levels[level].map[mp];
                        hx = rx;
                        hy = ry;
                        disH = Distance(player.x, player.y, hx, hy);
                        dof = levels[level].mapX;
                    }
                    else {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }
            }
            //ST7735_FillRect((levels[level].xOffset + ((rx >> 12) / (mapS / levels[level].scale))) - 1, (levels[level].yOffset + ((ry >> 12) / (mapS / levels[level].scale))) - 1, 2, 2, ST7735_Color565(255, 0, 255));

            //---Vertical Lines---
            dof = 0;
            int disV = 1000000000;
            int vx = player.x, vy = player.y;
            if(!((ra > 6434 - Degree && ra < 6434 + Degree) || (ra > 19302 - Degree && ra < 19302 + Degree))) {
                fraction_t nTan = new fraction_t(-SN.numerator * CS.denominator, SN.denominator * CS.numerator);
                if(ra > 6434 && ra < 19302) { // left
                    rx = ((((player.x >> 12) / mapS) * mapS) << 12) - 4;
                    ry = (((player.x - rx) * nTan.numerator) / nTan.denominator) + player.y;
                    xo = -(mapS << 12);
                    yo = (-xo * nTan.numerator) / nTan.denominator;
                }
                else if(ra < 6434 || ra > 19302) { // right
                    rx = ((((player.x >> 12) / mapS) * mapS) << 12) + (mapS << 12);
                    ry = (((player.x - rx) * nTan.numerator) / nTan.denominator) + player.y;
                    xo = mapS << 12;
                    yo = (-xo * nTan.numerator) / nTan.denominator;
                }

                while(dof < levels[level].mapY) {
                    mx = (rx >> 12) / mapS;
                    my = (ry >> 12) / mapS;
                    mp = my * levels[level].mapX + mx;

                    if(mp >= 0 && mp < (levels[level].mapX * levels[level].mapY) && levels[level].map[mp] > 0) {
                        vmt = levels[level].map[mp];
                        vx = rx;
                        vy = ry;
                        disV = Distance(player.x, player.y, vx, vy);
                        dof = levels[level].mapY;
                    }
                    else {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }
            }
            //ST7735_FillRect((levels[level].xOffset + ((rx >> 12) / (mapS / levels[level].scale))) - 1, (levels[level].yOffset + ((ry >> 12) / (mapS / levels[level].scale))) - 1, 2, 2, ST7735_Color565(255, 255, 0));

            // it just makes the horizontal lines darker
            int shade1 = 0;
            //fraction_t shade2 = {1, 1};

            // sets whatever values needed based on which wall is closer
            if(disV < disH) {
                mt = vmt;
                rx = vx;
                ry = vy;
                disT = disV;
            }
            else {
                mt = hmt;
                rx = hx;
                ry = hy;
                disT = disH;

                shade1 = 1;
            }

            // remember that we store this to test with sprites
            depth[r] = disT;

            //---Draw Walls---
            // this fixes a fish eye effect. i would explain why it happens, but its hard to put into words
            // just comment it out and you'll see what happens
            int ca = player.a - ra;
            if(ca < 0) {
                ca += 25736;
            }
            if(ca > 25736) {
                ca -= 25736;
            }
            fraction_t FCS = Sin(6434 - ca);
            disT = (disT * FCS.numerator) / FCS.denominator;
            // if you do comment the fish eye fix out, stop here

            // find line height on screem
            int lineH = Int32.MaxValue;
            if (disT > 0) {
                lineH = (mapS * 160) / disT;
            }

            // scale wall texture using wall height
            int ty_step = (16 << 12) / lineH;
            int ty_off = 0;

            // set limits to how large the wall can appear on screen
            if(lineH > 160) {
                ty_off = (lineH - 160) / 2;
                lineH = 160;
            }
            int lineO = 80 - (lineH >> 1);

            // create a temporary bitmap to draw to screen
            Color[] bitmap = new Color[326];
            int ty = ty_off * ty_step; //+ (((mt - 1) * 16) << 12);
            int tx; // (rx / (mapS / tres)) % tres

            // ehhhhh if it's horizontal wall, flip the texture
            // i don't wanna explain why this happens and why the code is needed
            if(shade1 == 1) {
                tx = (rx >> 14) % 16;
                if(ra < 12868) {
                    tx = 15 - tx;
                }
            }
            else {
                tx = (ry >> 14) % 16;
                if(ra > 6434 && ra < 19302) {
                    tx = 15 - tx;
                }
            }

            // offset is to know which texture to access, because wall textures are in a single array
            int mapOffset = (mt - 1) * 768; // 768 = 16 * 16 * 3 (16x16 texture, 3 red green blue values for every pixel)

            // uhhhhh im so bored commenting about this so imma be simple
            // goes through each pixel of wall in ray, draws it to bitmap upside down
            // because for some reason the bitmaps are drawn upside down
            for(int y = 0; y < lineH; y++) {
                //int c = ((walls[(ty >> 12) * 16 + tx] * 127) >> shade1); // * shade2;

                int pixel = (((ty >> 12) << 4) + tx) * 3 + mapOffset; // 16 * 16 * 3 = 768
                int red = (textures.walls[pixel] >> shade1);// * shade2.numerator / shade2.denominator;
                int green = (textures.walls[pixel + 1] >> shade1);// * shade2.numerator / shade2.denominator;
                int blue = (textures.walls[pixel + 2] >> shade1);// * shade2.numerator / shade2.denominator;

                int index = (159 - (lineO + y)) * 2;
                Color color = new Color(red / 255f, green / 255f, blue / 255f);
                bitmap[index] = color;
                bitmap[index + 1] = color;

                ty += ty_step;
            }

            // this was supposed to draw textured floors, but its slow and honestly a lil ugly.
            // (Rays width / 2) / tan(fov / 2)
            /*fraction_t raFix = Sin(6434 - ca);
            int CSC = (((((CS.numerator * 82) / CS.denominator) << 4) * raFix.denominator) / raFix.numerator);
            int SNC = (((((SN.numerator * 82) / SN.denominator) << 4) * raFix.denominator) / raFix.numerator);
            for(int y = lineO + lineH; y < 160; y++) {
                int dy = y - 80;

                tx = ((player.x / 4) >> 12) + CSC / dy;
                ty = ((-player.y / 4) >> 12) - SNC / dy;

                int c = blackWalls[(-ty & 15) * 16 + (tx & 15)] * 127;
                int color = ST7735_Color565(c, c, c);

                int index = (159 - y) * 2;
                bitmap[index] = color;
                bitmap[index + 1] = color;
            }*/

            // draw sprites from sorted array
            for(int i = 0; i < 7; i++) {
                DrawSpriteColumn(bitmap, sortedSprites[i], r);
            }

            // draw bitmap
            screen.ST7735_DrawBitmap(r * (128 / Rays), 0, bitmap, 128 / Rays, 152);

            //ST7735_FillRect((levels[level].xOffset + ((rx >> 12) / (mapS / levels[level].scale))) - 1, (levels[level].yOffset + ((ry >> 12) / (mapS / levels[level].scale))) - 1, 2, 2, ST7735_Color565(0, 255, 0));

            // next ray
            ra += Degree;
            if(ra < 0) {
                ra += 25736;
            }
            if(ra > 25736) {
                ra -= 25736;
            }

            if (IQRCheck()) {
                yield return null;
            }
        }
    }

    int partition(sprite_t[] arr, int low, int high) {
        int pivot = 0;
        pivot = arr[high].b;

        int i = (low - 1); // index of smaller element
        for(int j = low; j < high; j++) {
            // If current element is smaller than or
            // equal to pivot
            if (arr[j].b > pivot) {
                i++;

                // swap arr[i] and arr[j]
                sprite_t temp1 = arr[i];
                arr[i] = arr[j];
                arr[j] = temp1;
            }
        }

        // swap arr[i+1] and arr[high] (or pivot)
        sprite_t temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1;
    }

    void sortSprites(sprite_t[] arr, int low, int high) {
        if (low < high) {
            /* pi is partitioning index, arr[p] is now
            at right place */
            int pi = partition(arr, low, high);

            sortSprites(arr, low, pi - 1);  // Before pi
            sortSprites(arr, pi + 1, high); // After pi
        }
    }

    int ADCin() {
        int input = (int)(Input.GetAxis("Horizontal") * 819f);

        //Debug.Log(input);
        if (input > 0) {
            return 1638 - input;
        }
        else if (input < 0) {
            return 2458 - input;
        }
        return 2047;
    }

    void CalculateSprite(sprite_t sp) {
        if(sp.state != 0) {
            // calculates depth of player
            sp.sx = sp.x - player.x;
            sp.sy = sp.y - player.y;

            fraction_t CS = Sin(6434 - player.a);
            fraction_t SN = Sin(player.a);

            int a = ((sp.sy * CS.numerator) / CS.denominator) - ((sp.sx * SN.numerator) / SN.denominator);
            sp.b = ((sp.sx * CS.numerator) / CS.denominator) + ((sp.sy * SN.numerator) / SN.denominator);

            // you only need to calculate if the sprite is infront of you
            // saves execution time
            if((sp.b >> 12) > 0) {
                // calculates screen x and y position
                sp.sx = a; sp.sy = sp.b;

                sp.sx = (sp.sx * 108 / sp.sy) + 64; // 108 is just some constant that had been found to work
                sp.sy = (sp.z * 108 / sp.sy) + 80;

                // using the depth of the sprite, it calculates how large it should be on the screen
                sp.xScale2 = sp.xScale * (80 << 12) / (sp.b >> 12);
                sp.yScale2 = sp.yScale * (80 << 12) / (sp.b >> 12);

                // just set limits to the screen scale so it isn't too large or small
                if(sp.xScale2 <= (sp.ps << 12)) {
                    sp.xScale2 = (sp.ps << 12);
                }
                else if(sp.xScale2 > (64 << 12)) {
                    sp.xScale2 = (64 << 12);
                }

                if(sp.yScale2 <= (sp.ps << 12)) {
                    sp.yScale2 = (sp.ps << 12);
                }
                else if(sp.yScale2 > (80 << 12)) {
                    sp.yScale2 = (80 << 12);
                }

                // find how much you should tranverse the texture array by using the screen scale
                sp.t_x_step = (sp.xScale << 12) / (sp.xScale2 >> 12);
                sp.t_y_step = (sp.yScale << 12) / (sp.yScale2 >> 12);

                // this was used to darken the sprite if it got farther away
                // but i removed it on the walls (cus of lag) so it didnt make sense to keep it on the sprites
                /*if((sp.b >> 12) > longest) {
                    sp.shade = 0;
                }
                else {
                    sp.shade = 1 - (1.0 * (sp.b >> 12) / longest);
                }*/
            }
        }
    }

    int Distance(int ax, int ay, int bx, int by) {
        int dx = (bx - ax) >> 12;
        int dy = (by - ay) >> 12;

        // same thing as absolute value cus the approximation doesn't work without it
        if(dx < 0) {
            dx *= -1;
        }
        if(dy < 0) {
            dy *= -1;
        }

        // depending on which number is larger, the approximation will be more accuarte
        if(dx < dy) {
            return dy + ((dx * dx) / (2 * dy));
        }
        return dx + ((dy * dy) / (2 * dx));
    }

    fraction_t Sin(int angle) {
        // its on wikipedia you can find it
        int flip = 1;

        // only works from 0 to 180 degrees, so change it to fit that range, and turn it negative
        while(angle > 12868 || angle < 0) {
            if(angle < 0) {
                angle += 12868;
            }
            if(angle > 12868) {
                angle -= 12868;
            }
            flip *= -1;
        }

        int piminusx = 12868 - angle; // pi - angle

        int numerator = 16 * angle;
        numerator *= piminusx;

        int denominator = 827922424;
        denominator -= (4 * angle) * piminusx;

        fraction_t fraction = new fraction_t((numerator * flip) >> 23, denominator >> 23);

        return fraction;
    }

    void ResetLevel(int lvl) {
        for(int j = 0; j < levels[lvl].mapX * levels[lvl].mapY; j++) {
            if(levels[level].map[j] == -1) { // destroyable block
                levels[level].map[j] = 2;
            }
        }
    }

    void DrawMap2D() {
        // goes through level array. if it is greater than 0, it is wall so draw white block
        // we could have done a different color for each wall type, but i like the mystery of not knowing
        screen.ST7735_FillRect(32, 32, 64, 64, screen.ST7735_BLACK);
        Color[] bitmap = new Color[levels[level].mapX * levels[level].mapY * levels[level].scale * levels[level].scale];

        for(int x = 0; x < levels[level].mapX; x++) {
            for(int y = 0; y < levels[level].mapY; y++) {
                int mt = levels[level].map[y * levels[level].mapX + x];
                Color pixel = screen.ST7735_BLACK;
                if(mt == 1) {
                    pixel = screen.ST7735_WHITE;
                }
                else if(mt == 2 || mt == -1) {
                    pixel = screen.ST7735_BLUE;
                }

                int start = (levels[level].mapX * (levels[level].mapY - y - 1) * levels[level].scale + x) * levels[level].scale;
                for (int ys = 0; ys < levels[level].scale; ys++) {
                    for (int xs = 0; xs < levels[level].scale; xs++) {
                        bitmap[start + ((levels[level].mapX * levels[level].scale) * ys + xs)] = pixel;
                    }
                }
            }
        }

        screen.ST7735_DrawBitmap(levels[level].xOffset, (160 - levels[level].yOffset) - (levels[level].mapY * levels[level].scale), bitmap, levels[level].mapX * levels[level].scale, levels[level].mapY * levels[level].scale);
        screen.ST7735_FillRect((levels[level].xOffset + ((levels[level].player1X >> 12) / (mapS / levels[level].scale))) - (player.w / 2), (levels[level].yOffset + ((levels[level].player1Y >> 12) / (mapS / levels[level].scale))) - (player.h / 2), player.w, player.h, screen.ST7735_RED);
        screen.ST7735_FillRect((levels[level].xOffset + ((levels[level].player2X >> 12) / (mapS / levels[level].scale))) - (player.w / 2), (levels[level].yOffset + ((levels[level].player2Y >> 12) / (mapS / levels[level].scale))) - (player.h / 2), player.w, player.h, screen.ST7735_RED);
    }

    void DrawImage(int[] buffer, int x, int y, int w, int h, int scale) {
        Color[] bitmap = new Color[w * h * scale * scale];

        for(int j = 0; j < h; j++) {
            for(int i = 0; i < w; i++) {
                int pixel = (w * j + i) * 3;
                int red = buffer[pixel];
                int green = buffer[pixel + 1];
                int blue = buffer[pixel + 2];

                if(!(red == 255 && green == 0 && blue == 255) && !(red == 0 && green == 0 && blue == 0)) {
                    int start = (w * (h - j - 1) * scale + i) * scale;

                    for (int ys = 0; ys < scale; ys++) {
                        for (int xs = 0; xs < scale; xs++) {
                            bitmap[start + ((w * scale) * ys + xs)] = new Color(red / 255f, green / 255f, blue / 255f);
                        }
                    }
                }
            }
        }

        screen.ST7735_DrawBitmap(x, (160 - y) - (h * scale), bitmap, w * scale, h * scale);
    }
}

public class player_t {
    public int x, y;
    public int dx, dy, s;
    public int a;
    public int w, h;

    public player_t(int px, int py, int pdx, int pdy, int ps, int pa, int pw, int ph) {
        x = px;
        y = py;
        dx = pdx;
        dy = pdy;
        s = ps;
        a = pa;
        w = pw;
        h = ph;
    }
};

public class sprite_t { // i * r, r = 2^12 (z = floor, i = 45)
    public int state;
    public int[] texture;
    public int xScale;
    public int yScale;
    public int ps;
    public int x, y, z;
    public bool wasDrawn;

    // rendering on screen
    public int sx;
    public int sy;
    public int a;
    public int b;
    public int xScale2;
    public int yScale2;
    public int t_x;
    public int t_y;
    public int t_x_step;
    public int t_y_step;
    //float shade;

    public sprite_t(int pstate, int[] ptexture, int pxScale, int pyScale, int pps, int px, int py, int pz, bool pwasDrawn) {
        state = pstate;
        texture = ptexture;
        xScale = pxScale;
        yScale = pyScale;
        ps = pps;
        x = px;
        y = py;
        z = pz;
        wasDrawn = pwasDrawn;

        sx = 0;
        sy = 0;
        a = 0;
        b = 0;
        xScale2 = 0;
        yScale2 = 0;
        t_x = 0;
        t_y = 0;
        t_x_step = 0;
        t_y_step = 0;
    }
};

public class bullet_t {
    public sprite_t sp;
    public int dx;
    public int dy;

    public bullet_t(sprite_t psp, int pdx, int pdy) {
        sp = psp;
        dx = pdx;
        dy = pdy;
    }
};

public class fraction_t {
    public int numerator;
    public int denominator;

    public fraction_t(int pnumerator, int pdenominator) {
        numerator = pnumerator;
        denominator = pdenominator;
    }
};

public class level_t {
    // level width and height
    public int mapX;
    public int mapY;

    // player1 starting values
    public int player1X;
    public int player1Y;
    public int player1a;

    // player2 starting values
    public int player2X;
    public int player2Y;
    public int player2a;

    // map display on menu
    public int xOffset;
    public int yOffset;
    public int scale;

    // actual level array. the reason its at the end is because structs allow you to create arrays with variable length, which is usually not allowed.
    // but it has to be at the end of the struct declaration so the compilier can allocate any memory amount.
    public int[] map;

    public level_t(int pmapX, int pmapY, int pplayer1X, int pplayer1Y, int pplayer1a, int pplayer2X, int pplayer2Y, int pplayer2a, int pxOffset, int pyOffset, int pscale, int[] pmap) {
        mapX = pmapX;
        mapY = pmapY;

        player1X = pplayer1X;
        player1Y = pplayer1Y;
        player1a = pplayer1a;

        player2X = pplayer2X;
        player2Y = pplayer2Y;
        player2a = pplayer2a;

        xOffset = pxOffset;
        yOffset = pyOffset;
        scale = pscale;
        
        map = pmap;
    }
};