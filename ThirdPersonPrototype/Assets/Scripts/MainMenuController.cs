using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NOTE YOu can make canvas objects prefabs and that is probably a good idea
public class MainMenuController : MonoBehaviour
{

    public Text textTitle;
    
    public Button button;
    public string startText = "Start Text";
    public string playText = "THIS IS HAPPENING";

    public void Start()
    {
        textTitle.text = startText;
    }

    void Update()
    {
        RectTransform rt = (button.transform as RectTransform);
        rt.rotation = Quaternion.Euler(0, Time.time, 0);
        // textTitle.rectTransform.position += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
        //button.transform.position += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
    }

   public void PlayButtonPressed()
    {
        print("YOU PRESSED DA BUTTON");
        textTitle.text = playText;
        //Load new scene
        //SceneManager.LoadScene("MyLevelNameHere");e
    }
}
