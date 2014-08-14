using UnityEngine;
using System.Collections.Generic;

public interface IDragChangeView {
	int xInterv { get; }
	void RefreshParty(bool isRight);
	void RefreshView(List<PageUnitItem> view);
}
