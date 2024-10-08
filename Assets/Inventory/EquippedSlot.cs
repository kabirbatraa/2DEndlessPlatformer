using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler
{
    //SLOT APPEARANCE//
    [SerializeField] private Image slotImage;

    [SerializeField] private TMP_Text slotName;

    [SerializeField] private Image playerDisplayImage;

    //SLOT DATA//
    [SerializeField] private ItemType itemType = new ItemType();

    private Sprite itemSprite;
    private string itemName;
    private string itemDescription;

    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
    }

    //OTHER VARIABLES//
    private bool slotInUse;

    [SerializeField] public GameObject selectedShader;
    [SerializeField] public bool thisItemSelected;


    public void OnPointerClick(PointerEventData eventData)
    {
        //On left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        //On right click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    void OnLeftClick()
    {
        if (thisItemSelected && slotInUse)
        {
            UnEquipGear();
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
    }

    void OnRightClick()
    {
        UnEquipGear();
    }

    public void EquipGear(Sprite itemSprite, string itemName, string itemDescription)
    {
        //If something is already equipped, send it back before equipping the new item
        if (slotInUse)
        {
            UnEquipGear();
        }

        //Update image
        this.itemSprite = itemSprite;
        slotImage.sprite = this.itemSprite;
        slotImage.enabled = true;
        slotName.enabled = false;

        //Update data
        this.itemName = itemName;
        this.itemDescription = itemDescription;

        //Update player stats
        for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
        {
            if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
            {
                equipmentSOLibrary.equipmentSO[i].EquipItem();
            }
        }

        slotInUse = true;
    }

    public void UnEquipGear()
    {
        inventoryManager.DeselectAllSlots();

        inventoryManager.AddItem(itemName, 1, itemSprite, itemDescription, itemType);

        //Update Slot Image
        selectedShader.SetActive(false);
        thisItemSelected = false;
        slotInUse = false;
        slotImage.enabled = false;
        slotName.enabled = true;

        //Update player stats
        for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
        {
            if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
            {
                equipmentSOLibrary.equipmentSO[i].UnEquipItem();
            }
        }
    }
}
