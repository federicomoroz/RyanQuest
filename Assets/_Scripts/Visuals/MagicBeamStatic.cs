using UnityEngine;

namespace MagicArsenal
{

public class MagicBeamStatic : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject _beamLineRendererPrefab;
    [SerializeField] private GameObject _beamStartPrefab;
    [SerializeField] private GameObject _beamEndPrefab; 

                     private GameObject   _beamStart;
                     private GameObject   _beamEnd;
                     private GameObject   _beam;
                     private LineRenderer _line;

    [Header("Beam Options")]
    [SerializeField] private bool  _alwaysOn           = true; //Enable this to spawn the beam when script is loaded.
    [SerializeField] private bool  _beamCollides       = true; //Beam stops at colliders
    [SerializeField] private float _beamLength         = 100; //Ingame beam length
    [SerializeField] private float _beamEndOffset      = 0f; //How far from the raycast hit point the end effect is positioned
    [SerializeField] private float _textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    [SerializeField] private float _textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
                                            //Example: if texture is 200 pixels in height and 600 in length, set this to 3

    private void OnEnable()
    {
        if (_alwaysOn) //When the object this script is attached to is enabled, spawn the beam.
            SpawnBeam();
    }

    private void OnDisable() //If the object this script is attached to is disabled, remove the beam.
    {
        RemoveBeam();
    }

    void FixedUpdate()
    {
        if (_beam) //Updates the beam
        {
            _line.SetPosition(0, transform.position);

            Vector3 end;
            RaycastHit hit;
            if (_beamCollides && Physics.Raycast(transform.position, transform.forward, out hit)) //Checks for collision
                end = hit.point - (transform.forward * _beamEndOffset);
            else
                end = transform.position + (transform.forward * _beamLength);

            _line.SetPosition(1, end);

            if (_beamStart)
            {
                _beamStart.transform.position = transform.position;
                _beamStart.transform.LookAt(end);
            }
            if (_beamEnd)
            {
                _beamEnd.transform.position = end;
                _beamEnd.transform.LookAt(_beamStart.transform.position);
            }

            float distance = Vector3.Distance(transform.position, end);
            _line.material.mainTextureScale = new Vector2(distance / _textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            _line.material.mainTextureOffset -= new Vector2(Time.deltaTime * _textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
        }
    }

    public void SpawnBeam() //This function spawns the prefab with linerenderer
    {
        if (_beamLineRendererPrefab)
        {
            if (_beamStartPrefab)
                _beamStart = Instantiate(_beamStartPrefab);
            if (_beamEndPrefab)
                _beamEnd = Instantiate(_beamEndPrefab);
            _beam = Instantiate(_beamLineRendererPrefab);
            _beam.transform.position = transform.position;
            _beam.transform.parent = transform;
            _beam.transform.rotation = transform.rotation;
            _line = _beam.GetComponent<LineRenderer>();
            _line.useWorldSpace = true;
			_line.positionCount = 2;
		
        }
        else
            print("Add a prefab with a line renderer to the MagicBeamStatic script on " + gameObject.name + "!");
    }

    public void RemoveBeam() //This function removes the prefab with linerenderer
    {
        if (_beam)
            Destroy(_beam);
        if (_beamStart)
            Destroy(_beamStart);
        if (_beamEnd)
            Destroy(_beamEnd);
    }
}
}