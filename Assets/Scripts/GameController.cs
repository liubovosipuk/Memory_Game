using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite bgImage; // background of Instantiated prefabs

    [SerializeField] private Sprite[] puzzles; // all candies sprites

    public List<Sprite> gamePuzzles = new List<Sprite>(); // candies sprites for each Instantiated Puzzle Button 

    public List<Button> btns = new List<Button>(); // prefab Puzzle Button

    private bool firstGuess,
                 secondGuess;

    private int countGuesses,
                countCorrectGuesses,
                gameGuesses;

    private int firstGuessIndex,
                secondGuessIndex;

    private string firstGuessPuzzle,
                   secondGuessPuzzle;



    private void Awake() {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Candy"); // would put in all our sprites into the List 
    }



    // using Start() cause we use it into the AddButton script, if we use Awake() it doesn't work
    private void Start() {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }



    void GetButtons() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton"); 
        for (int i = 0; i < objects.Length; i++) {
            btns.Add(objects[i].GetComponent<Button>()); 
            btns[i].image.sprite = bgImage; 
        }
    }
    


    // the goal find two same images 
    void AddGamePuzzles() {
        int looper = btns.Count; 
        int index = 0;

        for (int i = 0; i < looper; i++) {

            if (index == looper / 2) {
                index = 0; 
            }

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }



    void AddListeners() {
        foreach (Button btn in btns) {
            btn.onClick.AddListener(() => PickAPuzzleButton());
        }
    }



    public void PickAPuzzleButton() {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // determine which button are clicking
        Debug.Log("You are clicking a button named " + name); // we get a name of a clicked button  

        // user when he open buttons have just 1 attempt (2 opens)
        if (!firstGuess) {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name; // get the name of opened button 
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];

        } else if (!secondGuess) {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name; // get the name of opened button 
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            // compare of two puzzles (opened buttons)
            //if(firstGuessPuzzle == secondGuessPuzzle) {
            //    Debug.Log("The Puzzles Match");

            //} else {
            //    Debug.Log("The Puzzles don't Match");

            //}
            countGuesses++;
            StartCoroutine(CheckIfThePuzzlesMatched());

        }
    }



    IEnumerator CheckIfThePuzzlesMatched() {

        yield return new WaitForSeconds(1f);


        if(firstGuessPuzzle == secondGuessPuzzle) {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].interactable = false; // do buttons untouchable
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0); // set new color for untouchable buttons
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);


            CheckIfTheGameIsFinished();

        } else {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }


        yield return new WaitForSeconds(0.5f);

        firstGuess = secondGuess = false;

    }



    void CheckIfTheGameIsFinished() {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses) {
            Debug.Log("Game is Finished!");
            Debug.Log("It took you " + countGuesses + " many guess(es) to finished the game");
            


            //SceneManager.LoadScene(1); // after finished game load new scene
        }

    }



    void Shuffle(List<Sprite> list) {

        for (int i = 0; i < list.Count; i++) {
            Sprite temp = list[i]; // getting the reference from the List 
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex]; // assingning our random element
            list[randomIndex] = temp; 

        }
    }



} // class GameController































