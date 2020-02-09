using System;
using System.Diagnostics;
using System.Linq;
using Core;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UI
{
    public class TaskBehaviour : MonoBehaviour, IHasToolTip
    {
        private TaskboardLane _currentLane;
        private bool _isPickedUp = false;

        public string Owner = "Max Mustermann";
        public string StartTime = "10:10 Uhr";
        public string EndTime = "10:10 Uhr";

        public bool IsStarted = false;
        public bool IsProgrammed = false;
        public bool IsTested = false;
        public bool IsDocumented = false;

        public Tooltip Tooltip;
        
        private string GetStatusText()
        {
            var status = "";

            if (IsProgrammed)
            {
                status += "umgesetzt, ";
            }

            if (IsTested)
            {
                status += "getestet, ";
            }

            if (IsDocumented)
            {
                status += "dokumentiert, ";
            }

            return status.Trim().TrimEnd(',');
        }
        
        private void OnEnable()
        {
            _currentLane = transform.parent.GetComponent<TaskboardLane>();
        }

        private TaskboardLane GetLaneUnderCursor()
        {
            var origin = Input.mousePosition;
            var hits = Physics2D.RaycastAll(origin, Vector2.zero);
            var lanes = hits.Where(hit => hit.collider.GetComponent<TaskboardLane>() != null).ToList();
            
            if (lanes.Count > 0)
            {
                return lanes[0].collider.GetComponent<TaskboardLane>();
            }
            else
            {
                return null;
            }
        }

        public void DragTask()
        {
            transform.position = Input.mousePosition;

            if (!_isPickedUp)
            {
                transform.SetParent(_currentLane.transform.parent);
                _isPickedUp = true;
            }
        }

        public void ShowTooltip()
        {
            Tooltip.Show(this);
        }
        
        public void HideTooltip()
        {
            Tooltip.Hide();
        }

        public void DropTask()
        {
            var lane = GetLaneUnderCursor();

            if (lane != null)
            {
                if (IsCorrectLane(lane))
                {
                    _currentLane = lane;
                    GameManager.Instance.Company.AddEffectToCompanyScore("Agilität", "Taskboard Pflege", 2);
                }
                else
                {
                    UiManager.Instance.TaskBoardScreen.DoErrorShake();
                }
            }

            transform.SetParent(_currentLane.transform, true);
            _isPickedUp = false;
        }

        private bool IsCorrectLane(TaskboardLane lane)
        {
            if (lane.laneType == TaskboardLaneType.TODO)
            {
                return IsStarted == false;
            }

            if (lane.laneType == TaskboardLaneType.DOING)
            {
                return IsStarted && !(IsProgrammed && IsDocumented && IsTested);
            }
            
            if (lane.laneType == TaskboardLaneType.DONE)
            {
                return IsProgrammed && IsDocumented && IsTested;
            }

            return false;
        }

        public string GetTooltip()
        {
            var tooltip = "";
            tooltip += "Bearbeitet von: " + Owner + "\n";
            tooltip += "Von: " + StartTime;
            if (EndTime != "")
            {
                tooltip += " Bis: " + EndTime;
            }

            tooltip += "\n\n";
            tooltip += GetStatusText();

            return tooltip;
        }
    }
}