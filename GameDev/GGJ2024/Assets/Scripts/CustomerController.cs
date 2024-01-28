using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CustomerController : MonoBehaviour
{
    public Transform _Target;
    
    public Transform _NeedsUIView;
	public Transform _NeedsUIContentRoot;

    private List<Item> _ItemInstanceList;
    public List<Item> _ItemList;

    private Animator _Anim;

    public string _SEFinish;

    //public Text _CountText;
    /*
    RectTransform rectTransform;

    public float customerMoveSpeed = 100f;
    public bool customerActived = false;
    */
    // Start is called before the first frame update
    void Start()
    {


		foreach (Transform child in _NeedsUIContentRoot)
        {
            Destroy(child.gameObject);
        }

		_NeedsUIView.gameObject.SetActive(false);
		_ItemInstanceList = new List<Item>();

		_Anim = GetComponent<Animator>();

        if (this == GSys.instance._CustomerInstList[0])
        {
            OnCustomerAcitvate();
        }

        

        /*
        rectTransform = GetComponent<RectTransform>();
        MoveRightToLeft();
        */
	}
    private void Update()
    {
		/*
        if (customerActived = true && rectTransform.localPosition.x <= -1593)
        {
            Destroy(gameObject);
        }

        if(customerActived)
        {

        }
        */
		//_CountText.text = "X"+_ItemInstanceList.Count.ToString();

	}
    [ContextMenu("Activate")]
    public void OnCustomerAcitvate()
    {

        _NeedsUIView.gameObject.SetActive(true);
        //customerActived = true;

        int maxNum = 3;
        if (GSys.instance._ScoreCustomerFinished < 10)
        { maxNum = 1; }
        else if (GSys.instance._ScoreCustomerFinished < 20)
        { maxNum = 2; }
        else if (GSys.instance._ScoreCustomerFinished < 30)
        { maxNum = 3; }

        int num = Random.Range(0, maxNum);
        for (int i = 0; i < num; i++)
        {
            _ItemList.Add(_ItemList[0]);

		}

		foreach (var i in _ItemList)
        {
            var itemInst = Instantiate(i, _NeedsUIContentRoot);
			_ItemInstanceList.Add(itemInst);
        }
	}

    public bool IsMatchItem(Item item)
    {
        if (!_NeedsUIView.gameObject.activeSelf)
            return false;
        return _ItemList.Contains(item);
    }
    public void MatchItem(Item item)
    {
        var idx = _ItemList.FindIndex(i => i == item);
        _ItemList.RemoveAt(idx);

        var itemInst = _ItemInstanceList[idx];
        _ItemInstanceList.RemoveAt(idx);
        Destroy(itemInst.gameObject);

        if (_ItemList.Count == 0)
        {
            _Anim.Play("Finish");
            DarkTonic.MasterAudio.MasterAudio.PlaySound(_SEFinish);
			var finishFX = Instantiate(GSys.instance._FX_Finished, GSys.instance._FXRoot);
			finishFX.position = _Target.position + new Vector3(0f,0f,1f);

            float time_add = 2f;
            var addTime = GSys.instance._FX_AddTime.SpawnGUI(_Target.GetComponent<RectTransform>(),Vector2.zero, time_add);
            GSys.instance._Timer._currentTime += time_add;


			GSys.instance.CustomerFinish();
            _NeedsUIView.gameObject.SetActive(false);
            transform.SetAsLastSibling();
            transform.DOMove(GSys.instance._CustomerFinishTarget.position, 3f).SetEase(Ease.OutCirc);
            StartCoroutine(FinishedCoroutine());

            GSys.instance._ScoreCustomerFinished++;

		}
    }

    public IEnumerator FinishedCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        yield return null;

    }
    /*
    IEnumerator MoveXAxis()
    {
        while (rectTransform.localPosition.x > -2000f)
        {
            float newX = rectTransform.localPosition.x - customerMoveSpeed * Time.deltaTime;
            rectTransform.localPosition = new Vector3(newX, rectTransform.localPosition.y, rectTransform.localPosition.z);
            yield return null;
        }
    }

    public void MoveRightToLeft()
    {
        StartCoroutine(MoveXAxis());
    }
    */
}
