using UnityEngine;

public class LevelManagerTitle : LevelManager<LevelManagerTitle>
{
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _controls;
    private bool _hasPlayButtonBeenPushed = false;


    public void ToggleCredits()
    {
        _credits.SetActive(!_credits.activeSelf);
    }

    public void ToggleControls()
    {
        _controls.SetActive(!_controls.activeSelf);
    }

    public override void StartGame()
    {
        if (!_hasPlayButtonBeenPushed)
        {
            _hasPlayButtonBeenPushed = true;
            base.StartGame();
        }

    }
}
