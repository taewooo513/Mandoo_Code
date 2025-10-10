using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorUI : MonoBehaviour
{
    [SerializeField] private GameObject[] cells;

    public void RearrangeCells(RoomDirection direction)
    {
        switch (direction)
        {
            case RoomDirection.Right:
                break;
            case RoomDirection.Left:
                SwitchCells();
                break;
            case RoomDirection.Up:
                RotateCells();
                break;
            case RoomDirection.Down:
                SwitchCells();
                RotateCells();
                break;
        }
    }

    private void SwitchCells()
    {
        (cells[0].transform.position, cells[3].transform.position) = (cells[3].transform.position, cells[0].transform.position);
        (cells[1].transform.position, cells[2].transform.position) = (cells[2].transform.position, cells[1].transform.position);
    }

    private void RotateCells()
    {
        foreach (var item in cells)
        {
            item.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
