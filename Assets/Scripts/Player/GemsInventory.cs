using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemsInventory : MonoBehaviour
{
    [SerializeField] private GameObject gemSlotGameObject;
    private int heightOffset = 1;
    private int widthOffset = 1;
    private ShopSystem shopSystem;

    private Inventory inventory;

    private void Awake()
    {
        shopSystem = FindObjectOfType<ShopSystem>();
    }

    private void Start()
    {
        inventory = Player.instance.Inventory;

        UpdateDisplay();
    }

    private void OnEnable()
    {
        if (inventory != null)
            UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        List<Gem> gems = inventory.GetAllGems();

        int count = 0;
        int y = 0;
        int x = 0;
        foreach (Gem gem in gems)
        {
            //get RectTransform corners to start at the top left corner
            Vector3[] corner = new Vector3[4];
            gameObject.transform.GetComponent<RectTransform>().GetWorldCorners(corner);
            Vector3 position = new Vector3(x, y, 0) + corner[1] + new Vector3(0.4f, -0.4f, 0);

            GameObject newGemSlot = Instantiate(gemSlotGameObject, position, Quaternion.identity, transform);

            GameObject gemGameObject = Resources.Load("Gems/" + gem.GetType()) as GameObject;
            GameObject newGem = Instantiate(gemGameObject, position, Quaternion.identity, transform);

            newGemSlot.GetComponent<GemSlot>().Gem = newGem.GetComponent<Gem>();

            if (shopSystem)
            {
                GemUI gemUI = newGem.GetComponentInChildren<GemUI>();
                gemUI.DisableDrag();
                gemUI.GemSlot = newGemSlot.GetComponent<GemSlot>();
                gemUI.GemSlot.FindShopSystem();
            }

            count++;
            count %= 3;

            if (count == 0)
            {
                y -= heightOffset;
                x = 0;
            }

            else
                x += widthOffset;
        }
    }
}
