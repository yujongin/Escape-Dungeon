using static Define;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
/// <summary> ���� Ű, ���콺 ���ε� �ɼ� </summary>
[Serializable]
public class InputBinding
{
    // ����, �ҷ����� �� ������, ���ϸ�, Ȯ����, ������ȣ
    public string localDirectoryPath = "Settings"; // "Assets/Settings"
    public string fileName = "InputBindingPreset";
    public string extName = "txt";
    public string id = "001";

    public Dictionary<UserAction, KeyCode> Bindings => _bindingDict;
    private Dictionary<UserAction, KeyCode> _bindingDict;

    public bool showDebug = false;

    /***********************************************************************
    *                               Constructors
    ***********************************************************************/
    #region .
    public InputBinding(bool initalize = true)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        if (initalize)
        {
            ResetAll();
        }
    }

    public InputBinding(SerializableInputBinding sib)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        foreach (var pair in sib.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    #endregion
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region .
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void DebugLog(object msg)
    {
        if (!showDebug) return;
        UnityEngine.Debug.Log(msg);
    }

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region .
    /// <summary> ���ο� ���ε� ���� ���� </summary>
    public void ApplyNewBindings(InputBinding newBinding)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>(newBinding._bindingDict);
    }
    /// <summary> ���ο� ���ε� ���� ���� </summary>
    public void ApplyNewBindings(SerializableInputBinding newBinding)
    {
        _bindingDict.Clear();

        foreach (var pair in newBinding.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    /// <summary> �ʱ� ���·� ���� </summary>
    public void ResetAll()
    {
        Bind(UserAction.Attack, KeyCode.K);

        Bind(UserAction.MoveForward, KeyCode.W);
        Bind(UserAction.MoveBackward, KeyCode.S);
        Bind(UserAction.MoveLeft, KeyCode.A);
        Bind(UserAction.MoveRight, KeyCode.D);

        Bind(UserAction.Dash, KeyCode.Space);
        Bind(UserAction.Postion, KeyCode.P);

        Bind(UserAction.UI_Status, KeyCode.Tab);
    }

    /// <summary> ���ε� ��� �Ǵ� ����
    /// <para/> - allowOverlap�� false�̸� ���� �ش� Ű�� ����� ������ Ű�� ������ ��� none���� ����
    /// <para/> - allowOverlap�� true�̸� ���ε��� �ߺ��ǵ��� ����
    /// </summary>
    public void Bind(in UserAction action, in KeyCode code, bool allowOverlap = false)
    {
        if (!allowOverlap && _bindingDict.ContainsValue(code))
        {
            var copy = new Dictionary<UserAction, KeyCode>(_bindingDict);

            foreach (var pair in copy)
            {
                if (pair.Value.Equals(code))
                {
                    _bindingDict[pair.Key] = KeyCode.None;
#if UNITY_EDITOR
                    DebugLog($"���� ���ε� ���� : [{pair.Key} - {pair.Value}]");
#endif
                }
            }
        }

#if UNITY_EDITOR
        if (_bindingDict.ContainsKey(action))
        {
            KeyCode prevCode = _bindingDict[action];
            DebugLog($"���ε� ���� : [{action}] {prevCode} -> {code}");
        }
        else
        {
            DebugLog($"���ε� ��� : [{action} - {code}]");
        }
#endif

        _bindingDict[action] = code;
    }

    public override string ToString()
    {
        StringBuilder Sb = new StringBuilder("");

        Sb.Append("Bindings \n");
        foreach (var pair in _bindingDict)
        {
            Sb.Append($"{pair.Key} : {pair.Value}\n");
        }

        return Sb.ToString();
    }

    #endregion
    /***********************************************************************
    *                               Input Delegate Examples
    ***********************************************************************/
    #region .
    /*
            public Func<KeyCode, bool> GetKey => (code => Input.GetKey(code));
            public Func<KeyCode, bool> KeyDown => (code => Input.GetKeyDown(code));
            public Func<KeyCode, bool> KeyUp => (code => Input.GetKeyUp(code));
    */
    #endregion
    /***********************************************************************
    *                               File IO Methods
    ***********************************************************************/
    #region .
    public bool SaveToFile()
    {
        SerializableInputBinding sib = new SerializableInputBinding(this);
        string jsonStr = JsonUtility.ToJson(sib);

        if (jsonStr.Length < 3)
        {
            Debug.Log("JSON Serialization Error");
            return false;
        }

        DebugLog($"Save : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");
        LocalFileIOHandler.Save(jsonStr, localDirectoryPath, $"{fileName}_{id}", extName);

        return true;
    }

    public bool LoadFromFile()
    {
        string jsonStr = LocalFileIOHandler.Load(localDirectoryPath, $"{fileName}_{id}", extName);

        if (jsonStr == null)
        {
            Debug.Log("File Load Error");
            return false;
        }

        var sib = JsonUtility.FromJson<SerializableInputBinding>(jsonStr);

        if (sib == null || sib.bindPairs == null)
        {
            Debug.Log("Empty File Loaded");
            return false;
        }

        ApplyNewBindings(sib);
        DebugLog($"Load : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");

        return true;
    }

    #endregion
}

[Serializable]
public class SerializableInputBinding
{
    public BindPair[] bindPairs;

    public SerializableInputBinding(InputBinding binding)
    {
        int len = binding.Bindings.Count;
        int index = 0;

        bindPairs = new BindPair[len];

        foreach (var pair in binding.Bindings)
        {
            bindPairs[index++] =
                new BindPair(pair.Key, pair.Value);
        }
    }
}

[Serializable]
public class BindPair
{
    public UserAction key;
    public KeyCode value;

    public BindPair(UserAction key, KeyCode value)
    {
        this.key = key;
        this.value = value;
    }
}