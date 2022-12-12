using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CutsceneSystemScript : MonoBehaviour
{
    int imageNumber;
    public int MaxImages;
    public int NextScene;

    GameObject canvas;

    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public GameObject Seven;
    public GameObject Eight;
    public GameObject Nine;
    public GameObject Ten;
    public GameObject Eleven;
    public GameObject Twelve;
    public GameObject Thirteen;
    public GameObject Fourteen;
    public GameObject Fifteen;
    public GameObject Sixteen;
    public GameObject Seventeen;
    public GameObject Eighteen;
    public GameObject Nineteen;
    public GameObject Twenty;

    // Start is called before the first frame update
    void Start()
    {
        imageNumber = 1;

        canvas = GameObject.Find("Canvas");

        //one = canvas.GetComponent<Image>().WorkCu
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            imageNumber += 1;
        }

        if (imageNumber == MaxImages)
        {
            SceneManager.LoadScene(NextScene);
        }

        if (imageNumber == 1)
        {
            One.SetActive(true);
        }

        if (imageNumber == 2)
        {
            Two.SetActive(true);
        }

        if (imageNumber == 3)
        {
            Three.SetActive(true);
        }

        if (imageNumber == 4)
        {
            Four.SetActive(true);
        }

        if (imageNumber == 5)
        {
            Five.SetActive(true);
        }

        if (imageNumber == 6)
        {
            Six.SetActive(true);
        }

        if (imageNumber == 7)
        {
            Seven.SetActive(true);
        }

        if (imageNumber == 8)
        {
            Eight.SetActive(true);
        }

        if (imageNumber == 9)
        {
            Nine.SetActive(true);
        }

        if (imageNumber == 10)
        {
            Ten.SetActive(true);
        }

        if (imageNumber == 11)
        {
            Eleven.SetActive(true);
        }

        if (imageNumber == 12)
        {
            Twelve.SetActive(true);
        }

        if (imageNumber == 13)
        {
            Thirteen.SetActive(true);
        }

        if (imageNumber == 14)
        {
            Fourteen.SetActive(true);
        }

        if (imageNumber == 15)
        {
            Fifteen.SetActive(true);
        }

        if (imageNumber == 16)
        {
            Sixteen.SetActive(true);
        }

        if (imageNumber == 17)
        {
            Seventeen.SetActive(true);
        }

        if (imageNumber == 18)
        {
            Eighteen.SetActive(true);
        }

        if (imageNumber == 19)
        {
            Nineteen.SetActive(true);
        }

        if (imageNumber == 20)
        {
            Twenty.SetActive(true);
        }

    }
}
