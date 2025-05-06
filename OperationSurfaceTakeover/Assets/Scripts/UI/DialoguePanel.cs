using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;

public class DialoguePanel : MonoBehaviour
{
    #region Fields
    [SerializeField]
    TextMeshProUGUI dialougeText; //reference to the dialogue text component
    [SerializeField]
    GameObject skipButton; //reference to the skip button object

    //references to audio clips for 3 different menu sound effects and 2 different dialogue sound effects
    [SerializeField]
    AudioClip menuSfx1;
    [SerializeField]
    AudioClip menuSfx2;
    [SerializeField]
    AudioClip menuSfx3;
    [SerializeField]
    AudioClip dialougeAudio1;
    [SerializeField]
    AudioClip dialougeAudio2;

    AudioSource myAudioSource; //reference to this objecta's audio source

    int audioMenuNum = 0; //the number that applies to each menu clip to play

    
    List<string> lines = new List<string>(); //list of the lines the panel prints

    float textSpeed = 0.05f; //the speed at which the dialogue panel will print letters

    int index; //current index of the list
    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>(); //sets the reference of the audio source on this script

        dialougeText.text = string.Empty; //sets the default text of the dialogue to empty

        index = 0; //sets the current index to 0

        lines.Clear(); //clears all lines from the list of lines

