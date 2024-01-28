using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneUI : MonoBehaviour
{
    public ButtonManager _Return;
    // Start is called before the first frame update
    void Start()
    {
		_Return.onClick.AddListener(() => {
			SceneManager.LoadScene("TitleScene");
		});
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
