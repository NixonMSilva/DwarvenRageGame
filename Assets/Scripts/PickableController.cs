using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickableController : MonoBehaviour
{
    [SerializeField] private List<PickableBase> _pickableList;

    [SerializeField] private List<bool> _wasNotPicked;

    private char[] limits = { '(', ')' };

    private void Awake ()
    {
        for (int i = 0; i < _pickableList.Count; ++i)
        {
            _wasNotPicked.Add(true);
        }

        foreach (PickableBase item in _pickableList)
        {
            item.OnPickUp += HandleItemPickup;
        }
    }

    private void HandleItemPickup (IPickable sender)
    {
        string name = sender.GetName();
        if (name.Contains("Item"))
        {
            string[] subs = name.Split(limits);
            //Debug.Log(subs[1]);
            int armorId = Int32.Parse(subs[1]);
            _wasNotPicked[armorId] = false;
        }
    }

    public void SetPickedList (bool[] arr)
    {
        _wasNotPicked = arr.ToList();
        for (int i = 0; i < _wasNotPicked.Count; ++i)
        {
            if (!_wasNotPicked[i])
            {
                Destroy(_pickableList[i].gameObject);
            }
        }
    }

    public bool[] GetPickedList ()
    {
        return _wasNotPicked.ToArray();
    }

    [ContextMenu("Autofill Pickables")]
    void AutofillPickables ()
    {
        _pickableList = GetComponentsInChildren<PickableBase>().ToList();
    }
}
