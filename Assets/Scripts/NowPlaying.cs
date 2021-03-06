﻿using TMPro;
using UnityEngine;
using static ResolutionRelation;

public class NowPlaying : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI songArtist;
    [SerializeField] private TextMeshProUGUI songTitle;
    [SerializeField] private TextMeshProUGUI soundDesignStudent;
    [SerializeField] private float distanceToMoveOnX = 500f;
    [SerializeField] private float transitionTime = 1.5f;
    [SerializeField] private float durationToLive = 3f;

    private enum State{ TransitionIn, Display, TransitionOut }
    private State _currentState = State.TransitionIn;
    private Vector2 _startingPos;
    private Vector2 _endPos;
    private float _timeElapsed = 0f;
    
    public void Setup(string bandName, string songName, string studentName)
    {
        songArtist.SetText($"{bandName}");
        songTitle.SetText($"{songName}");
        soundDesignStudent.SetText($"composed by: {studentName}");
    }

    private void Start()
    {
        var exists = FindObjectsOfType<NowPlaying>();
        foreach (var instance in exists)
        {
            if(instance != this)
                Destroy(instance.gameObject);
        }
        _startingPos = transform.position; 
        _endPos = new Vector2(_startingPos.x + distanceToMoveOnX * WidthRelation, _startingPos.y);
        Destroy(this.gameObject, 2 * transitionTime + durationToLive);
        
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.TransitionIn:
                if (_timeElapsed < transitionTime)
                {
                    transform.position = Vector2.Lerp(_startingPos, _endPos, _timeElapsed / transitionTime);
                }
                else
                    _currentState = State.Display;
                break;
               
            case State.Display:
                if (_timeElapsed > transitionTime + durationToLive)
                {
                    _currentState = State.TransitionOut;
                    _timeElapsed = 0;
                }
                break;
            
            case State.TransitionOut:
                if(_timeElapsed < transitionTime)
                    transform.position = Vector2.Lerp(_endPos, _startingPos, _timeElapsed / transitionTime);
                break;
        }
        
        _timeElapsed += Time.deltaTime;
    }
}