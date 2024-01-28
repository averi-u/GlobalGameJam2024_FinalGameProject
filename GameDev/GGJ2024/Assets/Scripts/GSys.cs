using DamageNumbersPro;
using DG.Tweening;
using Michsky.MUIP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static Michsky.MUIP.UIGradient;

public class GSys : MonoBehaviour
{
    public static GSys instance;
	public void Awake()
	{
        instance = this;
	}

	public CustomerController _CustomerActive
    {
        get {
            return _CustomerInstList[0];
        }
    }
	public Transform _ItemTransitionRoot;

	/*
    
    public List<GameObject> customerObjects;
    bool isCorountineRunning = false;
    CustomerController customerController;
    public float customerSpawnRatePerSecond = 10f;
    public GameObject customerPerfab1;
    public GameObject customerPerfab2;
    */

	public List<CustomerController> _CustomerList;
    [NonSerialized]
    public List<CustomerController> _CustomerInstList;
    public Transform _CustomerRoot;
    public Transform _CustomerQueueStart;
    public Transform _CustomerQueueEnd;
    public Transform _CustomerFinishTarget;

	public int _CustomerQueueCount = 5;

    public Transform _FXRoot;
    public Transform _FX_Clicked;
    public Transform _FX_Finished;
    public DamageNumber _FX_AddTime;
    public DamageNumber _FX_MinusTime;
    public CountDownTimer _Timer;

    public ButtonManager _BtnReset;

    [NonSerialized]
    public float _ScoreGameTime;
	[NonSerialized]
	public int _ScoreCustomerFinished;
	[NonSerialized]
	public int _ScoreCustomerError;
    // Start is called before the first frame update
    void Start()
    {
        _CustomerInstList = new List<CustomerController>();

		for (int i = 0; i < _CustomerQueueCount; i++)
        {
            var c = _CustomerList[UnityEngine.Random.Range(0,_CustomerList.Count)];
            var inst = Instantiate(c, _CustomerRoot);
            float blend = (float)i * 1f / (float)(_CustomerQueueCount - 1);

			inst.transform.position = Vector3.Lerp(_CustomerQueueEnd.position,_CustomerQueueStart.position,blend);
            inst.transform.SetAsFirstSibling();
            _CustomerInstList.Add(inst);

            var img = _CustomerInstList[i].transform.Find("Image").GetComponent<Image>();
            var mat = new Material(img.material);
			mat.SetFloat("_Saturation", Mathf.Lerp(1f, 0f, blend));
			mat.SetFloat("_Value", Mathf.Lerp(1f, 0.2f, Mathf.Pow(blend,3f)));
            img.material = mat;
		}


        //StartCoroutine(StartActivation());
        //_CustomerActive.OnCustomerAcitvate(); d
        _BtnReset.onClick.AddListener(()=> {
            SceneManager.LoadScene("TitleScene");
        });
	}

	public void CustomerFinish()
    {
		{
            var instFinished = _CustomerInstList[0];
            _CustomerInstList.RemoveAt(0);

			var c = _CustomerList[UnityEngine.Random.Range(0, _CustomerList.Count)];
			var inst = Instantiate(c, _CustomerRoot);
			inst.transform.SetAsFirstSibling();
			_CustomerInstList.Add(inst);
			inst.transform.position = Vector3.Lerp(_CustomerQueueEnd.position, _CustomerQueueStart.position, 1f + 1f / (float)(_CustomerQueueCount - 1));
			
		}

		for (int i = 0; i < _CustomerQueueCount; i++)
		{
			var inst = _CustomerInstList[i];
            var blend = (float)i * 1f / (float)(_CustomerQueueCount - 1);

			var targetPos = Vector3.Lerp(_CustomerQueueEnd.position, _CustomerQueueStart.position, blend);

            var tweener = inst.transform.DOJump(targetPos,0.5f,1, 0.5f).SetDelay((float)i * 0.3f).SetEase(Ease.OutCirc);
            if (i == 0)
            {
                inst.OnCustomerAcitvate();
            }

			var img = inst.transform.Find("Image").GetComponent<Image>();
			var mat = new Material(img.material);
			mat.SetFloat("_Saturation", Mathf.Lerp(1f, 0f, blend));
			mat.SetFloat("_Value", Mathf.Lerp(1f, 0.2f, Mathf.Pow(blend, 3f)));
			img.material = mat;
		}

	}
    public Text _TxtScore;

