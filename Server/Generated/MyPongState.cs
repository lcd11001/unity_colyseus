// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.35
// 

using Colyseus.Schema;
using Action = System.Action;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class MyPongState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public MyPongState() { }
	[Type(0, "map", typeof(MapSchema<PongPlayer>))]
	public MapSchema<PongPlayer> players = new MapSchema<PongPlayer>();

	[Type(1, "ref", typeof(PongBall))]
	public PongBall ball = new PongBall();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<MapSchema<PongPlayer>> __playersChange;
	public Action OnPlayersChange(PropertyChangeHandler<MapSchema<PongPlayer>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.players));
		__playersChange += __handler;
		if (__immediate && this.players != null) { __handler(this.players, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(players));
			__playersChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PongBall> __ballChange;
	public Action OnBallChange(PropertyChangeHandler<PongBall> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.ball));
		__ballChange += __handler;
		if (__immediate && this.ball != null) { __handler(this.ball, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(ball));
			__ballChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(players): __playersChange?.Invoke((MapSchema<PongPlayer>) change.Value, (MapSchema<PongPlayer>) change.PreviousValue); break;
			case nameof(ball): __ballChange?.Invoke((PongBall) change.Value, (PongBall) change.PreviousValue); break;
			default: break;
		}
	}
}

