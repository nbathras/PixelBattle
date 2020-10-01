using System;
using UnityEngine;

public abstract class BottomUI : MonoBehaviour {
    public EventHandler<OnShowUIEventArgs> OnShowUIEvent;
    public class OnShowUIEventArgs {
        public BottomUI bottomUI;
    }

    private Transform ui;

    protected virtual void Awake() {
        ui = transform.Find("ui");
    }

    protected virtual void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            HideUI();
        }
    }

    public Transform GetUI() {
        return ui;
    }

    public void ShowUI() {
        Debug.Log("ShowUI: " + name);

        OnShowUIEvent?.Invoke(this, new OnShowUIEventArgs { bottomUI = this });

        ui.gameObject.SetActive(true);
    }

    public void HideUI() {
        ui.gameObject.SetActive(false);
    }
}
 