        EventManager.AddListener(UIEvent.LevelSelectSpeech, LevelSelectLines); //events this script listens for
    }

    // Update is called once per frame
    void Update()
    {
        //checks for input from left mouse button
        if (Input.GetMouseButtonDown(0) && index < lines.Count)
        {
            //checks if the dialogue text is the same as the the current line in the list
            if (dialougeText.text == lines[index])
            {
                NextLine(); //runs the next line method
            }
            else
            {
                StopAllCoroutines(); //stops all coroutines

                dialougeText.text = lines[index]; //sets the dialogue text to the current line in the list
            }
        }

        //checks if theindex is greater than or equal to the number of lines in the list
        if (index >= lines.Count)
        {
            Time.timeScale = 1; //sets time scale to 1

            gameObject.SetActive(false); //deactivates self
        }
    }
    #endregion

    #region Methods and Events
    /// <summary>
    /// prints one letter at a time in the dialogue panel
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeLine()
    {
        dialougeText.text = ""; //sets the default text to blank

        //runs for as many character as there are in the current line
        foreach (char c in lines[index])
        {
            dialougeText.text += c; //adds current character to the dialogue text

            int audioChoice = Random.Range(0, 2); //picks a random number between 1 and 2

            //checks for what audio number it is and plays the dialogue sound effect acordingly
            if (audioChoice == 0)
            {
                myAudioSource.PlayOneShot(dialougeAudio1);
            }
            else if(audioChoice == 1)
            {
                myAudioSource.PlayOneShot(dialougeAudio2);
            }
            yield return new WaitForSecondsRealtime(textSpeed);
        }

    }

    /// <summary>
    /// starts the next line coroutine
    /// </summary>
    void NextLine()
    {
        StartCoroutine(PlayAudioNextLine());
    }

    /// <summary>
    /// starts the skip press coroutine
    /// </summary>
    public void SkipPress()
    {
        StartCoroutine(PlayAudioSkipPress());
    }

    /// <summary>
    /// selects what dialogue will be said based on the level
    /// </summary>
    /// <param name="data"></param>
    void LevelSelectLines(Dictionary<System.Enum, object> data)
    {
        //tries to get the data on what level number it is
        data.TryGetValue(UIEventData.LevelNum, out object output);
        int levelNum = (int)output;

        Time.timeScale = 0; //sets time scale to 0

        lines.Clear(); //clears the current lines

        //selects which set of dialogue will play based on what level it is
        switch (levelNum)
        {
            case 0:
                lines.Add("Finally. It's about damn time, Private. You know what to do—get to it.");
                lines.Add("What, you don't? Didn't they teach you anything at the Academy?");
                lines.Add("Look, I don't have time for this. Here are the basics: humanity went into hiding underground after a series of asteroids came crashing into Earth.");
                lines.Add("Turns out, those asteroids were full of uranium. So... animals started mutating, and now a species that used to be ants controls the surface.");
                lines.Add("Your job is to exterminate those ants while collecting uranium to send back to base. It powers our underground civilization.");
                lines.Add("I'm sure you have a lot of questions, but I don't have time for a nobody like you. So get to it, Private.");
                break;
            case 1:
                lines.Add("This may be your first mission, but I expect nothing short of total victory.");
                lines.Add("To think my time is being wasted teaching you the basics—what a waste of personnel.");
                lines.Add("You can use W, A, S, and D to navigate the map, and scroll the mouse wheel to zoom in and out.");
                lines.Add("On the right side of your screen, you'll find a selection of structures to defend the base.");
                lines.Add("We’ve supplied you with one mining drill to extract uranium from the fallen asteroids. You’ll use that uranium to build defenses.");
                lines.Add("To place a structure, just click it, then click the tile you want it on. You can also right click to cancel");
                lines.Add("Also the red lines show you where you cant place structures so don't try anything stupid.");
                lines.Add("As you’ll soon find out, those gross-looking eggs are nests for the mutant ants.");
                lines.Add("So survive the incoming waves, and that uranium will be ours.");
                lines.Add("Oh—and it goes without saying—if you don’t collect uranium, we won’t give you defenses. Got that, private?");
                lines.Add("Not that I care. A piece of gum on my boot has more promise than you.");
                lines.Add("At least I can tell the higher-ups I tried after you fail. Now hurry up, will you?");
                break;
            case 2:
                lines.Add("So, you *are* in fact useful for something after all.");
                lines.Add("But don’t get comfortable—things are only going to get harder as you push deeper into enemy territory.");
                lines.Add("Just so you know, I got a promotion thanks to your last performance, so keep up the good work.");
                lines.Add("And in case you were wondering—no, you’re not getting any credit for your efforts.");
                lines.Add("All that hard work? That’s mine now, as your commanding officer.");
                lines.Add("So keep that in mind as you fight and die for me.");
                break;
            case 3:
                lines.Add("I believe a thank you is in order.");
                lines.Add("Wait—you thought I meant *you* should be thanked?");
                lines.Add("You did *okay* for a private, but really, you should be thanking *me*.");
                lines.Add("Surely you noticed the new turret available to you.");
                lines.Add("You have no idea what kind of hoops I had to jump through to get that for you.");
                lines.Add("Just remember, the only reason you're able to kill those bugs is because of me.");
                lines.Add("Oh, one more thing—I’ve heard rumors the bugs are getting smarter.");
                lines.Add("They might try something unexpected... but come on, they’re just bugs. What threat could they possibly be?");
                lines.Add("Anyway, I’ve got more privates to go yell at. Get out of my sight.");
                break;
            case 4:
                lines.Add("I guess those bugs really *were* up to something. Good thing you had that turret I got you.");
                lines.Add("There’s a new hybrid ant roaming these parts. Eliminate it.");
                lines.Add("Oh, and by the way—retreat isn’t an option. It’s either you or the ant.");
                lines.Add("If I catch you running, I’ll shoot you myself. Don’t expect another word until it’s done.");
                break;
            case 5:
                lines.Add("See? You took care of that no problem.");
                lines.Add("I... was... doing... all... this... to help... *you*... grow?");
                lines.Add("Who the hell wrote this script? These HR clowns wouldn’t know real work if it bit them.");
                lines.Add("Long story short, apparently I’m being ‘too hard’ on the new recruits or whatever.");
                lines.Add("I can’t believe this. Can you? Actually, never mind—I don’t care what you think.");
                lines.Add("And don’t you *dare* report me to HR. I’ll have you fired yesterday if you try.");
                lines.Add("Side note—those ants are starting to multiply fast. You’ll be dealing with two at a time now.");
                lines.Add("That won’t be a problem for you, right, Private?");
                break;
            case 6:
                lines.Add("I can't believe this—I'm not even repulsed by your presence anymore.");
                lines.Add("You're starting to become what I might actually call a soldier.");
                lines.Add("Don't let that go to your head though—the last thing we need is another walking superiority complex.");
                lines.Add("Hope you're good at multitasking, because this terrain's going to force you to defend two fronts at once.");
                lines.Add("Oh, and you're going to love this—I got you a state-of-the-art flamethrower. Perfect for frying pests.");
                lines.Add("You can thank me by torching those bugs and getting me more of that sweet, succulent uranium.");
                break;
            case 7:
                lines.Add("Did you enjoy that flamethrower?");
                lines.Add("I could see the fireworks all the way from base.");
                lines.Add("Well, that’s pretty much all I had to say.");
                lines.Add("What? Were you expecting more?");
                lines.Add("I’m not your damn friend—you’re my lackey. So quit acting like we’re equals and get back to work.");
                break;
            case 8:
                lines.Add("I think these bugs might actually have a brain cell or two—they’re changing up their strategies.");
                lines.Add("Looks like they’re planning a full frontal assault since splitting their forces didn’t work.");
                lines.Add("All thanks to my state-of-the-art turrets, of course. Pretty impressive, huh?");
                lines.Add("Don’t fool yourself into thinking *you’re* the reason we’re winning—it’s entirely because of me.");
                lines.Add("And let’s be honest, who do you think the masses will believe? Some no-name private, or me—a general?");
                lines.Add("So fall in line and make me famous.");
                break;
            case 9:
                lines.Add("Now you’re really getting into the thick of it.");
                lines.Add("With three eggs to deal with, you’re gonna have your hands full.");
                lines.Add("Not that I’m worried—I’ve got, like... 500 people who could replace you tomorrow.");
                lines.Add("So don’t die. Or do. I don’t really care. Just grab me some uranium before you do, yeah?");
                break;
            case 10:
                lines.Add("Well, you finally made it to their stronghold.");
                lines.Add("It’s time for your last stand against the relentless horde.");
                lines.Add("This is definitely going to be a tough battle to watch.");
                lines.Add("Which is why I won’t be watching. I’m heading to the victory banquet early—so try not to disappoint everyone.");
                lines.Add("Then again, what can we really expect *you* to accomplish?");
                lines.Add("Oh look, there's the wine.");
                break;
            case 11:
                lines.Add("Wow. That’s all I can say. I never really expected you to survive that one.");
                lines.Add("Well, whoever created these ants is probably thrilled you made it this far and appreciates you seeing it through to the end.");
                lines.Add("Not that I’d know anything about that—I'm just a general, right?");
                lines.Add("Anyway, here’s a fun little endless mode. See how many waves you can survive. Or don’t. I’m off the clock now, so I don’t care.");
                break;

        }
        index = 0; //sets the index to 0

        StartCoroutine(TypeLine()); //starts the type line coroutine
    }

    /// <summary>
    /// plays the skip press audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioSkipPress()
    {
        audioMenuNum = Random.Range(1, 4); //selects a random number between 3 and 1

        //plays the audio clip corresponding to each number
        switch (audioMenuNum)
        {
            case 1:
                myAudioSource.clip = menuSfx1;
                myAudioSource.PlayOneShot(menuSfx1);
                break;
            case 2:
                myAudioSource.clip = menuSfx2;
                myAudioSource.PlayOneShot(menuSfx2);
                break;
            case 3:
                myAudioSource.clip = menuSfx3;
                myAudioSource.PlayOneShot(menuSfx3);
                break;
        }
        yield return new WaitForSecondsRealtime(myAudioSource.clip.length);

        lines.Clear(); //clears the current lines in the list
    }

    /// <summary>
    /// plays the next line audio clip
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudioNextLine()
    {
        audioMenuNum = Random.Range(1, 4); //selects a random number between 3 and 1

        //plays the audio clip corresponding to each number
        switch (audioMenuNum)
        {
            case 1:
                myAudioSource.clip = menuSfx1;
                myAudioSource.PlayOneShot(menuSfx1);
                break;
            case 2:
                myAudioSource.clip = menuSfx2;
                myAudioSource.PlayOneShot(menuSfx2);
                break;
            case 3:
                myAudioSource.clip = menuSfx3;
                myAudioSource.PlayOneShot(menuSfx3);
                break;
        }
        yield return new WaitForSecondsRealtime(myAudioSource.clip.length);

        //checks if the current line index is null
        if (lines[index] != null)
        {
            index++; //adds one to the index

            dialougeText.text = string.Empty; //changes the dialogue text to empty

            StartCoroutine(TypeLine()); //starts the type line coroutine
        }
        else
        {
            Time.timeScale = 1; //sets time scale to 1

            gameObject.SetActive(false); //deactivates self
        }
    }
    #endregion
}
