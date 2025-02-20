using System.Collections;
using System.Collections.Generic;
using Achonor.LBSMap;
using ShuJun.Event;
using UnityEngine;
using UnityEngine.UI;
public class BaiDuMap : MonoBehaviour {
    [SerializeField]
    private MapServices mMapServices;

    [SerializeField]
    private Button mStreetBtn;

    [SerializeField]
    private Button mSatelliteBtn;

    public RectTransform icon;
    private void Awake () {
        if (null == mMapServices) {
            mMapServices = GetComponent<MapServices> ();
        }

        EventManager.Register<TouchMoveEvent> ((param) => {
            mMapServices.MoveMap (param.MoveOffset);
        }, this);

        EventManager.Register<TouchMovedEvent> ((param) => {
            mMapServices.DoRender ();
        }, this);

        EventManager.Register<TouchZoomEvent> ((param) => {
            mMapServices.ZoomMap (param.ChangeZoom);
            mMapServices.DoRender ();
        }, this);

        EventManager.Register ((TouchRotateEvent param) => {
            mMapServices.RotateMap (param.ChangedEuler);
        }, this);

        EventManager.Register ((TouchRotatedEvent param) => {
            mMapServices.DoRender ();
        }, this);

        mStreetBtn.onClick.AddListener (() => {
            SetMapType (MapType.Street);
            mMapServices.DoRender ();
        });

        mSatelliteBtn.onClick.AddListener (() => {
            SetMapType (MapType.Satellite);
            mMapServices.DoRender ();
        });
        print (MCTransform.ConvertLL2MC (new Vector2D (180, 74)));
    }

    private void Start () {
        SetMapType (MapType.Street);
        mMapServices.SetZoomLevel (18);
        mMapServices.SetMapCenter (new Vector2D (112.888678, 28.213555));
        mMapServices.DoRender ();

    }
    Vector2D dd = new Vector2D (112.88937867829753, 28.215094803453077);
    void Update () {
        Vector2 dat = mMapServices.LngLat2ScreenPos (dd);
        icon.position = dat;
        //        Debug.Log (dat);
    }
    private void SetMapType (MapType mapType) {
        mStreetBtn.interactable = mapType == MapType.Satellite;
        mSatelliteBtn.interactable = mapType == MapType.Street;
        mMapServices.SetMapType (mapType);
    }
}