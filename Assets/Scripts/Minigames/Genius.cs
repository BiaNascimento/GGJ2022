using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Genius : MonoBehaviour
{
    [SerializeField] private int _turns;
    [SerializeField] private int _signs;
    [SerializeField] private RectTransform[] _colors;

    [SerializeField] private GameObject _correct;

    [SerializeField] private RectTransform _light;
    [SerializeField] private float _shineTime;

    private List<int> _order = new List<int>();
    private List<int> _playerOrder = new List<int>();

    private int _signsLeft;
    private bool _canCheck = true;

    private int _playerInputs;

    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _winPanel;

    private void Start()
    {
        _signsLeft = _signs;
        StartGenius();
    }

    private void StartGenius()
    {
        while (_signsLeft > 0 && _canCheck)
        {
            StartCoroutine(Shine(_shineTime));
        }
    }

    public void ListenButton(int id)
    {
        if (_signsLeft <= 0)
        {
            _playerOrder.Add(id);
            _playerInputs++;
            if (_playerInputs == _signs)
            {
                if (CheckOrder())
                {
                    _correct.SetActive(true);
                    if (_turns > 0)
                        NextRound();
                    else
                        _winPanel.SetActive(true);
                }
                else
                {
                    _loosePanel.SetActive(true);
                }
            }
        }
    }

    IEnumerator Shine(float time)
    {
        int n = Random.Range(0, 4);
        _order.Add(n);

        _canCheck = false;
        _light.position = _colors[n].position;

        yield return new WaitForSeconds(time / 2);

        _light.gameObject.SetActive(true);

        yield return new WaitForSeconds(time / 2);

        _light.gameObject.SetActive(false);
        _canCheck = true;

        _signsLeft--;

        StartGenius();
    }

    private bool CheckOrder()
    {
        for (int i = 0; i < _playerOrder.Count; i++)
        {
            if (_playerOrder[i] != _order[i])
                return false;
        }
        return true;
    }

    public void NextRound()
    {
        _correct.SetActive(false);
        _playerInputs = 0;
        _signs++;
        _signsLeft = _signs;

        _order.Clear();
        _playerOrder.Clear();
        StartGenius();
        _turns--;
    }
}
