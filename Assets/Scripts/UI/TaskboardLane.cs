using UnityEngine;

namespace UI
{
    public enum TaskboardLaneType 
    {
        TODO,
        DOING,
        DONE
    }
    
    public class TaskboardLane : MonoBehaviour
    {
        public TaskboardLaneType laneType;
        public int MaxTasks = 15;
    }
}