using UnityEngine;

public class DoorSwitcheable : Door
{
    [SerializeField] private Switch[] _switches;

    private void OnEnable()
    {
        Signal();
        for (int i = 0; i < _switches.Length; i++)
        {
            _switches[i].e_switchUpdate += Signal;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _switches.Length; i++)
        {
            _switches[i].e_switchUpdate -= Signal;
        }
    }

    public void Signal()
    {
        if (CheckSwitches())
            Open();
        else
            Close();
    }


    private bool CheckSwitches()
    {
        int switchesOn = 0;
        for (int i = 0; i < _switches.Length; i++)
        {
            bool currentSwitchState = _switches[i].GetComponent<IActivatable>().CheckState();
            if (currentSwitchState)
                switchesOn++;
        }
        
        return switchesOn >= _switches.Length;
    }
}
