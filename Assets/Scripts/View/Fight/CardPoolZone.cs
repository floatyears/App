using UnityEngine;
using System.Collections;

public class CardPoolZone : MonoBehaviour 
{
	private GameObject Card1;
	private GameObject Card2;
	private GameObject Card3;
	private GameObject Card4;
	public int ID;
	public enum States{ empty,full }
	public States currentState;
	private Vector3 cardOffset;
	void Start()
	{
		Card1 = ResourceManager.Instance.LoadLocalAsset("Prefabs/Cards/Card1")as GameObject;
		Card2 = ResourceManager.Instance.LoadLocalAsset("Prefabs/Cards/Card2")as GameObject;
		Card3 = ResourceManager.Instance.LoadLocalAsset("Prefabs/Cards/Card3")as GameObject;
		Card4 = ResourceManager.Instance.LoadLocalAsset("Prefabs/Cards/Card4")as GameObject;
		cardOffset = new Vector3(0, 0, -5);
		GiveID();
		currentState=States.empty;
		CheckEmpty();
	}

	void Update()
	{
		CheckEmpty();
	}

	void CheckEmpty()
	{
		if(currentState == States.empty)
		{
			LogHelper.Log("卡片池当前状态为空,需要立即进行填充");

			CreteCardOnRule();

			currentState=States.full;
		}
	}

	void GiveID()
	{
		switch(this.gameObject.name)
		{
		case "Zone1":
			ID=1;
			break;
		case "Zone2":
			ID=2;
			break;
		case "Zone3":
			ID=3;
			break;
		case "Zone4":
			ID=4;
			break;
		case "Zone5":
			ID=5;
			break;
		default:
			ID=0;
			break;
		}
		LogHelper.Log("卡片池ID给定完毕，名为"+this.gameObject.name+"的卡片池的ID为"+ID);
	}

	void CreteCardOnRule()
	{
		int cardID=RuleOfCardCreation.OnRule();
		switch(cardID)
		{
		case 1:
			LogHelper.Log("随机选中了卡片1并生成");

			GameObject card1=Instantiate(Card1,Vector3.zero,Quaternion.identity) as GameObject;

			card1.transform.parent = this.gameObject.transform;

			card1.transform.localPosition = cardOffset;

			card1.layer = MoveCard.UNPICKEDLAYER;

			card1.GetComponent<Card>().style = Card.Styles.fire;

			LogHelper.Log("canMove = "+card1.GetComponent<Card>().canMove);

			break;
		case 2:
			LogHelper.Log("随机选中了卡片2并生成");

			GameObject card2=Instantiate(Card2,Vector3.zero,Quaternion.identity) as GameObject;

			card2.transform.parent = this.gameObject.transform;

			card2.transform.localPosition = cardOffset;

			card2.layer = MoveCard.UNPICKEDLAYER;

			card2.GetComponent<Card>().style = Card.Styles.blood;

			LogHelper.Log("canMove = "+card2.GetComponent<Card>().canMove);

			break;
		case 3:
			LogHelper.Log("随机选中了卡片3并生成");

			GameObject card3 = Instantiate(Card3,Vector3.zero,Quaternion.identity) as GameObject;

			card3.transform.parent = this.gameObject.transform;

			card3.transform.localPosition = cardOffset;

			card3.layer = MoveCard.UNPICKEDLAYER;

			card3.GetComponent<Card>().style = Card.Styles.water;

			LogHelper.Log("canMove = "+card3.GetComponent<Card>().canMove);

			break;
		case 4:
			LogHelper.Log("随机选中了卡片4并生成");

			GameObject card4 = Instantiate(Card4,Vector3.zero,Quaternion.identity) as GameObject;

			card4.transform.parent=this.gameObject.transform;

			card4.transform.localPosition = cardOffset;

			card4.layer = MoveCard.UNPICKEDLAYER;

			card4.GetComponent<Card>().style = Card.Styles.wind;

			LogHelper.Log("canMove = "+card4.GetComponent<Card>().canMove);

			break;

		default:

			LogHelper.Log("创建发生异常");

			break;
		}
	}
}
