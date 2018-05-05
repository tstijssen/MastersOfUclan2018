using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsResolutionToggle : MonoBehaviour {

    public Dropdown m_DropdownMenu;

    public void OnClickDropdown()
    {
        Debug.Log("triggering");
        m_DropdownMenu.value = (m_DropdownMenu.value + 1) % m_DropdownMenu.options.Count;
    }

}
