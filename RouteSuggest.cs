using Godot;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Map;

namespace RouteSuggest;

[ModInitializer("ModLoaded")]
public static class RouteSuggest {
	public static RunState RunState { get; set; }

	public static void ModLoaded() {
		Log.Warn("RouteSuggest: Mod loaded");

		// listen to events
		var manager = RunManager.Instance;
		manager.RunStarted += OnRunStarted;
		manager.ActEntered += OnActEntered;
		manager.RoomEntered += OnRoomEntered;
		manager.RoomExited += OnRoomExited;
	}

	static void OnRunStarted(RunState runState)
    {
		Log.Warn("RouteSuggest: Run started");
		RouteSuggest.RunState = runState;
		PrintInfo();
    }

	static void OnActEntered()
    {
		Log.Warn("RouteSuggest: Act entered");
		PrintInfo();
    }

	static void OnRoomEntered()
    {
		Log.Warn("RouteSuggest: Room entered");
		PrintInfo();
    }

	static void OnRoomExited()
    {
		Log.Warn("RouteSuggest: Room exited");
		PrintInfo();
    }

	static void PrintInfo()
	{
		var runState = RouteSuggest.RunState;
		Log.Warn($"RouteSuggest: Current act index {runState.CurrentActIndex}");
		Log.Warn($"RouteSuggest: Floor {runState.ActFloor}/{runState.TotalFloor}");
		if (runState.CurrentMapPoint != null) {
			Log.Warn($"RouteSuggest: At map point {runState.CurrentMapPoint.coord}");

			// DFS to find all paths to Boss
			var currentPath = new List<MapPoint>();
			var allPaths = new List<List<MapPoint>>();
			FindAllPathsToBoss(runState.CurrentMapPoint, currentPath, allPaths);

			Log.Warn($"RouteSuggest: Found {allPaths.Count} path(s) to Boss");
			for (int i = 0; i < allPaths.Count; i++)
			{
				Log.Warn($"RouteSuggest: Path {i + 1}:");
				foreach (var point in allPaths[i])
				{
					Log.Warn($"RouteSuggest:   coord={point.coord}, type={point.PointType}");
				}
			}
		}
	}

	static void FindAllPathsToBoss(MapPoint current, List<MapPoint> currentPath, List<List<MapPoint>> allPaths)
	{
		if (current == null) return;

		// Add current point to path
		currentPath.Add(current);

		// Check if we reached the Boss
		if (current.PointType == MapPointType.Boss)
		{
			// Found a path to Boss, save a copy
			allPaths.Add(new List<MapPoint>(currentPath));
		}
		else if (current.Children != null && current.Children.Count > 0)
		{
			// Continue DFS on children
			foreach (var child in current.Children)
			{
				FindAllPathsToBoss(child, currentPath, allPaths);
			}
		}

		// Backtrack: remove current point from path
		currentPath.RemoveAt(currentPath.Count - 1);
	}
}
