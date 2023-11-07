using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ISerializable))]
public class Observer : MonoBehaviour, IObservable
{
    public event Action<Coordinate> NextStepReady;

    [SerializeField] private float _delay;
    [SerializeField] private string _fileName;
    [SerializeField] private bool _needSerialize;
    [SerializeField] private bool _needDeserialize;
    [SerializeField] private PhysicsRaycaster _raycaster;
    private ISerializable _handler;
    private List<string> _output;

    private void Awake()
    {
        _handler = GetComponent<ISerializable>();
    }

    private void OnEnable()
    {
        _handler.StepFinished += OnStepFinished;
    }

    private void OnDisable()
    {
        _handler.StepFinished -= OnStepFinished;
    }

    private void Start()
    {
        if (_needSerialize && !_needDeserialize)
        {
            File.Delete(_fileName + ".txt");
        }

        if (_needDeserialize && !_needSerialize)
        {
            _raycaster.enabled = false;
            var output = Deserialize();
            _output = output.Split(Environment.NewLine).ToList();
            OnStepFinished();
        }
    }

    public async Task Serialize(string input)
    {
        if (!_needSerialize && _needDeserialize)
            return;

        await using var fileStream = new FileStream(_fileName + ".txt", FileMode.Append);
        await using var streamWriter = new StreamWriter(fileStream);

        await streamWriter.WriteLineAsync(input);
    }

    private string Deserialize()
    {
        if (!File.Exists(_fileName + ".txt"))
            return null;

        using var fileStream = new FileStream(_fileName + ".txt", FileMode.Open);
        using var streamReader = new StreamReader(fileStream);

        var builder = new StringBuilder();
        while (!streamReader.EndOfStream)
        {
            builder.AppendLine(streamReader.ReadLine());
        }

        return builder.ToString();
    }

    private void OnStepFinished()
    {
        if (!_needDeserialize && _needSerialize)
            return;

        if (string.IsNullOrWhiteSpace(_output[0]))
        {
            _needSerialize = true;
            _needDeserialize = false;
            Debug.Log("Повтор игры завершен");
            _raycaster.enabled = true;
            return;
        }

        StartCoroutine(RepeatGame(_output[0]));
        _output.RemoveAt(0);
    }

    private IEnumerator RepeatGame(string input)
    {
        const string PLAYER_COMMAND_PATTERN = @"Player (\d+) (Move|Click|Remove)";
        const string COORDINATE_PATTERN = @"(\d+), (\d+)";

        Coordinate destinationPosition = default;
        yield return new WaitForSeconds(_delay);
        
        var playerCommandMatch = Regex.Match(input, PLAYER_COMMAND_PATTERN);
        var playerIndex = int.Parse(playerCommandMatch.Groups[1].Value);
        var command = playerCommandMatch.Groups[2].Value;

        var coordinateMatches = Regex.Matches(input, COORDINATE_PATTERN);
        var originPosition = (
            int.Parse(coordinateMatches[0].Groups[1].Value),
            int.Parse(coordinateMatches[0].Groups[2].Value)).ToCoordinate();

        if (command == "Move")
        {
            destinationPosition = (
                int.Parse(coordinateMatches[1].Groups[1].Value),
                int.Parse(coordinateMatches[1].Groups[2].Value)).ToCoordinate();
        }

        switch (command)
        {
            case "Click":
                Debug.Log($"Player {playerIndex} {command} to {originPosition.ToHumanString()}");
                NextStepReady?.Invoke(originPosition);
                break;

            case "Move":
                Debug.Log($"Player {playerIndex} {command} from {originPosition.ToHumanString()} to {destinationPosition.ToHumanString()}");
                NextStepReady?.Invoke(destinationPosition);
                break;

            case "Remove":
                Debug.Log($"Player {playerIndex} {command} checker at {originPosition.ToHumanString()}");
                NextStepReady?.Invoke(new Coordinate(-1, -1));
                break;

            default:
                throw new NullReferenceException("Событие пустое");
        }
    }
}