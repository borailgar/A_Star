using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {
public Transform oyuncu, hedef;
	GridScript grid;

	void Awake() {
		grid = GetComponent<GridScript> ();
	}

	void Update() {
		YolBul (oyuncu.position, hedef.position);
	}

	void YolBul(Vector3 baslangicPos, Vector3 hedefPos) {
			
		Node baslangicDugum = grid.GridDugumleri(baslangicPos);
		Node hedefDugum = grid.GridDugumleri(hedefPos);

		List<Node> openKume = new List<Node>();
		HashSet<Node> kapaliKume = new HashSet<Node>();
		openKume.Add(baslangicDugum);

		while (openKume.Count > 0) {
			Node node = openKume[0];
			for (int i = 1; i < openKume.Count; i ++) {
				if (openKume[i].fCost < node.fCost || openKume[i].fCost == node.fCost) {
					if (openKume[i].hCost < node.hCost)
						node = openKume[i];
				}
			}

			openKume.Remove(node);
			kapaliKume.Add(node);

			if (node == hedefDugum) {
				DonusYolu(baslangicDugum,hedefDugum);
				return;
			}

			foreach (Node komsudugumler in grid.KomsulukMatrisi(node)) {
				if (!komsudugumler.walkable || kapaliKume.Contains(komsudugumler)) {
					continue;
				}

				int newCostTokomsudugumler = node.gCost + UzaklikOlcumu(node, komsudugumler);
				if (newCostTokomsudugumler < komsudugumler.gCost || !openKume.Contains(komsudugumler)) {
					komsudugumler.gCost = newCostTokomsudugumler;
					komsudugumler.hCost = UzaklikOlcumu(komsudugumler, hedefDugum);
					komsudugumler.parent = node;

					if (!openKume.Contains(komsudugumler))
						openKume.Add(komsudugumler);
				}
			}
		}
	}

	void DonusYolu(Node baslangicDugum, Node endNode) {
		List<Node> path = new List<Node>();
		Node dugum = endNode;

		while (dugum != baslangicDugum) {
			path.Add(dugum);
			dugum = dugum.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int UzaklikOlcumu(Node dugumA, Node dugumB) {
		int dstX = Mathf.Abs(dugumA.gridX - dugumB.gridX);
		int dstY = Mathf.Abs(dugumA.gridY - dugumB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
