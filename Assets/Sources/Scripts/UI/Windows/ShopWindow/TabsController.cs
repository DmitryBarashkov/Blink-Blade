using UnityEngine;

public class TabsController : MonoBehaviour
{
    [SerializeField] private GameObject[] _categoryPanels;
    [SerializeField] private TabButton[] _buttons;

    private void Start()
    {
        SelectTab(0);
    }

    private void OnEnable()
    {
        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].TabChanged += SelectTab;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].TabChanged -= SelectTab;
    }

    private void SelectTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= _categoryPanels.Length) 
            return;

        for (int i = 0; i < _categoryPanels.Length; i++)
        {
            _categoryPanels[i].SetActive(i == tabIndex);
            _buttons[i].SetActive(i == tabIndex);
        }
    }
}
