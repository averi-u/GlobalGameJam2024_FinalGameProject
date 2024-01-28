using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    public ButtonManager _StartGame;
    public ButtonManager _Exit;

    // Start is called before the first frame update
    void Start()
    {
        _StartGame.onClick.AddListener(()=> {
            SceneManager.LoadScene("GameScene");
        });

		_Exit.onClick.AddListener(()=> {

            Application.Quit();
        });

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
