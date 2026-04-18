using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi (Yazý hatasý için þart)
using UnityEngine.UI; // Butonlar için þart

public class GameManager : MonoBehaviour
{
    public Deck deckScript; // Deck scriptine referans

    // Kartlarýn dizileceði yerler (Inspector'dan sürükle)
    public Transform playerCardArea;
    public Transform dealerCardArea;

    // Prefab (Kartýn oyundaki görüntüsü)
    public GameObject cardPrefab;

    // Skor Yazýlarý (Inspector'dan sürükle)
    public TMP_Text playerScoreText;
    public TMP_Text dealerScoreText;

    // Sonuç Yazýsý (O sürükleyemediðin Text (TMP) buraya gelecek)
    public TMP_Text infoText;

    // Butonlar
    public Button hitButton;
    public Button standButton;
    public Button restartButton;

    List<Card> playerHand = new List<Card>();
    List<Card> dealerHand = new List<Card>();

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // Masayý temizle
        foreach (Transform child in playerCardArea) Destroy(child.gameObject);
        foreach (Transform child in dealerCardArea) Destroy(child.gameObject);
        playerHand.Clear();
        dealerHand.Clear();

        deckScript.Shuffle();

        hitButton.interactable = true;
        standButton.interactable = true;
        infoText.text = ""; // Yazýyý temizle
        restartButton.gameObject.SetActive(false);

        // Ýlk daðýtým
        DealCard(playerHand, playerCardArea);
        DealCard(dealerHand, dealerCardArea);
        DealCard(playerHand, playerCardArea);
        DealCard(dealerHand, dealerCardArea);

        UpdateScores();

        // Blackjack kontrolü
        if (CalculateHandValue(playerHand) == 21)
        {
            GameOver("BLACKJACK! KAZANDIN!");
            return;
        }
    }

    public void OnHitButton()
    {
        DealCard(playerHand, playerCardArea);
        UpdateScores();

        if (CalculateHandValue(playerHand) > 21)
        {
            GameOver("BATTIN! (21'i geçtin)");
        }
    }

    public void OnStandButton()
    {
        hitButton.interactable = false;
        standButton.interactable = false;

        // DEALER MANTIÐI BURADA: 17 olana kadar çekiyor
        while (CalculateHandValue(dealerHand) < 17)
        {
            DealCard(dealerHand, dealerCardArea);
        }

        UpdateScores();
        CheckWinner();
    }

    public void OnRestartButton()
    {
        StartGame();
    }

    void DealCard(List<Card> hand, Transform area)
    {
        Card cardData = deckScript.Draw();
        hand.Add(cardData);

        GameObject newCard = Instantiate(cardPrefab, area);
        // Kart görselini ayarla
        newCard.GetComponent<Image>().sprite = cardData.sprite;
    }

    int CalculateHandValue(List<Card> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            total += card.value;
            if (card.value == 11) aceCount++;
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    void UpdateScores()
    {
        // Eðer bu deðiþkenler boþsa hata vermemesi için kontrol
        if (playerScoreText != null) playerScoreText.text = "Skor: " + CalculateHandValue(playerHand);
        if (dealerScoreText != null) dealerScoreText.text = "Skor: " + CalculateHandValue(dealerHand);
    }

    void CheckWinner()
    {
        int playerVal = CalculateHandValue(playerHand);
        int dealerVal = CalculateHandValue(dealerHand);

        if (dealerVal > 21) GameOver("DEALER BATTI! KAZANDIN!");
        else if (playerVal > dealerVal) GameOver("KAZANDIN!");
        else if (playerVal < dealerVal) GameOver("KAYBETTÝN...");
        else GameOver("BERABERE!");
    }

    void GameOver(string message)
    {
        infoText.text = message;
        hitButton.interactable = false;
        standButton.interactable = false;
        restartButton.gameObject.SetActive(true);
    }
}