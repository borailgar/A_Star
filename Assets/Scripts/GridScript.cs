using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

	//public Transform player;

	public LayerMask kenarMaskesi;
	public Vector2 gridWorld;
	public float nodeCapi;
	Node[,] grid;

	float nodeAlani;
	int gridX, gridY;

	void Awake() {
		nodeAlani = nodeCapi*2;
		gridX = Mathf.RoundToInt(gridWorld.x/nodeAlani);
		gridY = Mathf.RoundToInt(gridWorld.y/nodeAlani);
		GridOlustur();
	}

	void GridOlustur() {
		grid = new Node[gridX,gridY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorld.x/2 - Vector3.forward * gridWorld.y/2;

		for (int x = 0; x < gridX; x ++) {
			for (int y = 0; y < gridY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeAlani + nodeCapi) + Vector3.forward * (y * nodeAlani + nodeCapi);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeCapi,kenarMaskesi));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}

	public List<Node> KomsulukMatrisi(Node node) {
		List<Node> komsuluklar = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY) {
					komsuluklar.Add(grid[checkX,checkY]);
				}
			}
		}

		return komsuluklar;
	}
	

	public Node GridDugumleri(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorld.x/2) / gridWorld.x;
		float percentY = (worldPosition.z + gridWorld.y/2) / gridWorld.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridX-1) * percentX);
		int y = Mathf.RoundToInt((gridY-1) * percentY);
		return grid[x,y];
	}

	public List<Node> path;
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorld.x,1,gridWorld.y));

		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeAlani-.1f));
			}
		}
	}
}
