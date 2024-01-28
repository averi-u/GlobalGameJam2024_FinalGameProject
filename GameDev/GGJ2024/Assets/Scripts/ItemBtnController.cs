using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class ItemBtnController : MonoBehaviour, IPointerEnterHandler
{
    private Button _Btn;
    public Item _Item;

    public AudioClip clickSound;
    public ParticleSystem clickEffect; 
    private AudioSource audioSource; 

    void Start()
    {
        _Btn = GetComponent<Button>();

        _Btn.onClick.AddListener(ItemClicked);

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


	}


    void ItemClicked()
    {
        // Play click sound
        audioSource.PlayOneShot(clickSound);

        // Play click visual effect
        if (clickEffect != null)
        {
            Instantiate(clickEffect, transform.position, Quaternion.identity);
        }


        var customer = GSys.instance._CustomerInstList[0];
        if (customer.IsMatchItem(_Item))
        {
            var clickFX = Instantiate(GSys.instance._FX_Clicked, GSys.instance._FXRoot);
            clickFX.position = transform.position;

            var transitionItem = Instantiate(gameObject, GSys.instance._ItemTransitionRoot);
            transitionItem.transform.localPosition = transform.localPosition;
            var tweener = transitionItem.transform.DOMove(GSys.instance._CustomerActive._Target.position, 0.4f).SetEase(Ease.OutQuad);

			MasterAudio.PlaySound("Btn_Clicked");

			tweener.onComplete += () => {
                Destroy(transitionItem);
                customer.MatchItem(_Item);
                MasterAudio.PlaySound("Item_Match");
			};
		}
		else
        {
			float bias = 4f;
			if (GSys.instance._ScoreCustomerFinished < 10)
			{ bias = 1; }
			else if (GSys.instance._ScoreCustomerFinished < 20)
			{ bias = 2; }
			else if (GSys.instance._ScoreCustomerFinished < 30)
			{ bias = 3; }

			float time_minus = 1f * bias;
			var addTime = GSys.instance._FX_MinusTime.SpawnGUI(GetComponent<RectTransform>(), Vector2.zero, time_minus);
			
            GSys.instance._Timer._currentTime -= time_minus;

			MasterAudio.PlaySound("Btn_Clicked_Error");
			GSys.instance._ScoreCustomerError++;
		}
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		
        MasterAudio.PlaySound("Btn_Highlight");
    }
}