	private void Update()
	{
        _ScoreGameTime += Time.deltaTime;

		_TxtScore.text = ((int)(_ScoreGameTime * (_ScoreCustomerFinished - 2 * _ScoreCustomerError))).ToString();
	}

    public void EndGame()
    {
        SceneManager.sceneLoaded += ResultSceneLoaded;

        SceneManager.LoadScene("ResultScene");
    }

    private void ResultSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		var txt = GameObject.Find("Canvas/ResultTxt").GetComponent<Text>();
		
        string text = "Game Finished\n";
		text += string.Format("游戏时长 {0} 秒\n",_ScoreGameTime);
		text += string.Format("完成客人单数 {0} \n", _ScoreCustomerFinished);
		text += string.Format("失误数 {0} \n", _ScoreCustomerError);
		text += string.Format("总分 {0} \n", (int)(_ScoreGameTime * (_ScoreCustomerFinished - 2 * _ScoreCustomerError)));
		
        txt.text = text;


		SceneManager.sceneLoaded -= ResultSceneLoaded;
	}
	/*
    private void FixedUpdate()
    {
        // 添加新的顾客到队列里
        customerObjects = GameObject.FindGameObjectsWithTag("Customer").ToList();
        //Debug.Log(customerObjects[0].name + "is the current customer");
        customerController = customerObjects[0].GetComponent<CustomerController>();
        // 设置只有第一个顾客才会提需求
        if (customerObjects.Count > 0)
        {
            if (customerController != null && customerObjects[0].GetComponent<RectTransform>().localPosition.x <= 777)
            {
                customerController.customerActived = true; 
            }
        }
        ReorderCustomerObjects();
        if (isCorountineRunning == false)
        {
            StartCoroutine(CustomerTest());
            isCorountineRunning = true;
        }
        
    }
    IEnumerator CustomerTest()
    {
        yield return new WaitForSeconds(2f);
        customerController.OnCustomerAcitvate();
        yield return null;
	}
    // Update is called once per frame
    void Update()
    {
		
	}
    IEnumerator InstanitateNewCustomer() // 生成新的顾客
    {
        GameObject newCustomer_1 = Instantiate(customerPerfab1);
        newCustomer_1.transform.parent = GameObject.Find("CustomerRoot").transform;
        RectTransform newCustomer_1_RectTransform = newCustomer_1.GetComponent<RectTransform>();
        newCustomer_1_RectTransform.localPosition = new Vector3(1444, 0, 0);
        newCustomer_1_RectTransform.sizeDelta = new Vector2(100, 100);
        newCustomer_1_RectTransform.localScale = new Vector2(1, 1);
        yield return new WaitForSeconds(customerSpawnRatePerSecond);
        GameObject newCustomer_2 = Instantiate(customerPerfab2);
        newCustomer_2.transform.parent = GameObject.Find("CustomerRoot").transform;
        RectTransform newCustomer_2_RectTransform = newCustomer_2.GetComponent<RectTransform>();
        newCustomer_2_RectTransform.localPosition = new Vector3(1444, 0, 0);
        newCustomer_2_RectTransform.sizeDelta = new Vector2(100, 100);
        newCustomer_2_RectTransform.localScale = new Vector2(1, 1);
        yield return null;
    }
    private void ReorderCustomerObjects() // 替换第二个顾客到第一个
    {
        if (customerObjects.Count > 0 && customerObjects[0] == null)
        {
            isCorountineRunning = false;
            Debug.Log("running? " + isCorountineRunning);
            customerObjects.Insert(0, customerObjects[1]);
        }
    }
    */
}
