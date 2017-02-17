using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour {

    public bool cameraCanMoveHorizontally;
    public float horizontalSpeed;
    public bool cameraCanMoveVertically;
    public float verticalSpeed;

    private BackgroundLayer[] bgLayers;
    private Vector3 prevCamPos;

    private void Start() {
        // I need to know the base viewPort of main camera.
        Rect viewPort = Camera.main.pixelRect;
        prevCamPos = Camera.main.gameObject.transform.position;

        // All layers must be my childrens, so that I can move freely without 
        // any layer get left behind.
        bgLayers = new BackgroundLayer[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            GameObject go = transform.GetChild(i).gameObject;
            bgLayers[i] = go.GetComponent<BackgroundLayer>();
        }
    }

    void Update() {
        // If the game is Paused, I will do nothing.
        if(Time.deltaTime == 0) {
            return;
        }
        // Otherwise, I must update all of my layers, so that they can deliver the parallax
        // effect to the player.
        UpdateLayers();
    }

    void UpdateLayers() {
        // Update all camera position, for me to use to calculate any parallax tied to
        // camera movement instead of player movement.
        Vector3 camPosition = Camera.main.gameObject.transform.position;
        Vector3 camPosDiff = camPosition - prevCamPos;
        prevCamPos = camPosition;
        // Once I have all the camera values, I will update each layer so they can work properly.
        foreach(BackgroundLayer layer in bgLayers) {
            GameObject go = layer.gameObject;

            // If the current layer is static, just update he to follow the camera position.
            if(layer.isStatic) {
                go.transform.position = (Vector2)camPosition;
            }
            else {
                // Otherwise, I need to manually update in order to create parallax effect.

                // Magnitude will dictate where the current layer must move, and how much it should move.
                // It is more a movement vector than 'magnitude'. But this word looks cool, and will leave it
                // here...
                Vector3 magnitude = Vector3.zero;

                if(!cameraCanMoveHorizontally) {
                    magnitude += Vector3.right * Time.deltaTime * layer.horizontalParallaxFactor * horizontalSpeed;
                }
                else {
                    magnitude += Vector3.right * (camPosDiff.x * (1 - layer.horizontalParallaxFactor));
                }

                if(!cameraCanMoveVertically) {
                    magnitude += Vector3.up * Time.deltaTime * layer.verticalParallaxFactor * verticalSpeed;
                }
                else {
                    magnitude += Vector3.up * (camPosDiff.y * (1 - layer.verticalParallaxFactor));
                }

                // Once magnitude is setted, move the current layer.
                go.transform.position = go.transform.position + magnitude;

                // If layer is setted to loop, I need to make sure that, this layer, will not go out of bounds, and show
                // a lot of nothing to the player.
                if(layer.loopHorizontal) {
                    if(cameraCanMoveHorizontally) {
                        if(go.transform.position.x > camPosition.x) {
                            layer.MoveLeft();
                        }
                        else if(go.transform.position.x + (layer.hDistanceBetweenBgs * layer.transform.lossyScale.x) < camPosition.x) {
                            layer.MoveRight();
                        }
                    }
                    else {
                        if(layer.transform.localPosition.x > 0) {
                            layer.MoveLeft();
                        }
                        else if(layer.transform.localPosition.x < -layer.hDistanceBetweenBgs) {
                            layer.MoveRight();
                        }
                    }
                }
                if(layer.loopVertical) {
                    if(cameraCanMoveVertically) {
                        if(go.transform.position.y > camPosition.y) {
                            layer.MoveDown();
                        }
                        else if(go.transform.position.y + (layer.vDistanceBetweenBgs * layer.transform.lossyScale.y) < camPosition.y) {
                            layer.MoveUp();
                        }
                    }
                    else {
                        if(layer.transform.localPosition.y > 0) {
                            layer.MoveDown();
                        }
                        else if(layer.transform.localPosition.y < -layer.vDistanceBetweenBgs) {
                            layer.MoveUp();
                        }
                    }
                }
            }
        }
    }
}
