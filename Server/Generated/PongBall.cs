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

public partial class PongBall : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public PongBall() { }
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);

	[Type(2, "int32")]
	public int tick = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<float> __xChange;
	public Action OnXChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.x));
		__xChange += __handler;
		if (__immediate && this.x != default(float)) { __handler(this.x, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(x));
			__xChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __yChange;
	public Action OnYChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.y));
		__yChange += __handler;
		if (__immediate && this.y != default(float)) { __handler(this.y, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(y));
			__yChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __tickChange;
	public Action OnTickChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.tick));
		__tickChange += __handler;
		if (__immediate && this.tick != default(int)) { __handler(this.tick, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(tick));
			__tickChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(x): __xChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(y): __yChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(tick): __tickChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

