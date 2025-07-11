using System.Collections;
using UnityEngine;

public class ColorSpothLight : MonoBehaviour
{
    [SerializeField] private GameObject _spothlightHead;
    [SerializeField] private float _rotationSpeed = 18f;
    [SerializeField] private float _maxRotation = 44f;
    [SerializeField] private float _discoRotSpeed = 120f;
    private float _currentRotation;


    void Update()
    {
        RotateHead();
    }

    public IEnumerator SpothLightDiscoParty(float discoPartyTime)
    {
        float defaultRotSpeed = _rotationSpeed;
        _rotationSpeed = _discoRotSpeed;
        yield return new WaitForSeconds(discoPartyTime);
        _rotationSpeed = defaultRotSpeed;
    }

    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spothlightHead.transform.localRotation = Quaternion.Euler(0, 0, z);
    }

    private void RandomStartingRotation()
    {
        float randomStartingZ = Random.Range(-_maxRotation, _maxRotation);
        _spothlightHead.transform.localRotation = Quaternion.Euler(0, 0, randomStartingZ);
        _currentRotation = randomStartingZ + _maxRotation;
    }
}
