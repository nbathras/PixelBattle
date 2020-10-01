using System.Collections.Generic;
using UnityEngine;
using static BottomUI;

public class CanvasController : MonoBehaviour {
    [SerializeField] private List<BottomUI> bottomUIList;

    private void Start() {
        foreach (BottomUI bottomUI in bottomUIList) {
            bottomUI.OnShowUIEvent += BottomUI_OnShowUIEvent;
        }
    }

    private void BottomUI_OnShowUIEvent(object sender, OnShowUIEventArgs e) {
        foreach (BottomUI bottomUI in bottomUIList) {
            if (bottomUI != e.bottomUI) {
                bottomUI.HideUI();
            }
        }
    }
}
