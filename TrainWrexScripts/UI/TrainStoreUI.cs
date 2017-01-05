using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainStoreUI : MonoBehaviour {

    public Button quit_b;
    public Button confirm_b;
    public Button previousTrain_b;
    public Button nextTrain_b;
    public Button purchaseTrain_b;
    public Text TrainDescriptionText;
    public Text coinTotalText;

    private string[] TrainEffects;
    public int CurrentTrain = 0;
    private int coinTotal = 0;

    private string Purchases;
    // Use this for initialization
    void Start () {
        TrainEffects = new string[] {"Just a Normal Train",
                                     "Desert Train",
                                     "Haunted Train",
                                     "Space Train",
                                     };
        CurrentTrain = PlayerPrefs.GetInt("TrainEffect", 0);
        coinTotal = PlayerPrefs.GetInt("Coins", 0);
        //PlayerPrefs.SetString("TrainPurchases", "1000");//reset your purchases
        Purchases = PlayerPrefs.GetString("TrainPurchases", "1000");//the first train is prepurchased

        TrainDescriptionText.text = TrainEffects[CurrentTrain];
        coinTotalText.text = "Total Coins: " + coinTotal.ToString(); //"Total Coins: "
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void NextTrain()
    {
        CurrentTrain++;
        if (CurrentTrain >= TrainEffects.Length)
            CurrentTrain = 0;
        TrainDescriptionText.text = TrainEffects[CurrentTrain];
        if (Purchases[CurrentTrain] == '0')
        {
            TrainDescriptionText.text += "\n(Cost: " + PlayerPrefs.GetInt("trainPurchase" + CurrentTrain, 10).ToString() + ')';
            purchaseTrain_b.image.color = Color.red;
        }
        else
        {
            purchaseTrain_b.image.color = Color.white;
        }
    }

    public void PreviousTrain()
    {
        CurrentTrain--;
        if (CurrentTrain < 0)
            CurrentTrain = TrainEffects.Length - 1;
        TrainDescriptionText.text = TrainEffects[CurrentTrain];
        if (Purchases[CurrentTrain] == '0')
        {
            TrainDescriptionText.text += "\n(Cost: " + PlayerPrefs.GetInt("trainPurchase" + CurrentTrain, 10).ToString() + ')';
            purchaseTrain_b.image.color = Color.red;
        }
        else
        {
            purchaseTrain_b.image.color = Color.white;
        }
    }

    public void Confirm(int confirm)
    {
        if (confirm == 1 && Purchases[CurrentTrain] == '1')//if confirmed and current train selected is purchased
        {
            PlayerPrefs.SetInt("TrainEffect", CurrentTrain);
            Application.LoadLevel(0);
        }
        else if (confirm == 0)//if quit
            Application.LoadLevel(0);
    }

    public void PurchaseTrain()
    {
        if (Purchases[CurrentTrain] == '0' && coinTotal >= PlayerPrefs.GetInt("trainPurchase" + CurrentTrain, 10))
        {
            coinTotal -= PlayerPrefs.GetInt("trainPurchase" + CurrentTrain, 10);
            string s = "";
            for (int i = 0; i < TrainEffects.Length; i++)
            {
                if (i == CurrentTrain)
                {
                    s += '1';
                }
                else
                {
                    s += Purchases[i];
                }
            }
            //add the purchase to the prefab
            Purchases = s;
            PlayerPrefs.SetString("TrainPurchases", Purchases);
            PlayerPrefs.SetInt("Coins", coinTotal);
            purchaseTrain_b.image.color = Color.white;
            TrainDescriptionText.text = TrainEffects[CurrentTrain];
            coinTotalText.text = "Total Coins: " + coinTotal.ToString();
        }
    }
}
