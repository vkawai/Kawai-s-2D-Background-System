using System.Collections;using System.Collections.Generic;using UnityEngine;[RequireComponent(typeof(SpriteRenderer))]public class BackgroundLayer : MonoBehaviour {

    /**
     * <summary>Flag that determines if this layer should loop horizontally</summary>
     * */
    public bool loopHorizontal;
    /**
     * <summary>Flag that determines if this layer should loop vertically</summary>
     * */
    public bool loopVertical;    /**     * <summary>     * An static layer will follow the main camera.     * <para>This implies that this layer will neither loop horizontally or vertically, and will set both flags to false.</para>     * </summary>     * */    public bool isStatic;    /**     * <summary>     * Tells how much this layer will be influenced by camera/player horizontal displacement.     * </summary>     * */    public float verticalParallaxFactor;        /**     * <summary>     * Tells how much this layer will be influenced by camera/player vertical displacement.     * </summary>     * */    public float horizontalParallaxFactor;     /**     * <summary>     * Get the horizontal distance between loop centers.     * </summary>     * */    public float hDistanceBetweenBgs {
        get {
            return _hDistanceBetweenBgs;
        }
    }    private float _hDistanceBetweenBgs;         /**     * <summary>     * Get the vertical distance between loop centers.     * </summary>     * */    public float vDistanceBetweenBgs {
        get {
            return _vDistanceBetweenBgs;
        }
    }    private float _vDistanceBetweenBgs;    void Start() {        // If I have both factors setted to 0, then I'm an static layer.        if(!isStatic && verticalParallaxFactor == 0 && horizontalParallaxFactor == 0) {            isStatic = true;
            _hDistanceBetweenBgs = 0;
            _vDistanceBetweenBgs = 0;
        }                // If I'm not an static layer, I must know both my width and height based on game scene.        if(!isStatic) {
            SpriteRenderer objRender = gameObject.GetComponent<SpriteRenderer>();
            _hDistanceBetweenBgs = 2 * (1f / gameObject.transform.parent.localScale.x) * (objRender.bounds.max.x - transform.position.x);
            Debug.Log(objRender.bounds.max.x);
            _vDistanceBetweenBgs = 2 * (1f / gameObject.transform.parent.localScale.y) * (objRender.bounds.max.y - transform.position.y);            SetupParallax();        }        else {            verticalParallaxFactor = 0;            horizontalParallaxFactor = 0;        }	}    /**     * <summary>Move this layer to the right based on layer width</summary>     * */    public void MoveRight() {
        transform.localPosition = transform.localPosition + new Vector3(_hDistanceBetweenBgs, 0);
    }        /**     * <summary>Move this layer to the left based on layer width</summary>     * */    public void MoveLeft() {
        transform.localPosition = transform.localPosition - new Vector3(_hDistanceBetweenBgs, 0);
    }        /**     * <summary>Move this layer up based on layer height</summary>     * */    public void MoveUp() {
        transform.localPosition = transform.localPosition + new Vector3(0, _vDistanceBetweenBgs);
    }
        /**     * <summary>Move this layer down based on layer height</summary>     * */
    public void MoveDown() {
        transform.localPosition = transform.localPosition - new Vector3(0, _vDistanceBetweenBgs);
    }    /// <summary>
    /// Create a new Sub Layer.
    /// </summary>
    /// <returns></returns>    GameObject CreateLayerLoop() {
        // In order to create an layer loop, I will need a new Game Object that will inherit almost
        // all of my properties, except for Background Layer Component, since only me needs it in
        // order to work.

        SpriteRenderer objRender = gameObject.GetComponent<SpriteRenderer>();

        GameObject subLayer = new GameObject("SubLayer" + transform.childCount);
        subLayer.transform.SetParent(transform);
        SpriteRenderer render = subLayer.AddComponent<SpriteRenderer>();
        render.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        render.sortingLayerName = objRender.sortingLayerName;
        render.sortingOrder = objRender.sortingOrder;

        subLayer.transform.localScale = Vector3.one;
        return subLayer;
    }    void SetupParallax() {
        // If I was setted to loop horizontally, create a new sublayer (loop) to the right of the origin point.
        if(loopHorizontal) {
            CreateLayerLoop().transform.localPosition = new Vector2(_hDistanceBetweenBgs, 0);
        }

        // If I was setted to loop vertically, create a new sublayer (loop) up of the origin point.
        if(loopVertical) {
            CreateLayerLoop().transform.localPosition = new Vector2(0, _vDistanceBetweenBgs);
            // If setted to loop both ways, create an extra layer to fill diagonally
            if(loopHorizontal) {
                CreateLayerLoop().transform.localPosition = new Vector2(_hDistanceBetweenBgs, _vDistanceBetweenBgs);
            }
        }
    }}