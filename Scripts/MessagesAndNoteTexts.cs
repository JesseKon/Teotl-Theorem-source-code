using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagesAndNoteTexts : MonoBehaviour
{
    /* ***************************************************************************************** */
    // General messages

    /// <summary>
    /// 
    /// </summary>
    public string ExitMessage {
        get { return m_ExitMessage; }
    }
    private const string m_ExitMessage = "You're beign terminated...";

    /// <summary>
    /// 
    /// </summary>
    public string NoRelicsMessage {
        get { return m_NoRelicsMessage; }
    }
    private const string m_NoRelicsMessage = "Some kind of relic could fit in there...";

    /// <summary>
    /// 
    /// </summary>
    public string NoShardsFirstMessage {
        get { return m_NoShardsFirstMessage; }
    }
    private const string m_NoShardsFirstMessage = "It doesn't work like that...";

    /// <summary>
    /// 
    /// </summary>
    public string NoShardsSecondMessage {
        get { return m_NoShardsSecondMessage; }
    }
    private const string m_NoShardsSecondMessage = "I need more shards...";

    /// <summary>
    /// A warning that's shown to player when beign stuck at door
    /// </summary>
    public string StuckAtDoor {
        get { return m_StuckAtDoor; }
    }
    private const string m_StuckAtDoor = "W A R N I N G\n\nIf you are stuck, you'll get terminated";


    /* ***************************************************************************************** */
    // Hold ESC to do things

    /// <summary>
    /// 
    /// </summary>
    public string HoldEscToSkip {
        get { return m_HoldEscToSkip; }
    }
    private const string m_HoldEscToSkip = "Hold ESC to skip";

    /// <summary>
    /// 
    /// </summary>
    public string HoldEscToExit {
        get { return m_HoldEscToExit; }
    }
    private const string m_HoldEscToExit = "Hold ESC to exit";


    /* ***************************************************************************************** */
    // Intro messages

    /// <summary>
    /// 
    /// </summary>
    public string IntroBooting1 {
        get { return m_IntroBooting1; }
    }
    private const string m_IntroBooting1 = "Booting.";

    /// <summary>
    /// 
    /// </summary>
    public string IntroBooting2 {
        get { return m_IntroBooting2; }
    }
    private const string m_IntroBooting2 = "Booting..";

    /// <summary>
    /// 
    /// </summary>
    public string IntroBooting3 {
        get { return m_IntroBooting3; }
    }
    private const string m_IntroBooting3 = "Booting...";

    /// <summary>
    /// 
    /// </summary>
    public string IntroMessage1 {
        get { return m_IntroMessage1; }
    }
    private const string m_IntroMessage1 = "You are about to wake up\nin a new and strange place.";

    /// <summary>
    /// 
    /// </summary>
    public string IntroMessage2 {
        get { return m_IntroMessage2; }
    }
    private const string m_IntroMessage2 = "You are here for a reason and\nthat reason might be explained to you.";

    /// <summary>
    /// 
    /// </summary>
    public string IntroMessage3 {
        get { return m_IntroMessage3; }
    }
    private const string m_IntroMessage3 = "You are designed to go through\nvarious tasks and experiments.";

    /// <summary>
    /// 
    /// </summary>
    public string IntroMessage4 {
        get { return m_IntroMessage4; }
    }
    private const string m_IntroMessage4 = "Some of the task may seem ridiculous.\nDo not be concerned.";

    /// <summary>
    /// 
    /// </summary>
    public string IntroEnding {
        get { return m_IntroEnding; }
    }
    private const string m_IntroEnding = "Good luck.";


    /* ***************************************************************************************** */
    // Portal messages

    /// <summary>
    /// 
    /// </summary>
    public string Portal_FromLevel4ToLiminal_Message {
        get { return m_Portal_FromLevel4ToLiminal_Message; }
    }
    private const string m_Portal_FromLevel4ToLiminal_Message = "Dreams provide a way to escape.";

    /// <summary>
    /// 
    /// </summary>
    public string Portal1_FromLiminalToLiminal_Message {
        get { return m_Portal1_FromLiminalToLiminal_Message; }
    }
    private const string m_Portal1_FromLiminalToLiminal_Message = "In my dreams, time could be bended\nand twisted.";

    /// <summary>
    /// 
    /// </summary>
    public string Portal2_FromLiminalToLiminal_Message {
        get { return m_Portal2_FromLiminalToLiminal_Message; }
    }
    private const string m_Portal2_FromLiminalToLiminal_Message = "In my dreams, some obstacles were\nmere illusions.";

    /// <summary>
    /// 
    /// </summary>
    public string Portal3_FromLiminalToLiminal_Message {
        get { return m_Portal3_FromLiminalToLiminal_Message; }
    }
    private const string m_Portal3_FromLiminalToLiminal_Message = "In my dreams, I could put\nmy hand through a wall.";

    /// <summary>
    /// 
    /// </summary>
    public string Portal_FromLiminalToLevel4_Message {
        get { return m_Portal_FromLiminalToLevel4_Message; }
    }
    private const string m_Portal_FromLiminalToLevel4_Message = "Reality cannot be escaped.";

    /// <summary>
    /// 
    /// </summary>
    public string Portal_ToTrueExit_Message {
        get { return m_Portal_ToTrueExit_Message; }
    }
    private const string m_Portal_ToTrueExit_Message = "";

    /// <summary>
    /// 
    /// </summary>
    public string FakeExitMessage {
        get { return m_FakeExitMessage; }
    }
    private const string m_FakeExitMessage = "The end (bad ending)";

    /// <summary>
    /// 
    /// </summary>
    public string FakeExitQuote {
        get { return m_FakeExitQuote; }
    }
    private const string m_FakeExitQuote =
        "<i>Some day the last star is born</i>\n\n" +
        "<i>The other day it will die</i>\n\n" +
        "<i>Some day even physics will be\ntrapped in their silent graves</i>";

    /// <summary>
    /// 
    /// </summary>
    public string TrueExitMessage {
        get { return m_TrueExitMessage; }
    }
    private const string m_TrueExitMessage = "The end (good ending)";

    /// <summary>
    /// 
    /// </summary>
    public string TrueExitQuote {
        get { return m_TrueExitQuote; }
    }
    private const string m_TrueExitQuote =
        "<i>Soon there will be only the vast nothingness</i>\n\n" +
        "<i>Soon after that nothing is no more</i>\n\n" +
        "<i>In the end even the\nnothingness will cease to exist</i>";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_FirstRoom =
        "Hello and welcome to a test. You might not know why you are here, but the reasons will be explained to you during the test. " +
        "Let's begin by asking some thoughtful questions.\n\n" +
        ">ref 0x00 Why we seek for answers? Why aren't we comforted with the knowledge we already have?\n\n" +
        ">ref 0x04 Why do we have an urge to be in control of everything?\n\n";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_GeneratorIntro =
        "This thing is called generator. To activate it, you must feed it with an energy shard. " +
        "There should be one near by.\n\n" +
        "The generator is connected to the electric door, and it will open once its needs are fulfilled.\n\n" +
        "Once generator is activated, it will run for the duration of one's test. One shard can be used only once in each generator.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_Level2Lobby =
        "Above this note there is an energy shard. You might need some push to reach it. " +
        "\n\n" +
        "Remember, that you can use items to interact with them. Be careful though! " +
        "You are not alone in this test.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_Level2DeadEnd =
        "\n\n\nIf you are reading this note, there might or might not be something right behind you. " + 
        "\n\n\n\n\n" +
        "<i>You are allowed to terminate this test at any given point.</i>";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_TotemIntro =
        "Totems are another way to generate energy and operate machinery. They work a bit differently compared to the" + 
        "energy shards, but I am sure you will figure it out.\n\n" + 
        "If in doubt, try to match the symbols in the totem and in the plate.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_LoopArea =
        "What are colors that can't be seen or frequencies that can't be heard?\n\n" +
        "If reality by definition is something that can be measured, then what is infinity? What is the nature of non-existence?\n\n" +
        "";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_Liminal3 =
        "<i>You are an observer manifesting a focal point</i>\n\n" +
        "<i>You are the universe becoming conscious of itself</i>\n\n" +
        "<i>Life is eternal</i>\n\n" +
        "<i>It will not cease</i>\n\n" +
        "<i>Awareness is temporal</i>\n\n" +
        "<i>Materialization</i>\n\n";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_MainLobby =
        "Welcome to the main lobby. All you've learned so far will be put into a test. " +
        "Activate all four symbols in order to proceed.\n\n" +
        "You'll face two great challenges. Getting caught or dying by other means will erase you from the test, so be careful if you will.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_RedRoom =
        "<align=center>THE RED ROOM</align>\n\n" +
        "This is one of your ultimate tests. Be ready to run through wide fields and narrow corridors while avoiding " +
        "obstacles and [encounter5_darkroom]. The chase can not be avoided.\n\n" + 
        "Your goal is to find and activate the totem in this room.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_DarkRoom =
        "<align=center>THE DARK ROOM</align>\n\n" +
        "Besides being claustrofobic, your vision will also be more restricted. It is a maze and you will get lost in there.\n\n" +
        "Listen carefully and avoid running. There is something trying to find you. Crouch in ditches to escape its reach.\n\n" +
        "Your goal is to find and activate the totem in this room. You'll need 2 shards, which are scattered around the maze.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_Secret =
        "<i>Slowly came to stand still\n\nJourney reached the end\n\n</i>" +
        "<i>Here is nothing to be found\n\nCame all the way to find nothing\n\nNothing\n\n\n\n</i>" +
        "Thank you for your effort.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_Relic =
        "<align=center>CONGRATULATIONS</align>\n\n\n" +
        "You made it. Proceed this way to grab your prize, The Relic." +
        "\n\n\n" +
        "Remember, there might be multiple ways to spend The Relic.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_BadEnding =
        "<align=center>EXIT_FAILURE</align>\n\n" +
        "Your test wasn't succesful. You took the easiest way. There might still be more to find and explore." +
        "The portal in this room will offer you an exit.\n\n" +
        "" +
        ">0x28 exit_code_pending";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_GoodEnding1 =
        "What makes someone or something to be?\n\n" +
        "Is there anything, if there is no one experiencing it?\n\n" +
        "If we are an experiment in a closed system, is someone or something controlling us? Are each of us controlled individually?\n\n" +
        ">0x00 We seek for answers and try to gain more knowledge, because that would make us to be in control.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_GoodEnding2 =
        "What is real? Can ideas and concepts be real?\n\n" +
        "What are you, if your actions are planned beforehand by someone or something else?\n\n" +
        "Could we ever understand our underlying purposes even if they were explained to us explicitly?\n\n" +
        ">0x04 We want to be in control, because as a result our underlying reality can't harm us.";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_GoodEnding3 =
        "<align=center>EXIT_SUCCESS</align>\n\n" +
        "Ideas and memories will not die with you, but you as yourself will cease to exist when your environment is shut down.\n\n" +
        "The fact that you made it this far is incredible. But in the end, you only did what you were designed to do.\n\n" +
        "There is nothing more to find. Thank you for your contribution.\n\n" +
        ">0x24 exit_code_pending";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_StuckAtRedRoom =
        "\n\nYou have trapped yourself in the Red Room.\n\n\n\n\n<i>Your journey will essentially end in here.</i>";

    /// <summary>
    /// 
    /// </summary>
    private const string m_Note_StuckAtDarkRoom =
        "\n\nYou have trapped yourself in the Dark Room.\n\n\n\n\nYour journey will essentially end in here.";


    private void Start() {

        // First room
        AddNote("Note_FirstRoom", m_Note_FirstRoom);
        AddNote("Note_GeneratorIntro", m_Note_GeneratorIntro);

        // Second room
        AddNote("Note_Level2Lobby", m_Note_Level2Lobby);
        AddNote("Note_Level2DeadEnd", m_Note_Level2DeadEnd);

        // Totem intro
        AddNote("Note_TotemIntro", m_Note_TotemIntro);
        AddNote("Note_LoopArea", m_Note_LoopArea);
        AddNote("Note_Liminal3", m_Note_Liminal3);

        // Main lobby
        AddNote("Note_MainLobby", m_Note_MainLobby);
        AddNote("Note_RedRoom", m_Note_RedRoom);
        AddNote("Note_DarkRoom", m_Note_DarkRoom);

        // Last room
        AddNote("Note_Secret", m_Note_Secret);
        AddNote("Note_Relic", m_Note_Relic);

        // Endings
        AddNote("Note_BadEnding", m_Note_BadEnding);
        AddNote("Note_GoodEnding1", m_Note_GoodEnding1);
        AddNote("Note_GoodEnding2", m_Note_GoodEnding2);
        AddNote("Note_GoodEnding3", m_Note_GoodEnding3);

        // Stuck at red/dark room
        AddNote("Note_StuckAtRedRoom", m_Note_StuckAtRedRoom);
        AddNote("Note_StuckAtDarkRoom", m_Note_StuckAtDarkRoom);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="noteName"></param>
    /// <param name="noteContents"></param>
    private void AddNote(string noteName, string noteContents) {
        GameObject note = GameObject.Find(noteName);
        if (!note) {
            Debug.LogWarning("Note " + noteName + " wasn't found!");
            return;
        }

        note.transform.GetChild(1).GetComponent<TextMeshPro>().text = noteContents;
    }
}
