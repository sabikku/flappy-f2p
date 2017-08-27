using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Camera ))]
public class FlappyCamera : MonoBehaviour {

    public FlappyPlayer playerToFollow;
    public Vector3 offsetFromPlayer;
    public float fovMin = 40, fovMax = 60;
    public float fovSmoothSpeed = 1f;
    public float shakePower = 0.1f;
    public float shakeDuration = 0.5f;
    public UnityEngine.UI.Image flashImage;
    public float flashDuration = 0.25f;

    Camera cam;
    float playerMinSpeed, playerMaxSpeed;
    float startingY;

    float targetFOV;

    Vector3 deathEffectShakeOffset;
    float deathEffectStartTime;
    bool deathEffectStarted;
    
    void Start() {
        cam = GetComponent<Camera>();
        playerMinSpeed = playerToFollow.movingSpeed;
        playerMaxSpeed = playerToFollow.movingSpeed * 2;

        transform.position = playerToFollow.transform.position + offsetFromPlayer;
        startingY = transform.position.y;

        deathEffectShakeOffset = Vector2.zero;
        deathEffectStarted = false;
    }

    void FixedUpdate() {
        UpdateShake();
        UpdatePosition();
        UpdateFOV();
	}

	void UpdateShake() {
		if ( !deathEffectStarted ) {
			if ( playerToFollow.IsDead() ) {
				deathEffectStarted = true;
				deathEffectStartTime = Time.time;
				flashImage.color = Color.white;
			}
		} else {
			// Death screen shake interpolation
			if ( Time.time <= deathEffectStartTime + shakeDuration ) {
				float factor = (Time.time - deathEffectStartTime) / shakeDuration;
				Vector2 shake = Random.insideUnitCircle * factor;
				deathEffectShakeOffset = new Vector3( shake.x, 0, shake.y );
			}

			// Death screen flash interpolation
			if ( Time.time < deathEffectStartTime + flashDuration ) {
				float alpha = 1 - (Time.time - deathEffectStartTime) / flashDuration;
				flashImage.color = new Color( 1, 1, 1, alpha );
			}
		}
	}

    void UpdatePosition() {
        Vector3 newPos = playerToFollow.transform.position + offsetFromPlayer + deathEffectShakeOffset;
        newPos.y = startingY;
        transform.position = newPos;
    }

    void UpdateFOV() {
		// Additional effect: camera size is based on player velocity
        float fovFactor = playerToFollow.body.velocity.magnitude - playerToFollow.movingSpeed;
        fovFactor /= (playerToFollow.movingSpeed * 2);

        targetFOV = Mathf.Lerp( fovMin, fovMax, fovFactor );

        cam.fieldOfView = Mathf.Lerp( cam.fieldOfView, targetFOV, fovSmoothSpeed * Time.fixedDeltaTime );
    }

}