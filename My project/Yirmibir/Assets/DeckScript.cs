using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Kartlarýn tutulduðu deste
    public List<Card> deck = new List<Card>();

    // Unity Inspector'dan dolduracaðýmýz liste (52 resim burada olmalý)
    public Sprite[] cardSprites;

    void Awake()
    {
        CreateDeck();
        Shuffle();
    }
    public void ResetDeck()
    {
        deck.Clear();      // Listeyi sýfýrla
        CreateDeck();      // Yeniden oluþtur
        Shuffle();         // Karýþtýr
    }

    void CreateDeck()
    {
        // Önce listenin dolu olup olmadýðýný kontrol edelim
        if (cardSprites.Length < 52)
        {
            Debug.LogError("HATA: Inspector'da 'Card Sprites' listesine 52 kart yüklenmemiþ! Þu anki sayý: " + cardSprites.Length);
            return; // Hata varsa oyunu durdurur, çökmesini engeller.
        }

        string[] suits = { "Hearts", "Spades", "Clubs", "Diamonds" }; // Sýralama önemli!
        string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        int spriteIndex = 0;

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                Card c = new Card();
                c.name = rank + "_" + suit;

                if (rank == "A") c.value = 11;
                else if (rank == "J" || rank == "Q" || rank == "K") c.value = 10;
                else c.value = int.Parse(rank);

                // Kart resmini listeden alýp karta atýyoruz
                c.sprite = cardSprites[spriteIndex];

                spriteIndex++;
                deck.Add(c);
            }
        }
        Debug.Log("Deste oluþturuldu. Toplam kart: " + deck.Count);
    }

    public void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = Random.Range(0, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[rand];
            deck[rand] = temp;
        }
    }

    public Card Draw()
    {
        if (deck.Count == 0) return null;

        Card c = deck[0];
        deck.RemoveAt(0);
        return c;
    }
    
}